import { Component, OnInit, OnDestroy, Input, EventEmitter, ViewChild } from '@angular/core';
import { IReviewProfileSetup } from '../shared/review-profile-setup.model';
import { IReviewProfileEvaluation } from '../shared/review-profile-evaluation.model';
import { ReviewProfileContextService } from '../review-profile-context.service';
import { ReviewProfilesApiService } from '../review-profiles-api.service';
import { IReviewProfileEvaluationFeedback } from '../shared/review-profile-evaluation-feedback.model';
import { Subscription, zip, throwError } from 'rxjs';
import { catchError, concatMap } from 'rxjs/operators';
import { EvaluationRoleType } from '@ds/performance/evaluations';
import { Output } from '@angular/core';
import { FormControl, NgForm } from '@angular/forms';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { HttpErrorResponse } from '@angular/common/http';
import * as _ from 'lodash';
import { IReviewRating } from '../../ratings/shared/review-rating.model';
import { PerformanceReviewsService } from '../../shared/performance-reviews.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { IFeedback, IFeedbackSetup } from '@ds/performance/feedback';
import { FeedbackApiService } from '@ds/performance/feedback/feedback-api.service';

class Section {
    private _isEnabled = false;
    get isEnabled() {
        return this._isEnabled;
    }
    set isEnabled(val: boolean) {
        this._isEnabled = val;

        // //open on first enable
        // if(val && !this.isOpen)
        //     this.isOpen = true;

        this.isOpen = this._isEnabled;
    }
    isOpen = false;
}

class EvaluationSection extends Section {
    evalSettings: IReviewProfileEvaluation;
    feedbackItems: FeedbackItem[] = [];

    get isSupervisor() {
        return this.evalSettings.role === EvaluationRoleType.Manager;
    }

    get isSelf() {
        return this.evalSettings.role === EvaluationRoleType.Self;
    }

    get selectedFeedbackCount() {
        return this.feedbackItems.filter(f => f.isEnabled).length;
    }

    constructor(settings: IReviewProfileEvaluation, feedback: IFeedbackSetup[]) {
        super();
        this.evalSettings = settings;
        this.isEnabled = settings.isActive;
        this.isOpen = this.isEnabled;

        //load feedback items
        feedback.forEach(f => {
            let item = new FeedbackItem();
            item.feedback = f;

            let profileFeedbackSettings = settings.feedback ? settings.feedback.find(x => x.feedbackId === f.feedbackId) : null;
            if (profileFeedbackSettings) {
                item.isEnabled = true;
                item.orderIndex = profileFeedbackSettings.orderIndex;
            }
            else {
                item.isEnabled = false;
            }

            this.feedbackItems.push(item);
        });
        this.sortFeedback();
    }

    toggleGoals() {
        if (!this.evalSettings.includeGoals) {
            this.evalSettings.isGoalsWeighted = false;
            this.evalSettings.isGoalCommentRequired = false;
        }
    }

    toggleCompetencies() {
        if (!this.evalSettings.includeCompetencies)
            this.evalSettings.isCompetencyCommentRequired = false;
    }

    toggleFeedbackSection() {
        if (!this.evalSettings.includeFeedback) {
            this.feedbackItems.forEach(i => {
                i.isEnabled = false;
                i.orderIndex = null;
            })
            this.sortFeedback();
        }
    }

    toggleFeedbackItem(item: FeedbackItem) {
        item.orderIndex = item.isEnabled ? this.selectedFeedbackCount : null;
        this.sortFeedback();
        this.syncFeedbackOrderIndexes();
    }

    private sortFeedback() {
        this.feedbackItems.sort((f1, f2) => {
            if (f1.isEnabled !== f2.isEnabled)
                return f1.isEnabled ? -1 : 1;

            if (f1.isEnabled)
                return f1.orderIndex < f2.orderIndex ? -1 : 1;

            return f1.feedback.body < f2.feedback.body ? -1 : 1;
        });
    }

    private syncFeedbackOrderIndexes() {
        this.feedbackItems.forEach((item, idx) => {
            item.orderIndex = item.isEnabled ? idx : null;
        });
    }
}

class FeedbackItem {
    orderIndex: number;
    isEnabled: boolean = false;
    feedback: IFeedbackSetup;
}

@Component({
    selector: 'ds-review-profile-setup-form',
    templateUrl: './review-profile-setup-form.component.html',
    styleUrls: ['./review-profile-setup-form.component.scss']
})
export class ReviewProfileSetupFormComponent implements OnInit {

    @Output("save")
    onSave = new EventEmitter<IReviewProfileSetup>();

    @Output("cancel")
    onCancel = new EventEmitter();

    isLoading = true;
    isScoringEnabledForClient = false;
    reviewProfile: IReviewProfileSetup;
    feedbackList: IFeedbackSetup[];
    evalSections: EvaluationSection[] = [];
    meetingSection: Section;
    scoringSection: Section;
    payrollRequestSection: Section;
    reviewRatings: IReviewRating[];
    user: UserInfo;

    constructor(
        private reviewProfileCtx: ReviewProfileContextService,
        private reviewProfileSvc: ReviewProfilesApiService,
        private msgSvc: DsMsgService,
        private feedbackSvc: FeedbackApiService,
        private performenceReviewSvc: PerformanceReviewsService,
        private accountService: AccountService, ) {
    }

    ngOnInit() {
        let feedback$ = this.feedbackSvc.getFeedbackSetup();
        this.performenceReviewSvc.getScoreModelForCurrentClient().subscribe(x => this.isScoringEnabledForClient = x.data.isActive);

        this.accountService
            .getUserInfo()
            .pipe(
            concatMap((user: UserInfo) => {
                this.user = user;
                return this.performenceReviewSvc.getPerformanceReviewRatings(this.user.lastClientId || this.user.clientId)
            }))
            .subscribe(ratings => {
                this.reviewRatings = ratings;
            });


        zip(this.reviewProfileCtx.activeReviewProfileSetup$, feedback$, (reviewProfile, feedback) => { return { reviewProfile, feedback } })
            .subscribe(data => {
                if (!data.reviewProfile) {
                    this.onCancel.emit();
                    return;
                }

                this.reviewProfile = _.cloneDeep(data.reviewProfile);
                this.feedbackList = data.feedback.filter(x => !x.isArchived);
                this.initProfile();
                this.isLoading = false;
            });
    }

    initProfile() {

        if (!this.reviewProfile)
            return;

        let eeEvalSettings: IReviewProfileEvaluation;
        let supEvalSettings: IReviewProfileEvaluation;

        if (this.reviewProfile.evaluations) {
            eeEvalSettings = this.reviewProfile.evaluations.find(x => x.role === EvaluationRoleType.Self);
            supEvalSettings = this.reviewProfile.evaluations.find(x => x.role === EvaluationRoleType.Manager);
        }

        if (!supEvalSettings)
            supEvalSettings = <IReviewProfileEvaluation>{ role: EvaluationRoleType.Manager, selectedCompetenceyRatings: [], selectedGoalRatings: [] };
        this.evalSections.push(new EvaluationSection(supEvalSettings, this.feedbackList.filter(f => !!f.isSupervisor)));

        if (!eeEvalSettings)
            eeEvalSettings = <IReviewProfileEvaluation>{ role: EvaluationRoleType.Self, selectedCompetenceyRatings: [], selectedGoalRatings: [] };
        this.evalSections.push(new EvaluationSection(eeEvalSettings, this.feedbackList.filter(f => !!f.isSelf)));

        this.meetingSection = new Section();
        this.meetingSection.isEnabled = !!this.reviewProfile.includeReviewMeeting;

        this.scoringSection = new Section();
        this.scoringSection.isEnabled = !!this.reviewProfile.includeScoring;

        this.payrollRequestSection = new Section();
        this.payrollRequestSection.isEnabled = !!this.reviewProfile.includePayrollRequests;
    }

    saveReviewProfile(mainForm: NgForm) {
      mainForm.controls.markAsTouched;
        (<any>mainForm).submitted = true;
        if (mainForm.invalid)
            return;

        this.reviewProfile.includeReviewMeeting = this.meetingSection.isEnabled;
        this.reviewProfile.includePayrollRequests = this.payrollRequestSection.isEnabled;
        this.reviewProfile.includeScoring = this.scoringSection.isEnabled;
        this.reviewProfile.evaluations = [];

        this.evalSections.forEach(x => {
            if (!x.isEnabled && !x.evalSettings.reviewProfileEvaluationId)
                return;

            let settings = x.evalSettings;
            settings.isActive = x.isEnabled;

            //TODO: hardcoding for now ... until we support this in the UI
            settings.isDisclaimerRequired = true;
            settings.isSignatureRequired = true;

            settings.feedback = [];
            x.feedbackItems.forEach(f => {
                if (f.isEnabled)
                    settings.feedback.push({ feedbackId: f.feedback.feedbackId, orderIndex: f.orderIndex, feedbackBody: f.feedback.body });
            });

            this.reviewProfile.evaluations.push(settings);
        });

        this.msgSvc.loading(true);
        this.reviewProfileSvc.saveReviewProfileSetup(this.reviewProfile)
            .pipe(catchError((err: HttpErrorResponse, caught) => {
                this.msgSvc.showWebApiException(err.error);
                return throwError("Error saving review");
            }))
            .subscribe(profile => {
                this.msgSvc.setTemporarySuccessMessage("Review profile saved successfully");
                this.onSave.emit(profile);
            });
    }

    cancel() {
        this.onCancel.emit();
    }

    updateCompetencyRatings(settings: IReviewProfileEvaluation, reviewRatingId, event) {
        var checked = event.target.checked;
        var index = -1;
        if (settings.selectedCompetenceyRatings) {
            index = settings.selectedCompetenceyRatings.indexOf(reviewRatingId);
        }

        if (checked && index === -1) {
            settings.selectedCompetenceyRatings.push(reviewRatingId);
        }

        if (!checked && index !== -1) {
            settings.selectedCompetenceyRatings.splice(index, 1);
        }
    }

    updateGoalRatings(settings: IReviewProfileEvaluation, reviewRatingId, event) {
        var checked = event.target.checked;
        var index = settings.selectedGoalRatings.indexOf(reviewRatingId);
        if (checked && index === -1) {
            settings.selectedGoalRatings.push(reviewRatingId);
        }

        if (!checked && index !== -1) {
            settings.selectedGoalRatings.splice(index, 1);
        }
    }
}

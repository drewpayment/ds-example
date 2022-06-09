import { isEmpty } from 'lodash';
import { ICompetencyEvaluation } from '..';
import { IReviewRating } from '@ds/performance/ratings';
import { ICompetencyRateCommentRequired } from './competency-rate-comment-required.model';
import { ApprovalProcessStatus } from './approval-process-status.enum';
import { IApprovalProcessStatusIdAndIsEditedByApprover } from './approval-process-status-id-and-is-edited-by-approver.interface';

export class CompetencyEvalItem implements IApprovalProcessStatusIdAndIsEditedByApprover {
    private _competency: ICompetencyEvaluation;
    set competency(comp: ICompetencyEvaluation) {
        this._competency = comp;
        this.comments = comp.comment ? comp.comment.description : null;
        this.ratingValue = comp.ratingValue;
    }
    pushActivity(val:any) {
        this._competency.activityFeed.push(val);
    }
    get competency() {
        return this._competency;
    }
    get competencyId() {
        return this._competency.competencyId;
    }
    isLoading = false;
    isActive = false;
    comments: string;
    private _ratingValue: number;
    set ratingValue(val: number) {
        this.selectedRating = _.find(this.ratings, r => r.rating === val);
        this._ratingValue = val;
    }

    get ratingValue() {
        return this._ratingValue;
    }

    selectedRating: IReviewRating;
    hoverRating: IReviewRating;
    get hasContent() {
        return this.competency.comment || this.competency.ratingValue;
    }

    get hasComment() {
        return (this.competency.comment != null && this.competency.comment.description.length > 0);
    }

    get hasChanges() {
        let savedComments = this.competency.comment ? this.competency.comment.description : null;
        let currentComments = this.comments ? this.comments.trim() : null;
        return this.competency.ratingValue !== this._ratingValue || savedComments != currentComments;
    }

    get isComplete() {
        if (this.competency.isCommentRequired) {
            return !!this.ratingValue && !isEmpty(this._competency.comment ? this._competency.comment.description : null);
        }
        else {
            return !!this.ratingValue;
        }
    }

    get isWithoutComment() {
        return !!this.ratingValue && this.isCommentRequired && isEmpty(this.comments);
    }

    constructor(competencyEval: ICompetencyEvaluation, private ratings: IReviewRating[]) {
        this.competency = competencyEval;
    }

    get activityFeed() {
        return this._competency.activityFeed;
    }

    set activityFeed(val: any) {
        this._competency.activityFeed = val;
    }

    get approvalProcessStatusId() {
        return this._competency.approvalProcessStatusId;
    }
    set approvalProcessStatusId(val: ApprovalProcessStatus) {
        this._competency.approvalProcessStatusId = val;
    }

    get isEditedByApprover() {
        return this._competency.isEditedByApprover;
    }

    private _competencyRateCommentRequired: ICompetencyRateCommentRequired[];

    set CompetencyRateCommentRequired(value: ICompetencyRateCommentRequired[]){
        this._competencyRateCommentRequired = value
    }

    get CompetencyRateCommentRequired(){
        return this._competencyRateCommentRequired || [];
    }

    get isCommentRequired() {
        return this.CompetencyRateCommentRequired.some(x => x.reviewRating.rating == this.ratingValue);
    }

    set isCommentRequired(val: boolean) {
        this._competency.isCommentRequired = val;
    }

    color(isFormSubmitted: boolean) {
        if (isFormSubmitted && !this.isComplete)
            return "danger";

        if (!isFormSubmitted && this.isWithoutComment)
            return "danger";

        if (this.isActive && !this.isWithoutComment)
            return "info";

        if (!this.isComplete)
            return "info-special";

        return "success";
    }

    approvalColor() {

        if (this.isWithoutComment)
            return "danger";

        if (this._competency.approvalProcessStatusId === ApprovalProcessStatus.Rejected)
            return "warning"

        if (this._competency.approvalProcessStatusId === ApprovalProcessStatus.Approved)
            return "success"

        return "info";
    }

    approvalBtnClass() {

        if (this.isWithoutComment)
            return "btn-danger";

        if (this._competency.approvalProcessStatusId === ApprovalProcessStatus.Rejected)
            return "btn-warning"

        if (this._competency.approvalProcessStatusId === ApprovalProcessStatus.Approved)
            return "btn-success"

        return "btn-info";
    }

    approvalBtnLabel() {

        if (this.isWithoutComment)
            return "Incomplete";

        if (this._competency.approvalProcessStatusId === ApprovalProcessStatus.Rejected)
            return "Needs Revision"

        if (this._competency.approvalProcessStatusId === ApprovalProcessStatus.Approved)
            return "Approved"

        return "Select Status";
    }
}

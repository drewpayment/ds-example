import { IGoalEvaluation } from "..";
import * as _ from "lodash";
import { IReviewRating } from '@ds/performance/ratings';
import { IGoalRateCommentRequired } from './goal-rate-comment-required.model';
import { IApprovalProcessStatusIdAndIsEditedByApprover } from './approval-process-status-id-and-is-edited-by-approver.interface';
import { ApprovalProcessStatus } from './approval-process-status.enum';

export class GoalEvalItem implements IApprovalProcessStatusIdAndIsEditedByApprover {
    private _goal: IGoalEvaluation;
    set goal(goal: IGoalEvaluation) {
        this._goal       = goal;
        this.comments    = goal.comment ? goal.comment.description : null;
        this.ratingValue = goal.ratingValue;
    }
    get goal() {
        return this._goal;
    }

    isLoading = false;
    isActive = false;
    comments: string;
    private _ratingValue: number;
    set ratingValue(val: number){
        this.selectedRating = _.find(this.ratings, r => r.rating === val);
        this._ratingValue = val;
    }

    get ratingValue() {
        return this._ratingValue;
    }

    selectedRating: IReviewRating;
    hoverRating: IReviewRating;
    get hasContent() {
        return this.goal.comment || this.goal.ratingValue;
    }

    get hasComment() {
        return (this.goal.comment != null && this.goal.comment.description.length > 0)
    }

    get hasChanges() {
        let savedComments = this.goal.comment ? this.goal.comment.description : null;
        let currentComments = this.comments ? this.comments.trim() : null;
        return this.goal.ratingValue !== this._ratingValue || savedComments != currentComments;
    }

    get isComplete() {
        if (this.isCommentRequired) {
            return !!this.ratingValue && !_.isEmpty(this.comments);
        }
        else {
            return !!this.ratingValue;
        }
    }

    private _GoalRateCommentRequired: IGoalRateCommentRequired[];

    set GoalRateCommentRequired(value: IGoalRateCommentRequired[]){
        this._GoalRateCommentRequired = value
    }

    get GoalRateCommentRequired(){
        return this._GoalRateCommentRequired || [];
    }

    get isWithoutComment() {
        return !!this.ratingValue && this.isCommentRequired && _.isEmpty(this.comments);
    }

    constructor(goalEval: IGoalEvaluation, private ratings: IReviewRating[]) {
        this.goal = goalEval;
    }

    get activityFeed() {
        return this.goal.activityFeed;
    }

    get approvalProcessStatusId() {
        return this.goal.approvalProcessStatusId;
    }

    set approvalProcessStatusId(value: ApprovalProcessStatus) {
        this.goal.approvalProcessStatusId = value;
    }

    get isEditedByApprover() {
        return this.goal.isEditedByApprover;
    }

    get isCommentRequired() {
        return this.GoalRateCommentRequired.some(x => x.reviewRating.rating == this.ratingValue);
    }

    set isCommentRequired(val: boolean) {
        this.goal.isCommentRequired = val;
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

        if (this.approvalProcessStatusId == 0)
            return "warning"

        if (this.approvalProcessStatusId == 1)
            return "success"

        return "info";
    }

    approvalBtnClass() {

        if (this.isWithoutComment)
            return "btn-danger";

        if (this._goal.approvalProcessStatusId == 0)
            return "btn-warning"

        if (this._goal.approvalProcessStatusId == 1)
            return "btn-success"

        return "btn-info";
    }

    approvalBtnLabel() {

        if (this.isWithoutComment)
            return "Incomplete";

        if (this._goal.approvalProcessStatusId == 0)
            return "Needs Revision"

        if (this._goal.approvalProcessStatusId == 1)
            return "Approved"

        return "Select Status";
    }

    weightColor(onReview:boolean){
        if(onReview)
            return "info";

        return "disabled";
    }
}

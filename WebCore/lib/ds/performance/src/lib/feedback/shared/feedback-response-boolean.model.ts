import { BaseSingleFeedbackResponse } from "./feedback-response-single.model";

export class BooleanFeedbackResponse extends BaseSingleFeedbackResponse<boolean> {
    responseText() {
        return this.hasResponse() ? (this.value ? "Yes" : "No") : super.responseText();
    }
}
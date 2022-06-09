import { BaseSingleFeedbackResponse } from "./feedback-response-single.model";
import { IFeedbackItem } from "./feedback-item.model";

export class ListItemFeedbackResponse extends BaseSingleFeedbackResponse<IFeedbackItem> {
    feedbackItems: IFeedbackItem[];
    responseText() {
        return this.hasResponse() ? this.value.itemText : super.responseText();
    }
}
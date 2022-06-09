import { BaseSingleFeedbackResponse } from "./feedback-response-single.model";
import { listDesc } from './list-desc.func';

export class TextItemFeedbackResponse extends BaseSingleFeedbackResponse<string> {
    hasResponse() {
        return this.value && !!this.value.trim();
    }
    responseText() {
        return this.hasResponse() ?  listDesc(this.value.toString()) : "No response given";
    }
}
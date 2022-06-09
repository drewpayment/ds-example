import { BaseSingleFeedbackResponse } from "./feedback-response-single.model";
import * as moment from "moment";
export class DateFeedbackResponse extends BaseSingleFeedbackResponse<string | Date> {
    responseText() {
        return this.hasResponse() ? moment(this.value).format("MM/DD/YYYY") : super.responseText();
    }
}
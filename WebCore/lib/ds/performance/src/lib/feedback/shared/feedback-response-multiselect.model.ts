import { BaseSingleFeedbackResponse } from "./feedback-response-single.model";
import { IFeedbackItem } from "./feedback-item.model";

export class MultiSelectFeedbackResponse extends BaseSingleFeedbackResponse<string> {
    feedbackItems: IFeedbackItem[];
    hasResponse() {
        return this.value && !!this.value.trim();
    }
    responseText() {
        var txt = '';
        if(this.value){
            this.feedbackItems.forEach( fI => {
                txt += '[ ';
                txt += ( fI.checked ? 'X' : ' ' );
                txt += ' ] ' + fI.itemText + '    ';
            });
            if(txt) txt = txt.substring(0,txt.length-1);
        } else {
            txt = 'Not specified';
        }
        return txt.trim();
    }
}
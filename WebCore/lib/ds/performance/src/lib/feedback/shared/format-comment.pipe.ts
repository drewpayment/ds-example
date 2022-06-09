import { Pipe, PipeTransform } from '@angular/core';
import { listDesc } from './list-desc.func';

@Pipe({
  name: 'formatComment',
  pure: true
})
export class FormatCommentPipe implements PipeTransform {
  transform = listDesc;
}



@Pipe({
  name: 'noCommentsEntered',
  pure: true
})
export class NoCommentsEnteredPipe implements PipeTransform {

  transform(item?: {comment: { description: string }}): string {
    if(!item){
      return '';
    }
    
    return (item.comment != null && item.comment.description != null && item.comment.description != '') 
        ? 'Comments: ' + item.comment.description : 'No comments entered.';
  }
}

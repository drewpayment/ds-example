import { Pipe, PipeTransform } from '@angular/core';
import { IReview } from '@ds/performance/reviews';
import { ApprovalStatus } from '@ds/performance/evaluations/shared/approval-status.enum';

@Pipe({
  name: 'proposalApprovalStatus',
  pure: true
})
export class ProposalApprovalStatusPipe implements PipeTransform {
  transform(data: IReview[], args: number[]): any {
    return data.filter(review => {
      return args.includes((review.proposal ? review.proposal.approvalStatus : 4));
    });
  }

}
@Pipe({
  name: 'proposalItemsApprovalStatus',
  pure: true
})
export class ProposalItemsApprovalStatusPipe implements PipeTransform {
  transform(data: IReview[], args: ApprovalStatus[]): any {
    return data.filter(review => {
      if(review.proposal){
        let mi = review.proposal.meritIncreases;
        let isPresent = false;
        args.forEach(x => {
          if( (mi && mi.length > 0 && mi.map(y=>y.approvalStatusID).includes(x)) || 
            (review.proposal.oneTimeEarning && review.proposal.oneTimeEarning.approvalStatusID == x) ) 
              isPresent = true;
        });
        return isPresent;
      }
      return args.includes(4);
    });
  }

}

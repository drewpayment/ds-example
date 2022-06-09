import { Pipe, PipeTransform } from '@angular/core';
import { IApprovalProcessStatusIdAndIsEditedByApprover } from '../../shared/approval-process-status-id-and-is-edited-by-approver.interface';
import { ApprovalProcessStatus } from '../../shared/approval-process-status.enum';

// Adding this because, for some reason,
// the template using this pipe fails to infer the proper type from the generic...
// The whole point was so that we'd have inference in the template -_-
interface IApprovalProcessStatusIdAndIsEditedByApprover_Plus extends IApprovalProcessStatusIdAndIsEditedByApprover {
    [key: string]: any;
}

@Pipe({
  name: 'status',
  pure: true
})
export class StatusPipe implements PipeTransform {

  transform<T extends IApprovalProcessStatusIdAndIsEditedByApprover_Plus>(data: Array<T>, apId: ApprovalProcessStatus, isEdited: boolean): Array<T> {

    return data.filter(item => {

      if (apId === ApprovalProcessStatus.Null) {
        return (item.approvalProcessStatusId == ApprovalProcessStatus.Rejected
             || item.approvalProcessStatusId == null
             || item.approvalProcessStatusId == ApprovalProcessStatus.Null
        );
      }

      return (item.approvalProcessStatusId == apId && item.isEditedByApprover == isEdited);

    });
  }

}

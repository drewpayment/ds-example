import { IRemark } from './../../../../../lib/ds/core/src/lib/shared/remark.model';

export interface IClientNotes {
    clientNoteID: number;
    clientID: number;
    clientNoteSubjectID: ClientNoteSubjectType;
    remarkID: number;
    remark: IRemark;

}

export enum ClientNoteSubjectType {
    setUp = 1,
    taxIssue = 2,
    labourSourceIssue = 3,
    paySourceIssue = 4,
    generalCustomerService = 5,
    sales = 6,
    applicantTrackingIssues = 7,
    clientContact = 8,
    clientFeature = 9,
}

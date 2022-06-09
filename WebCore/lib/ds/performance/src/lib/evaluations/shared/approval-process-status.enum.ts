
export enum ApprovalProcessStatus {
    Null = -1, // "Fake" status, only used in the angular templates for filtering purposes.
    Rejected = 0,
    Approved,
    ApprovedWithEdits,
}

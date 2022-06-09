import { ProposalApprovalStatusPipe } from './proposal-approval-status.pipe';

describe('ProposalApprovalStatusPipe', () => {
  it('create an instance', () => {
    const pipe = new ProposalApprovalStatusPipe();
    expect(pipe).toBeTruthy();
  });
});

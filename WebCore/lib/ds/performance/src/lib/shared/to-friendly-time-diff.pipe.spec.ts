import { ToFriendlyTimeDiffPipe } from './to-friendly-time-diff.pipe';

describe('ToFriendlyTimeDiffPipe', () => {
  it('create an instance', () => {
    const pipe = new ToFriendlyTimeDiffPipe();
    expect(pipe).toBeTruthy();
  });
});

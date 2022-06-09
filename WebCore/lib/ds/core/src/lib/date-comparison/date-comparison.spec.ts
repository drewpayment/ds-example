import { CompareDatePipe } from './convert-and-use-handler.pipe';

describe('ConvertAndUseHandlerPipe', () => {
  it('create an instance', () => {
    const pipe = new CompareDatePipe();
    expect(pipe).toBeTruthy();
  });
});

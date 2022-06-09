import { ConvertAndUseHandlerPipe } from './ds/core/src/lib/pipes/convert-and-use-handler.pipe';

describe('ConvertAndUseHandlerPipe', () => {
  it('create an instance', () => {
    const pipe = new ConvertAndUseHandlerPipe();
    expect(pipe).toBeTruthy();
  });
});

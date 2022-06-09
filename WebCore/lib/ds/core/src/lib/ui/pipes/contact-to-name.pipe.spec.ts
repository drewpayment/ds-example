import { ContactToNamePipe } from './contact-to-name.pipe';

describe('ContactToNamePipe', () => {
  it('create an instance', () => {
    const pipe = new ContactToNamePipe();
    expect(pipe).toBeTruthy();
  });
});

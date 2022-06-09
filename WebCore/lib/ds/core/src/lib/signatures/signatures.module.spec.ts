import { DsSignaturesModule } from './signatures.module';

describe('SignaturesModule', () => {
    let signaturesModule: DsSignaturesModule;

    beforeEach(() => {
        signaturesModule = new DsSignaturesModule();
    });

    it('should create an instance', () => {
        expect(signaturesModule).toBeTruthy();
    });
});

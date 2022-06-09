import { DsCoreContactsModule } from './contacts.module';

describe('ContactsModule', () => {
    let contactsModule: DsCoreContactsModule;

    beforeEach(() => {
        contactsModule = new DsCoreContactsModule();
    });

    it('should create an instance', () => {
        expect(contactsModule).toBeTruthy();
    });
});

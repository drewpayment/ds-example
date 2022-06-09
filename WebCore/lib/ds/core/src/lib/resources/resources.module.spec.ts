import { DsCoreResourcesModule } from './resources.module';

describe('ResourcesModule', () => {
    let resourcesModule: DsCoreResourcesModule;

    beforeEach(() => {
        resourcesModule = new DsCoreResourcesModule();
    });

    it('should create an instance', () => {
        expect(resourcesModule).toBeTruthy();
    });
});

import { TestBed } from '@angular/core/testing';

import { PerformanceManagerService } from './performance-manager.service';

describe('PerformanceManagerService', () => {
    beforeEach(() => TestBed.configureTestingModule({}));

    it('should be created', () => {
        const service: PerformanceManagerService = TestBed.get(PerformanceManagerService);
        expect(service).toBeTruthy();
    });
});

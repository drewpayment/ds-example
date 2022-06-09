import { TestBed } from '@angular/core/testing';

import { LeaveManagementApiService } from './leave-management-api.service';

describe('LeaveManagementApiService', () => {
    beforeEach(() => TestBed.configureTestingModule({}));

    it('should be created', () => {
        const service: LeaveManagementApiService = TestBed.get(LeaveManagementApiService);
        expect(service).toBeTruthy();
    });
});

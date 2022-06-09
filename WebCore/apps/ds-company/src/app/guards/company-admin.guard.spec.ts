import { TestBed, async, inject } from '@angular/core/testing';

import { CompanyAdminGuard } from './company-admin.guard';

describe('CompanyAdminGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CompanyAdminGuard]
    });
  });

  it('should ...', inject([CompanyAdminGuard], (guard: CompanyAdminGuard) => {
    expect(guard).toBeTruthy();
  }));
});

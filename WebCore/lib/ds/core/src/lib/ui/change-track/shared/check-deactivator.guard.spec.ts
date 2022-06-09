import { TestBed, async, inject } from '@angular/core/testing';

import { CheckDeactivatorGuard } from './check-deactivator.guard';

describe('CheckDeactivatorGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CheckDeactivatorGuard]
    });
  });

  it('should ...', inject([CheckDeactivatorGuard], (guard: CheckDeactivatorGuard) => {
    expect(guard).toBeTruthy();
  }));
});

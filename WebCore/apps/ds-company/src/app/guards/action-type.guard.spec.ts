import { TestBed, async, inject } from '@angular/core/testing';

import { ActionTypeGuard } from './action-type.guard';

describe('ActionTypeGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ActionTypeGuard]
    });
  });

  it('should ...', inject([ActionTypeGuard], (guard: ActionTypeGuard) => {
    expect(guard).toBeTruthy();
  }));
});

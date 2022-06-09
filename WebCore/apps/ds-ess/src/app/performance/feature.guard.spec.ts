import { TestBed, async, inject } from '@angular/core/testing';

import { FeatureGuard } from './feature.guard';

describe('FeatureGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FeatureGuard]
    });
  });

  it('should ...', inject([FeatureGuard], (guard: FeatureGuard) => {
    expect(guard).toBeTruthy();
  }));
});

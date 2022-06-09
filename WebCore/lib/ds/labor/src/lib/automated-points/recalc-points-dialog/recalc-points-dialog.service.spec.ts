import { TestBed } from '@angular/core/testing';

import { RecalcPointsDialogService } from './recalc-points-dialog.service';

describe('RecalcPointsDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RecalcPointsDialogService = TestBed.get(RecalcPointsDialogService);
    expect(service).toBeTruthy();
  });
});

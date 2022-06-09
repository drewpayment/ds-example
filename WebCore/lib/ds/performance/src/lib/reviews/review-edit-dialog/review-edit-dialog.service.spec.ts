import { TestBed } from '@angular/core/testing';

import { ReviewEditDialogService } from './review-edit-dialog.service';

describe('ReviewEditDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReviewEditDialogService = TestBed.get(ReviewEditDialogService);
    expect(service).toBeTruthy();
  });
});

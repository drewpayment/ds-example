import { TestBed } from '@angular/core/testing';

import { CopyPlansDialogService } from './copy-plans-dialog.service';

describe('CopyPlansDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CopyPlansDialogService = TestBed.get(CopyPlansDialogService);
    expect(service).toBeTruthy();
  });
});

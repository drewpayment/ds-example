import { TestBed } from '@angular/core/testing';

import { AppResourceDialogService } from './app-resource-dialog.service';

describe('AppResourceDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AppResourceDialogService = TestBed.get(AppResourceDialogService);
    expect(service).toBeTruthy();
  });
});

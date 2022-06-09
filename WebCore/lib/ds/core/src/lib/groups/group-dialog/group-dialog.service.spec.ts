import { TestBed } from '@angular/core/testing';

import { GroupDialogService } from './group-dialog.service';

describe('GroupDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GroupDialogService = TestBed.get(GroupDialogService);
    expect(service).toBeTruthy();
  });
});

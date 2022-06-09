import { TestBed } from '@angular/core/testing';

import { ChangeTrackerService } from './change-tracker.service';

describe('ChangeTrackerService', () => {
  let service: ChangeTrackerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChangeTrackerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { DeactivatorService } from './deactivator.service';

describe('DeactivatorService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DeactivatorService = TestBed.get(DeactivatorService);
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { EmpEeocService } from './emp-eeoc.service';

describe('EmpEeocService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EmpEeocService = TestBed.get(EmpEeocService);
    expect(service).toBeTruthy();
  });
});

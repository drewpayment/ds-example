import { TestBed } from '@angular/core/testing';

import { MeritIncreaseService } from './merit-increase.service';

describe('MeritIncreaseService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MeritIncreaseService = TestBed.get(MeritIncreaseService);
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { DsCardService } from './ds-card.service';

describe('DsCardService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DsCardService = TestBed.get(DsCardService);
    expect(service).toBeTruthy();
  });
});

import { TestBed, inject } from '@angular/core/testing';

import { DsExpansionService } from './ds-expansion.service';

describe('DsExpansionService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DsExpansionService]
    });
  });

  it('should be created', inject([DsExpansionService], (service: DsExpansionService) => {
    expect(service).toBeTruthy();
  }));
});

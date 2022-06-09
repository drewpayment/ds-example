import { TestBed } from '@angular/core/testing';

import { TermsAndConditionsModalService } from './terms-and-conditions-modal.service';

describe('TermsAndConditionsModalService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TermsAndConditionsModalService = TestBed.get(TermsAndConditionsModalService);
    expect(service).toBeTruthy();
  });
});

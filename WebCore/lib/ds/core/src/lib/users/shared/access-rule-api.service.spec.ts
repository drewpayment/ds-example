import { TestBed } from '@angular/core/testing';

import { AccessRuleApiService } from './access-rule-api.service';

describe('AccessRuleApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AccessRuleApiService = TestBed.get(AccessRuleApiService);
    expect(service).toBeTruthy();
  });
});

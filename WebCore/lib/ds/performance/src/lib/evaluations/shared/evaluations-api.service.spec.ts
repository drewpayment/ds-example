import { TestBed } from '@angular/core/testing';

import { EvaluationsApiService } from './evaluations-api.service';

describe('EvaluationsApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EvaluationsApiService = TestBed.get(EvaluationsApiService);
    expect(service).toBeTruthy();
  });
});

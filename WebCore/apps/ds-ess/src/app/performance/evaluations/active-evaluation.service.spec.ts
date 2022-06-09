import { TestBed } from '@angular/core/testing';

import { ActiveEvaluationService } from './active-evaluation.service';

describe('ActiveEvaluationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ActiveEvaluationService = TestBed.get(ActiveEvaluationService);
    expect(service).toBeTruthy();
  });
});

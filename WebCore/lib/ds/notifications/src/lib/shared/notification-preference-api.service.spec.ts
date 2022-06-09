import { TestBed } from '@angular/core/testing';

import { NotificationPreferenceApiService } from './notification-preference-api.service';

describe('NotificationPreferenceApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: NotificationPreferenceApiService = TestBed.get(NotificationPreferenceApiService);
    expect(service).toBeTruthy();
  });
});

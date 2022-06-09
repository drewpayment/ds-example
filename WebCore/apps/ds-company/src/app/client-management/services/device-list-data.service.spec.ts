import { TestBed } from '@angular/core/testing';

import { DeviceListDataService } from './device-list-data.service';

describe('DeviceListDataService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DeviceListDataService = TestBed.get(DeviceListDataService);
    expect(service).toBeTruthy();
  });
});

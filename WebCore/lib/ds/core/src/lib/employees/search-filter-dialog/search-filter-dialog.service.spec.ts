import { TestBed } from '@angular/core/testing';

import { SearchFilterDialogService } from './search-filter-dialog.service';

describe('SearchFilterDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SearchFilterDialogService = TestBed.get(SearchFilterDialogService);
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { CookieMonsterService } from './cookie-monster.service';

describe('CookieMonsterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CookieMonsterService = TestBed.get(CookieMonsterService);
    expect(service).toBeTruthy();
  });
});

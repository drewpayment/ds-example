import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountSettingsOutletComponent } from './account-settings-outlet.component';

describe('AccountSettingsOutletComponent', () => {
  let component: AccountSettingsOutletComponent;
  let fixture: ComponentFixture<AccountSettingsOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountSettingsOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountSettingsOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientBankInfoSetupFormComponent } from './client-bank-info-setup-form.component';

describe('ClientBankInfoSetupFormComponent', () => {
  let component: ClientBankInfoSetupFormComponent;
  let fixture: ComponentFixture<ClientBankInfoSetupFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientBankInfoSetupFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientBankInfoSetupFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

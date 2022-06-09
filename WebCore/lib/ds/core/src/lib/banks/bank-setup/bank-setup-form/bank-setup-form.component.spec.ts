import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BankSetupFormComponent } from './bank-setup-form.component';

describe('BankSetupFormComponent', () => {
  let component: BankSetupFormComponent;
  let fixture: ComponentFixture<BankSetupFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BankSetupFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BankSetupFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

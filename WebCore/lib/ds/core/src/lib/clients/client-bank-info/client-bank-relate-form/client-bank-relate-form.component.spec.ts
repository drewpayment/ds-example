import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientBankRelateFormComponent } from './client-bank-relate-form.component';

describe('ClientBankRelateFormComponent', () => {
  let component: ClientBankRelateFormComponent;
  let fixture: ComponentFixture<ClientBankRelateFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientBankRelateFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientBankRelateFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

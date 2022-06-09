import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsFooterCardComponent } from './client-accruals-footer-card.component';

describe('ClientAccrualsFooterCardComponent', () => {
  let component: ClientAccrualsFooterCardComponent;
  let fixture: ComponentFixture<ClientAccrualsFooterCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAccrualsFooterCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAccrualsFooterCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsGeneralCardComponent } from './client-accruals-general-card.component';

describe('ClientAccrualsGeneralCardComponent', () => {
  let component: ClientAccrualsGeneralCardComponent;
  let fixture: ComponentFixture<ClientAccrualsGeneralCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAccrualsGeneralCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAccrualsGeneralCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

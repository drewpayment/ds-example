import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsDisplayCardComponent } from './client-accruals-display-card.component';

describe('ClientAccrualsDisplayCardComponent', () => {
  let component: ClientAccrualsDisplayCardComponent;
  let fixture: ComponentFixture<ClientAccrualsDisplayCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAccrualsDisplayCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAccrualsDisplayCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

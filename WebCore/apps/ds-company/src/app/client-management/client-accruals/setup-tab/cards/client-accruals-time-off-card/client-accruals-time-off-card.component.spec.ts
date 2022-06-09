import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsTimeOffCardComponent } from './client-accruals-time-off-card.component';

describe('ClientAccrualsTimeOffCardComponent', () => {
  let component: ClientAccrualsTimeOffCardComponent;
  let fixture: ComponentFixture<ClientAccrualsTimeOffCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAccrualsTimeOffCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAccrualsTimeOffCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

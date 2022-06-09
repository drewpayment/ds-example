import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsComputationCardComponent } from './client-accruals-computation-card.component';

describe('ClientAccrualsComputationCardComponent', () => {
  let component: ClientAccrualsComputationCardComponent;
  let fixture: ComponentFixture<ClientAccrualsComputationCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAccrualsComputationCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAccrualsComputationCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

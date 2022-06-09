import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckStockTriggerComponent } from './check-stock-trigger.component';

describe('CheckStockTriggerComponent', () => {
  let component: CheckStockTriggerComponent;
  let fixture: ComponentFixture<CheckStockTriggerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckStockTriggerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckStockTriggerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

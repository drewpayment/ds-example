import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateBalanceTriggerComponent } from './update-balance-trigger.component';

describe('UpdateBalanceTriggerComponent', () => {
  let component: UpdateBalanceTriggerComponent;
  let fixture: ComponentFixture<UpdateBalanceTriggerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateBalanceTriggerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateBalanceTriggerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

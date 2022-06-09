import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CopyPlansTriggerComponent } from './copy-plans-trigger.component';

describe('CopyPlansTriggerComponent', () => {
  let component: CopyPlansTriggerComponent;
  let fixture: ComponentFixture<CopyPlansTriggerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CopyPlansTriggerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CopyPlansTriggerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

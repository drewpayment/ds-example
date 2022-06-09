import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeClockHardwareComponent } from './time-clock-hardware.component';

describe('TimeClockHardwareComponent', () => {
  let component: TimeClockHardwareComponent;
  let fixture: ComponentFixture<TimeClockHardwareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimeClockHardwareComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeClockHardwareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

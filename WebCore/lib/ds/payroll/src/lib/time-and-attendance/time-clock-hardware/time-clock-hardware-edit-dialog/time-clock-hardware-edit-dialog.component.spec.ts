import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeClockHardwareEditDialogComponent } from './time-clock-hardware-edit-dialog.component';

describe('TimeClockHardwareEditDialogComponent', () => {
  let component: TimeClockHardwareEditDialogComponent;
  let fixture: ComponentFixture<TimeClockHardwareEditDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimeClockHardwareEditDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeClockHardwareEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarChipIconComponent } from './calendar-chip-icon.component';

describe('CalendarChipIconComponent', () => {
  let component: CalendarChipIconComponent;
  let fixture: ComponentFixture<CalendarChipIconComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalendarChipIconComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarChipIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

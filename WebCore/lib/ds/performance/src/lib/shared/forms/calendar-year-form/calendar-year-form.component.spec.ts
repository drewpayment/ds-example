import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarYearFormComponent } from './calendar-year-form.component';

describe('CalendarYearFormComponent', () => {
  let component: CalendarYearFormComponent;
  let fixture: ComponentFixture<CalendarYearFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalendarYearFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarYearFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

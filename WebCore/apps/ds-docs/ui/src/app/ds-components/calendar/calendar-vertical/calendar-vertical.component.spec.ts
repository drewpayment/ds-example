import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarVerticalComponent } from './calendar-vertical.component';

describe('CalendarVerticalComponent', () => {
  let component: CalendarVerticalComponent;
  let fixture: ComponentFixture<CalendarVerticalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalendarVerticalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarVerticalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarDocsComponent } from './calendar-docs.component';

describe('CalendarDocsComponent', () => {
  let component: CalendarDocsComponent;
  let fixture: ComponentFixture<CalendarDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalendarDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

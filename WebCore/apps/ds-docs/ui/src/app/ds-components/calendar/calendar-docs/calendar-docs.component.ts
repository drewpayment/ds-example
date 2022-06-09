import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-calendar-docs',
  templateUrl: './calendar-docs.component.html',
  styleUrls: ['./calendar-docs.component.scss']
})
export class CalendarDocsComponent implements OnInit {

  toggleCalendar = false;
  toggleVerticalCalendar = false
  toggleCalendarChip = false
  toggleCalendarChipIcon = false
  constructor() { }

  ngOnInit() {
  }

}

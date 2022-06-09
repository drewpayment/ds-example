import { Component, OnInit, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { IFormItemComponent } from '../calendar-year-form/calendar-year-form.model';
import { MeritIncreaseComp } from '../calendar-year-form/calendar-year-form.component';

@Component({
  selector: 'ds-merit-increase',
  templateUrl: './merit-increase.component.html',
  styleUrls: ['./merit-increase.component.scss']
})
export class MeritIncreaseComponent implements OnInit, IFormItemComponent<MeritIncreaseComp> {
  @Input()
  data: MeritIncreaseComp;

  constructor() { }

  ngOnInit() {
  }

}

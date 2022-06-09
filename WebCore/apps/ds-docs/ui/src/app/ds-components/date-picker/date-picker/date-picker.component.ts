import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import * as moment from 'moment';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.scss']
})
export class DatePickerComponent implements OnInit {
  @Input() data; //initial value
  @Input() ctrl: string; //user control html element
  @Input() enabled; //whether or not its enabled
  events: string[] = [];
  public form: FormGroup;
  dateSelected: string;
  txtDate: HTMLInputElement;
  date1: FormControl;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    //Converts the 'enabled' input from a string to a Boolean, sets the disabled property of matInput
    this.enabled = !(this.enabled === 'true' || this.enabled === 'True' || this.enabled === true) //matInput takes disabled property !enabled = disabled

    //Converts the 'ctrl' input from a string to an HTML reference to the User Control's rendered textbox
    this.txtDate = <HTMLInputElement>document.getElementById(this.ctrl);

    //Converts the 'data' input from a string to a Date and creates a FormControl with the value as that Date. The disabled property is set here
    this.date1 = new FormControl({value: new Date(this.data), disabled: this.enabled})
  }

  //Called on (dateChange) on the matInput. The input is a MatDatePickerInputEvent which is a ‘moment’.
  changeDateSelected(event: MatDatepickerInputEvent<Date>) {
    //Check to see if this is from AngularMaterialDateBox.ascx user control
    if (this.txtDate != null){
      //Format the date
      this.dateSelected = moment(event.value).format('MM/DD/YYYY');
      //Set the value of the asp:TextBox on the user control
      this.txtDate.setAttribute("value", this.dateSelected);
      this.txtDate.focus();
      //force input validation to re-validate
      if($ && $.fn.validator) {
        $(this.txtDate).trigger("input");
      }
    }
  }

}

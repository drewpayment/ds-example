import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import * as moment from 'moment';

@Component({
  selector: 'ds-angular-material-datebox',
  templateUrl: './angular-material-datebox.component.html',
  styleUrls: ['./angular-material-datebox.component.scss']
})
export class AngularMaterialDateboxComponent implements OnInit {
  @Input() data; //initial value
  @Input() ctrl: string; //user control html element
  @Input() enabled; //whether or not its enabled
  @Input() max; //end date
  @Input() min; //start date
  @Input() required; //whether or not a date is required
  @Input() button;
  events: string[] = [];
  public form: FormGroup;
  dateSelected: string;
  txtDate: HTMLInputElement; //reference to the user control's textbox that legacy pages use to find the value
  date1: FormControl;

  constructor() {}

  ngOnInit() {
    this.required = (this.required === 'true' || this.required === 'True' || this.required === true) //convert to boolean in case of string input
    this.enabled = !(this.enabled === 'true' || this.enabled === 'True' || this.enabled === true) //matInput takes disabled property !enabled = disabled
    this.txtDate = <HTMLInputElement>document.getElementById(this.ctrl);
    this.date1 = new FormControl({value: new Date(this.data), disabled: this.enabled})
    if (this.max != "") this.watchEndDate(); //If an End Date id is provided, watch for changes
    if (this.min != "") this.watchStartDate(); //If a Start Date id is provided, watch for changes
  }

  changeDateSelected(event: MatDatepickerInputEvent<Date>) {
    //Check to see if this is from AngularMaterialDateBox.ascx user control
    if (this.txtDate != null){
        if ( event.value ) this.dateSelected = moment(event.value).format('MM/DD/YYYY');
        if ( moment(this.dateSelected, 'MM/DD/YYYY').isValid() ) this.removeError();
        this.txtDate.setAttribute("value", this.dateSelected);
        this.txtDate.value = this.dateSelected;
        this.txtDate.click();
        if ( this.txtDate.value == null || this.txtDate.value == "undefined" ) this.txtDate.value = ""; // Reset txtDate control to nothing if date is cleared out
    }
  }

  blurEvent(event:any){
    let value = event.target.value;
    //empty validation
    if ( value == "" && this.required ){
      this.showError("Please enter a date.");
    }
    //invalid date validation
    else if ( !moment(value, 'MM/DD/YYYY' ).isValid() && value != "" ){
      this.showError("Please enter a valid date.");
    }
    //start date validation
    else if ( moment(this.max, 'MM/DD/YYYY').isValid() ) {
      ( new Date(value) > this.max ) ? this.showError("Please enter a date before the end date.") : this.removeError();
    }
    //end date validation
    else if ( moment(this.min, 'MM/DD/YYYY').isValid() ) {
      ( new Date(value) < this.min ) ? this.showError("Please enter a date after the start date.") : this.removeError();
    }
    //no errors
    else {
      this.removeError();
    }
  }


  watchStartDate() {
    const startDate = <HTMLInputElement>document.getElementById(this.min);
    if ( startDate ) {
      if ( startDate.value ) this.min = new Date(startDate.value);
      const minObserver = new MutationObserver((mutations) => {
        mutations.forEach((mutation:any) => this.min = new Date(mutation.target.value));
      });
      minObserver.observe(startDate, {
        attributes: true,
        childList: true,
        characterData: true
      });
    }
  }

  watchEndDate() {
    const endDate = <HTMLInputElement>document.getElementById(this.max);
    if ( endDate ) {
      if ( endDate.value ) this.max = new Date(endDate.value);
      const maxObserver = new MutationObserver((mutations) => {
        mutations.forEach((mutation:any) => this.max = new Date(mutation.target.value));
      });
      maxObserver.observe(endDate, {
        attributes: true,
        childList: true,
        characterData: true
      });
    }
  }

  showError(errorMessage:string) {
    let b = document.getElementById(this.button);
    if ( b ) {
      b.classList.add("disabled");
      b.setAttribute("disabled", "disabled");
    }
    this.txtDate.classList.add("is-invalid")

    let error = document.getElementById(this.txtDate.id + '_error');
    if ( !error ) {
      let fg = this.txtDate.closest(".form-group");
      if ( fg ) {
        fg.classList.add("has-error","has-danger", "date-picker-error");

        let msg = fg.querySelector("div.invalid-feedback");
        msg.innerHTML = errorMessage;
      }
    }
  }

  removeError() {
    let fg = this.txtDate.closest(".form-group");
    if ( fg ) fg.classList.remove("has-error","has-danger");

    let anyError = document.querySelectorAll(".form-group.has-danger.has-error.date-picker-error");
    if ( !anyError.length ) {
      let b = document.getElementById(this.button);
      if ( b )  {
        b.classList.remove("disabled");
        b.removeAttribute("disabled");
      }
    }
  }
}

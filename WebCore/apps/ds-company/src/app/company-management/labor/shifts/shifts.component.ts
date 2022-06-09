import { Component, OnInit } from '@angular/core';
import { CompanyManagementService } from '../../../services/company-management.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { IClientShift } from '@ajs/client/models/client-shift.model';
import { HttpErrorResponse } from '@angular/common/http';
import * as moment from 'moment';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from '@ds/core/shared';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { map, tap } from 'rxjs/operators';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';


@Component({
  selector: 'ds-shifts',
  templateUrl: './shifts.component.html',
  styleUrls: ['./shifts.component.scss']
})
export class ShiftsComponent implements OnInit {
    isSaving = false;
    loaded: boolean = false;
    user: UserInfo;
    isSubmitted = false;
    form: FormGroup;
    shifts: IClientShift[];
    blockedShifts: number[];
    earnings: any;
    inactiveEarnings: any;
    selectedShift: IClientShift;
    showDelete = false;
    confirmed: any;
    startTimePeriodIsInvalid: boolean;
    stopTimePeriodIsInvalid: boolean;
    hasAutoShiftByHrsWorkedOption: false;
    get shift() { return this.form.get('shift') as FormControl };
    get name() { return this.form.get('name') as FormControl };
    get computationMethod() { return this.form.get('additionalAmountTypeId') as FormControl };
    get additionalAmount() { return this.form.get('additionalAmount') as FormControl };
    get additionalPremiumAmount() { return this.form.get('additionalPremiumAmount') as FormControl };
    get limit() { return this.form.get('limit') as FormControl };
    get destination() { return this.form.get('destination') as FormControl };
    get startTolerance() { return this.form.get('shiftStartTolerance') as FormControl };
    get endTolerance() { return this.form.get('shiftStopTolerance') as FormControl };
    get startTime() { return this.form.get('startTime') as FormControl };
    get stopTime() { return this.form.get('stopTime') as FormControl };

    constructor(
        private fb: FormBuilder,
        private ngxMsgSvc: NgxMessageService,
        private CompanyManagementService: CompanyManagementService,
        private accountService: AccountService,
        private confirmDialog: ConfirmDialogService,
        private _currencyPipe: CurrencyPipe,
        private _decimalPipe: DecimalPipe

    ) { }

    ngOnInit() {
      this.showDelete = false;
      this.accountService.getUserInfo()
        .pipe(
          tap(u => this.user = u)).subscribe(() => {
            this.getClientShifts();
            this.initForm();
          });
        
    }

    getClientShifts(){
      this.CompanyManagementService.GetCompanyShiftInformation().subscribe((data:any) => {
        this.earnings = this.checkActiveEarnings(data.earnings);
        this.shifts = data.shifts;
        this.hasAutoShiftByHrsWorkedOption = data.hasAutoShiftByHrsWorkedOption;
        this.blockedShifts = data.blockedShifts;
        
        this.loaded = true;
      });
    }

    checkActiveEarnings(earnings) {
      this.inactiveEarnings = [];
      for ( let i = earnings.length -1; i >= 0; --i ) {
        if (earnings[i].isActive == false) {
          earnings[i].description = earnings[i].description + " -- inactive";
          this.inactiveEarnings.push(earnings[i]); //make an array of inactive earning ID's to compare against on save
          earnings.splice(i, 1);
        }
      }
      return earnings;
    }

    initForm() {
      this.form = this.fb.group({
        additionalAmount: [null, [Validators.required, Validators.pattern('^\\d*(,\\d+)*\\.?[0-9]+$')]], // Amount
        additionalAmountTypeId: [1], // Computation Method
        additionalPremiumAmount: [null, [Validators.pattern('^\\d*(,\\d+)*\\.?[0-9]+$')]], // Amount By Earning
        clientId: this.user.clientId,
        clientShiftId: [0], // Shift
        description: [null, Validators.required], // Name
        destination: [0],
        isFriday: true,
        isMonday: true,
        isSaturday: true,
        isSunday: true,
        isThursday: true,
        isTuesday: true,
        isWednesday: true,
        limit: [null, [Validators.pattern('[0-9]*')]],
        shiftEndTolerance: [null, [Validators.pattern('^\d*(\.\d+)?$')]],
        shiftStartTolerance: [null, [Validators.pattern('^\d*(\:\d+)?$')]],
        startTime: [null,[Validators.pattern(/^([0-2]?[0-9])?:?[0-5][0-9]$/)]],
        stopTime: [null, [Validators.pattern(/^([0-2]?[0-9])?:?[0-5][0-9]$/)]],
        startTimePeriod: [null],
        stopTimePeriod: [null]
      });
      this.showDelete = false;
    }

    changeShift() {
        this.isSubmitted = false;
        this.selectedShift = this.shifts.find(s => s.clientShiftId == this.form.controls['clientShiftId'].value);

        if ( this.selectedShift ) {
            let additionalAmountFormatted;
            let additionalPremiumAmountFormatted;
            let startToleranceFormatted;
            let stopToleranceFormatted;
            this.showDelete = true;

            //Hide/Show Delete
            if (this.blockedShifts.some(x => x == this.selectedShift.clientShiftId)) this.showDelete = false;

            //Disable or Enable 'Add Into Rate'
            this.computationMethodChanged(this.selectedShift.additionalAmountTypeId);

            //Show inactive Destination if selected
            let active = this.earnings.some( e => e.clientEarningId === this.selectedShift.destination);
            let inactive = this.inactiveEarnings.filter(e => e.clientEarningId === this.selectedShift.destination);
            if ( inactive.length > 0 && !active) this.earnings.push(inactive[0]); //Add inactive to this.earnings in order to show selected option
            if ( inactive.length == 0 ) this.earnings = this.earnings.filter(e => !e.description.includes(" -- inactive")) //Remove any inactives added if selected is active

            // Format Amount
            additionalAmountFormatted = this._currencyPipe.transform( this.selectedShift.additionalAmount , 'USD', '', '1.4' );
            this.selectedShift.additionalAmount = additionalAmountFormatted;

            // Format Amount by Earning
            additionalPremiumAmountFormatted = this._currencyPipe.transform( this.selectedShift.additionalPremiumAmount , 'USD', '', '1.4' );
            this.selectedShift.additionalPremiumAmount = additionalPremiumAmountFormatted;

            // Format Tolerance 
            startToleranceFormatted = this._decimalPipe.transform( this.selectedShift.shiftStartTolerance, '1.2-2' ); 
            this.selectedShift.shiftStartTolerance = startToleranceFormatted;

            stopToleranceFormatted = this._decimalPipe.transform( this.selectedShift.shiftEndTolerance, '1.2-2' ); 
            this.selectedShift.shiftEndTolerance = stopToleranceFormatted;

            //Set the form values to the selected shift's values
            this.form.reset({
                ... this.form.value,
                additionalAmount:  this.selectedShift.additionalAmount,
                additionalAmountTypeId: this.selectedShift.additionalAmountTypeId,
                additionalPremiumAmount: this.selectedShift.additionalPremiumAmount,
                description: this.selectedShift.description,
                destination: this.selectedShift.destination,
                isFriday: this.selectedShift.isFriday,
                isMonday: this.selectedShift.isMonday,
                isSaturday: this.selectedShift.isSaturday,
                isSunday: this.selectedShift.isSunday,
                isThursday: this.selectedShift.isThursday,
                isTuesday: this.selectedShift.isTuesday,
                isWednesday: this.selectedShift.isWednesday,
                limit: this.selectedShift.limit,
                shiftEndTolerance: this.selectedShift.shiftEndTolerance,
                shiftStartTolerance: this.selectedShift.shiftStartTolerance,
                startTime: this.selectedShift.startTime ? moment.utc(this.selectedShift.startTime as Date).format('hh:mm') : null,
                stopTime: this.selectedShift.stopTime ? moment.utc(this.selectedShift.stopTime as Date).format('hh:mm') : null,
                startTimePeriod: this.selectedShift.startTime ? moment.utc(this.selectedShift.startTime as Date).format('a').toUpperCase() : null,
                stopTimePeriod: this.selectedShift.stopTime ? moment.utc(this.selectedShift.stopTime as Date).format('a').toUpperCase() : null
            });

            // Prevents change track from being called when only the shifts dropdown is dirty. 
            //this.form.markAsPristine;
        } else {
            this.initForm();
        }
    }

    save() {
        this.isSaving = true;
        this.isSubmitted = true;
        this.form.markAllAsTouched();
        this.checkPeriodOnSave();
        
        if ( this.form.errors || this.startTimePeriodIsInvalid || this.stopTimePeriodIsInvalid ) {
          this.isSaving = false;
          return;
        }

        //Check if Destination is active
        let inactive = this.inactiveEarnings.some(e => e.clientEarningId === this.form.value.destination);
        if ( inactive ){
          this.destination.setErrors({'inactive': true});
          this.isSaving = false;
          return;
        }

        //Check if name exists
        let duplicate = this.shifts.find(s => s.description === this.form.value.description);
        if ( duplicate && duplicate.clientShiftId != this.form.value.clientShiftId) {
          this.ngxMsgSvc.setErrorMessage("The specified shift name already exists. Changes were not saved.");
          this.isSaving = false;
          return;
        }

        
        //Set model for save
        // Reformat dates back to API format
        if (this.selectedShift) this.selectedShift.isDateFormatted = false;

        this.selectedShift = {
          additionalAmount: this.form.value.additionalAmount,
          additionalAmountTypeId: this.form.value.additionalAmountTypeId,
          additionalPremiumAmount: this.form.value.additionalPremiumAmount,
          clientId: this.user.clientId, 
          clientShiftId: this.form.value.clientShiftId,
          description: this.form.value.description,
          destination: this.form.value.destination,
          isFriday: this.form.value.isFriday,
          isMonday: this.form.value.isMonday,
          isSaturday: this.form.value.isSaturday,
          isSunday: this.form.value.isSunday,
          isThursday: this.form.value.isThursday,
          isTuesday: this.form.value.isTuesday,
          isWednesday: this.form.value.isWednesday,
          limit: this.form.value.limit,
          shiftEndTolerance: this.form.value.shiftEndTolerance,
          shiftStartTolerance: this.form.value.shiftStartTolerance,
          startTime: this.getDateFromTime(this.form.value.startTime, this.form.value.startTimePeriod),
          stopTime: this.getDateFromTime(this.form.value.stopTime, this.form.value.stopTimePeriod)
        }

        //Save
        this.CompanyManagementService.SaveCompanyShiftInformation(this.selectedShift).subscribe((response: IClientShift) => {
          this.ngxMsgSvc.setSuccessMessage("Shift Saved Successfully");
          this.isSubmitted = false;
          this.isSaving = false;

          if ( response ) {
            let index = this.shifts.map(s => s.clientShiftId).indexOf(response.clientShiftId);
            if ( index == -1 ) {
              this.shifts.push(response); //insert
            }
            else{
              this.shifts[index] = response; //update
            }
            this.form.controls['clientShiftId'].patchValue(response.clientShiftId);
            this.changeShift();
          }
        }, (error: HttpErrorResponse) => {
          this.isSaving = false;
          this.ngxMsgSvc.setErrorResponse(error);
        });
    }

    deleteClientShift(confirmed) {
        if ( confirmed ) {
          this.CompanyManagementService.DeleteCompanyShiftInformation(this.selectedShift).subscribe((res) => {
            this.ngxMsgSvc.setSuccessMessage("Shift Deleted Successfully");
            this.shifts = this.shifts.filter((shift) => shift.clientShiftId != this.selectedShift.clientShiftId); //remove from shifts array
            this.initForm(); //clear form
          },
          (error: HttpErrorResponse) => {
            this.ngxMsgSvc.setErrorResponse(error);
          });
        }
    }

    deleteShift() {
        const options = {
            title: 'Are you sure you want to delete this shift?',
            confirm: "Delete"
        };
        this.confirmDialog.open(options);
        this.confirmDialog.confirmed().pipe(
            map(confirmation => this.confirmed = confirmation)
        ).subscribe(() => {
            this.deleteClientShift(this.confirmed);
        });
    }

    // Set start time am/pm
    startTimePeriod(period: string) {
      this.form.patchValue({
        startTimePeriod: period.toUpperCase()
      });
      this.startTimePeriodIsInvalid = false;
    }

    // Set stop time am/pm
    stopTimePeriod(period: string) {
      this.form.patchValue({
        stopTimePeriod: period.toUpperCase()
      });
      this.stopTimePeriodIsInvalid = false;
    }

    // check if period is set to prevent users from forgetting, saving all shifts as am
    checkPeriodOnSave() {
      this.startTimePeriodIsInvalid = false;
      this.stopTimePeriodIsInvalid = false;
      if ( this.form.value.startTime && !this.form.value.startTimePeriod ) this.startTimePeriodIsInvalid = true;
      if ( this.form.value.stopTime && !this.form.value.stopTimePeriod ) this.stopTimePeriodIsInvalid = true;
    }

    computationMethodChanged(additionalAmountTypeId){
      if (additionalAmountTypeId == 3)
      {
        document.getElementById('addIntoRate').setAttribute('disabled', 'true');
        this.form.controls['destination'].patchValue(this.earnings[0].clientEarningId);

      }
      else{
        document.getElementById('addIntoRate').removeAttribute('disabled')
      }
    }

    getDateFromTime(initialTimeValue: string, shiftTime: string) {
      if ( !initialTimeValue ) return; //return null if no time selected 
      if ((+initialTimeValue.slice(0,2)) > 12)
        return moment.utc(initialTimeValue, 'HH:mm', true).toDate();
      return moment.utc(initialTimeValue + ' ' + shiftTime, 'hh:mm a', true).toDate();
    }

    formatTimeOffFocus(time: string, shift: string ) {
      let formattedTimeLength = time ? time.length : 0;

      if ( 
        time && formattedTimeLength == 5 && time.includes(":") ||
        time && formattedTimeLength <= 4 && formattedTimeLength != 0 ) {

        /*
        * Check if the user added a colon or it it's already been formatted. 
        * Remove the colon if it exists and reset formattedTimeLength
        */
        if ( time.includes(":") ) {
          time = time.replace(":","");
          formattedTimeLength = time.length;
        }

        /* Create a 4 digit value */
        if ( formattedTimeLength <= 2 ) {
          if ( formattedTimeLength == 1 ) time = "0" + time;
          time =  time + "00";

        } else if ( formattedTimeLength == 3 ) {
          let hours = (time.slice(0,2));
          let doubleDigitsHour = false;

          if (hours == "10" || hours == "11" || hours == "12") doubleDigitsHour = true;
          
          if (doubleDigitsHour) {
            time = time.slice(0,2) + time.slice(2,3) + "0";
          } else {
            time = "0" + time.slice(0,1) + time.slice(1,3);
          } 
        }

        /* format to time with colon */
         time = time.slice(0,2) + ':' + time.slice(2,4);
    } else {
      /* reset to nothing if there are more numbers than hours and minutes */
      time = ""
    }

    if ( shift == "start" ) {
      this.form.patchValue({
          startTime: time,
      })
    }
    if ( shift == "end" ) {
        this.form.patchValue({
            stopTime: time,
        })
    }
  }
}

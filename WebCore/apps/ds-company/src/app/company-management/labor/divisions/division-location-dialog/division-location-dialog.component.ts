import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ICountry, IState } from '@ajs/applicantTracking/shared/models';
import { IClientContact } from '@ds/core/contacts';
import { CompanyManagementService } from '../../../../services/company-management.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'ds-division-location-dialog',
  templateUrl: './division-location-dialog.component.html',
  styleUrls: ['./division-location-dialog.component.scss']
})
export class DivisionLocationDialogComponent implements OnInit {
  contacts: IClientContact[];
  states: IState[];
  allStates: IState[];
  countries: ICountry[];
  form: FormGroup;
  isSubmitted = false;
  get zip() { return this.form.controls.zip as FormControl }
  constructor(
    private CompanyManagementService: CompanyManagementService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<DivisionLocationDialogComponent>,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
    this.initForm();
    this.countries = this.data.countries;
    this.states = this.data.states;
    this.allStates = this.data.states
    this.contacts = this.data.contacts;
  }

  initForm() {
    this.form = this.fb.group({
      contact: [this.data.selectedAddress.clientContactId, Validators.required], // Client Contacts
      name: [this.data.selectedAddress.name, Validators.required],
      country: [this.data.selectedAddress.countryId, Validators.required],
      address1: [this.data.selectedAddress.address1, Validators.required],
      address2: [this.data.selectedAddress.address2],
      city: [this.data.selectedAddress.city, Validators.required],
      state: [this.data.selectedAddress.stateId, Validators.required],
      zip: [this.data.selectedAddress.zip, Validators.required]
    })
  }
  close() {
    this.dialogRef.close();
  }

  save() {
    this.isSubmitted = true;
    this.form.markAllAsTouched();
    
    if ( this.form.invalid ) {
        return;
    }

    //Set model for save
    let model = {
      clientDivisionAddressId: this.data.selectedAddress.clientDivisionAddressId,
      clientDivisionId: this.data.selectedAddress.clientDivisionId,
      clientContactId: this.form.value.contact,
      name: this.form.value.name,
      address1: this.form.value.address1,
      address2: this.form.value.address2,
      city: this.form.value.city,
      stateId: this.form.value.state,
      zip: this.form.value.zip,
      countryId: this.form.value.country
    };

    //Save
    this.CompanyManagementService.SaveClientDivisionAddress(model).subscribe((response) => {
      this.dialogRef.close(response);
      this.isSubmitted = false;
    }, (error: HttpErrorResponse) => {
      this.dialogRef.close(error);
    });
  }

  changeCountry(){
    this.states = this.allStates.filter(x => x.countryId == this.form.controls['country'].value);
    if ( this.states.length <= 0) {
      this.form.controls['state'].disable();
      this.form.controls['state'].clearValidators();
    }
    else {
      this.form.controls['state'].enable();
      this.form.controls['state'].setValidators(Validators.required);
      this.form.controls['state'].updateValueAndValidity();
    }
  }

  setZipCodeValidation() {
      // If USA
      // this.zipCodePattern = /^\d{5}(-\d{4})?$/;
      
      // If Canada
      //  this.zipCodePattern = /^([ABCEGHJKLMNPRSTVXY]|[abceghjklmnprstvxy]){1}\d{1}([ABCEGHJKLMNPRSTVWXYZ]|[abceghjklmnprstvwxyz]){1}( |-){0,1}\d{1}([ABCEGHJKLMNPRSTVWXYZ]|[abceghjklmnprstvwxyz]){1}\d{1}$/;
    
      //this.zip.setValidators([Validators.pattern(this.zipCodePattern)]);
  }
}

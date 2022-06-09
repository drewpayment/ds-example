import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { UserInfo } from '@ds/core/shared';
import { IEmployeeContactInfo } from '@ajs/employee/models';
import { FormBuilder, FormGroup, Validators, FormArray, FormControl, AbstractControl } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EmployeeProfileService } from '../../shared/employee-profile-api.service';
import { ICountry } from '../../../common/shared/country.model';
import { IState } from '../../../common/shared/state.model';
import { ICounty } from '../../../common/shared/county.model';
import { EmployeeCommonService } from '../../../common/shared/employee-common-api.service';
import { DsCustomFilterCallbackPipe } from '@ds/core/shared/ds-custom-filter-callback.pipe';
import { Observable, Subject } from "rxjs";
import * as _ from 'lodash';
import * as moment from 'moment';
import { AccountService } from '@ds/core/account.service';

interface DialogData {
  user: UserInfo,
  employeeContactInfo: IEmployeeContactInfo,
  hasEditPermissions: boolean
}

@Component({
  selector: 'ds-employee-contact-info-form',
  templateUrl: './employee-contact-info-form.component.html',
  styleUrls: ['./employee-contact-info-form.component.scss']
})
export class EmployeeContactInfoFormComponent implements OnInit, AfterViewInit {
    user: UserInfo;
    employeeContactInfo: IEmployeeContactInfo;
    hasEditPermissions: boolean;
    countries: ICountry[];
    states: IState[];
    counties: ICounty[];
  f: FormGroup;
  formSubmitted: boolean;
  pageTitle: string;
  private _editMode: boolean;

  constructor(
    public dialogRef: MatDialogRef<EmployeeContactInfoFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private fb: FormBuilder,
    private accountService: AccountService,
    private commonService: EmployeeCommonService,
    private employeeProfileService: EmployeeProfileService
  ) { }

  ngOnInit(): void {
      this.user = this.data.user;
      this.hasEditPermissions = this.data.hasEditPermissions;
    if (!this.user)
        this.accountService.getUserInfo().subscribe(u => {
            this.user = u;
            this.initializePage();
        });
    else
        this.initializePage();
  }

  private initializePage() {
    this.employeeContactInfo = _.cloneDeep(this.data.employeeContactInfo) || this.createEmptyEmployeeContactInfo();
    this.pageTitle = "Edit Employee Contact Info";

    this.createForm();

      this.commonService.getCountries().subscribe(countries => {
          this.countries = countries;
          if (this.employeeContactInfo.countryId) {
              this.f.value.country = this.employeeContactInfo.countryId;
              this.commonService.getStatesByCountryId(this.employeeContactInfo.countryId).subscribe(states => {
                  this.states = states;
                  if (this.employeeContactInfo.stateId) {
                      this.f.value.state = this.employeeContactInfo.stateId;
                      if (this.employeeContactInfo.stateId) {
                          this.commonService.getCountiesByStateId(this.employeeContactInfo.stateId).subscribe(counties => {
                              this.counties = counties;
                              if (this.employeeContactInfo.countyId) {
                                  this.f.value.county = this.employeeContactInfo.countyId;
                              }
                          });
                      }
                  }
              });
          }
      });

  }

    loadStates(e) {
        this.f.controls['state'].setValue("");
        //this.f.value.state = "";
        this.states = [];
        if (e.target.value) {
            this.commonService.getStatesByCountryId(e.target.value).subscribe(states => {
                this.states = states;
            });
        }
    }

    loadCounties(e) {
        this.f.controls['county'].setValue("");
        //this.f.value.county = "";
        this.counties = [];
        if (e.target.value) {
            this.commonService.getCountiesByStateId(e.target.value).subscribe(counties => {
                this.counties = counties;
            });
        }
    }

  private createEmptyEmployeeContactInfo(): IEmployeeContactInfo {
    return {
      employeeId: null,
      firstName: null,
      middleInitial: null,
      lastName: null,
      addressLine1: null,
      addressLine2: null,
      city: null,
      stateId: null,
      countryId: null,
      postalCode: null,
      countyId: null,
      gender: null,
      birthDate: null,
      maritalStatusId: 0,
      homePhoneNumber: null,
      cellPhoneNumber: null,
      emailAddress: null,
      driversLicenseExpirationDate: null,
      driversLicenseNumber: null,
      driversLicenseIssuingStateId: null,
      noDriversLicense: null,
      stateName: null,
      countryName: null,
      countyName: null,
      driversLicenseIssuingStateName: null,
      jobTitleInfoDescription: null,
      divisionId: null,
      divisionName: null,
      departmentId: null,
      departmentName: null
    }
  }

  saveEmployeeContactInfo(): void {
    this.formSubmitted = true;
    this.f.markAllAsTouched();
    this.f.updateValueAndValidity();
    if (this.f.invalid) return;

    const dto = this.prepareModel();
    if (dto.countyId <= 0)
    {
        dto.countryId = 1; //just assume United States
    }
    this.dialogRef.close(dto);
  }

    private validateDate(control: AbstractControl) {
        var m = moment(control.value, 'MM/DD/YYYY');
        return { validDate: m.isValid() };
    }

  onNoClick(): void {
      this.dialogRef.close();
  }

  private createForm(): void {
      this.f = this.fb.group({
          firstName: this.fb.control(this.employeeContactInfo.firstName || '', [Validators.required, Validators.maxLength(25)]),
          middleInitial: this.fb.control(this.employeeContactInfo.middleInitial.trim() || '', [Validators.maxLength(1)]),
          lastName: this.fb.control(this.employeeContactInfo.lastName || '', [Validators.required, Validators.maxLength(25)]),
          addressLine1: this.fb.control(this.employeeContactInfo.addressLine1 || '', [Validators.maxLength(50)]),
          addressLine2: this.fb.control(this.employeeContactInfo.addressLine2 || '', [Validators.maxLength(50)]),
          city: this.fb.control(this.employeeContactInfo.city || '', [Validators.maxLength(25)]),
          county: this.fb.control(this.employeeContactInfo.countyId || ''),
          state: this.fb.control(this.employeeContactInfo.stateId || '', [Validators.required]),
          country: this.fb.control(this.employeeContactInfo.countryId || '', [Validators.required]),
          postalCode: this.fb.control(this.employeeContactInfo.postalCode || '', [Validators.maxLength(10)]),
          homePhone: this.fb.control(this.employeeContactInfo.homePhoneNumber || '', [Validators.pattern("^\\d{3}-\\d{3}-\\d{4}$")]),
          cellPhone: this.fb.control(this.employeeContactInfo.cellPhoneNumber || '', [Validators.pattern("^\\d{3}-\\d{3}-\\d{4}$")]),
          emailAddress: this.fb.control(this.employeeContactInfo.emailAddress || '', [Validators.required, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")]),
          driversLicenseNumber: this.fb.control(this.employeeContactInfo.driversLicenseNumber || '', [Validators.maxLength(20)]),
          driversLicenseIssuingState: this.fb.control(this.employeeContactInfo.driversLicenseIssuingStateId || '', []),
          driversLicenseExpirationDate: this.fb.control(this.employeeContactInfo.driversLicenseExpirationDate || '', []),
          maritalStatus: this.fb.control(this.employeeContactInfo.maritalStatusId || 0, []),
      });

      this.setDriverLicenseValidation(this.f.value.driversLicenseNumber);
      this.f.get('driversLicenseNumber').valueChanges.subscribe(val => {
          this.setDriverLicenseValidation(val);
          this.f.patchValue({ driversLicenseIssuingState: '', driversLicenseExpirationDate: '' });
      });
  }

    private setDriverLicenseValidation(licenseNumber) {
        if (licenseNumber) {
            this.f.get('driversLicenseIssuingState').setValidators([Validators.required]);
            this.f.get('driversLicenseExpirationDate').setValidators([Validators.required]);
        }
        else {
            this.f.get('driversLicenseIssuingState').setValidators(null);
            this.f.get('driversLicenseExpirationDate').setValidators(null);
        }
    }

  private prepareModel(): IEmployeeContactInfo {
    return {
        employeeId: this.employeeContactInfo.employeeId,
        firstName: this.f.value.firstName,
        middleInitial: this.f.value.middleInitial,
        lastName: this.f.value.lastName,
        addressLine1: this.f.value.addressLine1,
        addressLine2: this.f.value.addressLine2,
        city: this.f.value.city,
        stateId: this.f.value.state,
        stateName: (!this.f.value.state) ? "" : _.find(this.states, (state) => {
            return state.stateId == this.f.value.state;
        }).name || "",
        countryId: this.f.value.country,
        countryName: (!this.f.value.country) ? "" : _.find(this.countries, (country) => {
            return country.countryId == this.f.value.country;
        }).name || "",
        postalCode: this.f.value.postalCode,
        countyId: this.f.value.county,
        countyName: (!this.f.value.county || this.f.value.county == "") ? "" : _.find(this.counties, (county) => {
            return county.countyId == this.f.value.county;
        }).name || "",
        homePhoneNumber: this.f.value.homePhone,
        cellPhoneNumber: this.f.value.cellPhone,
        emailAddress: this.f.value.emailAddress,
        driversLicenseExpirationDate: moment(this.f.value.driversLicenseExpirationDate).toDate(),
        driversLicenseNumber: this.f.value.driversLicenseNumber,
        driversLicenseIssuingStateId: this.f.value.driversLicenseIssuingState,
        noDriversLicense: (!this.f.value.driversLicenseNumber) ? true : false,
        jobTitleInfoDescription: this.employeeContactInfo.jobTitleInfoDescription,
        divisionId: this.employeeContactInfo.divisionId,
        divisionName: this.employeeContactInfo.divisionName,
        departmentId: this.employeeContactInfo.departmentId,
        departmentName: this.employeeContactInfo.departmentName,
        maritalStatusId: this.f.value.maritalStatus,
    };
  }

    getFormControlError(field: string, errorCodes: string[]): boolean {
        const control = this.f.get(field);
        let flag: boolean = false;
        _.forEach(errorCodes, (errorCode) => {
            flag = control.hasError(errorCode) && (control.touched || this.formSubmitted);
            if (flag === true)
                return false;
        });
        return flag;
    }

  ngAfterViewInit() {

  }

}

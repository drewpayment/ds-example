import { ICountry, IState } from '@ajs/applicantTracking/shared/models';
import { Employee } from '@ajs/ds-external-api/models/employee-dto.model';
import { HttpErrorResponse } from '@angular/common/http';
import { toBase64String } from '@angular/compiler/src/output/source_map';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '@ds/core/account.service';
import { IBankBasicInfo } from '@ds/core/banks';
import { IClientContact, IContact } from '@ds/core/contacts';
import { ClientDivision } from '@ds/core/employee-services/models';
import { ClientDivisionAddress } from '@ds/core/employee-services/models/client-division-address.model';
import { ClientDivisionLogo } from '@ds/core/employee-services/models/client-division-logo.model';
import { UserInfo } from '@ds/core/shared';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { Observable, Subscriber } from 'rxjs';
import { map } from 'rxjs/operators';
import { CompanyManagementService } from '../../../services/company-management.service';
import { DivisionLocationDialogComponent } from './division-location-dialog/division-location-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { IClientDepartment } from '@ajs/client/models/client-department.model';
import { ClientGLAssignment, QuickViewInformation } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-divisions',
  templateUrl: './divisions.component.html',
  styleUrls: ['./divisions.component.scss']
})
export class DivisionsComponent implements OnInit {

  constructor(
    private CompanyManagementService: CompanyManagementService,
    private fb: FormBuilder,
    private ngxMsgSvc: NgxMessageService,
    private accountService: AccountService,
    private confirmDialog: ConfirmDialogService,
    private dialog: MatDialog
  ) { }
  loaded: boolean = false;
  user: UserInfo;
  isSubmitted = false;
  form: FormGroup;
  divisions: ClientDivision[];
  allDivisions: ClientDivision[];
  departments: IClientDepartment[];
  GLAssignments: ClientGLAssignment[];
  maxLogoFileSizeBytes: number;
  countries: ICountry[];
  states: IState[];
  allStates: IState[];
  contacts: IClientContact[];
  banks: IBankBasicInfo[];
  selectedDivision: ClientDivision;
  quickViewInformation: any;
  confirmed: boolean;
  showAccountFields: boolean;
  showOptionsAndAddressCards: boolean;
  employees: Employee[];
  addresses: ClientDivisionAddress[];
  divisionAddresses: ClientDivisionAddress[];
  logos: ClientDivisionLogo[];
  divisionLogoSrc: string;
  blockedDivisions: number[];
  showDelete: boolean = false;
  get zip() { return this.form.controls.zip as FormControl };
  get isUseSeparateAccountRoutingNumber() { return this.form.controls.isUseSeparateAccountRoutingNumber as FormControl };

  ngOnInit() {
    this.accountService.getUserInfo().subscribe((u) => {
      this.user = u;
    });
    this.CompanyManagementService.GetCompanyDivisionInformation().subscribe((data:any) => {
      this.initForm();
      this.setLocalVariables(data);
      let quickViewChange: QuickViewInformation = {
        employees: data.employees.length,
        departments: data.departments.length,
        activeEmployees: data.employees.filter(x => x.isActive == true).length,
      } as QuickViewInformation;
      this.setQuickViewInformation(quickViewChange);
      this.IsWOTC(data.isWotc);
      this.changeCountry();
      this.loaded = true;
    });

  }

  initForm() {
    this.showDelete = false;
    this.form = this.fb.group({
      division: [0],
      name: [null, Validators.required],
      addressLine1: [null, Validators.required],
      addressLine2: null,
      locationId: null,
      country: [1],
      city: [null, Validators.required],
      state: [null, Validators.required],
      zip: [
        null,
        [
          Validators.required,
          Validators.pattern('^\\d{5}(-\\d{4})?$')
        ]
      ],
      headOfDivision: null,
      sendCorrespondenceTo: [1],
      dateLocationOpened: [null, {updateOn: 'blur'}],
      dateLocationClosed: [null, {updateOn: 'blur'}],
      active: true,
      isUseAsStubAddress: false,
      isUseSeparateAccountRoutingNumber: false,
      bank: null,
      showInactiveCheck: false,
      account: "",
    });
    this.showDelete = false;
  }

  setLocalVariables(data) {
    this.allDivisions = data.divisions;
    this.divisions = data.divisions.filter((x) => x.isActive == true); //default active divisions only
    this.addresses = data.addresses;
    this.banks = data.banks; //routing numbers
    this.countries = data.countries;
    this.contacts = data.contacts;
    this.allStates = data.states;
    this.employees = data.employees;
    this.states = data.states.filter((x) => x.countryId == 1); //default to USA states
    this.logos = data.logos;
    this.blockedDivisions = data.blockedDivisions;
    this.departments = data.departments;
    this.GLAssignments = data.glAssignments;
    this.maxLogoFileSizeBytes = data.maxLogoFileSizeBytes
  }

  changeDivision() {
    this.isSubmitted = false;
    this.selectedDivision = this.divisions.find(s => s.clientDivisionId == this.form.controls['division'].value);

    if ( this.selectedDivision ) {
      //Show Address Card, Paystub Options, and Logo
      this.showOptionsAndAddressCards = true;
      //Show Delete
      this.showDelete = true;
      //Filter Addresses
      if ( this.addresses ) this.divisionAddresses = this.addresses.filter(x => x.clientDivisionId == this.form.controls['division'].value);
      //Load Logo
      if ( this.logos )  {
        let divisionLogo = this.logos.find(x => x.clientDivisionId == this.form.value.division);
        if ( divisionLogo ) this.divisionLogoSrc = "data:image/bmp;base64," + divisionLogo.divisionLogo;
        else this.divisionLogoSrc = null;
      }
      //Set Quick View for Selected Division
      let quickViewChange: QuickViewInformation = {
        employees: this.employees.filter(x => x.clientDivisionId == this.selectedDivision.clientDivisionId).length,
        departments: this.departments.filter(x => x.clientDivisionId == this.selectedDivision.clientDivisionId).length,
        activeEmployees: this.employees.filter(x => x.clientDivisionId == this.selectedDivision.clientDivisionId && x.isActive == true).length,
      } as QuickViewInformation;
      this.setQuickViewInformation(quickViewChange, this.selectedDivision.name);
      //Set the form values to the selected division's values
      this.form.reset({
        division: this.selectedDivision.clientDivisionId,
        name: this.selectedDivision.name,
        locationId: this.selectedDivision.locationId,
        country: this.selectedDivision.countryId,
        addressLine1: this.selectedDivision.addressLine1,
        addressLine2: this.selectedDivision.addressLine2,
        city: this.selectedDivision.city,
        state: this.selectedDivision.stateId,
        zip: this.selectedDivision.zip,
        headOfDivision: this.selectedDivision.clientContactId,
        sendCorrespondenceTo: this.selectedDivision.sendCorrespondenceTo,
        dateLocationOpened: this.selectedDivision.dateLocationOpened,
        dateLocationClosed: this.selectedDivision.dateLocationClosed,
        active: this.selectedDivision.isActive,
        isUseAsStubAddress: this.selectedDivision.isUseAsStubAddress,
        isUseSeparateAccountRoutingNumber: this.selectedDivision.isUseSeparateAccountRoutingNumber,
        bank: this.selectedDivision.bankId,
        account: this.selectedDivision.accountNumber,
        showInactiveCheck: this.form.value.showInactiveCheck
    })
    }
    else{
      this.showOptionsAndAddressCards = false;
      let showInactiveCheckHolder = this.form.value.showInactiveCheck;
      this.initForm();
      this.form.controls['showInactiveCheck'].patchValue(showInactiveCheckHolder);
      let quickViewChange: QuickViewInformation = {
        employees: this.employees.length,
        departments: this.departments.length,
        activeEmployees: this.employees.filter(x => x.isActive == true).length,
      } as QuickViewInformation;
      this.setQuickViewInformation(quickViewChange);
    }
  }

  save() {
    this.isSubmitted = true;
    this.form.markAllAsTouched();

    if ( this.form.invalid ) {
        return;
    }

    //If marking Division Inactive, check to see if it has departments or employees
    if ( this.form.value.active == false ) {
      let hasDepartments = this.departments.filter(x => x.clientDivisionId == this.selectedDivision.clientDivisionId).length > 0;
      let hasEmployees = this.employees.filter(x => x.clientDivisionId == this.selectedDivision.clientDivisionId).length > 0;
      if ( hasDepartments || hasEmployees ) {
        this.ngxMsgSvc.setErrorMessage("This division has employees or departments assigned to it and cannot be marked as inactive.");
        return;
      }
    }

    //Get State abbreviation for model
    let selectedState = this.states.find(s => s.stateId == this.form.value.state)
    if ( selectedState ) var stateAbbreviation = selectedState.abbreviation;
    else var stateAbbreviation = "";

    //Set model for save
    this.selectedDivision = {
      clientDivisionId: this.form.value.division,
      clientId: this.user.clientId,
      clientContactId: this.form.value.headOfDivision,
      name: this.form.value.name,
      addressLine1: this.form.value.addressLine1,
      addressLine2: this.form.value.addressLine2,
      city: this.form.value.city,
      stateId: this.form.value.state,
      zip: this.form.value.zip,
      countryId: this.form.value.country,
      sendCorrespondenceTo: this.form.value.sendCorrespondenceTo,
      isUseSeparateAccountRoutingNumber: this.form.value.isUseSeparateAccountRoutingNumber,
      accountNumber: this.form.value.account,
      bankId: this.form.value.bank,
      isActive: this.form.value.active,
      isUseAsStubAddress: this.form.value.isUseAsStubAddress,
      locationId: this.form.value.locationId,
      dateLocationOpened: this.form.value.dateLocationOpened,
      dateLocationClosed: this.form.value.dateLocationClosed,
      stateAbbreviation: stateAbbreviation
    }

    //Save
    this.CompanyManagementService.SaveCompanyDivisionInformation(this.selectedDivision).subscribe((response: ClientDivision) => {
      this.ngxMsgSvc.setSuccessMessage("Division Saved Successfully");
      this.isSubmitted = false;

      if ( response ) {
        let index = this.allDivisions.map(s => s.clientDivisionId).indexOf(response.clientDivisionId);
        if ( index == -1 ) {
          this.allDivisions.push(response); //insert
        }
        else{
          this.allDivisions[index] = response; //update
        }
        this.divisions = this.allDivisions;
        if ( this.form.value.showInactiveCheck ) this.divisions = this.allDivisions;
        else this.divisions = this.allDivisions.filter((x) => x.isActive == true);
        this.form.controls['division'].patchValue(response.clientDivisionId);
        this.changeDivision();
        //Set Quick View for Selected Division
        let quickViewChange: QuickViewInformation = {
          employees: this.employees.filter(x => x.clientDivisionId == response.clientDivisionId).length,
          departments: this.departments.filter(x => x.clientDivisionId == this.selectedDivision.clientDivisionId).length,
          activeEmployees: this.employees.filter(x => x.clientDivisionId == response.clientDivisionId && x.isActive == true).length,
        } as QuickViewInformation;
        this.setQuickViewInformation(quickViewChange, response.name);
      }
    }, (error: HttpErrorResponse) => {
      this.ngxMsgSvc.setErrorResponse(error);
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
    this.updateZipPatternValidation();
  }

  updateZipPatternValidation() {
    let selectedCountry = this.form.controls['country'].value;

    if ( selectedCountry == 7 ) {
      // If the country is Canada, set validators to Canadian format
      // This is first, assuming it's the second most popular choice after US
      this.form.get('zip').setValidators([Validators.required, Validators.pattern('^([ABCEGHJKLMNPRSTVXY]|[abceghjklmnprstvxy]){1}\\d{1}([ABCEGHJKLMNPRSTVWXYZ]|[abceghjklmnprstvwxyz]){1}( |-){0,1}\\d{1}([ABCEGHJKLMNPRSTVWXYZ]|[abceghjklmnprstvwxyz]){1}\\d{1}$')]);
      this.form.get('zip').updateValueAndValidity();
    }
    if ( selectedCountry == 1 ) {
      // If the country is US, set validators to US format
      this.form.get('zip').setValidators([Validators.required, Validators.pattern('^\\d{5}(-\\d{4})?$')]);
      this.form.get('zip').updateValueAndValidity();
    }
    if ( selectedCountry != 1 && selectedCountry != 7 ) {
      // We only want to validate US and Canada
      this.form.get('zip').clearValidators();
      this.form.get('zip').updateValueAndValidity();
    }
    this.form.markAsUntouched();
  }

  showInactive(){
    var showInactive = this.form.value.showInactiveCheck;
    if ( showInactive ) this.divisions = this.allDivisions.filter((x) => x.isActive == true);
    else this.divisions = this.allDivisions;
  }

  deleteClientDivision(confirmed){
    if ( confirmed ) {
      //Check if Division can be deleted
      if (this.blockedDivisions.some(x => x == this.selectedDivision.clientDivisionId)) {
        this.ngxMsgSvc.setErrorMessage("The specified division/location has history, departments, or employees assigned to it and cannot be deleted. Changes were not saved.");
      }
      else {
        this.CompanyManagementService.DeleteCompanyDivisionInformation(this.selectedDivision).subscribe(() => {
          this.ngxMsgSvc.setSuccessMessage("Division Deleted Successfully");
          this.allDivisions = this.allDivisions.filter((division) => division.clientDivisionId != this.selectedDivision.clientDivisionId); //remove from divisions array
          if ( this.form.value.showInactiveCheck ) this.divisions = this.allDivisions;
          else this.divisions = this.allDivisions.filter((x) => x.isActive == true);
          let showInactiveCheckHolder = this.form.value.showInactiveCheck;
          //clear form
          this.initForm();
          this.form.controls['showInactiveCheck'].patchValue(showInactiveCheckHolder);
          this.showOptionsAndAddressCards = false;
          //Set Quick View Information
          let quickViewChange: QuickViewInformation = {
            employees: this.employees.length,
            departments: this.departments.length,
            activeEmployees: this.employees.filter(x => x.isActive == true).length,
          } as QuickViewInformation;
          this.setQuickViewInformation(quickViewChange);
        },
        (error: HttpErrorResponse) => {
          this.ngxMsgSvc.setErrorResponse(error);
        });
      }
    }
  }

  deleteDivision(){
    let options;
      if ( this.hasGLAssignments() ) {
        options = {
          title: 'Are you sure you want to delete this address? If this Division is assigned to a General Ledger, then the assignment will be removed.',
          confirm: "Delete"
        };
      }
      else {
        options = {
          title: 'Are you sure you want to delete this address?',
          confirm: "Delete"
        };
      }
      this.confirmDialog.open(options);
      this.confirmDialog.confirmed().pipe(
          map(confirmation => this.confirmed = confirmation)
      ).subscribe(() => {
          this.deleteClientDivision(this.confirmed);
      });
  }

  openAddressDialog(selectedAddress){
    if ( selectedAddress == null ) {
      selectedAddress = {
        clientDivisionAddressId: 0,
        clientContactId: null,
        clientDivisionId: this.form.value.division,
        name: null,
        countryId: 1,
        address1: null,
        address2: null,
        city: null,
        state: null,
        zip: null,
      };
    }
    this.dialog.open(DivisionLocationDialogComponent, {
      width: '700px',
      data: {
        countries: this.countries,
        states: this.allStates,
        contacts: this.contacts,
        selectedAddress: selectedAddress
      },
    })
    .afterClosed()
    .subscribe((res) => {
      if ( res ) {
        if ( res.clientDivisionId ) {
          let index = this.addresses.map(s => s.clientDivisionAddressId).indexOf(res.clientDivisionAddressId);
          if ( index == -1 ) {
            this.addresses.push(res); //insert
          }
          else{
            this.addresses[index] = res; //update
          }
          this.divisionAddresses = this.addresses.filter(x => x.clientDivisionId == this.form.controls['division'].value);
          this.ngxMsgSvc.setSuccessMessage("Address saved successfully");
        }
        else {
          this.ngxMsgSvc.setErrorMessage(res);
        }
      }
    })
  }

  deleteClientAddress(confirmed, selectedAddress){
    if ( confirmed ) {
      this.CompanyManagementService.DeleteClientDivisionAddress(selectedAddress).subscribe((res) => {
        this.ngxMsgSvc.setSuccessMessage("Address Deleted Successfully");
        this.addresses = this.addresses.filter((adress) => adress.clientDivisionAddressId != selectedAddress.clientDivisionAddressId); //remove from adresses array
        this.divisionAddresses = this.addresses.filter(x => x.clientDivisionId == this.form.controls['division'].value);
        this.initForm(); //clear form
      },
      (error: HttpErrorResponse) => {
        this.ngxMsgSvc.setErrorResponse(error);
      });
    }
  }

  deleteAddress(selectedAddress){
    const options = {
      title: 'Are you sure you want to delete this address?',
      confirm: "Delete"
    };
      this.confirmDialog.open(options);
      this.confirmDialog.confirmed().pipe(
          map(confirmation => this.confirmed = confirmation)
      ).subscribe(() => {
          this.deleteClientAddress(this.confirmed, selectedAddress);
      });
  }

  IsWOTC(isWotc) {
    // If unemployement setup Is WOTC set 'Location', 'Date Opened', and 'Date Closed' to required
    if ( isWotc ) {
      this.form.controls['locationId'].setValidators(Validators.required);
      this.form.controls['dateLocationClosed'].setValidators(Validators.required);
      this.form.controls['dateLocationOpened'].setValidators(Validators.required);
    }
    // Else, fields are not required
    else {
      this.form.controls['locationId'].clearValidators();
      this.form.controls['dateLocationClosed'].clearValidators();
      this.form.controls['dateLocationOpened'].clearValidators();
    }
  }

  inputLogo(event: Event){
    if ( !(event.target as HTMLInputElement).files.length ) return; //Cancel was selected
    const file = (event.target as HTMLInputElement).files[0];
    if ( file.type != "image/bmp" ) return; //Only accept .bmp files
    if ( file.size >= this.maxLogoFileSizeBytes) {
      this.ngxMsgSvc.setErrorMessage("Logo image file size exceeds " + this.maxLogoFileSizeBytes / 1000 + " KB");
      return;
    }//3mb or less files ( in Decimal )
    //if ( file.size >= 3145728 ) return; //3mb or less files ( in Binary )
    const observable = new Observable((subscriber: Subscriber<any>) => {
      this.readFile(file, subscriber);
    });
    observable.subscribe((dataURL:string) => {
      let data = dataURL.replace("data:image/bmp;base64,", "");
      let logo = this.logos.find(x => x.clientDivisionId == this.form.value.division);
      if ( logo ) logo.divisionLogo = data; //update existing
      else logo = {
        clientDivisionLogoId: 0, //insert new
        clientId: this.selectedDivision.clientId,
        clientDivisionId: this.form.value.division,
        divisionLogo: data,
      };
      this.CompanyManagementService.SaveClientDivisionLogo(logo).subscribe((res: ClientDivisionLogo) => {
        let index = this.logos.map(s => s.clientDivisionId).indexOf(res.clientDivisionId);
        if ( index == -1 ) {
          this.logos.push(res); //insert
        }
        else{
          this.logos[index] = res; //update
        }
        this.ngxMsgSvc.setSuccessMessage("Division Logo Saved Successfully");
        this.divisionLogoSrc = dataURL; //set photo on UI
        this.isSubmitted = false;
      }),
      (error: HttpErrorResponse) => {
        this.ngxMsgSvc.setErrorResponse(error);
      }
    });
  }

  readFile(file: File, subscriber: Subscriber<any>) {
    const fileReader = new FileReader();
    fileReader.readAsDataURL(file);

    fileReader.onload = () => {
      subscriber.next(fileReader.result);
      subscriber.complete();
    };
    fileReader.onerror = (error) => {
      subscriber.error(error);
      subscriber.complete();
    }
  }

  deleteLogo(){
    const options = {
      title: 'Are you sure you want to delete this logo?',
      confirm: "Delete"
    };
    this.confirmDialog.open(options);
    this.confirmDialog.confirmed().pipe(
        map(confirmation => this.confirmed = confirmation)
    ).subscribe(() => {
        this.deleteClientLogo(this.confirmed);
    });
  }

  deleteClientLogo(confirmed){
    if ( confirmed ) {
      let logo = this.logos.find(x => x.clientDivisionId == this.form.value.division);
      if ( !logo ) return;
      this.CompanyManagementService.DeleteClientDivisionLogo(logo).subscribe(() => {
        this.ngxMsgSvc.setSuccessMessage("Logo Deleted Successfully");
        this.divisionLogoSrc = null; //set photo on ui
        this.logos = this.logos.filter(x => x.clientDivisionId != this.form.value.division); //remove from logo array
      },
      (error: HttpErrorResponse) => {
        this.ngxMsgSvc.setErrorResponse(error);
      });
    }
  }

  setQuickViewInformation(data:QuickViewInformation, title:string = "Quick View Information"){
    this.quickViewInformation = {
      title: title,
      employees: data.employees,
      departments: data.departments,
      activeEmployees: data.activeEmployees,
    };
  }

  //only called from html. Checks form value before its changed
  addAccountValidation() {
    if ( !this.form.value.isUseSeparateAccountRoutingNumber ) {
      this.form.controls['account'].setValidators(Validators.required);
      this.form.controls['bank'].setValidators(Validators.required);
    }
    else {
      this.form.controls['account'].clearValidators();
      this.form.controls['account'].updateValueAndValidity();
      this.form.controls['bank'].clearValidators();
      this.form.controls['bank'].updateValueAndValidity();
    }
  }

  hasGLAssignments(): boolean {
    return this.GLAssignments.some(x => x.clientDivisionId === this.selectedDivision.clientDivisionId);
  }
}

import { Component, Inject, OnInit, ElementRef, Input, Output, EventEmitter } from "@angular/core";
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn, ValidatorFn, ValidationErrors, AbstractControl } from '@angular/forms';

import { UserInfo } from '@ds/core/shared/user-info.model';
import { OnboardingEmployeeService } from '../../shared/onboarding-employee.service';
import { Observable, forkJoin, Subject } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import * as moment from 'moment';
import { IClientData, IEmployeeOnboardingData, II9DocumentData, ICountryData, IStateData, IEmployeeOnboardingI9DocumentData, IEmployeeOnboardingI9Data } from "@ajs/onboarding/shared/models";
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { tap, switchMap, takeUntil, filter } from 'rxjs/operators';

@Component({
  selector: 'ds-certify-i9-form',
  templateUrl: './certify-I9-form.component.html',
  styleUrls: ['./certify-I9-form.component.scss']
})
export class CertifyI9FormComponent implements OnInit {
  @Input() employee: IEmployeeOnboardingData;

  @Input() user: UserInfo;
  @Input() documents: Array<II9DocumentData>;
  @Input() countries: Array<ICountryData>;
  @Input() states: Array<IStateData>;
  @Input() isModal: boolean;

  @Output() i9StatusChange = new EventEmitter();

  form1: FormGroup;
  form2: FormGroup;
  form3: FormGroup;
  formSignature: FormGroup = this.fb.group({
    chkAccept: [false, this.agreementValidator()],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    title: ['', Validators.required],
  });

  formSubmitted: boolean;
  documentType: FormControl = new FormControl("");

  documentVerificationList: IEmployeeOnboardingI9DocumentData[] = [];
  items: any;
  isLoading: boolean = true;
  selection: string;
  addressClass: string = "col-sm-6";

  destroy$ = new Subject();

  constructor(
    private obEmpService: OnboardingEmployeeService,
    private msg: DsMsgService,
    private fb: FormBuilder,
  ) {}

  ngOnInit() {
    /// Build the form here
    this.form1 = this.createDocForm('A');
    this.form2 = this.createDocForm('B');
    this.form3 = this.createDocForm('C');

    if (this.employee) {
      this.loadItems(this.employee);
      this.updateFormSignatureForm();
    } else {
      // Hook the active employee selection changes
      this.obEmpService.activeEmployees
        .pipe(
          takeUntil(this.destroy$),
          filter(emp => !!emp),
        )
        .subscribe(emp => {
          this.employee = emp;
          this.loadItems(emp);
          this.updateFormSignatureForm();
        });
    }

    if (this.isModal) {
      this.addressClass = "col-sm-6 col-md";
      let self = this;

      //btnCertifyI9
      document.getElementById("btnCertifyI9").addEventListener('click', function (e: Event) {
        e.preventDefault();
        e.stopPropagation();
        self.certify();
      });
    }
  }

  private updateFormSignatureForm() {
    this.formSignature.patchValue({
      firstName: this.user.userFirstName,
      lastName: this.user.userLastName,
    });
  }

  private agreementValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      return control.value === true ? null : { 'unaccept': true };
    };
  }

  private createDocForm(typ: string) {
    let frm = this.fb.group({
      docId: ['', Validators.required],
      issuing: ['', Validators.required],
      number: ['', Validators.required],
      expirationDate: this.fb.control({ value: '', disabled: true }, Validators.required),
      noExpiration: [true, null],
      additionalInfo: [''],
      docType: [typ],
    });

    return frm;
  }

  private updateDocForm(data: IEmployeeOnboardingI9DocumentData) {
    let typ = data.category.toUpperCase();
    let frm = (typ == 'A') ? this.form1 : ((typ == 'B') ? this.form2 : this.form3);

    frm.reset();
    frm.patchValue({
      docId: data.i9DocumentId,
      issuing: data.issuingAuthority,
      number: data.documentNumber,
      expirationDate: (!data.expirationDate) ? '' : data.expirationDate,
      noExpiration: !data.expirationDate,
      additionalInfo: data.additionalInfo,
      docType: typ,
    });

    if (!data.expirationDate)
      frm.controls.expirationDate.disable();
    else
      frm.controls.expirationDate.enable();
  }

  private copyDocDataFromForm(data: IEmployeeOnboardingI9DocumentData) {
    let typ = data.category.toUpperCase();
    let frm: FormGroup = (typ == 'A') ? this.form1 : ((typ == 'B') ? this.form2 : this.form3);

    data.i9DocumentId = parseInt(frm.value.docId.toString());
    data.issuingAuthority = frm.value.issuing;
    data.documentNumber = frm.value.number;
    data.expirationDate = frm.value.noExpiration ? '' : frm.value.expirationDate;
    data.additionalInfo = frm.value.additionalInfo;
  }

  private clearForms() {
    this.documentType.setValue('A');
    this.selection = 'A';
    this.documentVerificationList.splice(0);

    this.updateDocForm(<IEmployeeOnboardingI9DocumentData>{ category: 'A' });
    this.updateDocForm(<IEmployeeOnboardingI9DocumentData>{ category: 'B' });
    this.updateDocForm(<IEmployeeOnboardingI9DocumentData>{ category: 'C' });

  }

  public resetExpiration(frm: FormGroup) {
    if (frm.controls.noExpiration.value) {
      frm.controls.expirationDate.setValue('');
      frm.controls.expirationDate.disable();
    } else {
      frm.controls.expirationDate.enable();
    }
  }

  private loadItems(emp) {
    this.isLoading = true;

    this.items = {};
    this.items.employeeName = emp.employeeFirstName + " " + emp.employeeLastName;
    this.items.user = {};
    this.items.user.firstName = this.user.firstName;
    this.items.user.lastName = this.user.lastName;
    this.items.titles = this.documents;
    this.items.documentVerificationList = [];

    this.clearForms();

    let I9Docs$ = this.obEmpService.getEmployeeOnboardingI9Docs(emp.employeeId).pipe(
      tap(documentData => {
        let documentVerificaitonList = [];
        let newDocument: IEmployeeOnboardingI9DocumentData;
        documentData.forEach((doc) => {
          newDocument = Object.assign(<IEmployeeOnboardingI9DocumentData>{}, doc);
          let listItem: any = this.documents
            .find(x => x.i9DocumentId === newDocument.i9DocumentId);
          newDocument.category = listItem.category;
          documentVerificaitonList.push(newDocument);
        });
        this.items.documentVerificationList = documentVerificaitonList;
      }),
      switchMap(documentData => this.obEmpService.getEmployeeOnboardingForms(emp.employeeId)),
      tap(data => {
        this.items.forms = data;
        for (var i = 0; i < this.items.forms.length; i++) {
          if (this.items.forms[i].formName === "I-9") {
            for (var j = 0; j < this.items.forms[i].signatures.length; j++) {
              if (this.items.forms[i].signatures[j].signatureDefinitionId === 16) {

                if (this.items.forms[i].signatures[j].signeeFirstName)
                  this.items.user.firstName = this.items.forms[i].signatures[j].signeeFirstName;
                if (this.items.forms[i].signatures[j].signeeLastName)
                  this.items.user.lastName = this.items.forms[i].signatures[j].signeeLastName;
                if (this.items.forms[i].signatures[j].signeeTitle)
                  this.items.user.title = this.items.forms[i].signatures[j].signeeTitle;
                break;
              }
            }

            break;
          }
        };

        if (this.items.documentVerificationList.length > 0) {
          this.documentVerificationList = this.items.documentVerificationList;

          if (this.items.documentVerificationList[0].category !== 'A') {
            //Using List B & C
            this.documentType.patchValue('B');
            this.selection = 'B';

            if (this.documentVerificationList.length === 1) {
              if (this.documentVerificationList.length === 1 &&
                this.documentVerificationList[0].category === 'B') {
                this.documentVerificationList.push(this.createDocumentItem("C"));
              } else {
                this.documentVerificationList.push(this.createDocumentItem("B"));
              }
            }
          }
        } else {
          //Setup default document
          this.documentVerificationList.push(this.createDocumentItem("A"));
        }

        // Bind the data to the forms
        this.documentVerificationList.forEach(x => this.updateDocForm(x));
      }));

    let empI9$ = this.obEmpService.getEmployeeI9(emp.employeeId).pipe(
      tap(employeeI9Data => {
        this.items.i9Data = employeeI9Data;
        this.items.countries = this.countries;
        this.items.status = this.i9StatusDesc(employeeI9Data.i9EligibilityStatusId);
        if (this.items.i9Data.i9EligibilityStatusId === 4 && this.items.i9Data.alienAdmissionNumberType === 2) {
          if (this.items.i9Data.admissionNumberFromCBP) {
            if (this.items.i9Data.passportCountryId) {
              let countryItem: any = this.countries
                .find(x => x.countryId === this.items.i9Data.passportCountryId);
              this.items.i9Data.country = countryItem.name;
            }
          }
        }
      }));


    let empInfo$ = this.obEmpService.getEmployeeInfo(emp.employeeId).pipe(
      tap(employeeData => {
        this.items.employeeData = employeeData;
        this.items.DOB = employeeData.birthDate;
        this.items.cityStateZip =
          employeeData.city + ", " + employeeData.stateName + " " + employeeData.postalCode;

        if (employeeData.countryId === 1) {
          let stateItem: any = this.states.find(x => x.stateId === employeeData.stateId);
          let stateAbbr = stateItem.abbreviation;
          this.items.cityStateZip = employeeData.city + ", " + stateAbbr + " " + employeeData.postalCode;
        } else {
          this.items.cityStateZip = employeeData.city + ", " + employeeData.stateName + " " + employeeData.postalCode;
        };
      }));

    // one call to all apis
    forkJoin(I9Docs$, empI9$, empInfo$).subscribe(x => { this.isLoading = false; })
  }

  private i9StatusDesc(id: number) {
    var desc = "unknown";
    switch (id) {
      case 1:
        desc = "United States citizen";
        break;
      case 2:
        desc = "United States noncitizen national";
        break;
      case 3:
        desc = "Lawful permanent resident";
        break;
      case 4:
        desc = "Alien authorized to work";
        break;
    }
    return desc;
  }

  private createDocumentItem(docTypeId: string) {
    let doc: any = {};

    doc.employeeId = this.items.employeeData.employeeId;
    doc.category = docTypeId;
    doc.title = null;
    doc.issuingAuthority = null;
    doc.documentNumber = null;
    doc.expirationDate = null;
    doc.additionalInfo = null;
    doc.createDate = new Date();

    return doc;
  }

  selectionChange(typ: string): void {
    this.selection = typ;
    //Using List B & C
    if (typ == 'B') {
      if (this.documentVerificationList.length === 1) {
        this.documentVerificationList[0].category = 'B';
        this.documentVerificationList[0].i9DocumentId = null;
        this.documentVerificationList[0].documentNumber = '';

        this.documentVerificationList.push(this.createDocumentItem("C"));
      } else if (this.documentVerificationList.length === 0) {
        this.documentVerificationList.push(this.createDocumentItem("B"));
        this.documentVerificationList.push(this.createDocumentItem("C"));
      }
    } else if (typ == 'A') {
      if (this.documentVerificationList.length > 1) {
        this.documentVerificationList.splice(1);
      } else if (this.documentVerificationList.length === 0) {
        this.documentVerificationList.push(this.createDocumentItem("A"));
      }

      this.documentVerificationList[0].category = 'A';
      this.documentVerificationList[0].i9DocumentId = null;
      this.documentVerificationList[0].documentNumber = '';
    }
  }

  public cancel(): void {
    this.i9StatusChange.emit("cancel");
  }

  clearExpirationDateErrors(frm: FormGroup) {
    if (frm.controls.noExpiration.value) {
      frm.controls.expirationDate.setErrors(null);
    }
  }

  public certify(): void {
    this.formSubmitted = true;
    let documentsValid = true;

    if (this.documentVerificationList.length == 1) {
      this.clearExpirationDateErrors(this.form1);

      this.form1.updateValueAndValidity();
      documentsValid = this.form1.valid;
    } else {
      this.clearExpirationDateErrors(this.form2);
      this.clearExpirationDateErrors(this.form3);

      this.form2.updateValueAndValidity();
      this.form3.updateValueAndValidity();
      documentsValid = this.form2.valid && this.form3.valid;
    }
    this.formSignature.updateValueAndValidity();


    let subscription: Observable<any> = null;
    if (this.formSignature.valid && documentsValid) {
      this.processCertification();
    }
  }

  processCertification() {
    let formI9: any = [];
    this.documentVerificationList.forEach(x => this.copyDocDataFromForm(x));

    for (var i = 0; i < this.items.forms.length; i++) {

      if (this.items.forms[i].formName === "I-9") {

        formI9.push(this.items.forms[i]);

        for (var j = 0; j < this.items.forms[i].signatures.length; j++) {

          if (this.items.forms[i].signatures[j].roleIdentifier === "employer.main") {

            this.items.forms[i].signatures[j].isSigned = true;
            //add the current timestamp as the signature date
            this.items.forms[i].signatures[j].signatureDate = new Date();

            //set the signature's full name text from the individual name fields
            this.items.forms[i].signatures[j].signatureName = this.formSignature.value.firstName + ' ' + this.formSignature.value.lastName;
            this.items.forms[i].signatures[j].signeeLastName = this.formSignature.value.lastName;
            this.items.forms[i].signatures[j].signeeFirstName = this.formSignature.value.firstName;

            this.items.forms[i].signatures[j].signeeTitle = this.formSignature.value.title;
            break;
          }

        }
        break;
      }
    }

    this.msg.sending(true);
    //Save I9 Docs and Admin Signature
    this.obEmpService
      .putEmployeeOnboardingFormsDocs(this.employee.employeeId, formI9, this.documentVerificationList).subscribe(() => {
        this.msg.setTemporarySuccessMessage('I9 certification completed successfully.');

        this.i9StatusChange.emit("certified");
      }, (error: HttpErrorResponse) => {
        this.msg.setTemporaryMessage(`Error : ${error.error}`, MessageTypes.error);
      });
  }

  clearDrawer() {
    this.i9StatusChange.emit("dismissed");
  }
}
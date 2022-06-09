import { Component, Inject, OnInit,  ElementRef, Input, Output, EventEmitter, ViewChild, HostListener} from "@angular/core";
import { FormControl, FormGroup, Validators, FormBuilder, AbstractControl } from '@angular/forms';

import { UserInfo } from '@ds/core/shared/user-info.model';
import { tap, filter } from 'rxjs/operators';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { MatSidenav } from "@angular/material/sidenav/sidenav";
import { LocationApiService } from '@ds/core/location/shared/location-api.service';
import { ICountry } from '@ds/core/location/shared/country.model';
import { IState } from '@ds/core/location/shared/state.model';
import { ICounty } from '@ds/core/location/shared/county.model';
import * as moment from 'moment';
import { IEmergencyContact } from '../../../models/contact/emergency-contact.model';
import { EmergencyContactService } from 'apps/ds-company/src/app/Services/emergency-contact.service';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { HttpErrorResponse } from '@angular/common/http';



@Component({
    selector: 'ds-emergency-contact-form',
    templateUrl: './emergency-contact-form.component.html',
    styleUrls: ['./emergency-contact-form.component.scss']
  })
  export class EmergencyContactFormComponent implements OnInit {
      
    @Input() user: UserInfo;
    @Input() emergencyContact: IEmergencyContact;
    @Input() isModal: boolean;
    @Input() pageSubmitted: boolean;
    @Input() isAdding: boolean;

    @Output() statusChange = new EventEmitter();

    submitted: boolean;
    
    
    hasEditPermissions: boolean;
    countries: ICountry[];
    states: IState[];
    counties: ICounty[];
    f: FormGroup;
    formSubmitted: boolean;
    pageTitle: string;
    private _editMode: boolean;

    constructor(
        private fb: FormBuilder,
        private commonService: LocationApiService,
        private contactApi: EmergencyContactService,
        private msg: NgxMessageService,
    ) { }

    ngOnInit(): void {
        this.pageTitle = "Edit Employee Contact Info";
        if(!this.f) this.createForm();

        
        this.contactApi.activeContact.subscribe( c => {
            if(c){
                this.emergencyContact = <IEmergencyContact>c;
                this.updateForm();
            }
        });
    }

    clearDrawer(){
        this.statusChange.emit(-1);
    }
    formatNumber(event:Event){
        let ctrl = <HTMLInputElement>event.target;
        let valu = ctrl.value.replace(/-/g,'');
        if(valu.length > 6) valu = valu.substring(0,3)+'-'+valu.substring(3,6)+'-'+valu.substring(6);
        else if(valu.length > 3) valu = valu.substring(0,3)+'-'+valu.substring(3);
        ctrl.value = valu;
    }
    onCountryChange(e) {
        this.f.value.state = "";
        this.states = [];
        if (e.target.value) this.loadStates(Number(e.target.value), null);
    }

    loadStates(countryId:number, stateId: number) {
        this.f.value.state = "";
        this.states = [];
        if (countryId) {
            this.commonService.getStatesByCountry(countryId).subscribe(states => {
                this.states = states;
                this.f.value.state = stateId || "";
            });
        }
    }

    private createForm(): void {
        this.f = this.fb.group({
            firstName: this.fb.control(this.emergencyContact.firstName || '', [Validators.required, Validators.maxLength(25)]),
            lastName: this.fb.control(this.emergencyContact.lastName || '', [Validators.required, Validators.maxLength(25)]),
            relationship: this.fb.control(this.emergencyContact.relation || '', [Validators.required, Validators.maxLength(20)]),
            homePhone: this.fb.control(this.emergencyContact.homePhoneNumber || '', [Validators.required, Validators.pattern("^\\d{3}-\\d{3}-\\d{4}$")]),
            cellPhone: this.fb.control(this.emergencyContact.cellPhoneNumber || '', [Validators.pattern("^\\d{3}-\\d{3}-\\d{4}$")]),
            emailAddress: this.fb.control(this.emergencyContact.emailAddress || '', [Validators.pattern("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$")]),
        });
    }

    private updateForm(): void{
        this.f.setValue({
            firstName: this.emergencyContact.firstName || '',
            lastName: this.emergencyContact.lastName || '',
            relationship: this.emergencyContact.relation || '',
            homePhone: this.emergencyContact.homePhoneNumber || '',
            cellPhone: this.emergencyContact.cellPhoneNumber || '',
            emailAddress: this.emergencyContact.emailAddress || '',
        });
    }

    private updateModel(): void {
        Object.assign( this.emergencyContact, {
            firstName: this.f.value.firstName,
            middleInitial: this.f.value.middleInitial,
            lastName: this.f.value.lastName,
            employeeId: this.emergencyContact.employeeId,
            relation: this.f.value.relationship,
            homePhoneNumber: this.f.value.homePhone,
            workPhoneNumber: this.f.value.workPhone,
            cellPhoneNumber: this.f.value.cellPhone,
            emailAddress: this.f.value.emailAddress,
        });
    }

    ngAfterViewInit() {

    }

    close() {
        this.f.reset();
        this.clearDrawer();
    }

    saveEmergencyContact(): void {
        this.formSubmitted = true;
        this.f.updateValueAndValidity();
        if (this.f.invalid) return;

        this.updateModel();
        this.msg.loading(true, 'Sending...');
        this.contactApi.putEmergencyContact(this.emergencyContact.employeeId, this.emergencyContact).pipe(
            tap( (contact:IEmergencyContact) => {
                this.msg.setSuccessMessage("Contact saved successfully.", 5000);
                this.emergencyContact.isDirty = false;
                this.emergencyContact.employeeEmergencyContactId = contact.employeeEmergencyContactId;
                this.contactApi.setActiveContact(this.emergencyContact);
                this.statusChange.emit(contact.employeeEmergencyContactId);
                this.clearDrawer();
            })
        ).subscribe( x=> {}, (error: HttpErrorResponse) => {
            this.msg.setErrorResponse(error);
        });
    }
    
    @HostListener("keydown.esc", ['$event'])
    onEsc(event) {
        event.preventDefault();
        event.stopPropagation();
        this.clearDrawer();
    }
}
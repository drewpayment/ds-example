import { Component, Inject, OnInit,  ElementRef, Input, Output, EventEmitter, ViewChild, HostListener} from "@angular/core";
import { FormControl, FormGroup, Validators, FormBuilder, AbstractControl } from '@angular/forms';
import { UserInfo, UserType } from "@ds/core/shared";
import { tap, filter } from 'rxjs/operators';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { MatSidenav } from "@angular/material/sidenav/sidenav";
import * as moment from 'moment';
import { IEmployeeDependent, IEmployeeDependentRelationship } from '../../../models/employee-dependents/employee-dependent.model';
import { EmployeeDependentsService } from 'apps/ds-company/src/app/Services/employee-dependents.service';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'ds-employee-dependent-form',
    templateUrl: './employee-dependent-form.component.html',
    styleUrls: ['./employee-dependent-form.component.scss']
  })
  export class EmployeeDependentFormComponent implements OnInit {
    @Input() user: UserInfo;
    @Input() employeeDependent: IEmployeeDependent;
    @Input() isModal: boolean;
    @Input() pageSubmitted: boolean;
    @Input() isAdding: boolean;

    @Output() statusChange = new EventEmitter();

    submitted: boolean;
    hasEditPermissions: boolean = true;
    showEffectiveDate: boolean = false;
    showSSN: boolean = false;
    relationshipList: IEmployeeDependentRelationship[];
    f: FormGroup;
    formSubmitted: boolean;
    pageTitle: string;
    private _editMode: boolean;

    get FirstName() { return this.f.controls.FirstName as FormControl; }
    get MiddleInitial() { return this.f.controls.MiddleInitial as FormControl; }
    get LastName() { return this.f.controls.LastName as FormControl; }
    get Relationship() { return this.f.controls.Relationship as FormControl; }
    get SocialSecurityNumber() { return this.f.controls.SocialSecurityNumber as FormControl; }
    get BirthDate() { return this.f.controls.BirthDate as FormControl; }
    get EffectiveDate() { return this.f.controls.EffectiveDate as FormControl; }
    get Gender() { return this.f.controls.Gender as FormControl; }
    get IsStudent() { return this.f.controls.IsStudent as FormControl; }
    get HasDisablity() { return this.f.controls.HasDisablity as FormControl; }
    get IsTobaccoUser() { return this.f.controls.IsTobaccoUser as FormControl; }
    get DependentStatus() { return this.f.controls.DependentStatus as FormControl; }
    get Notes() { return this.f.controls.Notes as FormControl; }

    constructor(
        private fb: FormBuilder,
        private dependentApi: EmployeeDependentsService,
        private msg: NgxMessageService,
    ) { }

    ngOnInit(): void {
        this.showSSN = (this.user.userTypeId == UserType.systemAdmin || this.user.userTypeId == UserType.companyAdmin);

        if(!this.f) this.createForm();
        this.loadRelationshipList();
        this.dependentApi.activeDependent.subscribe( c => {
            if(c){
                this.employeeDependent = <IEmployeeDependent>c;
                this.setPageTitle();
                this.updateForm();
            }
        });
    }

    setPageTitle() {
        this.pageTitle = "Add Employee Dependent";
        if (this.employeeDependent.employeeDependentId && this.employeeDependent.employeeDependentId > 0) {
            this.pageTitle = this.employeeDependent.firstName;

            if (this.employeeDependent.middleInitial)
                this.pageTitle = this.pageTitle + " " + this.employeeDependent.middleInitial;
            
            if (this.employeeDependent.lastName)
                this.pageTitle = this.pageTitle + " " + this.employeeDependent.lastName;
        }
    }

    clearDrawer(){
        this.statusChange.emit(-1);
    }

    loadRelationshipList(dependentRelationshipId?: number) {
        this.relationshipList = [];
        this.dependentApi.getEmployeeDependentRelationships().subscribe(relations => {
            this.relationshipList = relations;
        });
    }

    formatSSN(event:Event){
        let ctrl = <HTMLInputElement>event.target;
        let valu = ctrl.value.replace(/-/g,'');
        if(valu.length > 5) 
            valu = valu.substring(0,3)+'-'+valu.substring(3,5)+'-'+valu.substring(5);
        else if(valu.length > 3) 
            valu = valu.substring(0,3)+'-'+valu.substring(3);
        ctrl.value = valu;
    }

    private createForm(): void {
        this.f = this.fb.group({
            FirstName: this.fb.control('', [Validators.required]),
            MiddleInitial: this.fb.control('', []),
            LastName: this.fb.control('', [Validators.required]),
            Relationship: this.fb.control('', [Validators.required]),
            SocialSecurityNumber: this.fb.control('', [Validators.pattern('^(\\d{3}-?\\d{2}-?\\d{4}|XXX-XX-XXXX)$')]),
            BirthDate: this.fb.control('', [Validators.required]),
            EffectiveDate: this.fb.control('', []),
            Gender: this.fb.control(' ', []),
            IsStudent: this.fb.control(false, []),
            HasDisablity: this.fb.control(false, []),
            IsTobaccoUser: this.fb.control(false, []),
            DependentStatus: this.fb.control('', []),
            Notes: this.fb.control('', []),
        });

        this.DependentStatus.valueChanges.subscribe(val => {
            if (val == "1") {
              this.showEffectiveDate = false;
              this.EffectiveDate.clearValidators();
              this.f.patchValue ({
                EffectiveDate: '',
              });
            }
            else {
                this.showEffectiveDate = true;
                this.EffectiveDate.setValidators(Validators.required)
                this.f.patchValue ({
                  EffectiveDate: this.employeeDependent.inactiveDate || '',
                });
            }
        })
    }

    private updateForm(): void {
        this.f.patchValue({
            FirstName: this.employeeDependent.firstName || '',
            MiddleInitial: this.employeeDependent.middleInitial || '',
            LastName: this.employeeDependent.lastName || '',
            Relationship: this.employeeDependent.employeeDependentsRelationshipId || '',
            SocialSecurityNumber: this.employeeDependent.unmaskedSocialSecurityNumber || '',
            BirthDate: this.employeeDependent.birthDate || '',
            EffectiveDate: this.employeeDependent.inactiveDate || '',
            Gender: this.employeeDependent.gender || ' ',
            IsStudent: this.employeeDependent.isAStudent || false,
            HasDisablity: this.employeeDependent.hasADisability || false,
            IsTobaccoUser: this.employeeDependent.tobaccoUser || false,
            DependentStatus:  !this.employeeDependent.isInactive ? "1" : "0",
            Notes: this.employeeDependent.comments || '',
        });
    }

    private prepareModel(): void {
        let relation = this.relationshipList.find(x =>  x.employeeDependentsRelationshipId == this.Relationship.value) || null
        Object.assign(this.employeeDependent, {
            firstName: this.FirstName.value,
            middleInitial: this.MiddleInitial.value,
            lastName: this.LastName.value,
            employeeId: this.employeeDependent.employeeId,
            employeeDependentsRelationshipId: this.Relationship.value,
            relationship: relation ? relation.description : '',
            unmaskedSocialSecurityNumber: this.SocialSecurityNumber.value,
            birthDate: this.BirthDate.value,
            inactiveDate: this.EffectiveDate.value,
            gender: this.Gender.value.replace(/\s/g, "") || '',
            isAStudent: this.IsStudent.value,
            hasADisability: this.HasDisablity.value,
            tobaccoUser: this.IsTobaccoUser.value,
            isInactive: this.DependentStatus.value == '0' ? true : false,
            comments: this.Notes.value
        });
    }

    ngAfterViewInit() {

    }

    close() {
        this.f.reset();
        this.clearDrawer();
    }

    saveEmployeeDependent(): void {
        this.formSubmitted = true;
        this.f.updateValueAndValidity();
        if (this.f.invalid) return;

        this.prepareModel();
        this.msg.loading(true, 'Sending...');

        var parsedDate = Date.parse(this.BirthDate.value);
        if (isNaN(parsedDate)) {
            return;
        }        
        
        this.dependentApi.saveEmployeeDependent(this.employeeDependent.employeeId, this.employeeDependent, this.hasEditPermissions === true).pipe(
            tap( (dependent:IEmployeeDependent) => {
                this.msg.setSuccessMessage("Dependent saved successfully.", 5000);
                this.employeeDependent.isDirty = false;
                this.employeeDependent.employeeDependentId = dependent.employeeDependentId;
                this.dependentApi.setActiveDependent(this.employeeDependent);
                this.statusChange.emit(dependent.employeeDependentId);
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
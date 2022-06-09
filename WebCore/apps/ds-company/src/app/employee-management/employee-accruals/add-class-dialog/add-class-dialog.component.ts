import { Component, Inject, OnInit, HostListener } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn } from '@angular/forms';
import { UserInfo } from "@ds/core/shared";
import { IClientEmploymentClass } from '../../../models/employee-accruals/employee-accruals.model';
import { BenefitsAdminService } from '../../../services/benefits-admin.service';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { HttpErrorResponse } from '@angular/common/http';
@Component({
    selector: 'ds-add-class-dialog',
    templateUrl: './add-class-dialog.component.html',
    styleUrls: ['./add-class-dialog.component.scss']
  })
  export class AddClassDialogComponent implements OnInit {
    form1: FormGroup;
    formSubmitted: boolean;
    allClasses: Array<IClientEmploymentClass>;
    user:UserInfo;

    activeClassId: number;
    activeClass: IClientEmploymentClass;

    constructor(
        private ref:MatDialogRef<AddClassDialogComponent>,
        private fb: FormBuilder,
        private benAdminApi: BenefitsAdminService,
        private msg: NgxMessageService,
        @Inject(MAT_DIALOG_DATA) public data:any,
    ) {
        this.user               = data.user;
        this.allClasses         = data.classes || [];
        this.sort();
        this.activeClassId = 0;
    }


    ngOnInit() {
        this.createForm();
    }
    sort(){
        this.allClasses.sort( (a, b) => (a.isEnabled == b.isEnabled) ? 
            (a.description.toLowerCase().localeCompare(b.description.toLowerCase())) : (a.isEnabled < b.isEnabled ? 1 : -1 )  );
    }
    createForm(){
        this.form1 = this.fb.group({
            classId: '',
            className:   ['', [Validators.required,Validators.pattern('^[a-zA-Z0-9 ]*$')]],
            classCode:   ['', [Validators.required,Validators.pattern('^[a-zA-Z0-9 ]*$')]],
            isActive:   [true],
        });
    }
    updateForm(){
        this.form1.patchValue({
            classId: this.activeClass.clientEmploymentClassId,
            className:   this.activeClass.description,
            classCode:   this.activeClass.code,
            isActive:   this.activeClass.isEnabled,
        });
    }
    clearForm(){
        this.form1.reset();
        this.form1.patchValue({
            classId: '',
            className:   '',
            classCode:   '',
            isActive:   true,
        });
    }
    activeClassChanged(event: Event){
        let idStr:string = (<HTMLSelectElement>event.target).value;
        let id:number = idStr ? parseInt(idStr) : 0;
        if (!id) {
            this.activeClassId = 0;
            this.activeClass = null;
            this.clearForm();
        } else {
          this.activeClassId = id;
          this.activeClass = this.allClasses.find(x => {
            return (x.clientEmploymentClassId == this.activeClassId);
          });
          this.updateForm();
        }
    }
    clear() {
        this.ref.close( this.allClasses );
    }
    save() {
        this.formSubmitted = true;
        if(!this.form1.valid) return;

        let ctrlName = this.form1.get('className');
        let ctrlCode = this.form1.get('classCode');

        if( this.allClasses.find(x => x.description.toLowerCase() == ctrlName.value.trim().toLowerCase() && 
            x.clientEmploymentClassId != this.activeClassId)) {
            ctrlName.setErrors({ duplicate: true });
            return;
        }

        if( this.allClasses.find(x => x.code.toLowerCase() == ctrlCode.value.trim().toLowerCase() && 
            x.clientEmploymentClassId != this.activeClassId)) {
            ctrlCode.setErrors({ duplicate: true });
            return;
        }
        
        if(this.form1.valid)
        {
            let isAdd:boolean = true;
            let newItem = null;
            if(!this.activeClass){
                newItem = <IClientEmploymentClass>{
                    clientEmploymentClassId : 0,  
                    description:    ctrlName.value.trim(),
                    code:           ctrlCode.value.trim(),
                    clientId:       this.user.lastClientId || this.user.clientId,
                    isEnabled:      this.form1.value.isActive,
                };
                this.activeClass = newItem;
            } else {
                isAdd = false;
                this.activeClass.code = ctrlCode.value.trim();
                this.activeClass.description = ctrlName.value.trim();
                this.activeClass.isEnabled = this.form1.value.isActive;
            }
            
            this.msg.loading(true,`Sending...`);
            this.benAdminApi.saveClientEmploymentClass(this.user.lastClientId || this.user.clientId, this.activeClass)
            .subscribe(x => {
                this.msg.setSuccessMessage(`Employment class "${this.activeClass.description}" ${isAdd ? 'added':'updated'} successfully.`);
                if(isAdd) this.allClasses.push(newItem);

                this.activeClass.clientEmploymentClassId = x.clientEmploymentClassId;
                this.activeClassId = x.clientEmploymentClassId;
                this.updateForm();
                this.sort();
            }, (error: HttpErrorResponse) => {
                this.msg.setErrorResponse(error);
            });
        }
    }
}

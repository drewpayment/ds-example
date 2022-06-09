import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn } from '@angular/forms';

@Component({
    selector: 'ds-add-organization-dialog',
    templateUrl: './add-organization-dialog.component.html',
    styleUrls: ['./add-organization-dialog.component.scss']
  })
  export class AddOrganizationDialogComponent implements OnInit {
    constructor(
        private ref:MatDialogRef<AddOrganizationDialogComponent>,
        private fb: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public organizations:Array<string>,
    ) {}

    form1: FormGroup;
    formSubmitted: boolean;

    ngOnInit() {
        /// Build the form here
        this.form1 = this.fb.group({
            organizationName:   [null, Validators.required],            
        });
    }

    clear() {
        this.ref.close("");
    }
    add() {
        this.formSubmitted = true;
        
        var organizationNm = this.form1.value.organizationName;
        if(!organizationNm) return;
        if( this.organizations.find(x => x == organizationNm)) {
            this.form1.get('organizationName').setErrors({ duplicate: true });
            return;
        }
        this.form1.get('organizationName').setErrors({ duplicate: false });
        this.ref.close(organizationNm);
    }
}
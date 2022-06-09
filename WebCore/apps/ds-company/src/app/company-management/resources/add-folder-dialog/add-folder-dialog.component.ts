import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn } from '@angular/forms';
import { ICompanyResource } from '@ds/employees/resources/shared/company-resource.model';
import { IUserInfo } from '@ajs/user';
import { ICompanyResourceFolder } from '@models';
@Component({
    selector: 'ds-add-folder-dialog',
    templateUrl: './add-folder-dialog.component.html',
    styleUrls: ['./add-folder-dialog.component.scss']
  })
  export class AddFolderDialogComponent implements OnInit {
    form1: FormGroup;
    formSubmitted: boolean;

    header: string;
    currResourceFolder: ICompanyResourceFolder;
    allFolders: Array<string>;

    constructor(
        private ref:MatDialogRef<AddFolderDialogComponent>,
        private fb: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data:any,
    ) {
        this.currResourceFolder = data.folder;
        this.allFolders         = data.folders;
        this.header             = data.folder.isNew ? 'Add Folder' : 'Edit Folder';
    }


    ngOnInit() {
        /// Build the form here
        this.form1 = this.fb.group({
            folderName:   [this.currResourceFolder.description, Validators.required],
        });
    }
    clear() {
        this.ref.close("");
    }
    save() {
        this.formSubmitted = true;
        var folderNm = this.form1.value.folderName.trim();
        if(!folderNm) return;
        if( this.allFolders.find(x => x.toLowerCase() == folderNm.toLowerCase())) {
            this.form1.get('folderName').setErrors({ duplicate: true });
            return;
        }
        this.form1.get('folderName').setErrors({ duplicate: false });
        this.ref.close(folderNm);
    }
}

import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn } from '@angular/forms';
import { ICompanyResourceFolder } from '@models';
@Component({
    selector: 'ds-delete-folder-dialog',
    templateUrl: './delete-folder-dialog.component.html',
    styleUrls: ['./delete-folder-dialog.component.scss']
  })
  export class DeleteFolderDialogComponent implements OnInit {
    form1: FormGroup;
    formSubmitted: boolean;

    currResourceFolder: ICompanyResourceFolder;
    allFolders: Array<ICompanyResourceFolder>;

    constructor(
        private ref:MatDialogRef<DeleteFolderDialogComponent>,
        private fb: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data:any,
    ) {
        this.currResourceFolder = data.folder;
        this.allFolders         = data.folders;
    }


    ngOnInit() {
        /// Build the form here
        this.form1 = this.fb.group({
            destinationFolder:   [null, Validators.required],
        });
    }
    clear() {
        this.ref.close(null);
    }
    delete() {
        this.formSubmitted = true;
        var folderId = this.form1.value.destinationFolder;
        folderId = folderId ? folderId : 0;

        if(this.currResourceFolder.resourceCount > 0 && !folderId) return;
        this.ref.close({destinationFolderId : folderId});
    }
}

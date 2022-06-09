import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn } from '@angular/forms';
import { IUserInfo } from '@ajs/user';
import { IEmployeeAttachmentFolder } from '@models/attachment.model';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { HttpErrorResponse } from "@angular/common/http";
import { MessageService } from '../../../services/message.service';
import { AttachmentService } from '../../../services/attachment.service';

@Component({
    selector: 'ds-add-attachment-folder-dialog',
    templateUrl: './add-attachment-folder-dialog.component.html',
    styleUrls: ['./add-attachment-folder-dialog.component.scss']
  })
  export class AddAttachmentFolderDialogComponent implements OnInit {
    form1: FormGroup;
    formSubmitted: boolean;

    header: string;
    currResourceFolder: IEmployeeAttachmentFolder;
    allFolders: Array<string>;
    isNew: boolean;
    isCompanyAttachments: boolean;


    constructor(
        private ref:MatDialogRef<AddAttachmentFolderDialogComponent>,
        private fb: FormBuilder,
        private confirm: ConfirmDialogService,
        private msg: MessageService,
        private attachmentService: AttachmentService,
        @Inject(MAT_DIALOG_DATA) public data:any,
    ) {
        this.currResourceFolder = data.folder;
        this.allFolders         = data.folders;
        this.header             = data.folder.isNew ? 'Add Folder' : 'Edit Folder';
        this.isNew              = data.folder.isNew;
        this.isCompanyAttachments = data.isCompanyAttachments;
    }


    ngOnInit() {
        /// Build the form here
        this.form1 = this.fb.group({
            folderName:   [this.currResourceFolder.description, Validators.required],
            employeeView:   [this.currResourceFolder.isEmployeeView],
        });
    }
    clear() {
        this.ref.close("");
    }
    save() {
        this.formSubmitted = true;
        var folderNm = this.form1.value.folderName.trim();
        var empView = this.form1.value.employeeView;
        if(!folderNm) return;
        if( this.allFolders.find(x => x.toLowerCase() == folderNm.toLowerCase())) {
            this.form1.get('folderName').setErrors({ duplicate: true });
            return;
        }
        this.form1.get('folderName').setErrors({ duplicate: false });
        this.ref.close({folderName: folderNm, folderId: null, employeeView: empView});
    }

    deletefolder(folder) {
        const options = {
            title: '',
            message: 'Delete folder ' + folder.description + '?',
            confirm: 'Delete'
        };
        this.confirm.open(options);
        this.confirm.confirmed().subscribe(confirmed => {
            if (confirmed) {
                this.attachmentService.deleteEmployeeFolder(folder.employeeFolderId).subscribe(result => {
                    if (result.success) {
                        this.ref.close({ folderName: null, folderId: folder.employeeFolderId });
                    }
                }, (err: HttpErrorResponse) => {
                    let errMsg = (err.error && err.error.errors && err.error.errors.length) ? err.error.errors[0].msg : err.message;
                    this.msg.setErrorMessage(errMsg);
                });
            }
        }, (error: HttpErrorResponse) => {
            this.msg.setErrorMessage(error.message);
        });
    }
}


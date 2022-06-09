import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn, ValidatorFn, ValidationErrors } from '@angular/forms';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { AttachmentService } from '../../../services/attachment.service';
import { EMPTY, Observable } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { IEmployeeAttachmentFolder, IEmployeeAttachment } from '@models/attachment.model';
import { MessageService } from '../../../services/message.service';
import { switchMap } from "rxjs/operators";
import { I } from "@angular/cdk/keycodes";
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';

@Component({
    selector: 'ds-add-employee-attachment-dialog',
    templateUrl: './add-attachment-dialog.component.html',
    styleUrls: ['./add-attachment-dialog.component.scss']
  })
  export class AddEmployeeAttachmentDialogComponent implements OnInit {
    form1: FormGroup;
    formSubmitted: boolean;
    userinfo: UserInfo;

    currentResource: IEmployeeAttachment;
    allFolders: Array<IEmployeeAttachmentFolder>;
    oldFolderId: number;
    resourceFile: File = null;
    resourceFileTypeError: boolean;
    resourceFileDuplError: boolean;
    isCompanyAttachment: boolean;
    acceptedFileTypes = ['.pdf', '.doc', '.docx', '.xlsx', '.xls', '.txt', '.rtf','.jpeg', '.jpg', '.jfif', '.png'];
    header: string = 'Add Attachment';

    constructor(
        private ref:MatDialogRef<AddEmployeeAttachmentDialogComponent>,
        private attachmentService: AttachmentService,
        private msg: MessageService,
        private fb: FormBuilder,
        private confirm: ConfirmDialogService,
        @Inject(MAT_DIALOG_DATA) public data:any,
    ) {
        this.allFolders      = data.folders;
        this.currentResource = data.resource;
        this.oldFolderId     = data.resource.folderId;
        this.isCompanyAttachment = data.isCompanyAttachment;
    }



    ngOnInit() {

        if (this.currentResource.resourceId !=0 ) {
            this.header = 'Edit Attachment';
        }
        /// Build the form here
        this.form1 = this.fb.group({
            destinationFolder:  [this.oldFolderId ? this.oldFolderId.toString() : '', Validators.required],
            resourceName:       [this.currentResource.name, Validators.required],
            employeeView:       this.currentResource.isViewableByEmployee,
        });

        if(this.currentResource.source)
            this.resourceFile = new File([],this.fileName(this.currentResource.source), null);
    }

    public fileName(url:string){
        var inx = url.lastIndexOf('/') > -1 ? url.lastIndexOf('/') : -1;
        if(inx < 0) inx = url.lastIndexOf('\\') > -1 ? url.lastIndexOf('\\') : -1;
        if(inx > -1) return decodeURIComponent(url.substring(inx+1));
        else         return "";
    }

    clear() {
        this.ref.close(null);
    }

    public browseClicked = ():void => {document.getElementById('resourceAdded').click();}

    public onChange($event){
        this.resourceFile = (<HTMLInputElement>event.target).files[0];
        if(!this.form1.value.resourceName){
            this.form1.patchValue({
                resourceName              : this.resourceFile.name.split('.')[0],
            });
        }
        this.formSubmitted = false;
    }

    public isDuplicateInFolder(folderId: number):boolean{
        var resourceName = this.resourceFile.name;

        this.resourceFileDuplError = false;
        var folder = this.allFolders.find(x=> x.employeeFolderId == folderId);
        if(folder){
            if( folder.attachments
                .filter(x => x.resourceId != this.currentResource.resourceId)
                .map(x => this.fileName(x.source).toLowerCase()).indexOf(resourceName.toLowerCase()) > -1 ){
                this.resourceFileDuplError = true;
            }
        }
        return this.resourceFileDuplError;
    }

    public save():void {
        this.formSubmitted = true;
        var folder = this.allFolders.find(x=> x.employeeFolderId.toString() == this.form1.value.destinationFolder  );
        if( folder.attachments.find(x => x.name.toLowerCase() == this.form1.value.resourceName.toLowerCase()  &&
            x.resourceId != this.currentResource.resourceId )) {
            this.form1.get('resourceName').setErrors( {duplicate : true});
            return;
        }
        if(this.form1.get('resourceName').errors && this.form1.get('resourceName').errors.duplicate)
            this.form1.get('resourceName').errors.duplicate = null;

        // File & Extn validations
        if(this.resourceFile){
            if(this.acceptedFileTypes.findIndex( x => this.resourceFile.name.toLowerCase().indexOf(x) > -1 ) == -1){
                this.resourceFileTypeError = true;
                return;
            }
            if(this.isDuplicateInFolder(this.currentResource.employeeId))
                return;
        }

        let subscription:Observable<IEmployeeAttachment> = null;
        if(this.form1.valid){
            this.currentResource.folderId = this.form1.value.destinationFolder;
            this.currentResource.name   = this.form1.value.resourceName;
            this.currentResource.isViewableByEmployee  = this.form1.value.employeeView;

            let updateSubscription = null;
            // If there is no content associated with resource, then the resource has already been uploaded
            if(this.resourceFile && this.resourceFile.size > 0)
                updateSubscription = this.attachmentService.uploadEmployeeAttachment(this.currentResource, this.resourceFile);
            else
                updateSubscription = this.attachmentService.uploadEmployeeAttachmentInfo(this.currentResource);

            updateSubscription.subscribe(result => {
                var resource=result.data;
                if(this.oldFolderId == resource.folderId){
                    var inx = this.allFolders.findIndex(x=> x.employeeFolderId == resource.folderId);
                    var resourceInx = this.allFolders[inx].attachments.findIndex(x=> x.resourceId == resource.resourceId);
                    if(resourceInx>-1)
                        this.allFolders[inx].attachments[resourceInx] = resource;
                    else{
                        this.allFolders[inx].attachments.push(resource);
                        this.allFolders[inx].attachmentCount++;
                    }
                } else {
                    var oldInx = this.allFolders.findIndex(x=> x.employeeFolderId == this.oldFolderId);
                    var resourceInx = -1;
                    if(oldInx>-1){
                        resourceInx = this.allFolders[oldInx].attachments.findIndex(x=> x.resourceId == resource.resourceId);
                        if(resourceInx>-1){
                            this.allFolders[oldInx].attachments.splice(resourceInx, 1);
                            this.allFolders[oldInx].attachmentCount--;
                        }
                    }

                    var newInx = this.allFolders.findIndex(x=> x.employeeFolderId == resource.folderId);
                    this.allFolders[newInx].attachments = this.allFolders[newInx].attachments ? this.allFolders[newInx].attachments : [];
                    this.allFolders[newInx].attachments.push(resource);
                    this.allFolders[newInx].attachmentCount++;
                }
                this.ref.close(this.allFolders);
            }, (error: HttpErrorResponse) => {
                let errorMsg = error.error.errors != null && error.error.errors.length
                ? error.error.errors[0].msg
                : error.message;
                this.msg.setErrorMessage(errorMsg);
            });
        }
    }

    public isFileSelected(): boolean  {
        return this.resourceFile && (this.resourceFile.size > 0 || this.currentResource.resourceId !=0);
    };

    public deleteAttachment(attachment): void {
        var modalOptions = null
        this.attachmentService.isAttachmentForeignKey(attachment.resourceId).pipe(
            switchMap( (modalMessage) => {
                let message = '';
            if (modalMessage.data == '' || modalMessage.data == null) {
                message = 'Delete attachment ' + attachment.name + '?';
            } else {
                message = modalMessage.data;
            }
            const options = {
                title: '',
                message: message,
                confirm: 'Delete'
            };
            this.confirm.open(options);
            return this.confirm.confirmed();
        }),
            switchMap(confirmResult => {
                if(!confirmResult) return EMPTY;
                else
                {
                    const changes = {
                        attachmentId: attachment.resourceId,
                        remarkId: null,
                        change: 'd',
                        fileName: attachment.name + attachment.extension
                      }
                return this.attachmentService.removeEmployeeAttachmentForeignKeyRecord(changes).pipe(
                    switchMap( x =>
                        this.attachmentService.deleteEmployeeAttachment(attachment.resourceId, this.isCompanyAttachment)
                    ));
            }}),
        ).subscribe( x => {
                if (x.success) {
                    var removedFolderIndex = -1;
                    var removedAttachmentIndex = -1;
                    this.allFolders.forEach((currFolder, folderIndex) => {
                        if (currFolder.employeeFolderId == attachment.folderId) {

                            currFolder.attachments.forEach((currAttachment, attachmentIndex) => {
                                if (currAttachment.resourceId == attachment.resourceId) {
                                    removedFolderIndex = folderIndex;
                                    removedAttachmentIndex = attachmentIndex;
                                    return false;
                                }
                            });
                        }
                        if (removedFolderIndex > -1) {
                            return false;
                        }
                    });
                    if (removedFolderIndex > -1 && removedAttachmentIndex > -1) {
                        this.allFolders[removedFolderIndex].attachments.splice(removedAttachmentIndex, 1);
                        this.allFolders[removedFolderIndex].attachmentCount = this.allFolders[removedFolderIndex].attachmentCount - 1;
                    }
                    this.ref.close(this.allFolders);
                }
            },(error: HttpErrorResponse) => {
                this.msg.setErrorMessage(error.message);
        });
    }
}

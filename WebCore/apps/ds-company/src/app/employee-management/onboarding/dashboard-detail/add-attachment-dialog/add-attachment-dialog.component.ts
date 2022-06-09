import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn, ValidatorFn, ValidationErrors } from '@angular/forms';
import { IEmployeeAttachment } from '@ds/core/employees/employee-attachments/employee-attachment.model';
import { EmployeeAttachmentFolder } from '@ds/performance/attachments/shared/models/';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { EmployeeAttachmentApiService } from '@ds/core/employees/employee-attachments/employee-attachment-api.service';
import { Observable } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
    selector: 'ds-add-attachment-dialog',
    templateUrl: './add-attachment-dialog.component.html',
    styleUrls: ['./add-attachment-dialog.component.scss']
  })
  export class AddAttachmentDialogComponent implements OnInit {
    form1: FormGroup;
    formSubmitted: boolean;
    userinfo: UserInfo;

    currentResource: IEmployeeAttachment;
    allFolders: Array<EmployeeAttachmentFolder> = [];
    allAttachments: Array<IEmployeeAttachment>;
    oldFolderId: number;
    resourceFile: File = null;
    resourceFileTypeError: boolean;
    resourceFileDuplError: boolean;
    
    resourceTypes = [   { id: 1, desc: "Document" }, 
                        { id: 2, desc: "Link" }, 
                        { id: 4, desc: "Video" }];
    acceptedFileTypes = ['.pdf', '.doc', '.docx', '.xlsx', '.xls', '.txt', '.rtf']

    constructor(
        private ref:MatDialogRef<AddAttachmentDialogComponent>,
        private attachmentService: EmployeeAttachmentApiService, 
        private msg: NgxMessageService,
        private fb: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data:any,
    ) {
        this.allAttachments  = data.allAttachments;
        this.currentResource = data.attachment;
        this.allFolders      = data.allFolders;
        this.oldFolderId     = data.attachment.folderId;
    }
    
    

    ngOnInit() {
        /// Build the form here
        this.form1 = this.fb.group({
            destinationFolder:  [this.oldFolderId ? this.oldFolderId.toString() : '', Validators.required], 
            resourceType:       this.currentResource.sourceType.toString(),
            resourceName:       [this.currentResource.name, Validators.required],
            resourceURL:        [this.currentResource.source, null],
            isViewableResource:  this.currentResource.isViewableByEmployee,
        });

        const urlValidator = this.urlValidatorFn(this.form1.controls.resourceType as FormControl );
        (this.form1.controls.resourceURL as FormControl).setValidators([urlValidator
            , Validators.pattern(/^(http[s]?:\/\/(www\.)?|){1}([0-9A-Za-z-\.@:%_\+~#=]+)+(\.[a-zA-Z]{2,3}){1}(\/[^\?]*)?(\?[^\?]*)?$/)]);

        if(this.currentResource.source && this.currentResource.sourceType == 1)
            this.resourceFile = new File([],this.fileName(this.currentResource.source), null);
    }

    public fileName(url:string){
        var inx = url.lastIndexOf('/') > -1 ? url.lastIndexOf('/') : -1;
        if(inx < 0) inx = url.lastIndexOf('\\') > -1 ? url.lastIndexOf('\\') : -1;
        if(inx > -1) return decodeURIComponent(url.substring(inx+1));
        else         return "";
    }

    public urlValidatorFn(typeCtrl:FormControl): ValidatorFn{
        return (control: FormControl): ValidationErrors | null => {
            const url = control.value;
            return (typeCtrl.value != '1' && !url) ? {'required': true} : null;
        };
    }

    setResourceType(){
        this.currentResource.sourceType =  parseInt( this.form1.value.resourceType );
        this.form1.patchValue({
            resourceURL : '',
        });
        this.resourceFile = null;
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

    public isDuplicate():boolean{
        var resourceName = this.resourceFile.name;
        
        this.resourceFileDuplError = false;
        if( this.allAttachments.filter(x => x.resourceId != this.currentResource.resourceId)
            .map(x => this.fileName(x.source).toLowerCase()).indexOf(resourceName.toLowerCase()) > -1 ){
            this.resourceFileDuplError = true;
        }
        return this.resourceFileDuplError;
    }

    public save():void {
        this.formSubmitted = true;
        var folder = this.allFolders.find(x=> x.employeeFolderId.toString() == this.form1.value.destinationFolder  );
        if( this.allAttachments.find(x => x.name.toLowerCase() == this.form1.value.resourceName.toLowerCase()  &&
            x.resourceId != this.currentResource.resourceId )) {
            this.form1.get('resourceName').setErrors( {duplicate : true});
            return;
        }
        if(this.form1.get('resourceName').errors && this.form1.get('resourceName').errors.duplicate)
            this.form1.get('resourceName').errors.duplicate = null;

        // File & Extn validations
        if(this.currentResource.sourceType == 1){
            if(this.resourceFile){
                if(this.acceptedFileTypes.findIndex( x => this.resourceFile.name.toLowerCase().indexOf(x) > -1 ) == -1){
                    this.resourceFileTypeError = true;
                    return;
                }
                if(this.isDuplicate())
                    return;
                
            } else {
                if(this.currentResource.resourceId){
                    // There is no change to resource file
                } else {
                    return;
                }
            }
        }

        let subscription:Observable<IEmployeeAttachment> = null;
        if(this.form1.valid){
            this.currentResource.folderId = this.form1.value.destinationFolder;
            this.currentResource.name   = this.form1.value.resourceName;
            this.currentResource.sourceType = this.form1.value.resourceType;
            this.currentResource.isViewableByEmployee  = this.form1.value.isViewableResource;
            this.currentResource.source         = this.form1.value.resourceURL;
            
            let updateSubscription = null;
            // If there is no content associated with resource, then the resource has already been uploaded
            if(this.resourceFile && this.resourceFile.size > 0 )
                updateSubscription = this.attachmentService.uploadEmployeeAttachment(this.currentResource, this.resourceFile);
            else
                updateSubscription = this.attachmentService.updateEmployeeAttachment(this.currentResource);
            
            updateSubscription.subscribe((result: {data:IEmployeeAttachment}) => {
                this.ref.close(result.data);
            }, (error: HttpErrorResponse) => {
                let errorMsg = error.error.errors != null && error.error.errors.length
                ? error.error.errors[0].msg
                : error.message;
                this.msg.setErrorMessage(errorMsg);
            });
        }
    }
}
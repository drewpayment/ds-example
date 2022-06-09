import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { FormControl, FormGroup, Validators, FormBuilder, ValidatorFn, ValidationErrors } from '@angular/forms';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { ResourceService } from '../shared/resource.service';
import { Observable } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { ICompanyResource, ICompanyResourceFolder } from '@models';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
    selector: 'ds-add-resource-dialog',
    templateUrl: './add-resource-dialog.component.html',
    styleUrls: ['./add-resource-dialog.component.scss']
  })
  export class AddResourceDialogComponent implements OnInit {
    form1: FormGroup;
    formSubmitted: boolean;
    userinfo: UserInfo;

    currentResource: ICompanyResource;
    allFolders: Array<ICompanyResourceFolder>;
    oldFolderId: number;
    resourceFile: File = null;
    resourceFileTypeError: boolean;
    resourceFileDuplError: boolean;

    resourceSecurityLevel: any;
    securityLevels = [  { id: 2, desc: "Company Administrators" },
                        { id: 4, desc: "Supervisors" },
                        { id: 3, desc: "Everyone" }];
    resourceTypes = [   { id: 1, desc: "Document" },
                        { id: 2, desc: "Link" },
                        { id: 4, desc: "Video" }];
    acceptedFileTypes = ['.pdf', '.doc', '.docx', '.xlsx', '.xls', '.txt', '.rtf']

    constructor(
        private ref:MatDialogRef<AddResourceDialogComponent>,
        private resourceService: ResourceService,
        private msg: NgxMessageService,
        private fb: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data:any,
    ) {
        this.allFolders      = data.folders;
        this.currentResource = data.resource;
        this.oldFolderId     = data.resource.companyResourceFolderId;
    }



    ngOnInit() {
        /// Build the form here
        this.form1 = this.fb.group({
            destinationFolder:  [this.oldFolderId ? this.oldFolderId.toString() : '', Validators.required],
            resourceType:       this.currentResource.resourceTypeId.toString(),
            resourceName:       [this.currentResource.resourceName, Validators.required],
            resourceURL:        [this.currentResource.source, null],
            securityLevel:      this.currentResource.securityLevel,
            isManagerResource:  this.currentResource.isManagerLink,
        });

        const urlValidator = this.urlValidatorFn(this.form1.controls.resourceType as FormControl );
        (this.form1.controls.resourceURL as FormControl).setValidators([urlValidator
            , Validators.pattern(/^(http[s]?:\/\/(www\.)?|){1}([0-9A-Za-z-\.@:%_\+~#=]+)+(\.[a-zA-Z]{2,3}){1}(\/[^\?]*)?(\?[^\?]*)?$/)]);

        if(this.currentResource.source && this.currentResource.resourceTypeId == 1)
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
        this.currentResource.resourceTypeId =  parseInt( this.form1.value.resourceType );
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

    public setSecurityLevel(){
        this.currentResource.securityLevel = this.form1.value.securityLevel;
    }

    public isDuplicateInFolder(folderId: number):boolean{
        var resourceName = this.resourceFile.name;

        this.resourceFileDuplError = false;
        var folder = this.allFolders.find(x=> x.companyResourceFolderId == folderId);
        if(folder){
            if( folder.resourceList
                .filter(x => x.resourceId != this.currentResource.resourceId)
                .map(x => this.fileName(x.source).toLowerCase()).indexOf(resourceName.toLowerCase()) > -1 ){
                this.resourceFileDuplError = true;
            }
        }
        return this.resourceFileDuplError;
    }

    public save():void {
        this.formSubmitted = true;
        var folder = this.allFolders.find(x=> x.companyResourceFolderId.toString() == this.form1.value.destinationFolder  );
        if( folder.resourceList.find(x => x.resourceName.toLowerCase() == this.form1.value.resourceName.toLowerCase()  &&
            x.resourceId != this.currentResource.resourceId )) {
            this.form1.get('resourceName').setErrors( {duplicate : true});
            return;
        }
        if(this.form1.get('resourceName').errors && this.form1.get('resourceName').errors.duplicate)
            this.form1.get('resourceName').errors.duplicate = null;

        // File & Extn validations
        if(this.currentResource.resourceTypeId == 1){
            if(this.resourceFile){
                if(this.acceptedFileTypes.findIndex( x => this.resourceFile.name.toLowerCase().indexOf(x) > -1 ) == -1){
                    this.resourceFileTypeError = true;
                    return;
                }
                if(this.isDuplicateInFolder(this.currentResource.companyResourceFolderId))
                    return;
            } else {
                if(this.currentResource.resourceId){
                    // There is no change to resource file
                } else {
                    return;
                }
            }
        }

        let subscription:Observable<ICompanyResource> = null;
        if(this.form1.valid){
            this.currentResource.companyResourceFolderId = this.form1.value.destinationFolder;
            this.currentResource.resourceName   = this.form1.value.resourceName;
            this.currentResource.resourceTypeId = this.form1.value.resourceType;
            this.currentResource.securityLevel  = this.form1.value.securityLevel;
            this.currentResource.isManagerLink  = this.form1.value.isManagerResource;
            this.currentResource.source         = this.form1.value.resourceURL;

            let updateSubscription = null;
            // If there is no content associated with resource, then the resource has already been uploaded
            if(this.resourceFile && this.resourceFile.size > 0 )
                updateSubscription = this.resourceService.uploadCompanyResource(this.currentResource, this.resourceFile);
            else
                updateSubscription = this.resourceService.updateCompanyResource(this.currentResource);

            updateSubscription.subscribe(resource => {
                if(this.oldFolderId == resource.companyResourceFolderId){
                    var inx = this.allFolders.findIndex(x=> x.companyResourceFolderId == resource.companyResourceFolderId);
                    var resourceInx = this.allFolders[inx].resourceList.findIndex(x=> x.resourceId == resource.resourceId);
                    if(resourceInx>-1)
                        this.allFolders[inx].resourceList[resourceInx] = resource;
                    else{
                        this.allFolders[inx].resourceList.push(resource);
                        this.allFolders[inx].resourceCount++;
                    }
                } else {
                    var oldInx = this.allFolders.findIndex(x=> x.companyResourceFolderId == this.oldFolderId);
                    var resourceInx = -1;
                    if(oldInx>-1){
                        resourceInx = this.allFolders[oldInx].resourceList.findIndex(x=> x.resourceId == resource.resourceId);
                        if(resourceInx>-1){
                            this.allFolders[oldInx].resourceList.splice(resourceInx, 1);
                            this.allFolders[oldInx].resourceCount--;
                        }
                    }

                    var newInx = this.allFolders.findIndex(x=> x.companyResourceFolderId == resource.companyResourceFolderId);
                    this.allFolders[newInx].resourceList = this.allFolders[newInx].resourceList ? this.allFolders[newInx].resourceList : [];
                    this.allFolders[newInx].resourceList.push(resource);
                    this.allFolders[newInx].resourceCount++;
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
}

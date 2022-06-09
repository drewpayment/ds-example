import { Component, Inject, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators, FormBuilder, ValidatorFn, ValidationErrors } from '@angular/forms';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { CustomPagesService } from "apps/ds-company/src/app/services/custom-pages.service";
import { ResourceService } from 'apps/ds-company/src/app/services/resource.service';
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { ICompanyResource, ICompanyResourceFolder } from '@models';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";

@Component({
  selector: 'ds-manage-resource-item-dialog',
  templateUrl: './manage-resource-item-dialog.component.html',
  styleUrls: ['./manage-resource-item-dialog.component.scss']
})
export class ManageResourceItemDialogComponent implements OnInit {
  form: FormGroup;
  formSubmitted: boolean;
  userAccount: UserInfo;
  isLoading: boolean = true;

  currentResource: ICompanyResource;
  folders: Array<ICompanyResourceFolder> = [];

  oldFolderId: number;
  resourceFile: File = null;
  resourceFileTypeError: boolean;
  routeText: string = 'Document';

  pageType: any;
  whereFrom: string; //This component is called from two different places right now. This variable holds information about the calling component.

  securityLevels = [  { id: 2, desc: "Company Administrators" },
    { id: 4, desc: "Supervisors" },
    { id: 3, desc: "Everyone" }
  ];

  resourceTypes = [   { id: 1, desc: "Document" },
    { id: 2, desc: "Link" },
    { id: 4, desc: "Video" }
  ];

  acceptedFileTypes = ['.pdf', '.doc', '.docx', '.xlsx', '.xls', '.txt', '.rtf']

  constructor(
    private ref:MatDialogRef<ManageResourceItemDialogComponent>,
    private customPagesService: CustomPagesService,
    private resourceService: ResourceService,
    private msg: NgxMessageService,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data:any,
  ) {
    this.userAccount = data.userAccount;
    this.currentResource = data.resource;
    this.pageType = data.resource.resourceTypeId;
    this.oldFolderId = data.resource.companyResourceFolderId;
    this.folders = data.folders;
    this.whereFrom = data.whereFrom;
  }

  ngOnInit() {
    this.routeText = Utilities.getRouteByPageType(this.currentResource.resourceTypeId);
    this.buildForm();
  }

  buildForm() {
    this.form = this.fb.group({
      destinationFolder:  [this.oldFolderId ? this.oldFolderId.toString() : '', Validators.required],
      resourceType:       this.currentResource.resourceTypeId.toString(),
      resourceName:       [this.currentResource.resourceName, Validators.required],
      resourceURL:        [this.currentResource.source, (this.currentResource.resourceTypeId != 1) ? [Validators.required, Validators.pattern('(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?')] : []],
      securityLevel:      this.currentResource.securityLevel,
      isManagerResource:  this.currentResource.isManagerLink,
    });
  }

  validateFileName(url:string) {
    var inx = url.lastIndexOf('/') > -1 ? url.lastIndexOf('/') : -1;
    if(inx < 0) inx = url.lastIndexOf('\\') > -1 ? url.lastIndexOf('\\') : -1;
    if(inx > -1)
      return decodeURIComponent(url.substring(inx+1));
    else
      return "";
  }

  validateUrl(typeCtrl:FormControl): ValidatorFn {
    return (control: FormControl): ValidationErrors | null => {
        const url = control.value;
        return (typeCtrl.value != '1' && !url) ? {'required': true} : null;
    };
  }

  clear() {
    this.ref.close(null);
  }

  browseClicked = ():void => {document.getElementById('resourceAdded').click();}

  onChange($event) {
    this.resourceFileTypeError = false;
    this.resourceFile = (<HTMLInputElement>event.target).files[0];
    if(!this.form.value.resourceName) {
        this.form.patchValue({
            resourceName: this.resourceFile.name.split('.')[0],
        });
    }
  }

  setSecurityLevel() {
    this.currentResource.securityLevel = this.form.value.securityLevel;
  }

  save() : void {
    this.formSubmitted = true;

    // File & Extn validations
    if(this.currentResource.resourceTypeId == 1) {
        if(this.resourceFile) {
            if(this.acceptedFileTypes.findIndex( x => this.resourceFile.name.toLowerCase().indexOf(x) > -1 ) == -1){
                this.resourceFileTypeError = true;
                return;
            }
        } else
        {
            if(this.currentResource.resourceId){
                // There is no change to resource file
            } else {
                return;
            }
        }
    }

    let subscription:Observable<ICompanyResource> = null;
    if (this.form.valid) {
      this.currentResource.companyResourceFolderId = this.form.value.destinationFolder;
      this.currentResource.resourceName   = this.form.value.resourceName;
      this.currentResource.resourceTypeId = this.form.value.resourceType;
      this.currentResource.securityLevel  = this.form.value.securityLevel;
      this.currentResource.isManagerLink  = this.form.value.isManagerResource;
      this.currentResource.source         = this.form.value.resourceURL;

      if (this.currentResource.resourceTypeId == 1) {
        this.resourceService.uploadCompanyResource(this.currentResource, this.resourceFile)
        .subscribe(result => {
          if (this.whereFrom == 'resources') {
            this.ref.close({ savedResourceFolderList: result.data });
          }
          else {
            this.resourceService.getOnboardingWorkflowResourceByResourceId(result.resourceId)
            .pipe(tap( newResource => {
              this.ref.close({ savedResource: newResource });
            })).subscribe();
          }
        },
        (err: HttpErrorResponse) => {
            if (err.error && err.error.errors && err.error.errors.length) {
              this.msg.setErrorMessage(err.error.errors[0].msg);
            } else {
              this.msg.setErrorMessage(err.message);
            }
        });
      }
      else {
        this.resourceService.updateCompanyResource(this.currentResource)
        .subscribe(result => {
          if (this.whereFrom == 'resources') {
            this.ref.close({ savedResourceFolderList: result });
          }
          else {
            this.resourceService.getOnboardingWorkflowResourceByResourceId(result.resourceId)
            .pipe(tap( newResource => {
              this.ref.close({ savedResource: newResource });
            })).subscribe();
          }
        },
        (err: HttpErrorResponse) => {
          if (err.error && err.error.errors && err.error.errors.length) {
            this.msg.setErrorMessage(err.error.errors[0].msg);
          } else {
            this.msg.setErrorMessage(err.message);
          }
        });
      }
    }
  }
}

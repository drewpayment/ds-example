import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { FormGroup, FormBuilder } from '@angular/forms';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { tap } from 'rxjs/operators';
import { CustomPagesService } from "apps/ds-company/src/app/services/custom-pages.service";
import { ResourceService } from 'apps/ds-company/src/app/services/resource.service';
import { ManageResourceItemDialogComponent } from "apps/ds-company/src/app/company-management/onboarding/custom-pages/manage-resources/manage-resource-item-dialog/manage-resource-item-dialog.component";
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { ICompanyResource, ICompanyResourceFolder } from '@models';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: 'ds-manage-resources-dialog',
  templateUrl: './manage-resources-dialog.component.html',
  styleUrls: ['./manage-resources-dialog.component.scss']
})
export class ManageResourcesDialogComponent implements OnInit {
  form: FormGroup;
  formSubmitted: boolean;
  resources: Array<ICompanyResource> = [];
  searchResources: string;
  isLoading: boolean = true;
  resourceIsDocument: boolean = false;
  resourceIsLink: boolean = false;
  resourceIsVideo: boolean = false;

  allData: any;
  userAccount: UserInfo;
  listIsEmpty: boolean = true;
  selectedResources: Array<ICompanyResource> = [];
  pageType: number;
  pageFrom: string;
  selectedClientOrganization: number;
  routeText: string = 'Document';

  folders: Array<ICompanyResourceFolder> = [];

  disabled: boolean = false;
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
    private ref:MatDialogRef<ManageResourcesDialogComponent>,
    private customPagesService: CustomPagesService,
    private resourceService: ResourceService,
    private msg: NgxMessageService,
    private dialog: MatDialog,
    private confirmDialog: MatDialog,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data:any,
  ) {
    this.allData = data;
    this.userAccount = {...data.userAccount};
    this.selectedResources = data.selectedResources.map(x => Object.assign({}, x));
    this.pageType = data.pageType;
    this.pageFrom = data.pageFrom;
    this.selectedClientOrganization = data.selectedClientOrganization;
  }

  ngOnInit() {
    this.routeText = Utilities.getRouteByPageType(this.pageType);
    this.resourceService.getCompanyResourcesByClient(this.selectedClientOrganization)
      .pipe(tap(resources => {
        this.resources = resources;

        if (!resources.length){
          this.popupResourceDialog(null);
        }
        else {
          this.resources.forEach((resource, index) => {
            if (resource.resourceTypeId == 1) {
              this.resourceIsDocument = true;
              this.listIsEmpty = false;
              return;
            }
            if (resource.resourceTypeId == 2) {
              this.resourceIsLink = true;
              this.listIsEmpty = false;
              return;
            }
            if (resource.resourceTypeId == 4) {
              this.resourceIsVideo = true;
              this.listIsEmpty = false;
              return;
            }
          });

          if (this.pageType == 1 && this.resourceIsDocument == false) {
            this.popupResourceDialog(null);
          }
          else if (this.pageType == 2 && this.resourceIsLink == false) {
            this.popupResourceDialog(null);
          }
          else if (this.pageType == 4 && this.resourceIsVideo == false) {
            this.popupResourceDialog(null);
          }
          else {
            this.selectedResources.forEach((curSelectedResource, index) => {
              this.resources.forEach((currResource, index1) => {
                if (curSelectedResource.resourceId == currResource.resourceId) {
                  currResource.isSelectedResource = true;
                }
              });
            });
          }
        }

        this.buildForm();
        this.isLoading = false;
      })
    ).subscribe();
  }

  buildForm() {
    this.form = this.fb.group({
      searchResources: [''],
    });

    this.form.get('searchResources').valueChanges.subscribe(val => {
      this.searchResources = this.form.get('searchResources').value;
    });
  }

  popupResourceDialog(currResource:ICompanyResource) {
    let currentResource = <ICompanyResource> {
        companyResourceFolderId: 0,
        resourceId:0,
        resourceTypeId: this.pageType,
        resourceName: '',
        isNew: !currResource ? true : false,
        resourceFormat: '',
        securityLevel: 3,
        isManagerLink: false,
        modified: new Date(),
        modifiedBy: this.userAccount.userId,
        clientId: this.userAccount.lastClientId || this.userAccount.clientId,
        doesFileExist : false,
        isAzure : false,
        azureAccount : 0,
        cssClass: '',
        source: '',
        currentSource: '',
        isSelectedResource : false,
        previewResourceCssClass : false,
        addedDate : new Date(),
        addedBy : this.userAccount.userId,
        isDeleted : false,
        isFileReselected : false,
        hovered: false,
    };

    this.disabled = true;
    this.resourceService.getCompanyResourceFoldersByClient(this.userAccount.lastClientId || this.userAccount.clientId)
      .pipe(tap(folders => {
        this.folders = folders;
        let config = new MatDialogConfig<any>();
        config.width = "500px";
        config.data = {userAccount: this.userAccount, folders: this.folders, resource: currentResource, whereFrom: 'workflow'};
        this.dialog.open<ManageResourceItemDialogComponent, any, string>(ManageResourceItemDialogComponent, config)
        .afterClosed()
        .subscribe((result: any) => {
          this.disabled = false;
            currentResource = result.savedResource;
            currentResource.isSelectedResource = true;
            this.listIsEmpty = false;

            this.resources.unshift(currentResource);
            this.selectedResources.unshift(currentResource);
            this.resources = this.resources.slice();
            this.selectedResources = this.selectedResources.slice();

            this.msg.setSuccessMessage("Resource added successfully.");
        });
    })).subscribe();
  }

  save() {
    this.ref.close(this.selectedResources);
  }

  clear() {
    this.ref.close(null);
  }

  selectResource(selectedResource: ICompanyResource) {
    if (!selectedResource.isSelectedResource) {
      selectedResource.isSelectedResource = true;
      this.selectedResources.push(selectedResource);
    }
    else {
      this.selectedResources.forEach((currResource, index) => {
        if (currResource.resourceId == selectedResource.resourceId) {
          currResource.isSelectedResource = false;
          selectedResource.isSelectedResource = false;
          this.selectedResources.splice(index, 1);
        }
      });
    }
  }
}

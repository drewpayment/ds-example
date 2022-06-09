import { Component, OnInit, AfterViewInit, AfterViewChecked } from '@angular/core';
import { DsStyleLoaderService, IStyleAsset } from '@ajs/ui/ds-styles/ds-styles.service';

import { ICompanyResourceFolder } from '../shared/company-resource-folder.model'
import { IEmployeeAttachmentFolderDetail } from '../shared/employee-attachment-folder-detail.model';

import { CompanyResourcesService } from '../shared/company-resources-api.service';
import { EmployeeAttachmentsService } from '../shared/employee-attachments-api.service';
import { ResourceApiService } from '@ds/core/resources/shared/resources-api.service';

import { UserInfo } from '@ds/core/shared';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { map } from 'rxjs/operators';
import * as _ from "lodash";
import { DsCustomFilterCallbackPipe } from '@ds/core/shared/ds-custom-filter-callback.pipe';
import { ICompanyResource } from '../shared/company-resource.model';
import { Observable } from 'rxjs';
import { Maybe } from '@ds/core/shared/Maybe';
import { AccountService } from '@ds/core/account.service';
import { DsResourceApi } from '@ajs/core/ds-resource/ds-resource-api.service';
import { DsConfigurationService } from "@ajs/core/ds-configuration/ds-configuration.service";

@Component({
  selector: 'ds-resources-list',
  templateUrl: './resources-list.component.html',
  styleUrls: ['./resources-list.component.scss']
})
export class ResourcesListComponent implements OnInit, AfterViewChecked {
    mainStyle: IStyleAsset;
    user: UserInfo;

    selectedCompanyResourceFolder: ICompanyResourceFolder;
    nonEmptyCompanyResourceFolders: ICompanyResourceFolder[] = [];
    companyResourceFolders: ICompanyResourceFolder[] = [];
    showCompanyResourceFiles: boolean;
    showCompanyResourcesCard: boolean;

    selectedAttachmentFolder: IEmployeeAttachmentFolderDetail;
    nonEmptyAttachmentFolders: IEmployeeAttachmentFolderDetail[] = [];
    attachmentFolders: IEmployeeAttachmentFolderDetail[] = [];
    showAttachmentFiles: boolean;
    showAttachmentsCard: boolean;

    isLoading: boolean;

    constructor(
        private styles: DsStyleLoaderService,
        private companyResourcesService: CompanyResourcesService,
        private attachmentService: EmployeeAttachmentsService,
        private resourceService: ResourceApiService,
        private accountService: AccountService,
        private msgSvc: DsMsgService,
        private dsResourceApi: DsResourceApi,
        private dsConfig: DsConfigurationService,
    ) { }

    ngOnInit() {
        this.showCompanyResourcesCard = true;
        this.showAttachmentsCard = true;
        this.showCompanyResourceFiles = false;
        this.showAttachmentFiles = false;

        this.isLoading = true;

        this.accountService.getUserInfo().subscribe(user => {
            this.user = user;

            this.companyResourcesService.getCompanyResourcesByEmployee(this.user.employeeId).subscribe(companyResourceFolders => {
                this.companyResourceFolders = companyResourceFolders;
                this.nonEmptyCompanyResourceFolders = _.filter(companyResourceFolders, (folder: ICompanyResourceFolder) => {
                    return folder.resourceCount > 0;
                });

                this.attachmentService.getFolders(this.user.employeeId).subscribe(employeeAttachmentFolders => {
                    this.attachmentFolders = employeeAttachmentFolders;
                    this.nonEmptyAttachmentFolders = _.filter(employeeAttachmentFolders, (folder: IEmployeeAttachmentFolderDetail) =>{
                        return folder.attachmentCount > 0;
                    });

                    this.isLoading = false;
                });

            });
        });
    }

    companyResourceFolderClicked(folder: ICompanyResourceFolder) {        
        this.selectedCompanyResourceFolder = folder;
        this.showAttachmentsCard = false;
        this.showCompanyResourceFiles = true;

        this.selectedCompanyResourceFolder.resourceList.forEach(a => {
            if (a.isAzure) {
                this.getAzureLink(a.resourceId).subscribe(url => {
                    a.azureUrl = url;
                });
            }
        });
    }

    attachmentFolderClicked(folder: IEmployeeAttachmentFolderDetail) {
        this.selectedAttachmentFolder = folder;
        this.showCompanyResourcesCard = false;
        this.showAttachmentFiles = true;

        this.selectedAttachmentFolder.attachments.forEach(a => {
            if (a.isAzure) {
                this.getAzureLink(a.resourceId).subscribe(url => {
                    a.azureUrl = url;
                });
            }
        });
    }

    backToCompanyResourceFolders() {
        this.showAttachmentsCard = true;
        this.showCompanyResourceFiles = false;
        this.selectedCompanyResourceFolder = null;
    }

    backToAttachmentFolders() {
        this.showCompanyResourcesCard = true;
        this.showAttachmentFiles = false;
        this.selectedAttachmentFolder = null;
    }

    getMappedExtension(extension) {
        extension = new Maybe(extension).map(x => x.replace(".", "")).map(x => x.toUpperCase()).valueOr("");
        if (extension == "JFIF" || extension == "JPG")
            return "JPEG";
        return extension;
    }

    filterOutEmptyCompanyResourceFolders(folder: ICompanyResourceFolder) {
        return folder.resourceCount > 0;
    }

    filterOutEmptyAttachmentFolders(folder: IEmployeeAttachmentFolderDetail) {
        return folder.attachmentCount > 0;
    }

    filterDocumentResources(resource: ICompanyResource) {
        return resource.resourceTypeId == 1;
    }

    filterLinkResources(resource: ICompanyResource) {
        return resource.resourceTypeId == 2;
    }

    filterVideoResources(resource: ICompanyResource) {
        return resource.resourceTypeId == 4;
    }

    redirectTo(url: string) {
        window.open(url, "_blank");
    }

    downloadForm(resource: ICompanyResource) {
        if(resource.sourceType === 2 && resource.isATFile)
        {
            let encodedCss: string = document.querySelector("link[id$='StyleSheetMain']").attributes["href"].value;
            encodedCss = window.btoa(this.dsConfig.getAbsoluteUrl(encodedCss)).replace(/\//gi, '_');
            let url = resource.source.replace(/\{cssUrl\}/g,encodedCss);

            this.dsResourceApi.getUrlToDownload(url);
        }
        else if (resource.isAzure) {
            this.resourceService.openAzureLink(resource.resourceId);
        }
        else {
            this.resourceService.downloadResource(resource.resourceId, resource.resourceFormat);
        }
    }

    getAzureLink(resourceId: number): Observable<any> {
        return this.resourceService.getAzureLink(resourceId);
    }

    /**
     * We tell DsStyleLoaderService that this component should use main stylesheet AFTER OnInit and AfterViewInit
     * because we need to make sure that everything is resolved above this component. The DsStyleLoaderService is not
     * instantiated until after OutletComponent is finished loading.
     */
    ngAfterViewChecked() {
        this.styles.useMainStyleSheet();
    }
    onEvent(event:Event) {
        event.stopPropagation();
        event.preventDefault();
      }
      redirect(url:string) {
        window.open(url, "_blank");
      }
}

import {
  Component,
  OnInit,
  Input,
  Output,
  Inject,
  ViewChild,
} from '@angular/core';
import { AttachmentService } from '../../services/attachment.service';
import { AccountService } from '@ds/core/account.service';
import { DOCUMENT } from '@angular/common';
import { Observable, from, iif, of, Subject, forkJoin } from 'rxjs';
import { map, tap, switchMap, filter, takeUntil } from 'rxjs/operators';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { ClaimSource, IClientAccessInfo, IUserActionTypeClaimType, IUserAccessInfo, IAccountFeatureClaimType, IOtherAccessClaimType, IUserTypeClaimType, OtherAccessClaimType } from '@ds/core/users/shared';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MessageService } from '../../services/message.service';
import { AddAttachmentFolderDialogComponent } from '../../shared/dialogs/add-attachment-folder-dialog/add-attachment-folder-dialog.component';
import { AddEmployeeAttachmentDialogComponent } from '../../shared/dialogs/add-attachment-dialog/add-attachment-dialog.component';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { IEmployeeAttachmentFolder, IEmployeeAttachment  } from '@models/attachment.model';
import { ActivatedRoute, Params, Router, NavigationEnd } from '@angular/router';
import { Store } from "@ngrx/store";
import {
  EmployeeState,
  getEmployeeState,
} from "@ds/employees/header/ngrx/reducer";
import { IEmployeeSearchResult } from "@ds/employees/search/shared/models/employee-search-result";
import { HttpErrorResponse } from '@angular/common/http';
import { ConfigUrl, ConfigUrlType } from '@ds/core/shared/config-url.model';
import { AccessRuleApiService } from '@ds/core/users/shared/access-rule-api.service';

@Component({
  selector: 'ds-attachemnts',
  templateUrl: './attachments.component.html',
  styleUrls: ['./attachments.component.scss'],
})
export class AttachmentsComponent implements OnInit {
  currEmpId: number;
  currClientId: number;
  employeeVisible: boolean;
  pageTitle: string = 'Employee Attachments';
  sort = {by:'name'};
  isCompanyAttachments: boolean;
  isLoading: boolean;
  showFiles: boolean;
  folders: Array<IEmployeeAttachmentFolder> = [];
  selectedFolder: IEmployeeAttachmentFolder = null;
  isSupervisorOnHimself: boolean;
  userinfo: UserInfo;
  accessRights = {
    canCreateFolder: false,
    addEditAttachment: false,
    supervisorFolderCreateIndividualFoldersOnly: false,
    addAttachment: false
  };
  attachments: Array<IEmployeeAttachment>;
  pageType: number = 1;
  isNewFolder: boolean;
  destroy$ = new Subject();
  breadcrumb: string;
  payrollUrl: ConfigUrl;
  companyUrl: ConfigUrl;
  essResource: string;
  userAccessInfo: IUserAccessInfo = null;
  isHrBlocked: boolean;
  selectedEmployee$ = this.store.select(
        getEmployeeState((x) => x.selectedEmployee)
      ) as any as Observable<IEmployeeSearchResult>;

  constructor(
    private accountService: AccountService,
    private attachmentService: AttachmentService,
    private msg: MessageService,
    private dialog: MatDialog,
    @Inject(DOCUMENT) private document: Document,
    private route: ActivatedRoute,
    private store: Store<EmployeeState>,
    private router: Router,
    private ruleApi : AccessRuleApiService
  ) {}

  ngOnInit() {

    this.isLoading = true;
    this.showFiles = false;
    this.folders = [];
    this.selectedFolder = <IEmployeeAttachmentFolder>{};
    this.employeeVisible = true;
    this.isCompanyAttachments = true;
    this.currEmpId = null;
    this.isHrBlocked = false;
    this.pageType = this.route.snapshot.params['pageType'];


    this.accountService.canPerformAction('AttachmentFolder.CreateEmployeeAttachmentFolders')
            .subscribe(canPerform => {
                if (canPerform) {
                  this.accessRights.canCreateFolder = true;
                }
                else{
                  this.accessRights.canCreateFolder = false;
                }
            });
    this.accountService.canPerformAction('Attachment.AddEditAttachments')
            .subscribe(canPerform => {
                if (canPerform) {
                  this.accessRights.addEditAttachment = true;
                }
                else{
                  this.accessRights.addEditAttachment = false;
                }
            });

    this.accountService.canPerformAction('Attachment.AddAttachments')
            .subscribe(canPerform => {
                if (canPerform) {
                  this.accessRights.addAttachment = true;
                }
                else{
                  this.accessRights.addAttachment = false;
                }
            });
    this.checkCurrentUser()
      .pipe(
        switchMap((userInfo) =>
          forkJoin([
            this.attachmentService
              .getEmployeeFolderList(
                this.currEmpId, this.currClientId, this.isCompanyAttachments
              ),
            this.ruleApi.getUserAccessInfo(this.userinfo.userId)
          ])),
        tap(([folders, user]) => {
          this.userAccessInfo = user;
          let claimIndex = this.userAccessInfo.claims.findIndex(x => x.otherAccessType == OtherAccessClaimType.SupervisorFolderCreateIndividualFoldersOnly);
          if (claimIndex > -1) {
            this.accessRights.supervisorFolderCreateIndividualFoldersOnly = true;
          }
          this.folders = folders
            ? folders.sort((x, y) =>
              x.description.localeCompare(y.description)
            )
            : [];
          this.isLoading = false;
        })

      )
      .subscribe();

      this.InitPage();

      this.router.events.subscribe((event) => {
        if (event instanceof NavigationEnd) {
          this.pageType = this.route.snapshot.params['pageType'];
          this.isCompanyAttachments = (this.pageType == 1);
          this.InitPage();
        }
      })
    }

    private InitPage(){
      if(this.isCompanyAttachments)
      {
        this.pageTitle='Company Attachments'
      }
      else{
        this.pageTitle='Attachments'
      }

      this.selectedEmployee$
        .pipe(
          takeUntil(this.destroy$),
          filter((x) => !!x && x.employeeId != this.currEmpId),
          switchMap(( ee :  IEmployeeSearchResult) => {
            this.isLoading = true;
            this.showFiles = false;
            this.currEmpId = ee.employeeId ;
            this.isSupervisorOnHimself = this.userinfo.userEmployeeId == this.currEmpId;

            return this.attachmentService
            .getEmployeeFolderList(
              this.currEmpId, this.currClientId, this.isCompanyAttachments
            )
          }),
        )
        .subscribe((folders) => {
          this.folders = folders
            ? folders.sort((x, y) =>
                x.description.localeCompare(y.description)
              )
            : [];
          this.isLoading = false;
        }, (error: HttpErrorResponse) => {
          this.msg.setErrorResponse(error);
          this.isLoading = false;
        });
    }

  checkCurrentUser(): Observable<UserInfo> {
    return iif(
      () => this.userinfo == null,
      forkJoin([this.accountService.getUserInfo(), this.accountService.getSiteUrls()]).pipe(
        tap(([u,sites]) => {
          this.userinfo = u;
          this.currClientId=this.userinfo.lastClientId || this.userinfo.clientId;
          this.currEmpId=this.userinfo.lastEmployeeId;
          this.isSupervisorOnHimself = this.userinfo.userEmployeeId == this.currEmpId;
          this.isHrBlocked = this.userinfo.isHrBlocked;
          if(this.isHrBlocked) {
            //this.router.navigate(['error']);
            this.router.navigate(['error'],  { queryParams:  {showButton: false, showHelpMessage: false, message:'You do not have access to this information.'},
                                       queryParamsHandling: "merge" });
          }
          if(this.pageType==1 && this.userinfo.userTypeId!=1)
          {
            this.router.navigate(['error']);
          }
          this.isCompanyAttachments = (this.pageType==1 && this.userinfo.userTypeId==1);

          this.payrollUrl = sites.find((s) => s.siteType === ConfigUrlType.Payroll);
          let essUrl      = sites.find((s) => s.siteType === ConfigUrlType.Ess);
          this.companyUrl = sites.find((s) => s.siteType === ConfigUrlType.Company);
          this.essResource = `${essUrl.url}resources`;

          this.breadcrumb = `${this.payrollUrl.url}ChangeEmployee.aspx?SubMenu=Employee&Force=True&URL=${this.companyUrl.url}manage/attachments/0`;
          // if the user doesn't have an employee selected, redirect them to the employee select list
          if (this.currEmpId == null || this.currEmpId < 1) {
            document.location.href = this.breadcrumb;
            return;
          }
        })
      ).pipe(map(([u,sites]) => u)),
      of(this.userinfo)
    );
  }

  popupFolderDialog(currResourceFolder: IEmployeeAttachmentFolder) {
    this.isNewFolder = false;
    let currentResourceFolder: IEmployeeAttachmentFolder = currResourceFolder;
    if (!currResourceFolder) {
      currentResourceFolder = {
        employeeFolderId: 0,
        clientId: this.userinfo.lastClientId || this.userinfo.clientId,
        description: '',
        isNew: true,
        attachmentCount: 0,
        attachments: [],
        newResourceId: 0,
        hovered: false,
        isCompanyFolder: this.isCompanyAttachments,
        isAdminViewOnly: false,
        employeeId: this.currEmpId,
        isDefaultOnboardingFolder: false,
        isEmployeeView: this.employeeVisible,
        isDefaultPerformanceFolder: false,
        isSystemFolder: false,
        defaultATFolder: false,
        showAttachments: false
      };
      this.isNewFolder = true;
    }

    let config = new MatDialogConfig<any>();
    config.width = '400px';
    config.data = { folder: null, folders: null, isNew: this.isNewFolder, isCompanyAttachments: this.isCompanyAttachments };
    config.data.folders = this.folders
      .filter(
        (z) =>
          z.employeeFolderId !=
          currentResourceFolder.employeeFolderId
      )
      .map((x) => <string>x.description);
    config.data.folder = currentResourceFolder;
    return this.dialog
      .open<AddAttachmentFolderDialogComponent, any, string>(
        AddAttachmentFolderDialogComponent,
        config
      )
      .afterClosed()
      .subscribe((data:any) => {
        if(data.folderId)
        {
          let deleted = this.folders.findIndex((o) =>
              o.employeeFolderId == data.folderId
          );
          this.folders.splice(deleted,1);
          this.msg.setSuccessMessage('Folder deleted successfully.');
          return;
        }

        if (!data.folderName) return;


        currentResourceFolder.description = data.folderName;
        currentResourceFolder.isEmployeeView = data.employeeView;
        this.attachmentService
          .updateEmployeeAttachmentFolder(currentResourceFolder)
          .subscribe((result: any) => {
            if (this.isNewFolder) {
              currentResourceFolder.employeeFolderId=result.data.employeeFolderId;
              currentResourceFolder.isEmployeeView=result.data.isEmployeeView;
              currentResourceFolder.isNew=false;
              this.folders.push(currentResourceFolder);
              this.folders.sort((x, y) =>
                x.description.localeCompare(y.description)
              );
              this.msg.setSuccessMessage('Folder added successfully.');
            } else {
              let modified = this.folders.find(
                (o) =>
                  o.employeeFolderId == result.data.employeeFolderId
              );
              modified.description = result.data.description;
              modified.isEmployeeView = result.data.isEmployeeView;
              this.msg.setSuccessMessage(
                'Folder updated successfully.'
              );
            }
          });
      });
  }

  folderClicked(folder) {
    if(this.sort.by=='name'){
      this.attachments = folder.attachments.sort((x, y) =>
        x.name.localeCompare(y.name)
      );
    }
    else{
      this.attachments = folder.attachments.sort((x, y) =>
        x.addedDate.localeCompare(y.addedDate)
      );
    }
    this.selectedFolder = folder;
    this.showFiles = true;
  }

  backToFolders() {
    this.showFiles = false;
    this.selectedFolder = <IEmployeeAttachmentFolder>{};
  }

  private folderKlicked: IEmployeeAttachmentFolder = null;
  makeHovered(folder) {
    if (this.folderKlicked) {
      this.folderKlicked.hovered = false;
      this.folderKlicked = null;
    }
    folder.hovered = true;
  }
  cogClicked(folder) {
    this.folderKlicked = folder;
    this.folderKlicked.hovered = false;
    setTimeout(() => {
      folder.hovered = true;
    }, 100);
  }

  absoluteUrl(url: string){
    return location.protocol + '//' + location.host + url
  }

  download(resource: IEmployeeAttachment) {
    if (resource.sourceType === 2 && resource.isATFile) {
      let encodedCss: string = document.querySelector("link[id$='StyleSheetMain']").attributes["href"].value;

        encodedCss = window.btoa( this.absoluteUrl( encodedCss) ).replace(/\//gi, '_');
        let url = resource.source.replace(/\{cssUrl\}/g,encodedCss);

        this.attachmentService.getUrlToDownload( 'api/' + url, resource.name + resource.extension).subscribe();
    }
    else {
      this.attachmentService.getFileResourceToDownload(resource);
    }
  }

  private resourceKlicked: IEmployeeAttachmentFolder = null;

  makeAttachmentHovered(resource) {
    if (this.resourceKlicked) {
      this.resourceKlicked.hovered = false;
      this.resourceKlicked = null;
    }
    resource.hovered = true;
  }

  concat(x1: string, x2: string): string {
    return x1 + ' ' + x2;
  }

  popupAttachmentDialog(currResource: IEmployeeAttachment) {
    var isNewReSource = false;
    var currentResource: IEmployeeAttachment = currResource;
    var currentFolderId = 0;

    if (this.showFiles && this.selectedFolder)
        currentFolderId = this.selectedFolder.employeeFolderId;

    if (!currResource) {
      currentResource = <IEmployeeAttachment>{
        clientId: this.userinfo.lastClientId || this.userinfo.clientId,
        employeeId: this.currEmpId,
        folderId: currentFolderId,
        resourceId : 0,
        name:'',
        extension: '',
        sourceType: 1,
        isAzure : false,
        cssClass: '',
        source: '',
        addedDate : new Date(),
        isDeleted : false,
        hovered: false,
        addedByUsername: '',
        isATFile: false,
        isCompanyAttachment: this.isCompanyAttachments,
        isViewableByEmployee: true,
        onboardingWorkflowTaskId: 0
      };
      isNewReSource = true;
    }

    let config = new MatDialogConfig<any>();
    config.width = '500px';
    config.data = { folders: null, resource: currentResource, isCompanyAttachment: this.isCompanyAttachments };
    config.data.folders = this.folders;

    return this.dialog
      .open<AddEmployeeAttachmentDialogComponent>(
        AddEmployeeAttachmentDialogComponent,
        config
      )
      .afterClosed()
      .subscribe((folders: IEmployeeAttachmentFolder[]) => {
        if (folders) {
          this.folders = folders;

          if (currentResource.resourceId == 0) {
            this.msg.setSuccessMessage('Attachment added successfully.');

            if (currentFolderId > 0) {
              this.selectedFolder = this.folders.find(x => x.employeeFolderId == currentResource.folderId);

              this.attachments = this.selectedFolder.attachments.sort((x, y) =>
                x.name.localeCompare(y.name)
              );
            }
          } else {
            this.msg.setSuccessMessage('Attachment updated successfully.');
          }
        }
      });
  }
  errorMessage(error) {
    var str = error.message;
    if (error.error.errors && error.error.errors.length > 0) {
      str += '. ' + error.error.errors[0].msg;
    }
    return str;
  }

  showEditFolder(folder: IEmployeeAttachmentFolder){
    if(folder.isCompanyFolder && this.accessRights.supervisorFolderCreateIndividualFoldersOnly)
    {
      return false;
    }
    return true;
  }
}

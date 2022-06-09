import {
  Component,
  OnInit,
  Inject,
} from '@angular/core';
import { ResourceService } from './shared/resource.service';
import { AccountService } from '@ds/core/account.service';
import { DOCUMENT } from '@angular/common';
import { Observable, iif, of } from 'rxjs';
import { tap, switchMap, filter } from 'rxjs/operators';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { AddFolderDialogComponent } from './add-folder-dialog/add-folder-dialog.component';
import { DeleteFolderDialogComponent } from './delete-folder-dialog/delete-folder-dialog.component';
import { AddResourceDialogComponent } from './add-resource-dialog/add-resource-dialog.component';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { ICompanyResourceFolder, ICompanyResource } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-resources',
  templateUrl: './resources.component.html',
  styleUrls: ['./resources.component.scss'],
})
export class ResourcesComponent implements OnInit {
  isLoading: boolean = true;
  showFiles: boolean = false;
  folders: Array<ICompanyResourceFolder> = [];
  selectedFolder: ICompanyResourceFolder = null;

  userinfo: UserInfo;
  resources: Array<ICompanyResource>;

  constructor(
    private accountService: AccountService,
    private resourceService: ResourceService,
    private msg: NgxMessageService,
    private dialog: MatDialog,
    @Inject(DOCUMENT) private document: Document,
    private confirmDialog2: ConfirmDialogService
  ) {}

  ngOnInit() {
    this.isLoading = true;
    this.showFiles = false;
    this.folders = [];
    this.selectedFolder = <ICompanyResourceFolder>{};

    this.checkCurrentUser()
      .pipe(
        switchMap((userInfo) =>
          this.resourceService
            .getCompanyResourceFolderByClient(
              userInfo.lastClientId || userInfo.clientId
            )
            .pipe(
              tap((folders) => {
                this.folders = folders
                  ? folders.sort((x, y) =>
                      x.description.localeCompare(y.description)
                    )
                  : [];
                this.isLoading = false;
              })
            )
        )
      )
      .subscribe();
  }

  checkCurrentUser(): Observable<UserInfo> {
    return iif(
      () => this.userinfo == null,
      this.accountService.getUserInfo().pipe(
        tap((u) => {
          this.userinfo = u;
        })
      ),
      of(this.userinfo)
    );
  }

  popupFolderDialog(currResourceFolder: ICompanyResourceFolder) {
    var isNewFolder = false;
    let currentResourceFolder: ICompanyResourceFolder = currResourceFolder;
    if (!currResourceFolder) {
      currentResourceFolder = {
        companyResourceFolderId: 0,
        clientId: this.userinfo.lastClientId || this.userinfo.clientId,
        description: '',
        isNew: true,
        resourceCount: 0,
        resourceList: [],
        newResourceId: 0,
        hovered: false,
      };
      isNewFolder = true;
    }

    let config = new MatDialogConfig<any>();
    config.width = '400px';
    config.data = { folder: null, folders: null };
    config.data.folders = this.folders
      .filter(
        (z) =>
          z.companyResourceFolderId !=
          currentResourceFolder.companyResourceFolderId
      )
      .map((x) => <string>x.description);
    config.data.folder = currentResourceFolder;
    return this.dialog
      .open<AddFolderDialogComponent, any, string>(
        AddFolderDialogComponent,
        config
      )
      .afterClosed()
      .subscribe((folderName: string) => {
        if (!folderName) return;

        currentResourceFolder.description = folderName;

        this.resourceService
          .updateCompanyResourceFolder(currentResourceFolder)
          .subscribe((result: ICompanyResourceFolder) => {
            if (currentResourceFolder.isNew) {
              this.folders.push(result);
              this.folders.sort((x, y) =>
                x.description.localeCompare(y.description)
              );
              this.msg.setSuccessMessage('Company folder added successfully.');
            } else {
              let modified = this.folders.find(
                (o) =>
                  o.companyResourceFolderId == result.companyResourceFolderId
              );
              modified.description = result.description;
              this.msg.setSuccessMessage(
                'Company folder updated successfully.'
              );
            }
          });
      });
  }

  deleteFolderDialog(currentResourceFolder: ICompanyResourceFolder) {
    if (currentResourceFolder.resourceList.length == 0) {
      const options = {
        title: 'Are you sure you want to delete this folder?',
        confirm: 'Delete Folder',
      };
      this.confirmDialog2.open(options);
      this.confirmDialog2
        .confirmed()
        .pipe(
          filter((x) => !!x), // Only on confirmation
          switchMap((okFlag: any) =>
            this.resourceService.deleteCompanyResourceFolder(
              currentResourceFolder.companyResourceFolderId,
              null
            )
          )
        )
        .subscribe(
          (folder: ICompanyResourceFolder) => {
            this.folders = this.folders.filter(
              (o) =>
                o.companyResourceFolderId !=
                currentResourceFolder.companyResourceFolderId
            );
            this.msg.setSuccessMessage('Company folder deleted successfully.');
          },
          (error) => {
            this.msg.setErrorMessage(this.errorMessage(error));
          }
        );
    } else {
      let config = new MatDialogConfig<any>();
      config.width = '450px';
      config.data = { folder: null, folders: null };
      config.data.folders = this.folders.filter(
        (z) =>
          z.companyResourceFolderId !=
          currentResourceFolder.companyResourceFolderId
      );
      config.data.folder = currentResourceFolder;

      return this.dialog
        .open<DeleteFolderDialogComponent, any, string>(
          DeleteFolderDialogComponent,
          config
        )
        .afterClosed()
        .subscribe((result: any) => {
          if (result) {
            this.resourceService
              .deleteCompanyResourceFolder(
                currentResourceFolder.companyResourceFolderId,
                result.destinationFolderId
              )
              .subscribe((desFldr: any) => {
                this.folders = this.folders.filter(
                  (o) =>
                    o.companyResourceFolderId !=
                    currentResourceFolder.companyResourceFolderId
                );
                if (desFldr) {
                  let dest = this.folders.find(
                    (o) =>
                      o.companyResourceFolderId ==
                      desFldr.companyResourceFolderId
                  );
                  dest.resourceCount = desFldr.resourceCount;
                  dest.resourceList = desFldr.resourceList;
                }
                this.msg.setSuccessMessage(
                  'Company folder deleted successfully.'
                );
              });
          }
        });
    }
  }
  folderClicked(folder) {
    this.resources = folder.resourceList.sort((x, y) =>
      x.resourceName.localeCompare(y.resourceName)
    );
    this.selectedFolder = folder;
    this.showFiles = true;
  }

  backToFolders() {
    this.showFiles = false;
    this.selectedFolder = <ICompanyResourceFolder>{};
  }

  private folderKlicked: ICompanyResourceFolder = null;
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

  visit(resource: ICompanyResource) {
    window.open(resource.source, '_blank');
  }
  download(resource: ICompanyResource) {
    this.resourceService.getFileResourceToDownload(resource).subscribe(
      (res) => {},
      (error) => this.msg.setErrorMessage(this.errorMessage(error))
    );
  }
  resourceClicked(resource: ICompanyResource) {
    if (resource.resourceTypeId == 1) this.download(resource);
    else this.visit(resource);
  }
  private resourceKlicked: ICompanyResourceFolder = null;
  makeResourceHovered(resource) {
    if (this.resourceKlicked) {
      this.resourceKlicked.hovered = false;
      this.resourceKlicked = null;
    }
    resource.hovered = true;
  }
  cogResourceClicked(resource) {
    this.resourceKlicked = resource;
    this.resourceKlicked.hovered = false;
    setTimeout(() => {
      resource.hovered = true;
    }, 100);
  }
  concat(x1: string, x2: string): string {
    return x1 + ' ' + x2;
  }

  //Resource Area
  popupResourceDialog(currResource: ICompanyResource) {
    var isNewReSource = false;
    var currentResource: ICompanyResource = <ICompanyResource>{};

    if (!currResource) {
      var currentFolderId = 0;
      if (this.showFiles && this.selectedFolder)
        currentFolderId = this.selectedFolder.companyResourceFolderId;

      currentResource = <ICompanyResource>{
        companyResourceFolderId: currentFolderId,
        resourceId: 0,
        resourceTypeId: 1,
        resourceName: '',
        isNew: true,
        resourceFormat: '',
        securityLevel: 3,
        isManagerLink: false,
        modified: new Date(),
        modifiedBy: this.userinfo.userId,
        clientId: this.userinfo.lastClientId || this.userinfo.clientId,
        doesFileExist: false,
        isAzure: false,
        azureAccount: 0,
        cssClass: '',
        source: '',
        currentSource: '',
        isSelectedResource: false,
        previewResourceCssClass: false,
        addedDate: new Date(),
        addedBy: this.userinfo.userId,
        isDeleted: false,
        isFileReselected: false,
        hovered: false,
      };
      isNewReSource = true;
    } else {
      currentResource = Object.assign({}, currResource);
    }
    let config = new MatDialogConfig<any>();
    config.width = '500px';
    config.data = { folders: null, resource: currentResource };
    config.data.folders = this.folders;

    return this.dialog
      .open<AddResourceDialogComponent>(
        AddResourceDialogComponent,
        config
      )
      .afterClosed()
      .subscribe((folders: ICompanyResourceFolder[]) => {
        if (folders) {
          this.folders = folders;

          if (currentResource.resourceId == 0) {
            this.msg.setSuccessMessage('Resource added successfully.');

            if (currentFolderId > 0) {
              this.selectedFolder = this.folders.find(x => x.companyResourceFolderId == currentResource.companyResourceFolderId);

              this.resources = this.selectedFolder.resourceList.sort((x, y) =>
                x.resourceName.localeCompare(y.resourceName)
              );
            }
          } else {
            this.msg.setSuccessMessage('Resource updated successfully.');
          }
        }
      });
  }

  popupDeleteResourceDialog(resource: ICompanyResource) {
    const options = {
      title: 'Are you sure you want to delete this resource?',
    };
    this.confirmDialog2.open(options);
    this.confirmDialog2
      .confirmed()
      .pipe(
        switchMap((okFlag: any) => {
          if (okFlag)
            return this.resourceService.deleteCompanyResource(resource);
          else return of(null);
        })
      )
      .subscribe(
        (deletedResource: ICompanyResource) => {
          if (deletedResource) {
            var inx = this.folders.findIndex(
              (x) =>
                x.companyResourceFolderId == resource.companyResourceFolderId
            );
            var resourceInx = this.folders[inx].resourceList.findIndex(
              (x) => x.resourceId == resource.resourceId
            );
            if (resourceInx > -1) {
              this.folders[inx].resourceList.splice(resourceInx, 1);
              this.folders[inx].resourceCount--;
            }
            this.msg.setSuccessMessage('Resource deleted successfully.');
          }
        },
        (error) => {
          this.msg.setErrorMessage(this.errorMessage(error));
        }
      );
  }

  errorMessage(error) {
    var str = error.message;
    if (error.error.errors && error.error.errors.length > 0) {
      str += '. ' + error.error.errors[0].msg;
    }
    return str;
  }
}

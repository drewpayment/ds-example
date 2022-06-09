import {
  Component,
  OnInit,
  Inject,
} from '@angular/core';
import { OrganizationService } from '../shared/organization.service';
import { AccountService } from '@ds/core/account.service';
import {
  IClientOrganization,
  IClientOrganizationClient,
  UpdateOrganizationRequest,
} from '../shared/organization.model';
import { DOCUMENT } from '@angular/common';
import {
  FormControl,
  FormGroup,
  Validators,
  FormBuilder,
} from '@angular/forms';
import { Observable, from, iif, of, forkJoin } from 'rxjs';
import { map, tap, switchMap, filter, skip } from 'rxjs/operators';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { AddClientDialogComponent } from './add-client-dialog/add-client-dialog.component';
import { AddOrganizationDialogComponent } from './add-organization-dialog/add-organization-dialog.component';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-organization-client',
  templateUrl: './organization-client.component.html',
  styleUrls: ['./organization-client.component.scss'],
})
export class OrganizationClientComponent implements OnInit {
  form1: FormGroup;

  empty: boolean;
  formError: boolean;
  isLoading: boolean = true;
  isEmpty: boolean = false;
  showError: boolean = false;

  organization: IClientOrganization = null;
  organizations: Array<IClientOrganization> = [];
  organizationClients: Array<IClientOrganizationClient> = [];
  selectedOrganizationId?: number;
  allClients: Array<IClientOrganizationClient> = [];

  userinfo: UserInfo;
  get orgNameCtrl(): FormControl {
    return this.form1.controls['organizationName'] as FormControl;
  }

  constructor(
    private accountService: AccountService,
    private organizationService: OrganizationService,
    private msg: NgxMessageService,
    private fb: FormBuilder,
    private dialog: MatDialog,
    private confirmDialog: ConfirmDialogService,
    @Inject(DOCUMENT) private document: Document
  ) {}

  ngOnInit() {
    /// Build the form here
    this.form1 = this.fb.group({
      organization: ['', null],
      organizationName: this.fb.control('', {
        updateOn: 'blur',
        validators: Validators.required,
      }),
    });

    this.orgNameCtrl.valueChanges
      .pipe(
        skip(1),
        filter(
          (val) => !!val && val != this.organization.clientOrganizationName
        ),
        switchMap((changedName) => this.updateOrganizationName(changedName))
      )
      .subscribe((list) => {
        if (list) {
          this.msg.setSuccessMessage('Organization name updated successfully.');
        }
      });

    const clients$ = this.organizationService.getUnAssignedClients().pipe(
      tap((unAssignedClientsData) => {
        if (!unAssignedClientsData) unAssignedClientsData = [];

        this.allClients = unAssignedClientsData
          .sort((a, b) =>
            a.isAssigned > b.isAssigned
              ? -1
              : b.isAssigned > a.isAssigned
              ? 1
              : 0
          )
          .sort((a, b) =>
            a.clientName > b.clientName
              ? 1
              : b.clientName > a.clientName
              ? -1
              : 0
          );
      })
    );

    forkJoin(this.checkCurrentUser(), clients$)
      .pipe(
        switchMap((userInfo) =>
          this.organizationService.getOrganizationsList().pipe(
            tap((organizationsData) => {
              this.organizations = organizationsData;
            })
          )
        ),
        switchMap((x) =>
          this.organizationService.getOrganizationByClientId(
            this.userinfo.lastClientId || this.userinfo.clientId
          )
        ),
        tap((org) => {
          this.organization = org;

          this.setOrganization(org);
          this.isLoading = false;
          this.isEmpty = false;
        })
      )
      .subscribe();
  }

  setOrganization(org: IClientOrganization, reflectOrg: boolean = true) {
    this.formError = false;
    this.organization = org;

    if (this.organization) {
      this.selectedOrganizationId = this.organization.clientOrganizationId;
      this.organizationClients = this.allClients.filter(
        (c) => c.organizationId == org.clientOrganizationId
      );

      if (reflectOrg) {
        this.form1.patchValue({
          organization: this.organization.clientOrganizationId,
          organizationName: this.organization.clientOrganizationName,
        });
      } else {
        this.form1.patchValue({
          organizationName: this.organization.clientOrganizationName,
        });
      }
    } else {
      this.organizationClients = [];
      this.selectedOrganizationId = 0;

      if (reflectOrg) {
        this.form1.patchValue({
          organization: '',
          organizationName: '',
        });
      } else {
        this.form1.patchValue({
          organizationName: '',
        });
      }
    }
  }

  updateOrganizationName(changedName: string): Observable<IClientOrganization> {
    if (
      this.organizations.find(
        (x) =>
          x.clientOrganizationName == changedName &&
          x.clientOrganizationId != this.selectedOrganizationId
      )
    ) {
      this.orgNameCtrl.setErrors({ duplicate: true });
      this.formError = true;
      return of(null);
    }
    this.orgNameCtrl.setErrors({ duplicate: false });
    this.formError = false;

    let organizationDetail = {
      clientOrganizationId: this.selectedOrganizationId,
      clientOrganizationName: changedName,
      isNew: this.selectedOrganizationId == 0,
    };

    return this.organizationService.updateOrganization(organizationDetail).pipe(
      map((orgs) => {
        this.organizations = orgs;
        
        let org = this.organizations.find(
          (x) => x.clientOrganizationId == this.selectedOrganizationId
        );
        org.clientOrganizationName = changedName;
        return org;
      }),
    );
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

  public getOrganizationClients(): void {
    this.formError = false;
    this.selectedOrganizationId = parseInt(this.form1.value.organization);

    if (this.selectedOrganizationId) {
      let selectedOrg = this.organizations.find(
        (org) => this.selectedOrganizationId == org.clientOrganizationId
      );
      this.setOrganization(selectedOrg, false);
    } else {
      this.setOrganization(null, false);
    }
  }

  public addOrganization() {
    let config = new MatDialogConfig<Array<string>>();
    config.width = '400px';
    config.data = this.organizations.map(
      (x) => <string>x.clientOrganizationName
    );

    return this.dialog
      .open<AddOrganizationDialogComponent, Array<string>, string>(
        AddOrganizationDialogComponent,
        config
      )
      .afterClosed()
      .subscribe((orgName: string) => {
        if (!orgName) return;

        let organizationDetail: UpdateOrganizationRequest = {
          clientOrganizationId: 0,
          clientOrganizationName: orgName,
          isNew: true,
        };

        this.organizationService
          .updateOrganization(organizationDetail)
          .subscribe((orgs) => {
            const orgData = orgs.find(x => x.clientOrganizationName === organizationDetail.clientOrganizationName);
            
            if (orgData) {
              this.organizations.push(orgData);
              this.setOrganization(orgData);  
            }
            
            this.msg.setSuccessMessage('Organization Added successfully.');
          });
      });
  }

  public addClients() {
    let selectedOrganizationId = parseInt(this.form1.value.organization);

    if (selectedOrganizationId) {
      let config = new MatDialogConfig<any>();
      config.width = '600px';
      config.data = {
        selectedOrganizationId: selectedOrganizationId,
        unAssignedClients: this.allClients.filter((x) => !x.isAssigned),
      };

      return this.dialog
        .open<AddClientDialogComponent, any, any>(
          AddClientDialogComponent,
          config
        )
        .afterClosed()
        .subscribe((result: any) => {
          if (!result) return;
          if (result.selectedClients.length > 0) {
            result.selectedClients.forEach((element) => {
              if (
                this.organizationClients
                  .map((x) => x.clientId)
                  .indexOf(element.clientId) < 0
              )
                this.organizationClients.push(element);
            });
          }
        });
    } else {
      this.msg.setSuccessMessage(
        'Organization needs to be added before adding a client.'
      );
    }
  }

  public deleteClients(clientId, clientCode): void {
    let selectedOrganizationId = parseInt(this.form1.value.organization);

    const options = {
      title: 'Remove Client ' + clientCode + '?',
      message: '',
      confirm: 'Remove',
    };
    this.confirmDialog.open(options);

    this.confirmDialog.confirmed().subscribe((ok) => {
      if (ok) {
        let c = this.allClients.find((x) => x.clientId == clientId);
        if (c) {
          let inx = this.organizationClients.findIndex(
            (x) => x.clientId == c.clientId
          );
          if (inx >= 0) this.organizationClients.splice(inx, 1);
        }
      }
    });
  }

  public deleteOrganization(): void {
    let selectedOrganizationId = parseInt(this.form1.value.organization);

    let config = new MatDialogConfig<string>();
    config.width = '40vw';
    config.data = 'Remove Organization ' + this.orgNameCtrl.value + '?';

    const options = {
      title: 'Remove Organization ' + this.orgNameCtrl.value + '?',
      message: '',
      confirm: 'Remove',
    };
    this.confirmDialog.open(options);

    this.confirmDialog.confirmed().subscribe((ok) => {
      if (ok) {
        this.organizationService
          .deleteOrganization(selectedOrganizationId)
          .pipe(
            switchMap((x) =>
              this.organizationService.getOrganizationByClientId(
                this.userinfo.lastClientId || this.userinfo.clientId
              )
            )
          )
          .subscribe((org) => {
            // remove the removed organization
            this.organizations = this.organizations.filter(
              (x) => x.clientOrganizationId != selectedOrganizationId
            );
            this.allClients
              .filter((x) => x.organizationId == selectedOrganizationId)
              .forEach((y) => {
                y.isAssigned = false;
                y.organizationId = null;
              });

            this.setOrganization(org);
            this.msg.setSuccessMessage('Organization removed successfully.');
          });
      }
    });
  }

  public save(): void {
    var organizationNm = this.orgNameCtrl.value;
    let selectedOrganizationId = parseInt(this.form1.value.organization);

    if (!organizationNm) return;

    // Map the org Id before saving.
    this.organizationClients.forEach((x) => {
      x.organizationId = selectedOrganizationId;
      x.isAssigned = true;
    });

    let organizationClientDetail = {
      clientOrganizationId: selectedOrganizationId,
      isNew: false,
      clientOrganizationClient: this.organizationClients,
    };

    this.updateOrganizationName(organizationNm)
      .pipe(
        filter((x) => !!x),
        switchMap((x) =>
          this.organizationService.updateOrganizationClients(
            organizationClientDetail
          )
        )
      )
      .subscribe((data) => {
        this.msg.setSuccessMessage('Organization updated successfully.');

        // Change the mappings only for removed clients
        this.allClients
          .filter(
            (c) =>
              c.organizationId == selectedOrganizationId &&
              this.organizationClients
                .map((d) => d.clientId)
                .indexOf(c.clientId) == -1
          )
          .forEach((c) => {
            c.isAssigned = false;
            c.organizationId = null;
          });

        this.formError = false;
      });
  }
}

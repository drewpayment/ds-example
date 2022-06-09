import { Component, OnInit, AfterViewInit, AfterViewChecked } from '@angular/core';
import { DsStyleLoaderService, IStyleAsset } from '@ajs/ui/ds-styles/ds-styles.service';
import { MatDialog } from "@angular/material/dialog";
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { IDsConfirmOptions } from '@ajs/ui/confirm/ds-confirm.interface';
import { IEmployeePersonalInfo } from '../shared/employee-personal-info.model';
import { EmployeeProfileService } from '../shared/employee-profile-api.service';
import { EmployeeBioFormComponent } from "../employee-bio-form/employee-bio-form.component";
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { map } from 'rxjs/operators';
import { DomSanitizer } from "@angular/platform-browser";
import * as _ from "lodash";

@Component({
  selector: 'ds-employee-bio',
  templateUrl: './employee-bio.component.html',
  styleUrls: ['./employee-bio.component.scss']
})
export class EmployeeBioComponent implements OnInit, AfterViewChecked {
    mainStyle: IStyleAsset;
    user: UserInfo;
    employeePersonalInfo: IEmployeePersonalInfo;
    hasViewPermissions: boolean;
    hasEditPermissions: boolean;
    isLoading = true;

    constructor(
        private styles: DsStyleLoaderService,
        private service: EmployeeProfileService,
        private accountService: AccountService,
        private msgSvc: DsMsgService,
        private dialog: MatDialog,
        private confirmService: DsConfirmService
    ) { }

    ngOnInit() {
        this.hasViewPermissions = false;
        this.hasEditPermissions = false;

        this.accountService.getUserInfo().subscribe(user => {
            this.user = user;

            this.accountService.canPerformActions("Employee.EmployeeBioView").subscribe(x => {
                if (x === true) {
                    this.hasViewPermissions = true;
                }

                this.employeePersonalInfo = <IEmployeePersonalInfo>{ employeeId: this.user.employeeId, bio : '' };
                this.service.getEmployeePersonalInfo(this.user.employeeId).subscribe(data => {
                    if(data) this.employeePersonalInfo = data;
                    this.isLoading = false;
                });
            });
        });
    }

    showEditBioDialog(employeePersonalInfo: IEmployeePersonalInfo): void {
        this.dialog.open(EmployeeBioFormComponent, {
            width: '410px',
            data: {
                user: this.user,
                employeePersonalInfo: employeePersonalInfo
            }
        })
        .afterClosed()
            .subscribe(result => {
            if (result == null) return;
                this.msgSvc.sending(true);

            this.service.updateEmployeePersonalInfo(result).subscribe(data => {
                this.employeePersonalInfo = data;
                this.msgSvc.setTemporarySuccessMessage('Your changes have been saved.', 5000);
            });
        });
    }

    /**
     * We tell DsStyleLoaderService that this component should use main stylesheet AFTER OnInit and AfterViewInit
     * because we need to make sure that everything is resolved above this component. The DsStyleLoaderService is not
     * instantiated until after OutletComponent is finished loading.
     */
    ngAfterViewChecked() {
        this.styles.useMainStyleSheet();
    }
}

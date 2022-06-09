import { Component, OnInit, AfterViewInit, AfterViewChecked } from '@angular/core';
import { DsStyleLoaderService, IStyleAsset } from '@ajs/ui/ds-styles/ds-styles.service';
import { MatDialog } from '@angular/material/dialog';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { IDsConfirmOptions } from '@ajs/ui/confirm/ds-confirm.interface';
import { IEmployeeDependent } from '../shared/employee-dependent.model';
import { EmployeeDependentsFormComponent } from '../employee-dependents-form/employee-dependents-form.component';
import { EmployeeProfileService } from '../shared/employee-profile-api.service';
import { UserInfo } from '@ds/core/shared';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { map } from 'rxjs/operators';
import { DomSanitizer } from "@angular/platform-browser";
import * as _ from "lodash";
import { AccountService } from '@ds/core/account.service';
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';

@Component({
  selector: 'ds-employee-dependents',
  templateUrl: './employee-dependents.component.html',
  styleUrls: ['./employee-dependents.component.scss']
})
export class EmployeeDependentsComponent implements OnInit, AfterViewChecked {
    mainStyle: IStyleAsset;
    user: UserInfo;
    newEmployeeDependent: IEmployeeDependent;
    employeeDependents: IEmployeeDependent[];
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
        this.employeeDependents = [];

        this.accountService.getUserInfo().subscribe(user => {
            this.user = user;
            this.newEmployeeDependent = this.createEmptyEmployeeDependent(this.user.employeeId, this.user.lastClientId || this.user.clientId);
            this.accountService.canPerformActions('Employee.EmployeeDependentView').subscribe(x => {
                if (x === true) {
                    this.hasViewPermissions = true;
                }

                this.accountService.getClientAccountFeature(this.user.lastClientId || this.user.clientId, Features.EmployeeChangeRequests).subscribe(employeeChangeRequestfeature => {
                    this.hasEditPermissions = _.isEmpty(employeeChangeRequestfeature);

                    this.service.getEmployeeDependents(this.user.employeeId, false).subscribe(data => {
                        this.employeeDependents = data;
                        this.isLoading = false;
                    });

                });
            });
        });

    }

    private createEmptyEmployeeDependent(employeeId: number, clientId: number): IEmployeeDependent {
        return {
            employeeDependentId: 0,
            clientId: clientId,
            firstName: null,
            middleInitial: null,
            lastName: null,
            employeeId: employeeId,
            unmaskedSocialSecurityNumber: null,
            maskedSocialSecurityNumber: null,
            relationship: null,
            gender: null,
            comments: null,
            birthDate: null,
            insertStatus: 0,
            lastModifiedByDescription: '0',
            lastModifiedDate: new Date(),
            isAStudent: false,
            hasADisability: false,
            tobaccoUser: false,
            isSelected: false,
            primaryCarePhysician: null,
            hasPcp: false,
            employeeDependentsRelationshipId: null,
            isChild: false,
            isSpouse: false,
            isInactive: false,
            inactiveDate: null
        };
    }

    showEditDependentsDialog(employeeDependent: IEmployeeDependent): void {
        this.dialog.open(EmployeeDependentsFormComponent, {
            width: '800px',
            data: {
                user: this.user,
                employeeDependent: employeeDependent,
                hasEditPermissions: this.hasEditPermissions
            }
        })
        .afterClosed()
        .subscribe(result => {
            if (result == null) return;
            this.msgSvc.sending(true);

            this.service.updateEmployeeDependent(result, this.hasEditPermissions).subscribe(data => {
                if (!_.isEmpty(data)) {
                    if (employeeDependent.employeeDependentId == 0)
                        this.employeeDependents.push(data);
                    else {
                        const index = this.employeeDependents.indexOf(employeeDependent);
                        this.employeeDependents[index] = data;
                    }

                    let successMessage = this.hasEditPermissions ? "Successfully updated the changes." : "Successfully submitted change request.";
                    this.msgSvc.setTemporarySuccessMessage(successMessage, 5000);
                }
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

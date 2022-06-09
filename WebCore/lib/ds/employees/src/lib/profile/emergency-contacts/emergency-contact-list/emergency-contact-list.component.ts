import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { DsStyleLoaderService, IStyleAsset } from '@ajs/ui/ds-styles/ds-styles.service';
import { MatDialog } from '@angular/material/dialog';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { IEmergencyContact } from '../../shared/emergency-contact.model';
import { EmergencyContactFormComponent } from '../emergency-contact-form/emergency-contact-form.component';
import { EmployeeProfileService } from '../../shared/employee-profile-api.service';
import { UserInfo } from '@ds/core/shared';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { map } from 'rxjs/operators';
import { DomSanitizer } from "@angular/platform-browser";
import * as _ from "lodash";
import { AccountService } from '@ds/core/account.service';
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';


@Component({
  selector: 'ds-emergency-contact-list',
  templateUrl: './emergency-contact-list.component.html',
  styleUrls: ['./emergency-contact-list.component.scss']
})
export class EmergencyContactListComponent implements OnInit, AfterViewChecked {
    mainStyle: IStyleAsset;
    user: UserInfo;
    newEmergencyContact: IEmergencyContact;
    emergencyContacts: IEmergencyContact[];
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
        this.emergencyContacts = [];

        this.accountService.getUserInfo().subscribe(user => {
            this.user = user;
            this.newEmergencyContact = this.createEmptyEmergencyContact(this.user.employeeId, this.user.lastClientId || this.user.clientId);

            this.accountService.canPerformActions('Employee.EmployeeEmergencyContactView').subscribe(x => {
                if (x === true) {
                    this.hasViewPermissions = true;
                }

                this.accountService.getClientAccountFeature(this.user.lastClientId || this.user.clientId, Features.EmployeeChangeRequests).subscribe(employeeChangeRequestfeature => {
                    this.hasEditPermissions = _.isEmpty(employeeChangeRequestfeature);

                    this.service.getEmergencyContacts(this.user.employeeId).subscribe(data => {
                        this.emergencyContacts = data;
                        this.isLoading = false;
                    });
                });
            });
        });
    }

    showEditEmergencyContactDialog(emergencyContact: IEmergencyContact): void {
        this.dialog.open(EmergencyContactFormComponent, {
            width: '810px',
            data: {
                user: this.user,
                emergencyContact: emergencyContact,
                hasEditPermissions: this.hasEditPermissions
            }
        })
        .afterClosed()
        .subscribe(result => {
            if (result == null) return;
            this.msgSvc.sending(true);

            this.service.updateEmergencyContact(result, this.hasEditPermissions).subscribe(data => {
                if (!_.isEmpty(data)) {
                    if (emergencyContact.employeeEmergencyContactId == 0)
                        this.emergencyContacts.push(data);
                    else {
                        const index = this.emergencyContacts.indexOf(emergencyContact);
                        this.emergencyContacts[index] = data;
                    }

                    const successMessage = this.hasEditPermissions ? 'Successfully updated the changes.' : 'Successfully submitted change request.';
                    this.msgSvc.setTemporarySuccessMessage(successMessage, 5000);
                }
            });
        });
    }

    private createEmptyEmergencyContact(employeeId: number, clientId: number): IEmergencyContact {
        return {
            employeeEmergencyContactId: 0,
            employeeId: employeeId,
            clientId: clientId,
            homePhoneNumber: null,
            cellPhoneNumber: null,
            relation: null,
            emailAddress: null,
            insertApproved: 0,
            firstName: null,
            lastName: null
        };
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

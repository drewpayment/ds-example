import { Component, OnInit, Input } from '@angular/core';
import { EmployeeService } from '../shared/employee.service';
import { UserInfo, MOMENT_FORMATS } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { tap } from 'rxjs/operators';
import * as moment from 'moment';
import { Moment } from 'moment';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmModalComponent } from '@ds/core/resources/confirm-modal/confirm-modal.component';
import { EmployeeExitInterviewRequest } from '../shared/models/employee-exit-interview-request.model';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';

@Component({
    selector: 'ds-employee-exit-interview-request',
    templateUrl: './employee-exit-interview-request.component.html',
    styleUrls: ['./employee-exit-interview-request.component.scss'],
})

export class EmployeeExitInterviewRequestComponent implements OnInit {

    user: UserInfo;
    exitInterviewRequestedOn: Moment;
    exitInterviewSentOn: Moment;
    exitInterviewRequestStatus: string;

    constructor(
        private employeeApi: EmployeeService,
        private accountService: AccountService,
        private msg: DsMsgService,
        private dialog: MatDialog
    ) { }

    ngOnInit() {
        this.getEmployeeExitInterviewRequest();
    }

    getEmployeeExitInterviewRequest() {
        this.accountService.getUserInfo().pipe(tap(u => this.user = u)).subscribe(() => {

            this.employeeApi.getEmployeeExitInterviewRequest(this.user.lastEmployeeId).subscribe(data => {
                if (data) {
                    this.exitInterviewRequestedOn = data.requestedOn ? moment(data.requestedOn) : null;
                    this.exitInterviewSentOn = data.sentOn ? moment(data.sentOn) : null;

                    this.setRequestStatusLabel(data);
                }
            });

        });
    }

    private setRequestStatusLabel(request: EmployeeExitInterviewRequest) {
        if (request.sentOn) {
            this.exitInterviewRequestStatus = 'Request Sent on ' + moment(request.sentOn).format(MOMENT_FORMATS.MDY_12HR);
        } else if (request.requestedOn) {
            this.exitInterviewRequestStatus = 'Requested on ' + moment(request.requestedOn).format(MOMENT_FORMATS.MDY_12HR);
        }
    }

    updateEmployeeExitInterviewRequest() {
        const currentUserId = this.user.userId;

        if (this.exitInterviewRequestedOn && !this.exitInterviewSentOn) {

            this.employeeApi.updateEmployeeExitInterviewRequest(this.user.lastEmployeeId, currentUserId)
                .subscribe(result => {
                    this.setRequestStatusLabel(result);
                    this.msg.setTemporarySuccessMessage('Exit Interview Requested Successfully!');
                }, this.handleApiError);

        } else if (this.exitInterviewSentOn) {

            this.dialog.open(ConfirmModalComponent, {
                width: '25vw',
                data: {
                    displayText: 'Request another exit interview?',
                    cautionText: 'Your exit interview has already been sent to Nobscot.',
                    confirmButtonText: 'Request',
                    cancelButtonText: 'Cancel',
                    swapOkClose: true,
                }
            })
            .afterClosed()
            .subscribe(isConfirmed => {
                if (!isConfirmed) return;
                this.employeeApi.updateEmployeeExitInterviewRequest(this.user.lastEmployeeId, currentUserId)
                    .subscribe(result => {
                        this.setRequestStatusLabel(result);
                        this.msg.setTemporarySuccessMessage('Exit Interview Requested Successfully!');
                    });
            }, this.handleApiError);

        } else {

            this.employeeApi.createEmployeeExitInterviewRequest(this.user.lastEmployeeId, currentUserId)
                .subscribe(result => {
                    this.setRequestStatusLabel(result);
                    this.msg.setTemporarySuccessMessage('Exit Interview Requested Successfully!');
                }, this.handleApiError);

        }
    }

    private handleApiError(error: any) {
        const errorMsg = error.error.errors != null && error.error.errors.length
            ? error.error.errors[0].msg
            : error.message;
        this.msg.setTemporaryMessage(errorMsg, MessageTypes.error);
    }

}

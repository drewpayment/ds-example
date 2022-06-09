import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { WorkNumberModalComponent } from './work-number-modal.component';
import { AccountService } from '@ds/core/account.service';
import { UserInfo, BetaFeatureType } from '@ds/core/shared';

@Component({
    selector: 'ds-work-number-trigger',
    templateUrl: './work-number-trigger.component.html',
})
export class WorkNumberModalTriggerComponent implements OnInit {
    user: UserInfo;
    hasMenuWrapper: any;
    feature: any;

    constructor(
        private dialog: MatDialog,
        private accountService: AccountService
    ) {}

    ngOnInit() {

        // CODE REVIEW
        this.accountService.getUserInfo()
            .subscribe(user => {
                this.user = user;
                this.feature = this.user.betaFeatures.find(b => b.betaFeatureId == BetaFeatureType.MenuWrapper);
                this.hasMenuWrapper = this.feature && this.feature.isBetaActive;
            });
    }

    openWorkNumber() {
        const config  = new MatDialogConfig<any>();

        config.width = '800px';
        config.disableClose = true;
        config.panelClass = 'equifax';

        const dialogRef = this.dialog.open(WorkNumberModalComponent, config);

        dialogRef.afterClosed().subscribe(result => {
        });
    }

}

import { Component, OnInit } from '@angular/core';
import { ArDeposit } from '../shared/ar-deposit.model';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { EditPostingDialogComponent } from './edit-posting-dialog.component';
import { Observable } from 'rxjs';
import { ArService } from '../shared/ar.service';
import { IArPayment } from '../shared/ar-payment.model';
import { tap } from 'rxjs/operators';

@Component({
    selector: 'ds-edit-posting-trigger',
    templateUrl: './edit-posting-trigger.component.html'
})
export class EditPostingTriggerComponent implements OnInit {
    isLoading$: Observable<any>;
    payments: IArPayment[];

    constructor(private dialog: MatDialog, private arService: ArService) {

    }

    ngOnInit() {
    }

    openPosting(posting: ArDeposit = null) {
        const config = new MatDialogConfig<any>();

        config.width = '1000px';
        config.disableClose = true;
        //config.panelClass = 'eeoc-locations';
        config.data = {
            posting: posting,
            payments: this.payments
        };

        const dialogRef = this.dialog.open(EditPostingDialogComponent, config);
        dialogRef.afterClosed().subscribe(() => {
            if(posting.postedDate){
                posting.postedDate = config.data.posting.postedDate;
            } else {
                posting.total = config.data.posting.total;
            }

            // this.eeocApiService.getEeocLocationsListMultipleClients(this.clientIdList, false, false).subscribe((eeocLocations) => {
            //     this.eeocLocations = eeocLocations
            //     this.empEeocService.notifyLocationsChanged();

            //     //if from aspx page: make sure to click dom element hidden button that refreshes update panel
            //     if(this.saveSuccessClickTarget != null) ang.element(document).find('#' + this.saveSuccessClickTarget).click();
            // });
        });
    }

}

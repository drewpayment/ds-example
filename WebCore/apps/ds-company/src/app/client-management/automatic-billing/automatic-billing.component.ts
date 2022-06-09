import { Component, OnInit, ViewChild } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from '@ds/core/shared';
import { FormControl } from '@angular/forms';
import { skip } from 'rxjs/operators';
import { AutomaticBillingDialogComponent } from './automatic-billing-dialog/automatic-billing-dialog.component';
import { BillingWhatToCount } from '../../enums/billing-what-to-count.enum';
import { BillingFrequency } from '../../enums/billing-frequency.enum';
import { BillingPeriod } from '../../enums/billing-period.enum';
import { BillingService } from '../services/billing.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ConfirmModalComponent } from '@ds/core/resources/confirm-modal/confirm-modal.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { AutomaticBilling, BillingItem, FeatureOptions } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-automatic-billing',
  templateUrl: './automatic-billing.component.html',
  styleUrls: ['./automatic-billing.component.scss']
})
export class AutomaticBillingComponent implements OnInit {

  isLoading = true;
  featureOption = new FormControl("")
  user: UserInfo;
  featureOptions: FeatureOptions[];
  selectedFeature: FeatureOptions;
  billingItems: AutomaticBilling[];

  /** Mat Table variables */
  billingSource: any;
  billingColumns:string[] = ['Name', 'Frequency', 'Line', 'Flat', 'Per Qty', 'What To Count', 'actions'];
  billingPaginator: MatPaginator;


  /** Enum Helpers for readability */
  billingWTCHelper = BillingWhatToCount;
  billingFreqHelper = BillingFrequency;
  billingPeriodHelper = BillingPeriod;


  /** Setting Paginator Dynamically */
  @ViewChild('billingPaginator', {static: false}) set matBillingPaginator(mp: MatPaginator) {
    this.billingPaginator = mp;
    this.setBillingDataSourceAttr();
  }

  constructor(
    private accountService: AccountService,
    private billingService: BillingService,
    private dialog: MatDialog,
    private ngxMsg: NgxMessageService
  ) { }

  ngOnInit() {
    this.billingService.getFeatureOptions().subscribe((features) => {
      this.featureOptions = features;
      this.selectedFeature = this.featureOptions[0];
      this.featureOption.setValue(this.featureOptions[0].featureOptionId);
      this.billingService.fetchAutomaticBillingItems();
    }, (error: HttpErrorResponse) => {
      this.ngxMsg.setErrorResponse(error);
    });

    this.featureOption.valueChanges.pipe(skip(1)).subscribe(val => {
      this.featureOptionChanged(val);
    });

    this.billingService.automaticBillingItems$.subscribe((billingItems) => {
      if (billingItems == null) return;
      this.billingItems = billingItems;
      this.featureOptionChanged(this.selectedFeature.featureOptionId);
      this.isLoading = false;
    });

  }

  featureOptionChanged(val: number) {
    this.selectedFeature = this.featureOptions.find(x => x.featureOptionId == val);
    if (this.billingItems) {
      this.billingSource = new MatTableDataSource<AutomaticBilling>(this.billingItems.filter(x => x.featureOptionId == this.selectedFeature.featureOptionId));
    }
  }

  openModal(billingItem?: BillingItem) {
    const dialogRef = this.dialog.open(AutomaticBillingDialogComponent, {
      width:"600px",
      data: {
        billingItem: billingItem,
        featureOptionId: this.selectedFeature.featureOptionId
      }
    });
  }

  confirmDeletion(id: number) {

    const confirmDialogRef = this.dialog.open(ConfirmModalComponent, {
        width: "350px",
        data: {
            displayText: 'Are you sure you want to delete this billing item?',
            confirmButtonText: 'Delete',
            cancelButtonText: 'Cancel',
            swapOkClose: true
        }
    });

    confirmDialogRef.afterClosed().subscribe((confirmed: boolean) => {
        if (confirmed) {
            this.delete(id);
        }
    });
  }

  delete(automaticBillingId: number) {
    this.billingService.deleteAutomaticBillingItem(automaticBillingId).subscribe(() => {
      this.ngxMsg.setSuccessMessage("Billing item deleted successfully.");
      this.billingService.fetchAutomaticBillingItems();
    }, (error: HttpErrorResponse) => {
      this.ngxMsg.setErrorResponse(error);
    });
  }

  setBillingDataSourceAttr() {
    if (this.billingSource) this.billingSource.paginator = this.billingPaginator;
  }


}

import { Component, OnInit }       from '@angular/core';
import { CheckStockDialogService } from '../check-stock-dialog/check-stock-dialog.service';

@Component({
  selector    : 'ds-check-stock-trigger',
  templateUrl : './check-stock-trigger.component.html',
  styleUrls   : ['./check-stock-trigger.component.scss']
})
export class CheckStockTriggerComponent implements OnInit {

  constructor(private checkStockDialog : CheckStockDialogService) { }

  ngOnInit() {

  }

  orderSupplies() {
    let dialog = this.checkStockDialog.open(0);

    dialog.afterClosed().subscribe(result => {
      
    });
  }
}

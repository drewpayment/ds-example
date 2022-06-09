import { UpdateBalanceDialogService } from './../update-balance-dialog/update-balance-dialog.service';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'ds-update-balance-trigger',
  templateUrl: './update-balance-trigger.component.html',
  styleUrls: ['./update-balance-trigger.component.scss']
})
export class UpdateBalanceTriggerComponent implements OnInit {

    @Input() clientId: number;
    @Input() userId: number;

    constructor(private modal: UpdateBalanceDialogService) { }

    ngOnInit() {
    }

    openUpdateBalanceModal(){
        this.modal.openUpdateBalanceModal(this.clientId, this.userId);
    }
}

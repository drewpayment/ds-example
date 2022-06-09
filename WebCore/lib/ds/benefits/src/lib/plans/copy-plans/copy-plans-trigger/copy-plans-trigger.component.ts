import { Component, OnInit, Input } from '@angular/core';
import { CopyPlansDialogService } from '../copy-plans-dialog/copy-plans-dialog.service';

@Component({
    selector: 'ds-copy-plans-trigger',
    templateUrl: './copy-plans-trigger.component.html',
    styleUrls: ['./copy-plans-trigger.component.scss']
})
export class CopyPlansTriggerComponent implements OnInit {
    private _clientId: number;

    @Input()
    set clientId(value: number) {
        this._clientId = value;
    }

    constructor(private copyPlansDialogSvc: CopyPlansDialogService) { }

    ngOnInit() {
    }

    openDialog() {
        this.copyPlansDialogSvc.open(this._clientId);
    }

}

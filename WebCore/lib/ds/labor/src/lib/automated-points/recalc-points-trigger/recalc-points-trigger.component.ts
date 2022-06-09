import { RecalcPointsDialogService } from './../recalc-points-dialog/recalc-points-dialog.service';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'ds-recalc-points-trigger',
  templateUrl: './recalc-points-trigger.component.html',
  styleUrls: ['./recalc-points-trigger.component.scss']
})
export class RecalcPointsTriggerComponent implements OnInit {

    @Input() clientId: number;

    constructor(private modal: RecalcPointsDialogService) { }

    ngOnInit() {
    }

    openRecalcPointsModal(){
        this.modal.openRecalcPointsModal(this.clientId);
    }
}

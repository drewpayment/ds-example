import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from "@angular/material/dialog";

@Component({
  selector: 'ds-competency-delete-confirm-dialog',
  templateUrl: './competency-delete-confirm-dialog.component.html',
  styleUrls: ['./competency-delete-confirm-dialog.component.scss']
})
export class CompetencyDeleteConfirmDialogComponent implements OnInit {

    constructor(public ref:MatDialogRef<CompetencyDeleteConfirmDialogComponent>) {}

    ngOnInit(){
        
    }

    onNoClick():void {
        this.ref.close();
    }

    deleteCompetency():void {
        this.ref.close(true);
    }
}

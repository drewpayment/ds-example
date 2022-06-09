import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";

@Component({
  selector: 'ds-preview-final-disclaimer-dialog',
  templateUrl: './preview-final-disclaimer-dialog.component.html',
  styleUrls: ['./preview-final-disclaimer-dialog.component.scss']
})
export class PreviewFinalDisclaimerDialogComponent implements OnInit {
    
    previewText:string;
    agreementText:string;
    
    constructor(
        public dialogRef:MatDialogRef<PreviewFinalDisclaimerDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:any
    ) {}

    ngOnInit() {
        this.previewText = this.data.main;
        this.agreementText = this.data.agreement;
    }

    onNoClick():void {
        this.dialogRef.close();
    }
}
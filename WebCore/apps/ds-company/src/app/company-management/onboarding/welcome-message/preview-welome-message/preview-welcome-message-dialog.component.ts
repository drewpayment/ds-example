import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";

@Component({
  selector: 'ds-preview-welcome-message-dialog',
  templateUrl: './preview-welcome-message-dialog.component.html',
  styleUrls: ['./preview-welcome-message-dialog.component.scss']
})
export class PreviewWelcomeMessageDialogComponent implements OnInit {
    
    previewText:string;
    
    constructor(
        public dialogRef:MatDialogRef<PreviewWelcomeMessageDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:string
    ) {}

    ngOnInit() {
        this.previewText = this.data;
    }

    onNoClick():void {
        this.dialogRef.close();
    }
}
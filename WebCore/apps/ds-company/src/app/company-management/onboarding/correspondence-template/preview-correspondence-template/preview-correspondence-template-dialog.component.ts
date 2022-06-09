import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";

@Component({
  selector: 'ds-preview-correspondence-template-dialog',
  templateUrl: './preview-correspondence-template-dialog.component.html',
  styleUrls: ['./preview-correspondence-template-dialog.component.scss']
})
export class PreviewCorrespondenceTemplateDialogComponent implements OnInit {
    
    previewText:string;
    subject:string;
    description:string;
    isText:boolean;
    
    constructor(
        public dialogRef:MatDialogRef<PreviewCorrespondenceTemplateDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:any
    ) {}

    ngOnInit() {
        this.previewText = this.data.body;
        this.subject = this.data.subject;
        this.description = this.data.description;
        this.isText=this.data.isText;
    }

    onNoClick():void {
        this.dialogRef.close();
    }
}
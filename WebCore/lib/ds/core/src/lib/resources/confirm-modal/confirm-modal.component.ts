import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

interface DialogData {
    displayText: string;
    cautionText: string;
    confirmButtonText: string;
    cancelButtonText: string;
    swapOkClose: boolean;
}
@Component({
  selector: 'ds-confirm-modal',
  templateUrl: './confirm-modal.component.html',
  styleUrls: ['./confirm-modal.component.scss']
})
export class ConfirmModalComponent implements OnInit {
    displayText = '';
    cautionText = '';
    confirmButtonText = 'Accept';
    cancelButtonText = 'Cancel';
    swapOkClose = false;
    constructor(
        public dialogRef: MatDialogRef<ConfirmModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
    ) {
        if (data) {
            this.displayText = data.displayText || this.displayText;
            this.cautionText = data.cautionText || this.cautionText;
            this.confirmButtonText = data.confirmButtonText || this.confirmButtonText;
            this.cancelButtonText = data.cancelButtonText || this.cancelButtonText;
            this.swapOkClose = data.swapOkClose || this.swapOkClose;
        }
    }

    ngOnInit() {
    }

    save(): void {
        this.dialogRef.close(true);
    }

    cancel(): void {
        this.dialogRef.close(false);
    }
}

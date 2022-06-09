import { Component, HostListener, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'ds-confirm-dialog',
    templateUrl: './ds-confirm-dialog.component.html',
    styleUrls: ['./ds-confirm-dialog.component.scss']
})
export class DsConfirmDialogContentComponent {

    constructor(
        private dialogRef: MatDialogRef<DsConfirmDialogContentComponent>,
        @Inject(MAT_DIALOG_DATA) public data: {
            title: string;
            message: string;
            confirm: string;
            allowAction?: boolean;
        }
    ) {}

    onConfirm(): void {
        this.dialogRef.close(true);
    }

    onCancel(): void {
        this.dialogRef.close(false);
    }

    @HostListener("keydown.esc")
    public onEsc() {
        this.dialogRef.close(false);
    }
}

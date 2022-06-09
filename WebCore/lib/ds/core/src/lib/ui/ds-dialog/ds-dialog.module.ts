import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DsDialogComponent, DsDialogHeaderDirective, DsDialogFooterDirective } from '@ds/core/ui/ds-dialog/ds-dialog.component';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
        DsDialogComponent,
        DsDialogHeaderDirective,
        DsDialogFooterDirective
    ],
    exports: [
        DsDialogComponent,
        DsDialogHeaderDirective,
        DsDialogFooterDirective
    ]
})
export class DsDialogModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DsConfirmDialogContentComponent } from './ds-confirm-dialog.component';
import { ConfirmDialogService } from './ds-confirm-dialog.service';


@NgModule({
  declarations: [DsConfirmDialogContentComponent],
  imports: [
    CommonModule
  ],
  providers: [
    ConfirmDialogService
  ],
  entryComponents: [
    DsConfirmDialogContentComponent
  ]
})
export class DsConfirmDialogModule { }

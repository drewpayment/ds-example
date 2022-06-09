import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { MarkdownModule } from 'ngx-markdown';
import { ConfirmDocsComponent } from './confirm-docs/confirm-docs.component';
import { CommunicationDialogComponent } from './communication-dialog/communication-dialog.component';
import { DsConfirmDialogModule } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.module';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';

@NgModule({
  declarations: [ConfirmDocsComponent, CommunicationDialogComponent, ConfirmDialogComponent],
  imports: [
    CommonModule,
    DocsMaterialModule,
    MarkdownModule.forChild(),
    DsConfirmDialogModule
  ],
  entryComponents: [
    CommunicationDialogComponent
  ]
})
export class ConfirmDialogModule { }

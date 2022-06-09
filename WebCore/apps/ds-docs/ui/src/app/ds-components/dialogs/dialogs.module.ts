import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { DialogDocsComponent } from './dialog-docs/dialog-docs.component';
import { BasicDialogComponent } from './basic-dialog/basic-dialog.component';
import { DsDialogModule } from '@ds/core/ui/ds-dialog/ds-dialog.module';
import { BorderedDialogComponent } from './bordered-dialog/bordered-dialog.component';
import { NavDialogComponent } from './nav-dialog/nav-dialog.component';

@NgModule({
  declarations: [DialogDocsComponent, BasicDialogComponent, BorderedDialogComponent, NavDialogComponent],
  imports: [
    CommonModule,
    DocsMaterialModule,
    MarkdownModule.forChild(),
    DsDialogModule
  ],
  entryComponents: [
    BasicDialogComponent,
    BorderedDialogComponent,
    NavDialogComponent
  ],
})
export class DialogsModule { }

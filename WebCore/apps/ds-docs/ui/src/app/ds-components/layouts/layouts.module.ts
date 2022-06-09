import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { MarkdownModule } from 'ngx-markdown';
import { LayoutsDocsComponent } from './layouts-docs/layouts-docs.component';
import { StandardFormComponent } from './standard-form/standard-form.component';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { SideBySideComponent } from './side-by-side/side-by-side.component';
import { AlignmentComponent } from './alignment/alignment.component';
import { OverflowColumnsComponent } from './overflow-columns/overflow-columns.component';
import { TextBreakComponent } from './text-break/text-break.component';

@NgModule({
  declarations: [
      LayoutsDocsComponent,
      StandardFormComponent,
      SideBySideComponent,
      AlignmentComponent,
      OverflowColumnsComponent,
      TextBreakComponent
  ],
  imports: [
    CommonModule,
    DocsMaterialModule,
    MarkdownModule.forChild(),
    DsCardModule
  ]
})
export class LayoutsModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { PrintableFormDocsComponent } from './printable-form-docs/printable-form-docs.component';
import { PrintableFormComponent } from './printable-form/printable-form.component';

@NgModule({
  declarations: [PrintableFormDocsComponent, PrintableFormComponent],
  imports: [
    CommonModule,
    MarkdownModule.forChild(),
    DocsMaterialModule
  ]
})
export class PrintableFormsModule { }

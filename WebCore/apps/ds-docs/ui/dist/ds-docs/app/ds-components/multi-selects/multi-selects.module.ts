import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { MultiSelectDocsComponent } from './multi-select-docs/multi-select-docs.component';
import { MultiSelectComponent } from './multi-select/multi-select.component';
import { MultiSelectBulletedListComponent } from './multi-select-bulleted-list/multi-select-bulleted-list.component';

@NgModule({
  declarations: [MultiSelectDocsComponent, MultiSelectComponent, MultiSelectBulletedListComponent],
  imports: [
    CommonModule,
    DocsMaterialModule,
    MarkdownModule.forChild()
  ]
})
export class MultiSelectsModule { }

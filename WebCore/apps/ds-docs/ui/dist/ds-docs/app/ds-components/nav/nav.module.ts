import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { NavTitleComponent } from './nav-title/nav-title.component';
import { NavDocsComponent } from './nav-docs/nav-docs.component';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { RouterModule, Routes } from '@angular/router';

@NgModule({
  declarations: [NavTitleComponent, NavDocsComponent],
  imports: [
    CommonModule,
    DocsMaterialModule,
    DsCardModule,
    MarkdownModule.forChild(),
  ],
})
export class NavModule { }

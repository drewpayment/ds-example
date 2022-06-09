import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { MarkdownModule } from 'ngx-markdown';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { SidenavComponent } from './sidenav/sidenav.component';
import { SidenavDocsComponent } from './sidenav-docs/sidenav-docs.component';

@NgModule({
  declarations: [
    SidenavDocsComponent,
    SidenavComponent
  ],
  imports: [
    CommonModule,
    DocsMaterialModule,
    MarkdownModule.forChild(),
    DsCardModule
  ]
})
export class SidenavModule { }

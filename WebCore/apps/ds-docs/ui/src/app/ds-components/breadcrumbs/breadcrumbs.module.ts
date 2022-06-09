import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { BreadcrumbsDocsComponent } from './breadcrumbs-docs/breadcrumbs-docs.component';
import { BasicBreadcrumbComponent } from './basic-breadcrumb/basic-breadcrumb.component';
import { MaterialModule } from '@ds/core/ui/material';
import { BreadcrumbLayoutComponent } from './breadcrumb-layout/breadcrumb-layout.component';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { BreadcrumbEmployeeComponentComponent } from './breadcrumb-employee-component/breadcrumb-employee-component.component';

@NgModule({
  declarations: [BreadcrumbsDocsComponent, BasicBreadcrumbComponent, BreadcrumbLayoutComponent, BreadcrumbEmployeeComponentComponent],
  imports: [
    CommonModule,
    MaterialModule,
    MarkdownModule.forChild(),
    DsCardModule
  ]
})
export class BreadcrumbsModule { }

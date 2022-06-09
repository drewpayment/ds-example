import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material';
import { MarkdownModule } from 'ngx-markdown';
import { WidgetDocsComponent } from './widget-docs/widget-docs.component';
import { WidgetComponent } from './widget/widget.component';
import { DsWidgetModule } from '@ds/core/ui/ds-widget/ds-widget.module';
import { WidgetTopComponent } from './widget-top/widget-top.component';

@NgModule({
  declarations: [WidgetDocsComponent, WidgetComponent, WidgetTopComponent],
  imports: [
    CommonModule,
    MaterialModule,
    MarkdownModule.forChild(),
    DsWidgetModule
  ]
})
export class WidgetModule { }

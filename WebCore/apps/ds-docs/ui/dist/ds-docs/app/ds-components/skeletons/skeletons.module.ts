import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SkeletonsDocsComponent } from './skeletons-docs/skeletons-docs.component';
import { dsCardTitle, dsCardTitleValue, DsWidgetComponent, dsWidgetContent, dsWidgetDetail, DsWidgetHeader, dsWidgetIcon } from './ds-widget/ds-widget.component';
import { MarkdownModule } from 'ngx-markdown';
import { MaterialModule } from '@ds/core/ui/material';



@NgModule({
  declarations: [SkeletonsDocsComponent, DsWidgetComponent, dsWidgetContent, DsWidgetHeader, dsWidgetDetail, dsWidgetIcon, dsCardTitle, dsCardTitleValue],
  imports: [
    CommonModule,
    MarkdownModule.forChild(),
    MaterialModule,
  ]
})
export class SkeletonsModule { }

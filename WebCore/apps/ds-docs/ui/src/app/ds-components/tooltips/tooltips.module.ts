import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { MarkdownModule } from 'ngx-markdown';
import { TooltipsDocsComponent } from './tooltips-docs/tooltips-docs.component';
import { TooltipComponent } from './tooltip/tooltip.component';
import { DsTooltipModule } from '@ds/core/ui/ds-tooltip/ds-tooltip.module';

@NgModule({
  declarations: [TooltipsDocsComponent, TooltipComponent],
  imports: [
    CommonModule,
    DocsMaterialModule,
    MarkdownModule.forChild(),
    DsTooltipModule
  ]
})
export class TooltipsModule { }

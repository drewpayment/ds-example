import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { ProgressDocsComponent } from './progress-docs/progress-docs.component';
import { ProgressComponent } from './progress/progress.component';
import { ProgressPositionComponent } from './progress-position/progress-position.component';
import { ProgressLogoComponent } from './progress-logo/progress-logo.component';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsProgressModule } from '@ds/core/ui/ds-progress';

@NgModule({
  declarations: [ProgressDocsComponent, ProgressComponent, ProgressPositionComponent, ProgressLogoComponent],
  imports: [
    CommonModule,
    MarkdownModule.forChild(),
    DocsMaterialModule,
    LoadingMessageModule,
    DsProgressModule
  ]
})
export class ProgressModule { }

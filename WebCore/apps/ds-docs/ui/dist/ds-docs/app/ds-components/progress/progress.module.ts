import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { ProgressDocsComponent } from './progress-docs/progress-docs.component';
import { ProgressComponent } from './progress/progress.component';
import { ProgressPositionComponent } from './progress-position/progress-position.component';
import { ProgressLogoComponent } from './progress-logo/progress-logo.component';
import { LogoComponent } from './logo/logo.component';

@NgModule({
  declarations: [ProgressDocsComponent, ProgressComponent, ProgressPositionComponent, ProgressLogoComponent, LogoComponent],
  imports: [
    CommonModule,
    MarkdownModule.forChild(),
    DocsMaterialModule
  ]
})
export class ProgressModule { }

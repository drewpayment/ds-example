import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeDocsComponent } from './home-docs/home-docs.component';
import { DsHomeRoutingModule } from './ds-home-routing.module';
import { MarkdownModule } from 'ngx-markdown';


@NgModule({
  declarations: [HomeDocsComponent],
  imports: [
    CommonModule,
    DsHomeRoutingModule,
    MarkdownModule.forChild(),
  ]
})
export class DsHomeModule { }

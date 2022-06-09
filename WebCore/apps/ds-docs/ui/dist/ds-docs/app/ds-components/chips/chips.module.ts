import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { MaterialModule } from '@ds/core/ui/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { SharedComponentsModule } from '../shared-components/shared-components.module';
import { ChipsDocsComponent } from './chips-docs/chips-docs.component';



@NgModule({
  declarations: [
    ChipsDocsComponent
  ],
  imports: [
    CommonModule,
    MarkdownModule.forRoot(),
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    DsCardModule,
    SharedComponentsModule
  ]
})
export class ChipsModule { }

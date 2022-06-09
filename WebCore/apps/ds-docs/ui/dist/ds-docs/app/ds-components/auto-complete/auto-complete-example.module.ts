import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DsAutocompleteModule } from '@ds/core/ui/ds-autocomplete';
import { AutoCompleteDocsComponent } from './auto-complete-docs/auto-complete-docs.component';
import { MarkdownModule } from 'ngx-markdown';
import { MaterialModule } from '@ds/core/ui/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { AutoCompleteComponent } from './auto-complete/auto-complete.component';
import { SharedComponentsModule } from '../shared-components/shared-components.module';



@NgModule({
  declarations: [
    AutoCompleteDocsComponent, 
    AutoCompleteComponent,
  ],
  imports: [
    CommonModule,
    MarkdownModule.forRoot(),
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    DsAutocompleteModule,
    DsCardModule,
    SharedComponentsModule
  ]
})
export class AutoCompleteExampleModule { }

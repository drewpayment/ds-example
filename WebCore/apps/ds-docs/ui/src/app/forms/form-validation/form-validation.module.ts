import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { FormValidationDocsComponent } from './form-validation-docs/form-validation-docs.component';
import { TemplateValidationComponent } from './template-validation/template-validation.component';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { ReactiveValidationComponent } from './reactive-validation/reactive-validation.component';

@NgModule({
  declarations: [FormValidationDocsComponent, TemplateValidationComponent, ReactiveValidationComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DsCoreFormsModule,
    DsCardModule,
    MarkdownModule.forChild()
  ]
})
export class FormValidationModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputButtonGroupComponent } from '../form-elements/input-button-group/input-button-group.component';
import { MaterialModule } from '@ds/core/ui/material';
import { AutoCompleteMultipleComponent } from './auto-complete-multiple/auto-complete-multiple.component';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { AutoCompleteContactExampleComponent } from './auto-contact-complete-example/auto-complete-contact-example.component';
import { DsAutocompleteModule } from '@ds/core/ui/ds-autocomplete';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    InputButtonGroupComponent,
    AutoCompleteMultipleComponent,
    AutoCompleteContactExampleComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    DsCardModule,
    DsAutocompleteModule,
    ReactiveFormsModule
  ],
  exports: [
    InputButtonGroupComponent,
    AutoCompleteMultipleComponent,
    AutoCompleteContactExampleComponent
  ]
})
export class SharedComponentsModule { }

import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { DsContactAutocompleteComponent } from "@ds/core/ui/ds-autocomplete/ds-contact-autocomplete/ds-contact-autocomplete.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatChipsModule } from "@angular/material/chips";
import { MatIconModule } from "@angular/material/icon";
import {
  ContactToNamePipe,
  MapToContactPipe,
} from "../pipes/contact-to-name.pipe";
import { SortContactsPipe } from "../pipes/sort-contacts.pipe";
import { AvatarModule } from "../avatar/avatar.module";
import { DsTypeHintComponent } from './ds-type-hint/ds-type-hint.component';

@NgModule({
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatChipsModule,
    MatIconModule,
    FormsModule,
    ReactiveFormsModule,
    AvatarModule,
  ],
  exports: [
    DsContactAutocompleteComponent,
    ContactToNamePipe,
    MapToContactPipe,
    SortContactsPipe,
    DsTypeHintComponent,
  ],
  declarations: [
    DsContactAutocompleteComponent,
    ContactToNamePipe,
    MapToContactPipe,
    SortContactsPipe,
    DsTypeHintComponent,
  ],
  entryComponents: [DsContactAutocompleteComponent],
})
export class DsAutocompleteModule {}

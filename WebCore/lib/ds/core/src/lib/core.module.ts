import { NgModule, InjectionToken } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { ExtractMaybe } from "@ds/core/shared/maybe.pipe";
import { ToMaybe } from "@ds/core/shared/to-maybe.pipe";
import {
  CompareDatePipe,
  FindMaxDatePipe,
  FindMinDatePipe,
} from "@ds/core/date-comparison/date-comparison";
import {
  attachSaveHandlerFnProvider,
  createSaveHandlerFnProvider,
  AttachErrorHandlerFnProvider,
} from "./shared/shared-api-fn";
import { GroupDialogComponent } from "./groups/group-dialog/group-dialog.component";
import { MaterialModule } from "./ui/material/material.module";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { DsCoreFormsModule } from "./ui/forms";
import { DsAutocompleteModule } from "./ui/ds-autocomplete";
import { ToAutocompleteItemPipe } from "./groups/group-dialog/to-autocomplete-item.pipe";
import { SortTemplatesPipe } from "./groups/shared/sort-templates.pipe";
import { ContactToNamePipe } from "./ui/pipes/contact-to-name.pipe";
import { SortContactsPipe } from "./ui/pipes/sort-contacts.pipe";
import { ConvertToMomentPipe } from "./pipes/convert-to-moment.pipe";
import { WindowRef } from "./shared/window-ref.provider";
import { UserTypePipe } from "./pipes/user-type.pipe";
import { FilterResourcePipe } from "./ui/pipes/filter-resources.pipe";
import { PermissionErrorModule } from "./ui/permission-error/permission-error.module";
import { NgxMessageComponent } from './ngx-message/ngx-message.component';
import { EmployeeStatusTypePipe } from './pipes/employee-status-type.pipe';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    DsCoreFormsModule,
    DsAutocompleteModule,
    PermissionErrorModule
  ],
  declarations: [
    ExtractMaybe,
    ToMaybe,
    CompareDatePipe,
    FindMaxDatePipe,
    FindMinDatePipe,
    ConvertToMomentPipe,
    GroupDialogComponent,
    ToAutocompleteItemPipe,
    SortTemplatesPipe,
    UserTypePipe,
    FilterResourcePipe,
    NgxMessageComponent,
    EmployeeStatusTypePipe,
  ],
  providers: [
    attachSaveHandlerFnProvider,
    createSaveHandlerFnProvider,
    AttachErrorHandlerFnProvider,
    WindowRef,
  ],
  exports: [
    ExtractMaybe,
    ToMaybe,
    CompareDatePipe,
    FindMaxDatePipe,
    FindMinDatePipe,
    ConvertToMomentPipe,
    ToAutocompleteItemPipe,
    SortTemplatesPipe,
    ContactToNamePipe,
    SortContactsPipe,
    UserTypePipe,
    FilterResourcePipe,
    NgxMessageComponent,
    EmployeeStatusTypePipe,
  ],
  entryComponents: [GroupDialogComponent],
})
export class CoreModule {}

import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { DsComponentsRoutingModule } from "./ds-components-routing.module";
import { DsComponentsDocsComponent } from "./ds-components-docs/ds-components-docs.component";
import { MarkdownModule } from "ngx-markdown";
import { BreadcrumbsModule } from "./breadcrumbs/breadcrumbs.module";
import { ButtonsModule } from "./buttons/buttons.module";
import { CalendarModule } from "./calendar/calendar.module";
import { DialogsModule } from "./dialogs/dialogs.module";
import { TablesModule } from "./tables/tables.module";
import { ListsModule } from "./lists/lists.module";
import { CardModule } from "./card/card.module";
import { NavModule } from "./nav/nav.module";
import { ChartsModule } from "./charts/charts.module";
import { ConfirmDialogModule } from "./confirm-dialog/confirm-dialog.module";
import { EmptyStatesModule } from "./empty-states/empty-states.module";
import { LayoutsModule } from "./layouts/layouts.module";
import { PrintableFormsModule } from "./printable-forms/printable-forms.module";
import { GridModule } from "./grid/grid.module";
import { NgxMaskModule, IConfig } from "ngx-mask";
import { FormElementsModule } from "./form-elements/form-elements.module";
import { MultiSelectsModule } from "./multi-selects/multi-selects.module";
import { ProgressModule } from "./progress/progress.module";
import { TooltipsModule } from "./tooltips/tooltips.module";
import { SwitchModule } from './switch/switch.module';
import { SkeletonsModule } from './skeletons/skeletons.module';
import { AvatarExampleModule } from './avatar/avatar.module';
import { AvatarSharedModule } from './shared-components/avatar-shared/avatar-shared.module';
import { AutoCompleteExampleModule } from './auto-complete/auto-complete-example.module';
import { DatePickerModule } from "./date-picker/date-picker.module";
import { FormsModule } from "@angular/forms";
import { DsConfirmDialogModule } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.module";
import { ChipsModule } from "./chips/chips.module";
import { SidenavModule } from "./sidenav/sidenav.module";

export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};

@NgModule({
  declarations: [DsComponentsDocsComponent],
  imports: [
    CommonModule,
    BreadcrumbsModule,
    ButtonsModule,
    CalendarModule,
    DialogsModule,
    TablesModule,
    NavModule,
    MarkdownModule.forChild(),
    ListsModule,
    CardModule,
    ChartsModule,
    ConfirmDialogModule,
    EmptyStatesModule,
    LayoutsModule,
    PrintableFormsModule,
    GridModule,
    FormElementsModule,
    MultiSelectsModule,
    ProgressModule,
    TooltipsModule,
    AvatarExampleModule,
    SwitchModule,
    DatePickerModule,
    SkeletonsModule,
    NgxMaskModule.forRoot(options),
    FormsModule,
    DsConfirmDialogModule,
    AvatarSharedModule,
    AutoCompleteExampleModule,
    ChipsModule,
    DsComponentsRoutingModule,
    SidenavModule
  ],
})
export class DsComponentsModule {}

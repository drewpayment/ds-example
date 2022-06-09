import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { DsComponentsDocsComponent } from "./ds-components-docs/ds-components-docs.component";
import { BreadcrumbsDocsComponent } from "./breadcrumbs/breadcrumbs-docs/breadcrumbs-docs.component";
import { ButtonDocsComponent } from "./buttons/button-docs/button-docs.component";
import { CalendarDocsComponent } from "./calendar/calendar-docs/calendar-docs.component";
import { ChartDocsComponent } from "./charts/chart-docs/chart-docs.component";
import { DialogDocsComponent } from "./dialogs/dialog-docs/dialog-docs.component";
import { ListDocsComponent } from "./lists/list-docs/list-docs.component";
import { TableDocsComponent } from "./tables/table-docs/table-docs.component";
import { CardDocsComponent } from "./card/card-docs/card-docs.component";
import { NavDocsComponent } from "./nav/nav-docs/nav-docs.component";
import { ConfirmDocsComponent } from "./confirm-dialog/confirm-docs/confirm-docs.component";
import { EmptyStatesDocsComponent } from "./empty-states/empty-states-docs/empty-states-docs.component";
import { LayoutsDocsComponent } from "./layouts/layouts-docs/layouts-docs.component";
import { PrintableFormDocsComponent } from "./printable-forms/printable-form-docs/printable-form-docs.component";
import { GridDocsComponent } from "./grid/grid-docs/grid-docs.component";
import { FormElementsDocsComponent } from "./form-elements/form-elements-docs/form-elements-docs.component";
import { MultiSelectDocsComponent } from "./multi-selects/multi-select-docs/multi-select-docs.component";
import { ProgressDocsComponent } from "./progress/progress-docs/progress-docs.component";
import { TooltipsDocsComponent } from "./tooltips/tooltips-docs/tooltips-docs.component";
import { DatePickerDocsComponent } from "./date-picker/date-picker-docs/date-picker-docs.component";
import { AutoCompleteDocsComponent } from "./auto-complete/auto-complete-docs/auto-complete-docs.component";
import { AvatarDocsComponent } from "./avatar/avatar-docs/avatar-docs.component";
import { SwitchDocsComponent } from './switch/switch-docs/switch-docs.component';
import { SkeletonsDocsComponent } from './skeletons/skeletons-docs/skeletons-docs.component';
import { ChipsDocsComponent } from "./chips/chips-docs/chips-docs.component";
import { SidenavDocsComponent } from "./sidenav/sidenav-docs/sidenav-docs.component";

const routes: Routes = [
  {
    path: "ds-components",
    children: [
      {
        path: "",
        component: DsComponentsDocsComponent,
      },
      {
        path: "autocomplete",
        component: AutoCompleteDocsComponent,
      },
      {
        path: "avatar",
        component: AvatarDocsComponent,
      },
      {
        path: "breadcrumbs",
        component: BreadcrumbsDocsComponent,
      },
      {
        path: "buttons",
        component: ButtonDocsComponent,
      },
      {
        path: "calendar",
        component: CalendarDocsComponent,
      },
      {
        path: "chips",
        component: ChipsDocsComponent,
      },
      {
        path: "card",
        component: CardDocsComponent,
      },
      {
        path: "charts",
        component: ChartDocsComponent,
      },
      {
        path: "confirm-dialog",
        component: ConfirmDocsComponent,
      },
      {
        path: "dialogs",
        component: DialogDocsComponent,
      },
      {
        path: "empty-states",
        component: EmptyStatesDocsComponent,
      },
      {
        path: "form-elements",
        component: FormElementsDocsComponent,
      },
      {
        path: "grid",
        component: GridDocsComponent,
      },
      {
        path: "layouts",
        component: LayoutsDocsComponent,
      },
      {
        path: "lists",
        component: ListDocsComponent,
      },
      {
        path: "multi-selects",
        component: MultiSelectDocsComponent,
      },
      {
        path: "nav",
        component: NavDocsComponent,
      },
      {
        path: "print",
        component: PrintableFormDocsComponent,
      },
      {
        path: "progress",
        component: ProgressDocsComponent,
      },
      {
        path: "sidenav",
        component: SidenavDocsComponent,
      },
      {
        path: "switch",
        component: SwitchDocsComponent,
      },
      {
        path: "skeletons",
        component: SkeletonsDocsComponent,
      },
      {
        path: "tables",
        component: TableDocsComponent,
      },
      {
        path: "date-picker",
        component: DatePickerDocsComponent,
      },
      // {
      //     path: 'Tooltips',
      //     component: TooltipsDocsComponent
      // }
    ],
  },
  // {
  //     path: '**',
  //     redirectTo: 'DsComponents'
  // }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DsComponentsRoutingModule {}

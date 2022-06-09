import { ModuleWithProviders, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule, Route, UrlSerializer } from "@angular/router";
import { MaterialModule } from "@ds/core/ui/material/material.module";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { SettingsOutletComponent } from "./settings-outlet/settings-outlet.component";
import { EmployeesModule } from "@ds/employees";
import { HeaderComponent } from "@ds/core/ui/menu-wrapper-header/header/header.component";
import { MenuWrapperHeaderModule } from "@ds/core/ui/menu-wrapper-header/menu-wrapper-header.module";
import { SidebarComponent } from "../sidebar/sidebar.component";
import { LowerCaseUrlSerializer } from "@ds/core/utilities";

const routes: Route[] = [
  {
    path: "pay",
    children: [
      {
        path: "",
        component: SettingsOutletComponent,
      },
      {
        path: "",
        component: HeaderComponent,
        outlet: "header",
      },
      {
        path: "",
        component: SidebarComponent,
        outlet: "sidebar",
      },
    ],
  },
];

@NgModule({
  declarations: [SettingsOutletComponent],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    EmployeesModule,
    MenuWrapperHeaderModule,

    RouterModule.forChild(routes),
  ],
  providers: [
    {
      provide: UrlSerializer,
      useClass: LowerCaseUrlSerializer,
    },
  ],
})
export class PaySettingsAppModule {}

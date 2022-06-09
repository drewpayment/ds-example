import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule, UrlSerializer } from '@angular/router';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ConsentsOutletComponent } from './consents-outlet.component';
import { EmployeesModule } from '@ds/employees';
import { ElectronicConsentsModule } from '@ds/core/employees/electronic-consents/electronic-consents.module';
import { HeaderComponent } from '@ds/core/ui/menu-wrapper-header/header/header.component';
import { MenuWrapperHeaderModule } from '@ds/core/ui/menu-wrapper-header/menu-wrapper-header.module';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { LowerCaseUrlSerializer } from '@ds/core/utilities';

export const ConsentsRoutes: Routes = [
  {
    path: 'consents',
    children: [
      { path: '', component: ConsentsOutletComponent },
      { path: '', component: HeaderComponent, outlet: 'header' },
      { path: '', component: SidebarComponent, outlet: 'sidebar' },
    ],
  },
];

@NgModule({
  declarations: [ConsentsOutletComponent],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    EmployeesModule,
    ElectronicConsentsModule,
    MenuWrapperHeaderModule,

    RouterModule.forChild(ConsentsRoutes),
  ],
  exports: [RouterModule],
  providers: [
    {
      provide: UrlSerializer,
      useClass: LowerCaseUrlSerializer,
    },
  ],
})
export class ConsentsAppModule {}

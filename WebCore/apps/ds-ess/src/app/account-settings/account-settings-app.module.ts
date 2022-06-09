import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule, UrlSerializer } from '@angular/router';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AccountSettingsOutletComponent } from './account-settings-outlet/account-settings-outlet.component';
import { EmployeesModule } from '@ds/employees';
import { HeaderComponent } from '@ds/core/ui/menu-wrapper-header/header/header.component';
import { MenuWrapperHeaderModule } from '@ds/core/ui/menu-wrapper-header/menu-wrapper-header.module';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { LowerCaseUrlSerializer } from '@ds/core/utilities';

export const AccountSettingsRoutes: Routes = [
  {
    path: 'account',
    children: [
      { path: '', component: AccountSettingsOutletComponent },
      { path: '', component: HeaderComponent, outlet: 'header' },
      { path: '', component: SidebarComponent, outlet: 'sidebar' },
    ],
  },
];

@NgModule({
  declarations: [AccountSettingsOutletComponent],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    EmployeesModule,
    MenuWrapperHeaderModule,

    RouterModule.forChild(AccountSettingsRoutes),
  ],
  providers: [
    {
      provide: UrlSerializer,
      useClass: LowerCaseUrlSerializer,
    },
  ],
})
export class AccountSettingsAppModule {}

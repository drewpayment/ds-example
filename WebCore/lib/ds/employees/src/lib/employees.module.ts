import { NgModule } from '@angular/core';
import { DsEmployeeDirectDepositsModule } from './direct-deposits/ds-direct-deposits.module'
import { DsEmployeeTaxesModule } from './taxes/ds-employee-taxes.module';
import { DsEmployeeAccountSettingsModule } from './account-settings/ds-account-settings.module';
import { DsEmployeeResourcesModule } from './resources/ds-resources.module';
import { DsEmployeeProfileModule } from './profile/ds-employee-profile.module';

@NgModule({
  declarations: [],
  imports: [],
  exports: [
    DsEmployeeTaxesModule,
    DsEmployeeDirectDepositsModule,
    DsEmployeeAccountSettingsModule,
    DsEmployeeResourcesModule,
    DsEmployeeProfileModule
  ]
})
export class EmployeesModule { }

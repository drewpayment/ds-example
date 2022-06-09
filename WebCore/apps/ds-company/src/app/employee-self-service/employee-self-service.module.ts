import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmployeeSelfServiceRoutingModule } from './employee-self-service-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { UpgradeModule } from '@angular/upgrade/static';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsCoreResourcesModule } from '@ds/core/resources';
import { DefaultComponent } from './landing-page/default.component';
import { TurboTaxModalComponent } from '../shared/turbo-tax-modal/turbo-tax-modal.component';
import { DominionShortcutWidgetComponent } from '../shared/dominion-shortcut-widget/dominion-shortcut-widget.component';
import { NoPaycheckModalComponent } from '../shared/no-paycheck-modal/no-paycheck-modal.component';
import { InputPunchesWidgetComponent } from '../shared/input-punches-widget/input-punches-widget.component';
import { MatInputModule } from '@angular/material/input';
import { NpsModule } from '@ds/admin/nps/nps.module';
import { ClockTimeCardWidgetComponent } from '../shared/clock-time-card-widget/clock-time-card-widget.component';
import { PunchService } from '@ds/core/timeclock/punch.service';
import { EmployeesModule } from '@ds/employees';
import { CoreModule } from '@ds/core/core.module';
import { SharedModule } from '../shared/shared.module';
import { DsEventsModule } from '@ds/employees/events/ds-events.module';
import { DsEmployeeProfileModule } from '@ds/employees/profile/ds-employee-profile.module';
import { DsAutocompleteModule } from '@ds/core/ui/ds-autocomplete';

@NgModule({
  declarations: [
    DefaultComponent,
    TurboTaxModalComponent,
    DominionShortcutWidgetComponent,
    NoPaycheckModalComponent,
    InputPunchesWidgetComponent,
    ClockTimeCardWidgetComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    DsCardModule,
    DsCoreFormsModule,
    DsAutocompleteModule,
    UpgradeModule,
    LoadingMessageModule,
    DsCoreResourcesModule,
    NpsModule,
    EmployeesModule,    
    CoreModule,
    SharedModule,
    DsEventsModule,
    DsEmployeeProfileModule,

    // ROUTING MODULE
    EmployeeSelfServiceRoutingModule,
  ],
  entryComponents: [
    TurboTaxModalComponent,
    NoPaycheckModalComponent,
  ],
  providers: [PunchService],
})
export class EmployeeSelfServiceModule {
  constructor() { }
}

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { UpgradeModule } from '@angular/upgrade/static';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCoreResourcesModule } from '@ds/core/resources';
import { HardwareComponent } from './hardware/hardware.component';
import { DeviceListComponent } from './client-machine-list/device-list.component';
import { UserListComponent } from './user-list/user-list.component';
import { ClientMachineComponent } from './client-machine/client-machine.component';
import { TemplateListComponent } from './template-list/template-list.component';
import { TransactionListComponent } from './transaction-list/transaction-list.component';
import { DsConfirmDialogModule } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.module';



@NgModule({
  declarations: [
      DeviceListComponent,
      HardwareComponent,
      UserListComponent,
      ClientMachineComponent,
      TemplateListComponent,
      TransactionListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    MatDialogModule,
    DsCardModule,
    DsCoreFormsModule,
    UpgradeModule,
    ScrollingModule,
    LoadingMessageModule,
    DsCoreResourcesModule,
    DsConfirmDialogModule
  ],
  entryComponents: [
      ClientMachineComponent
  ],
})
export class ClockModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientOptionsComponent } from '@ds/core/clients/client-options/client-options.component';
import { BrowserModule } from '@angular/platform-browser';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { FilterClientOptionsPipe } from '@ds/core/clients/client-options/filter-client-options.pipe';
import { LoadingMessageModule } from '../ui/loading-message/loading-message.module';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    AjsUpgradesModule,
    DsCardModule,
    LoadingMessageModule,
  ],
  declarations: [ClientOptionsComponent, FilterClientOptionsPipe],
  exports:[ClientOptionsComponent]
})
export class ClientsModule { }

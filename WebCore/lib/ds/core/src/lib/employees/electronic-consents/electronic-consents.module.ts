import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ElectronicConsentsComponent } from './electronic-consents.component';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { DsCardModule } from '@ds/core/ui/ds-card/ds-card.module';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';

@NgModule({
  imports: [
    CommonModule,
    DsCoreFormsModule,
    FormsModule,
    MatMenuModule,
    MatIconModule,
    MatTooltipModule,
    DsCardModule,
    MaterialModule,
    ReactiveFormsModule,
    LoadingMessageModule,
  ],
  declarations: [
    ElectronicConsentsComponent,
  ],
  exports: [
    ElectronicConsentsComponent,
  ]
})
export class ElectronicConsentsModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DsTooltipComponent } from './ds-tooltip.component';
import { MatTooltipModule } from '@angular/material/tooltip';

@NgModule({
  declarations: [DsTooltipComponent],
  imports: [
    CommonModule,
    MatTooltipModule
  ],
  exports: [
    DsTooltipComponent
  ]
})
export class DsTooltipModule { }

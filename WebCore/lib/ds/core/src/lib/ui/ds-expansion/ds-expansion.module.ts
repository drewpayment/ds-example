import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DsExpansionComponent, DsExpansionPanelActionRow } from '@ds/core/ui/ds-expansion/ds-expansion.component';
import { CdkAccordionModule } from '@angular/cdk/accordion';
import { PortalModule } from '@angular/cdk/portal';
import { DsExpansionContentDirective } from '@ds/core/ui/ds-expansion/ds-expansion-content.directive';
import { DsExpansionBase } from '@ds/core/ui/ds-expansion/ds-expansion-base';

@NgModule({
  imports: [
    CommonModule,
    CdkAccordionModule,
    PortalModule
  ],
  declarations: [
      DsExpansionComponent,
      DsExpansionContentDirective,
      DsExpansionPanelActionRow,
      DsExpansionBase
  ],
  exports: [
      DsExpansionComponent,
      DsExpansionContentDirective,
      DsExpansionPanelActionRow
  ]
})
export class DsExpansionModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DsWidgetComponent } from './ds-widget.component';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import {
    DsWidgetHeader,
    dsCardTitleValue,
    dsCardTitle,
    dsWidgetContent,
    dsWidgetDetail,
    dsWidgetIcon
} from './ds-widget.component';



@NgModule({
  imports: [
    CommonModule,
    MaterialModule
  ],
  exports: [
      DsWidgetComponent,
      DsWidgetHeader,
      dsCardTitleValue,
      dsCardTitle,
      dsWidgetContent,
      dsWidgetDetail,
      dsWidgetIcon
  ],
  declarations: [
      DsWidgetComponent,
      DsWidgetHeader,
      dsCardTitleValue,
      dsCardTitle,
      dsWidgetContent,
      dsWidgetDetail,
      dsWidgetIcon
  ],
})
export class DsWidgetModule { }

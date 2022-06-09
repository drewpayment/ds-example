import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { DsAppConfigModule } from '@ds/core/app-config';

@NgModule({
  imports: [
    CommonModule,
    DsAppConfigModule,
  ],
  declarations: [HeaderComponent]
})
export class EssCommonModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';

import { ButtonDocsComponent } from './button-docs/button-docs.component';
import { PositiveBtnsComponent } from './positive-btns/positive-btns.component';
import { NegativeBtnsComponent } from './negative-btns/negative-btns.component';
import { IconBtnsComponent } from './icon-btns/icon-btns.component';
import { DropdownBtnsComponent } from './dropdown-btns/dropdown-btns.component';
import { IconDropdownBtnsComponent } from './icon-dropdown-btns/icon-dropdown-btns.component';
import { ActionBtnsComponent } from './action-btns/action-btns.component';
import { OutlineIconBtnsComponent } from './outline-icon-btns/outline-icon-btns.component';
import { PillBtnsComponent } from './pill-btns/pill-btns.component';
import { BadgeBtnsComponent } from './badge-btns/badge-btns.component';
import { StaticBadgeBtnsComponent } from './static-badge-btns/static-badge-btns.component';
import { ToggleBtnsComponent } from './toggle-btns/toggle-btns.component';
import { SharedComponentsModule } from '../shared-components/shared-components.module';
import { ToggleBtnsMatSingleComponent } from './toggle-btns-mat-single/toggle-btns-mat-single.component';
import { ToggleBtnsLegacyMultipleComponent } from './toggle-btns-legacy-multiple/toggle-btns-legacy-multiple.component';
import { ToggleBtnsLegacySingleComponent } from './toggle-btns-legacy-single/toggle-btns-legacy-single.component';
import { DocsMaterialModule } from '@ds/docs/material.module';

@NgModule({
  declarations: [
    ButtonDocsComponent,
    PositiveBtnsComponent,
    NegativeBtnsComponent,
    IconBtnsComponent,
    DropdownBtnsComponent,
    IconDropdownBtnsComponent,
    ActionBtnsComponent,
    OutlineIconBtnsComponent,
    PillBtnsComponent,
    BadgeBtnsComponent,
    StaticBadgeBtnsComponent,
    ToggleBtnsComponent,
    ToggleBtnsMatSingleComponent,
    ToggleBtnsLegacyMultipleComponent,
    ToggleBtnsLegacySingleComponent
  ],
  imports: [
    CommonModule,
    MarkdownModule.forChild(),
    DocsMaterialModule,
    SharedComponentsModule
  ]
})
export class ButtonsModule { }

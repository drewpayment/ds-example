import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { MarkdownModule } from 'ngx-markdown';
import { InputsComponent } from './inputs/inputs.component';
import { SelectsComponent } from './selects/selects.component';
import { TextareasComponent } from './textareas/textareas.component';
import { CheckboxComponent } from './checkbox/checkbox.component';
import { CircleCheckboxComponent } from './circle-checkbox/circle-checkbox.component';
import { RadiosComponent } from './radios/radios.component';
import { PillCheckboxComponent } from './pill-checkbox/pill-checkbox.component';
import { PillRadioComponent } from './pill-radio/pill-radio.component';
import { FileInputsComponent } from './file-inputs/file-inputs.component';
import { FileInputQueueComponent } from './file-input-queue/file-input-queue.component';
import { LayoutInlineComponent } from './layout-inline/layout-inline.component';
import { BorderlessSelectComponent } from './borderless-select/borderless-select.component';
import { LayoutSpacingComponent } from './layout-spacing/layout-spacing.component';
import { LayoutNestingComponent } from './layout-nesting/layout-nesting.component';
import { DaySelectorComponent } from './day-selector/day-selector.component';
import { InputGroupsComponent } from './input-groups/input-groups.component';
import { SearchComponent } from './search/search.component';
import { FormElementsDocsComponent } from './form-elements-docs/form-elements-docs.component';
import { LayoutInlineInputComponent } from './layout-inline-input/layout-inline-input.component';
import { SharedComponentsModule } from '../shared-components/shared-components.module';

@NgModule({
  declarations: [
    FormElementsDocsComponent,
    InputsComponent, SelectsComponent,
    TextareasComponent,
    CheckboxComponent,
    CircleCheckboxComponent,
    RadiosComponent,
    PillCheckboxComponent,
    PillRadioComponent,
    FileInputsComponent,
    FileInputQueueComponent,
    LayoutInlineComponent,
    BorderlessSelectComponent,
    LayoutSpacingComponent,
    LayoutNestingComponent,
    DaySelectorComponent,
    InputGroupsComponent,
    SearchComponent,
    LayoutInlineInputComponent
  ],
  imports: [
    CommonModule,
    DocsMaterialModule,
    MarkdownModule.forChild(),
    SharedComponentsModule
  ]
})
export class FormElementsModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { ListDocsComponent } from './list-docs/list-docs.component';
import { ListComponent } from './list/list.component';
import { ListActionComponent } from './list-action/list-action.component';
import { ListSelectableComponent } from './list-selectable/list-selectable.component';
import { ListComplexComponent } from './list-complex/list-complex.component';
import { ListLargeComponent } from './list-large/list-large.component';
import { ListEmptyComponent } from './list-empty/list-empty.component';
import { ListButtonsComponent } from './list-buttons/list-buttons.component';
import { ListButtonsTopComponent } from './list-buttons-top/list-buttons-top.component';
import { ListCollapseComponent } from './list-collapse/list-collapse.component';

@NgModule({
  declarations: [ListDocsComponent, ListComponent, ListActionComponent, ListSelectableComponent, ListComplexComponent, ListLargeComponent, ListEmptyComponent, ListButtonsComponent, ListButtonsTopComponent, ListCollapseComponent],
  imports: [
    CommonModule,
    DocsMaterialModule,
    MarkdownModule.forChild()
  ]
})
export class ListsModule { }

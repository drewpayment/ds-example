import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GridDocsComponent } from './grid-docs/grid-docs.component';
import { GridComponent } from './grid/grid.component';
import { MarkdownModule } from 'ngx-markdown';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { NgxMaskModule, IConfig } from 'ngx-mask';

export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};

@NgModule({
  declarations: [GridDocsComponent, GridComponent],
  imports: [
    CommonModule,
    MarkdownModule.forChild(),
    DocsMaterialModule,
    NgxMaskModule.forRoot(options)
  ]
})
export class GridModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SwitchDocsComponent } from './switch-docs/switch-docs.component';
import { SwitchComponent } from './switch/switch.component';
import { MarkdownModule } from 'ngx-markdown';
import { SwitchTitleComponent } from './switch-title/switch-title.component';



@NgModule({
  declarations: [SwitchDocsComponent, SwitchComponent, SwitchTitleComponent],
  imports: [
    CommonModule,
    MarkdownModule.forChild(),
  ]
})
export class SwitchModule { }

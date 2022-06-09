import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AvatarDocsComponent } from './avatar-docs/avatar-docs.component';
import { MaterialModule } from '@ds/core/ui/material';
import { MarkdownModule } from 'ngx-markdown';
import { AvatarExampleComponent } from './avatar/avatar.component';
import { AvatarSharedModule } from '../shared-components/avatar-shared/avatar-shared.module';
import { AvatarModule } from '@ds/core/ui/avatar/avatar.module';
import { AvatarColorsComponent } from './avatar-colors/avatar-colors.component';



@NgModule({
  declarations: [
    AvatarDocsComponent, 
    AvatarExampleComponent, AvatarColorsComponent
  ],
  imports: [
    CommonModule,
    MarkdownModule.forChild(),
    MaterialModule,
    AvatarSharedModule,
    AvatarModule
  ]
})
export class AvatarExampleModule { }

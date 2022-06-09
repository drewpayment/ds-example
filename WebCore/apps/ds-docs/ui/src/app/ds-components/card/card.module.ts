import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { CardDocsComponent } from './card-docs/card-docs.component';
import { CardHeaderComponent } from './card-header/card-header.component';
import { CardFormComponent } from './card-form/card-form.component';
import { CardSubFormComponent } from './card-sub-form/card-sub-form.component';
import { CardSecondaryHeaderComponent } from './card-secondary-header/card-secondary-header.component';
import { CardSmallComponent } from './card-small/card-small.component';
import { CardObjectComponent } from './card-object/card-object.component';
import { CardCalloutComponent } from './card-callout/card-callout.component';
import { CardCalloutIconComponent } from './card-callout-icon/card-callout-icon.component';
import { CardWidgetLargeComponent } from './card-widget-large/card-widget-large.component';
import { CardWidgetCollapseComponent } from './card-widget-collapse/card-widget-collapse.component';
import { CardWidgetComponent } from './card-widget/card-widget.component';
import { CardHandleComponent } from './card-handle/card-handle.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { CardCalloutImageComponent } from './card-callout-image/card-callout-image.component';
import { ColorVariationInfoComponent } from './color-variation-info/color-variation-info.component';
import { ColorComponent } from './color/color.component';
import { AvatarModule } from '@ds/core/ui/avatar/avatar.module';
import { CardFullHeaderComponent } from './card-full-header/card-full-header.component';


@NgModule({
  declarations: [CardDocsComponent, CardHeaderComponent, CardFormComponent, CardSubFormComponent, CardSecondaryHeaderComponent, CardSmallComponent, CardObjectComponent, CardCalloutComponent, CardCalloutIconComponent, CardWidgetLargeComponent, CardWidgetCollapseComponent, CardWidgetComponent, CardHandleComponent, CardCalloutImageComponent, ColorVariationInfoComponent, ColorComponent, CardFullHeaderComponent],
  imports: [
    CommonModule,
    MarkdownModule.forChild(),
    DocsMaterialModule,
    DsCardModule,
    DragDropModule,
    AvatarModule,
  ]
})
export class CardModule { }

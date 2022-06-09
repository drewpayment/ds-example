import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { 
    DsCardComponent, 
    DsCardPrecontent,
    DsCardHeader, 
    DsCardTitle,
    DsCardIcon,
    DsCardIconContainer,
    DsCardIconLabel,
    DsCardAvatar,
    DsCardWidgetTitle,
    DsCardHeaderTitle,
    DsCardSubHeaderTitle,
    DsCardSectionTitle,
    DsCardTitleIcon, 
    DsCardInlineContent, 
    DsCardTitleRightContent, 
    DsCardContent, 
    DsCardCollapseInstructionText,
    DsCardFooter, 
    DsCardTitleAction,
    DsCardSubtitle,
    DsCardNav,
    DsCardBreadCrumb
} from './ds-card.component';
import { DsExpansionModule } from '@ds/core/ui/ds-expansion';

@NgModule({
    imports: [
        CommonModule,
        MaterialModule,
        DsExpansionModule
    ],
    exports: [
        DsCardComponent,
        DsCardPrecontent,
        DsCardHeader,
        DsCardTitle,
        DsCardIcon,
        DsCardIconContainer,
        DsCardIconLabel,
        DsCardAvatar,
        DsCardWidgetTitle,
        DsCardHeaderTitle,
        DsCardSubtitle,
        DsCardNav,
        DsCardSubHeaderTitle,
        DsCardSectionTitle,
        DsCardTitleIcon,
        DsCardInlineContent,
        DsCardTitleRightContent,
        DsCardTitleAction,
        DsCardContent,
        DsCardCollapseInstructionText,
        DsCardFooter,
        DsCardBreadCrumb

    ],
    declarations: [
        DsCardComponent,
        DsCardPrecontent,
        DsCardHeader,
        DsCardTitle,
        DsCardIcon,
        DsCardIconContainer,
        DsCardAvatar,
        DsCardIconLabel,
        DsCardWidgetTitle,
        DsCardHeaderTitle,
        DsCardSubtitle,
        DsCardNav,
        DsCardSubHeaderTitle,
        DsCardSectionTitle,
        DsCardTitleIcon,
        DsCardInlineContent,
        DsCardTitleRightContent,
        DsCardTitleAction,
        DsCardContent,
        DsCardCollapseInstructionText,
        DsCardFooter,
        DsCardBreadCrumb
    ]
})
export class DsCardModule { }

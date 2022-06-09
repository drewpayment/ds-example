import { NgModule } from "@angular/core";
import {
  CommonModule,
  APP_BASE_HREF,
  LocationStrategy,
  PathLocationStrategy,
} from "@angular/common";
import { MaterialModule } from "../material/material.module";
import { DsNavMenuComponent } from "./ds-nav-menu/ds-nav-menu.component";
import { DsNavMainContentComponent } from "./ds-nav-main-content/ds-nav-main-content.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { DsNavMainLegacyContentComponent } from "./ds-nav-main-legacy-content/ds-nav-main-legacy-content.component";
import { DsNavToolbarContentComponent } from "./ds-nav-toolbar-content/ds-nav-toolbar-content.component";
import { DsNavMenuContentComponent } from "./ds-nav-menu-content/ds-nav-menu-content.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { DsNavHelpLinksComponent } from "./ds-nav-help-links/ds-nav-help-links.component";
import { RouterModule } from "@angular/router";
import { WorkNumberModule } from "@ds/core/popup/equifax/work-number/work-number.module";
import { DsCustomOutletModule } from "../ds-custom-outlet/ds-custom-outlet.module";
import { DsNavMenuDirective } from "./ds-nav-menu/ds-nav-menu.directive";
import { CheckActiveStatePipe } from "./ds-nav-menu/check-active-state.pipe";
import { QualifiedUrlPipe } from "./ds-nav-menu/qualified-url.pipe";
import {
  TermsAndConditionsComponent,
  safeString,
} from "./ds-nav-help-links/terms-and-conditions/terms-and-conditions.component";
import { DsNavMenuItemDirective } from "./ds-nav-menu-item/ds-nav-menu-item.directive";
import { LocalLinkService } from "./ds-nav-menu-item/local-link.service";
import { AppConfig } from "@ds/core/app-config/app-config";
import { DsOnLoadModule } from "@ds/core/directives/on-load/on-load.module";
import { DsNavMainContentDirective } from './ds-nav-main-content/ds-nav-main-content.directive';

// export function getBaseHref() {
//     return location.origin + '/' + location.pathname[1];
// }

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    RouterModule,
    WorkNumberModule,
    DsCustomOutletModule,
    DsOnLoadModule,
  ],
  declarations: [
    DsNavMenuComponent,
    DsNavMainContentComponent,
    DsNavMainLegacyContentComponent,
    DsNavToolbarContentComponent,
    DsNavMenuContentComponent,
    DsNavHelpLinksComponent,
    DsNavMenuDirective,
    CheckActiveStatePipe,
    QualifiedUrlPipe,
    TermsAndConditionsComponent,
    safeString,
    DsNavMenuItemDirective,
    DsNavMainContentDirective,
  ],
  exports: [
    DsNavMenuComponent,
    DsNavMainContentComponent,
    DsNavMainLegacyContentComponent,
    DsNavToolbarContentComponent,
    DsNavMenuContentComponent,
    DsNavMenuDirective,
    CheckActiveStatePipe,
    QualifiedUrlPipe,
  ],
  entryComponents: [TermsAndConditionsComponent],
  providers: [LocalLinkService],
})
export class DsUiNavModule {}

import { NgModule } from "@angular/core";
import { HeaderComponent } from "@ds/core/ui/menu-wrapper-header/header/header.component";
import { CoreModule } from "@ds/core/core.module";
import { HttpClientModule } from "@angular/common/http";
import { MaterialModule } from "@ds/core/ui/material";
import { BetaFeaturesModule } from "@ds/core/users/beta-features/beta-features.module";
import { NgClientSelectorComponent } from "./ng-client-selector/ng-client-selector.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AjsUpgradesModule } from "@ds/core/ajs-upgrades/ajs-upgrades.module";
import { ClientSelectorService } from "./ng-client-selector/client-selector.service";
import { DsFocusModule } from "@util/focus/ds-focus.module";
import { DsUsersModule } from "@ds/core/users";
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [
    CommonModule,
    CoreModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    BetaFeaturesModule,
    DsFocusModule,

    AjsUpgradesModule.forRoot(),
    DsUsersModule,
  ],
  declarations: [HeaderComponent, NgClientSelectorComponent],
  exports: [HeaderComponent],
  entryComponents: [HeaderComponent, NgClientSelectorComponent],
  providers: [ClientSelectorService],
})
export class MenuWrapperHeaderModule {}

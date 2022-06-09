import { ModuleWithProviders, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MenuWrapperToggleComponent } from "./menu-wrapper-toggle/menu-wrapper-toggle.component";
import {
  ToggleFeedbackDialogComponent,
  ToggleThankYouComponent,
} from "./menu-wrapper-toggle/toggle-feedback-dialog/toggle-feedback-dialog.component";
import { MaterialModule } from "@ds/core/ui/material";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { DsOnLoadModule } from "@ds/core/directives/on-load/on-load.module";
import { CORE_ENVIRONMENT } from '@ds/core/core.tokens';

@NgModule({
  declarations: [
    MenuWrapperToggleComponent,
    ToggleFeedbackDialogComponent,
    ToggleThankYouComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    DsOnLoadModule,
  ],
  exports: [MenuWrapperToggleComponent],
  entryComponents: [
    MenuWrapperToggleComponent, // ONLY ADDED HERE TO DOWNGRADE TO AJS DIRECTIVE
    ToggleFeedbackDialogComponent,
    ToggleThankYouComponent,
  ],
})
export class BetaFeaturesModule {

  constructor() {}

  public static forRoot(environment: any): ModuleWithProviders<any> {
    return {
      ngModule: BetaFeaturesModule,
      providers: [
        {
          provide: CORE_ENVIRONMENT,
          useValue: environment,
        }
      ],
    };
  }

}

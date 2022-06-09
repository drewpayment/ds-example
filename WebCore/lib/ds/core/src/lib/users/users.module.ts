import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccessRuleManagerComponent } from './access-rule-manager/access-rule-manager.component';
import { BrowserModule } from '@angular/platform-browser';
import { MaterialModule } from '../ui/material/material.module';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AjsUpgradesModule } from '../ajs-upgrades/ajs-upgrades.module';
import { DsCoreFormsModule } from '../ui/forms/forms.module';
import { DsCardModule } from '../ui/ds-card';
import { AccessRuleSetEditorComponent } from './access-rule-set-editor/access-rule-set-editor.component';
import { BetaFeaturesModule } from './beta-features/beta-features.module';
import { StoreModule } from '@ngrx/store';
import * as fromReducer from './store/user.reducer';
import { EffectsModule } from '@ngrx/effects';
import { UserEffects } from './store/user.effects';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    AjsUpgradesModule,
    DsCoreFormsModule,
    DsCardModule,
    BetaFeaturesModule,
    StoreModule.forFeature(
      fromReducer.userStoreFeatureKey,
      fromReducer.reducer
    ),
    EffectsModule.forFeature([UserEffects]),
  ],
  declarations: [AccessRuleManagerComponent, AccessRuleSetEditorComponent],
  exports: [AccessRuleManagerComponent],
})
export class DsUsersModule {}

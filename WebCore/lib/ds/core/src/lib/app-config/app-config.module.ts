import { NgModule, InjectionToken } from "@angular/core";
import { CommonModule, DOCUMENT } from "@angular/common";
import { BrowserModule } from "@angular/platform-browser";
import { MaterialModule } from "@ds/core/ui/material/material.module";
import { RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AjsUpgradesModule } from "@ds/core/ajs-upgrades/ajs-upgrades.module";
import { DsCoreFormsModule } from "@ds/core/ui/forms/forms.module";
import { DsCardModule } from "@ds/core/ui/ds-card";
import { AppResourceManagerComponent } from "./app-resource-manager/app-resource-manager.component";
import { AppResourceDialogComponent } from "./app-resource-dialog/app-resource-dialog.component";
import { MenuBuilderComponent } from "./menu-builder/menu-builder.component";
import { MatTreeModule } from "@angular/material/tree";
import { MenuItemDialogComponent } from "./menu-item-dialog/menu-item-dialog.component";
import { StoreModule, MetaReducer } from "@ngrx/store";
import * as fromMenu from "@ds/core/app-config/ngrx-store/reducers/menu.reducer";
import {
  NgrxStorageService,
  NGRX_STORAGE_TOKEN,
} from "./ngrx-store/ngrx-storage.service";
import { storageMetaReducer } from "./ngrx-store/meta-reducers/storage-metareducer";
import {
  MENU_CONFIG_TOKEN,
  MENU_LOCALSTORAGE_KEY,
  MENU_STORAGE_KEYS,
} from "./ngrx-store/ngrx.tokens";
import { LOCAL_STORAGE, StorageService } from "ngx-webstorage-service";
import { EffectsModule } from "@ngrx/effects";
import { MenuEffects } from "./ngrx-store/effects/menu.effects";

export function getMenuConfig(
  saveKeys: string[],
  localStorageKey: string,
  storageService: NgrxStorageService,
  document: Document
) {
  return {
    metaReducers: [
      storageMetaReducer(saveKeys, localStorageKey, storageService, document),
    ],
  };
}

export function getStorageKey(key: string) {
  return new InjectionToken<StorageService>(key);
}

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
    MatTreeModule,
    StoreModule.forFeature(
      fromMenu.menuFeatureKey,
      fromMenu.reducer,
      MENU_CONFIG_TOKEN
    ),
    EffectsModule.forFeature([MenuEffects]),
  ],
  declarations: [
    AppResourceManagerComponent,
    AppResourceDialogComponent,
    MenuBuilderComponent,
    MenuItemDialogComponent,
  ],
  exports: [
    AppResourceManagerComponent,
    MenuBuilderComponent,
    MenuItemDialogComponent,
  ],
  providers: [
    {
      provide: NGRX_STORAGE_TOKEN,
      useExisting: LOCAL_STORAGE,
    },
    NgrxStorageService,
    {
      provide: MENU_LOCALSTORAGE_KEY,
      useValue: "__nav_storage__",
    },
    {
      provide: MENU_STORAGE_KEYS,
      useValue: [
        "menu",
        "isSidenavOpen",
        "isSideNavDrilled",
        "isSideNavDrilledNoAnimation",
        "isResourceSelected",
        "selectedUrlItem",
        "mainMenuItems",
        "displayMenuItems",
        "productTitle",
        "productIcon",
        "navigationHistory",
        "mainMenuItemId",
      ],
    },
    {
      provide: DOCUMENT,
      useValue: document,
    },
    {
      provide: MENU_CONFIG_TOKEN,
      deps: [
        MENU_STORAGE_KEYS,
        MENU_LOCALSTORAGE_KEY,
        NgrxStorageService,
        DOCUMENT,
      ],
      useFactory: getMenuConfig,
    },
  ],
})
export class DsAppConfigModule {}

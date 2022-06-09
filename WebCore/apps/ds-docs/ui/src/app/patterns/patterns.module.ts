import * as angular from "angular";
import { DsCoreModule } from "@ajs/core/ds-core.module";
import { DsUiModule } from "@ajs/ui/ds-ui.module";
import { DsMaterial } from "@ajs/ui/material/ds-material.module";
import { DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { PatternsStateConfig } from "./patterns.state";
import { AddressBlockStateConfig } from "./addressblocks/addressblocks.state";
import { ArchiveStateConfig } from "./archive/archive.state";

export module PatternsDocsModule {
    const MODULE_NAME = "ds.design.app.patterns";

    const MODULE_DEPENDENCIES = [
        DsCoreModule.AjsModule.name, 
        DsUiModule.AjsModule.name, 
        DsMaterial.AjsModule.name
    ];

    export const AjsModule = angular.module(MODULE_NAME, MODULE_DEPENDENCIES);

    AjsModule
        .config([DsStateServiceProvider.PROVIDER_NAME, (state) => {
            new PatternsStateConfig(state);
            new AddressBlockStateConfig(state);
            new ArchiveStateConfig(state);
        }])
}
import * as angular from "angular";
import { DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";

export module UtilitesDocsModule {
    export const MODULE_NAME = "ds.design.app.utilities";

    const MODULE_DEPENDENCIES = ["ds.core", "ds.ui"];

    export const AjsModule = angular.module(MODULE_NAME, MODULE_DEPENDENCIES);

    AjsModule
        .config([DsStateServiceProvider.PROVIDER_NAME, (state) => {
            state.registerState({
                name: "utilities",
                parent: "design",
                url: "^/Utilities",
                views:{
                    "@design":{
                        template: require('./utilities.html')
                    }
                }
            });
        }]);
}
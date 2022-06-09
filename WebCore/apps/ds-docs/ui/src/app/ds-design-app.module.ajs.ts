import * as angular from "angular";
import { ComponentsDocsModule } from "./components/components.module";
import { UtilitesDocsModule } from "./utilities/utilities.module";
import { PatternsDocsModule } from "./patterns/patterns.module";
import { NgxMigrationDemoAjsModule } from "./ngx-migration/demo/demo.module.ajs";
import { DsCoreModule } from "@ajs/core/ds-core.module";
import { DsUiModule } from "@ajs/ui/ds-ui.module";

import "./vendor";
import { DsStateServiceProvider } from "@ajs/core/ds-state/ds-state.service";
import { DsDesignModuleConfig } from "./ds-design-app.config";
import { IconStateConfig } from "./icons/icons.state";
import { DsDesignModuleStateConfig } from "./ds-design-app.state";
import { NgxMigrationDocsController } from "./ngx-migration/docs.controller";
import { DsProgressAjsModule } from "@ds/core/ui/ds-progress/ajs";
import { FormsStateConfig } from './forms/forms.state';
import { DsComponentsStateConfig } from './ds-components/ds-components.state';
import { DsHomeStateConfig } from './ds-home/ds-home.state';


export module DsDesignModule {
    const MODULE_NAME = "ds.design.app";

    const MODULE_DEPENDENCIES = [
        DsCoreModule.AjsModule.name, 
        DsUiModule.AjsModule.name,
        DsProgressAjsModule.AjsModule.name,
        "ngMessages", 
        ComponentsDocsModule.AjsModule.name, 
        UtilitesDocsModule.AjsModule.name,
        PatternsDocsModule.AjsModule.name,
        NgxMigrationDemoAjsModule.AjsModule.name
    ];

    export const AjsModule = angular.module(MODULE_NAME, MODULE_DEPENDENCIES);

    AjsModule
        .config(["$locationProvider", "$urlRouterProvider", (l,u) => new DsDesignModuleConfig(l,u)])
        .config([DsStateServiceProvider.PROVIDER_NAME, (state) => {
            new DsDesignModuleStateConfig(state);
            new IconStateConfig(state);
            new FormsStateConfig(state);
            new DsComponentsStateConfig(state);
            new DsHomeStateConfig(state);

            state.registerState({  
                name: "ngx",
                parent: "design",
                url: "/Ngx-Migration",
                views: {
                    "@design": {
                        template: require("raw-loader!./ngx-migration/docs.html"),
                        controller: NgxMigrationDocsController,
                        controllerAs: "$ctrl"
                    }
                }
            });
        }]);
}

import { IUiState } from "@ajs/core/ds-state/ds-state.service";

export class DsComponentsStateConfig {
    static STATE_CONFIG: IUiState = {
        name: "DsComponents",
        parent: "design",
        url: "^/DsComponents",
        views:{
            "@design":{
                template: ''
            }
        }
    };

    static CATCH_ALL_CONFIG: IUiState = {
        name: "DsComponents",
        parent: "design.DsComponents",
        url: "/*any",
        views:{
            "@design":{
                template: ''
            }
        }
    };

    constructor(private dsState) {
        this.$config();
    }

    $config(){
        this.dsState.registerState(DsComponentsStateConfig.STATE_CONFIG);
        this.dsState.registerState(DsComponentsStateConfig.CATCH_ALL_CONFIG);
    }
}

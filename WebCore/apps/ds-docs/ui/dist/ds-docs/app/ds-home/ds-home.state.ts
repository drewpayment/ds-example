import { IUiState } from "@ajs/core/ds-state/ds-state.service";

export class DsHomeStateConfig {
    static STATE_CONFIG: IUiState = {
        name: "Standards",
        parent: "design",
        url: "^/Standards",
        views:{
            "@design":{
                template: ''
            }
        }
    };

    static CATCH_ALL_CONFIG: IUiState = {
        name: "Standards",
        parent: "design.Standards",
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
        this.dsState.registerState(DsHomeStateConfig.STATE_CONFIG);
        this.dsState.registerState(DsHomeStateConfig.CATCH_ALL_CONFIG);
    }
}

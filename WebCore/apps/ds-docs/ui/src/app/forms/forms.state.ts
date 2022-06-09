import { IUiState } from "@ajs/core/ds-state/ds-state.service";

export class FormsStateConfig {
    static STATE_CONFIG: IUiState = {
        name: "forms",
        parent: "design",
        url: "^/Forms",
        views:{
            "@design":{
                template: ''
            }
        }
    };

    static CATCH_ALL_CONFIG: IUiState = {
        name: "forms",
        parent: "design.forms",
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
        this.dsState.registerState(FormsStateConfig.STATE_CONFIG);
        this.dsState.registerState(FormsStateConfig.CATCH_ALL_CONFIG);
    }
}

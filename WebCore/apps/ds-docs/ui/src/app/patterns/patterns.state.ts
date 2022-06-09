import { IUiState } from "@ajs/core/ds-state/ds-state.service";

export class PatternsStateConfig {
    static STATE_CONFIG: IUiState = {
        name: "patterns",
        parent: "design",
        url: "^/Patterns",
        views:{
            "@design":{
                template: require('./patterns.html')
            }
        }
    };
    constructor(private dsState) {
        this.$config();
    }

    $config(){
        this.dsState.registerState(PatternsStateConfig.STATE_CONFIG);
    }
}

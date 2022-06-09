import { IUiState } from "@ajs/core/ds-state/ds-state.service";

export class IconStateConfig {
    static STATE_CONFIG: IUiState = {
        name: "icons",
        parent: "design",
        url: "^/Icons",
        views: {
            "@design": {
                template: require('./icons.html')
            }
        }
    };
    constructor(private dsState) {
        this.$config();
    }

    $config(){
        this.dsState.registerState(IconStateConfig.STATE_CONFIG);
    }
}
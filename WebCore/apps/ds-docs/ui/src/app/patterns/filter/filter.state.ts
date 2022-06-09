import { IUiState } from "@ajs/core/ds-state/ds-state.service";

export class FilterStateConfig {
    static STATE_CONFIG: IUiState = {
        name: "filter",
        parent: "design.patterns",
        url: "/Filters",
        views: {
            "@design": {
                template: require("./filter.html")
            }
        }
    };
    constructor(private dsState) {
        this.$config();
    }

    $config(){
        this.dsState.registerState(FilterStateConfig.STATE_CONFIG)
    }
}
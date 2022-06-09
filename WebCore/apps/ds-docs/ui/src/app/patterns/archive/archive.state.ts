import { IUiState } from "@ajs/core/ds-state/ds-state.service";

export class ArchiveStateConfig {
    static STATE_CONFIG: IUiState = {
        name: "archive",
        parent: "design.patterns",
        url: "/Archive",
        views: {
            "@design": {
                template: require("raw-loader!./archive.html")
            }
        }
    };
    constructor(private dsState) {
        this.$config();
    }

    $config(){
        this.dsState.registerState(ArchiveStateConfig.STATE_CONFIG)
    }
}
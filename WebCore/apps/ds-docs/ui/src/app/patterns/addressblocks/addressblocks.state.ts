import { IUiState } from "@ajs/core/ds-state/ds-state.service";

export class AddressBlockStateConfig {
    static STATE_CONFIG: IUiState = {
        name: "addressblocks",
        parent: "design.patterns",
        url: "/AddressBlocks",
        views: {
            "@design": {
                template: require('./addressblocks.html')
            }
        }
    };
    constructor(private dsState) {
        this.$config();
    }

    $config(){
        this.dsState.registerState(AddressBlockStateConfig.STATE_CONFIG)
    }
}
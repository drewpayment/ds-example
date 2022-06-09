import { IUiState } from "@ajs/core/ds-state/ds-state.service";

export class DsDesignModuleStateConfig {
    static STATE_CONFIG: IUiState = {
        name: "design",
        abstract:true,
        template: "<div ui-view></div>"
    };

    constructor(private dsState) {
        this.$config();
    }

    $config(){
        this.dsState.registerState(DsDesignModuleStateConfig.STATE_CONFIG);
    }
}
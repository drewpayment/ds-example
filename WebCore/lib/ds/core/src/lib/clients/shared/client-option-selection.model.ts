import { ClientOption , IClientOptionItem } from '.';

export interface IClientOptionSelection {
    /**
     * Is weirdly named... In practice, this corresponds to `ClientOptionControlId`,
     * as in:
     * 
     * `select ClientOptionControlId from dbo.ClientOptionControls`
     */
    clientAccountOptionId?: number,
    clientId      : number,
    accountOption : ClientOption,
    selectValue   : string,
    isEnabled     : Boolean,
    selectedItem  : IClientOptionItem
}
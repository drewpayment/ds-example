import { IClientOptionItem, ClientOption, ClientOptionDataType, ClientOptionCategory } from "./index";

export interface IClientOption {
    accountOption      : ClientOption,
    description        : string,
    dataType           : ClientOptionDataType,
    category           : ClientOptionCategory,
    isEnabledByDefault : Boolean | null,
    clientId           : number | null,
    accountOptionItems : IClientOptionItem[]
    isSecurityOption   : number
}
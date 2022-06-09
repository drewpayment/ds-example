import { IClientOption, ClientOption } from "./index";

export interface IClientOptionItem {
    accountOptionItemId : number,
    accountOption       : ClientOption,
    accountOptionInfo   : IClientOption,
    description         : string,
    isDefault           : Boolean,
    value               : number | null,

}
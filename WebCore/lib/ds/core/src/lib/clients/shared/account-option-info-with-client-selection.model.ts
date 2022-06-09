import { IClientOptionSelection } from "./client-option-selection.model";
import { ALL_CLIENT_OPTIONS, ClientOption } from "./client-option.enum";
import { IClientOption } from "./client-option.model";

export interface IAccountOptionInfoWithClientSelection extends IClientOption {
    clientSelection: IClientOptionSelection;
}

export type ClientOptionKeyUncapitalize = Uncapitalize<keyof typeof ClientOption>;
export type ClientOptionKeyCapitalize = Capitalize<ClientOptionKeyUncapitalize>;

export type AccountOptionInfoWithClientSelectionByControlId = {
    [key in ClientOption]: IAccountOptionInfoWithClientSelection;
};

export type AccountOptionInfoWithClientSelectionByControlIdName = {
    [key in ClientOptionKeyUncapitalize]: IAccountOptionInfoWithClientSelection;
};

export function mapToAccountOptionInfoWithClientSelectionByControlId(dtos: IAccountOptionInfoWithClientSelection[])
: AccountOptionInfoWithClientSelectionByControlId {
    const dictionary = {} as AccountOptionInfoWithClientSelectionByControlId;
    if (!Array.isArray(dtos)) { return dictionary; }
    dtos.forEach(x => dictionary[x.accountOption] = x);
    return dictionary;
}

export function mapToAccountOptionInfoWithClientSelectionByControlIdName(dtos: IAccountOptionInfoWithClientSelection[])
: AccountOptionInfoWithClientSelectionByControlIdName {
    const dictionary = {} as AccountOptionInfoWithClientSelectionByControlIdName;
    if (!Array.isArray(dtos)) { return dictionary; }
    dtos.forEach(x => dictionary[ClientOption[x.accountOption]] = x);
    return dictionary;
}

export function isInstanceOfClientOptionKey(object: any): object is ClientOptionKeyUncapitalize {
    const isString = (typeof object === 'string' || object instanceof String);
    if (!isString) { return false; }
    
    // uppercase the first char
    const capitalizedObject = toCapitalizedString(object);
    return ALL_CLIENT_OPTIONS.some(x => x === capitalizedObject || x === object);
}

export function toCapitalizedString(s: string|String): string {
    return s.charAt(0).toUpperCase() + s.slice(1);
}
export function toUncapitalizedString(s: string|String): string {
    return s.charAt(0).toLowerCase() + s.slice(1);
}

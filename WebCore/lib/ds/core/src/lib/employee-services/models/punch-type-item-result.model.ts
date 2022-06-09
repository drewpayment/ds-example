import { PunchTypeItemSource, ClientEarningCategory } from '../enums';


export interface PunchTypeItemResult {
    source:PunchTypeItemSource;
    clientId:number;
    clockClientTimePolicyId:number;
    clockClientRulesId:number;
    
    items:PunchTypeItem[];
}

export interface PunchTypeItem {
    id:number|null;
    name:string;
    isDefault:boolean;
    clockClientLunchId?:number|null;
    clientEarningId?:number|null;
    earningCategory?:ClientEarningCategory;
    description?:string;
    code?:string;
}

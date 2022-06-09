export interface IBankBasicInfo {
    bankId             : number;
    name               : string;
    routingNumber      : string;
}

export interface IBankInfo extends IBankBasicInfo {
    address            :string;
    checkSequence      :string;
    achBankId          :number;
}
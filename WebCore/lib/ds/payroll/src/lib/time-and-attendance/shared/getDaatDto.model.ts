export interface WhatJustinPassesMe {
    approvalStatusDropdownSelectedValue?: string;
    daysDropdownSelectedValue?: string;
    startDateFieldText?: string;
    endDateFieldText?: string;
    payPeriodDropdownSelectedValue?: string;
    payPeriodDropdownSelectedItemText?: string;
    filter1Dropdown?: Dropdown;
    category1Dropdown?: Dropdown;
    filter2Dropdown?: Dropdown;
    category2Dropdown?: Dropdown;
    clientId: number;
    currentPage: number;
    pageSize: number;
}
export interface Dropdown {
    value?: number;
    visible?: boolean;
}
import { IAddressInfo } from "../../../../core/src/lib/contacts/shared/address-info.model";

export interface IVendorMaintenanceInfo extends IAddressInfo {
    vendorId       : number;
    name           : string;
    phone          : string;
    phoneExtension : string;
    accountNumber  : string;
    taxFrequencyId : number;
    bankId         : number;
    routingNumber  : string;
    accountTypeId  : number;
    isActive       : Boolean;
}
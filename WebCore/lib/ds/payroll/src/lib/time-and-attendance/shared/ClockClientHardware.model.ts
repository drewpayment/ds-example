import { Moment } from 'moment';

export interface IClockClientHardware {
    clockClientHardwareId: number,
    clientId: number,
    description: string,
    model: string,
    email: string,
    ipAddress: string,
    modified?: Date | string | Moment,
    modifiedBy?: number,
    number: string,
    clockClientHardwareFunctionId?: number,
    serialNumber: string,
    macAddress?: string,
    firmwareVersion: string,
    isRental: boolean,
    purchaseDate: Date | string | Moment,
    warranty: Date | string | Moment,
    warrantyEnd: Date | string | Moment
}
export interface IDeviceCommand {
    id: number;
    devSn: string;
    content: string;
    commitTime?: Date;
    transTime?: Date;
    responseTime?: Date;
    returnValue: string;
}

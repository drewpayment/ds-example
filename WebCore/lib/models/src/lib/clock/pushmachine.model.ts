export interface IPushMachine {
    id: number;
    devSn: string;
    devName: string;
    attLogStamp: string;
    operLogStamp: string;
    attPhotoStamp: string;
    errorDelay: string;
    delay: string;
    transFlag: string;
    realtime: string;
    transInterval: string;
    transTimes: string;
    encript: string; //correct this
    lastRequestTime?: Date;
    devIp: string;
    devMac: string;
    devFpVersion: string;
    devFirmwareVersion: string;
    userCount?: number;
    attCount?: number;
    fpCount?: number;
    timeZone: string;
    timeout?: number;
    syncTime?: number;
}

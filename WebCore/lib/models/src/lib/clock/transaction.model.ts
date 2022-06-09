export interface ITransaction {
    id: number;
    devSn: string;
    pin: string;
    attTime: Date;
    status: string;
    verify: string;
    workCode: string;
    reserved1: string;
    reserved2: string;
    jobCode1: number;
    jobCode2: number;
    jobCode3: number;
    jobCode4: number;
    jobCode5: number;
    jobCode6: number;
    tipCode1: number;
    tipCode2: number;
    tipCode3: number;
    clockName: string;
    ipAddress: string;
}
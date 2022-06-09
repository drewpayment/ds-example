import { IW2Client } from "./w2-client";

export interface IW2ProcessingSubmitted{
    uniqueId: string;
    clientList: IW2Client[]
}
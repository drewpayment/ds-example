import { IPushMachine } from "./pushmachine.model";

export interface IClientMachine {
    clientMachineId: number;
    clientId: number;
    description: string;
    modifiedBy: number;
    modified: Date;
    isRental: boolean;
    purchaseDate?: Date;
    warranty?: Date;
    warrantyEnd?: Date;
    pushMachineId: number;
    pushMachine?: IPushMachine;
    isSelected: boolean;
}

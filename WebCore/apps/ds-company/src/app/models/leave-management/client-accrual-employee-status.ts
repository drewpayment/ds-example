import { IHasIdAndDescription } from "./ihas-id-and-description";

export class ClientAccrualEmployeeStatus  implements IHasIdAndDescription {
    id: number;
    description: string;
    active: boolean;
}

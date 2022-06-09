import { IHasIdAndDescription } from "./ihas-id-and-description";

export class ServiceType implements IHasIdAndDescription {
    id: number;
    description: string;
}

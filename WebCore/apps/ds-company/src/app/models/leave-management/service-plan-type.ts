import { IHasIdAndDescription } from "./ihas-id-and-description";

export class ServicePlanType implements IHasIdAndDescription {
    id: number;
    description: string;
}

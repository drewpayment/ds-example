import { ApplicationSourceType } from "./application-source-type.enum";
import { ApplicationResourceType } from "./application-resource-type.enum";

export interface IApplicationResource {
    resourceId: number;
    name?: string;
    routeUrl?: string;
    applicationSourceType: ApplicationSourceType;
    resourceType: ApplicationResourceType;
}
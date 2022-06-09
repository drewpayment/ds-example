import { IImageType, ImageType } from './image-type.model';
import { IImageSizeType, ImageSizeType } from './image-size-type.model';
import { Moment } from "moment";
import { IResourceType } from './resource-type.model';

export interface IAzureViewDto {
    resourceId:number,
    clientId:number | null,
    employeeId:number | null,
    userId:number | null,
    sourceTypeId:IResourceType,
    source:string,
    addedDate:Date | string | Moment,
    isDeleted:boolean,
    addedBy:number | null,
    clientGuid:string,
    resourceGuid:string,
    name:string,
    token:string,
    size:ImageSizeType,
    type:ImageType
}

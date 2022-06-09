import { ImageType } from "@ds/core/resources/shared/image-type.model";
import { ImageSizeType } from "@ds/core/resources/shared/image-size-type.model";

export interface IProfileImageDto {
    resourceId: number;
    sourceTypeId: number;
    source: string;
    imageType: ImageType;
    imageSizeType: ImageSizeType;
}

export class ProfileImageDto implements IProfileImageDto {
    resourceId: number;
    sourceTypeId: number;
    source: string;
    imageType: ImageType;
    imageSizeType: ImageSizeType;
}

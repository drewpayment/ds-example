import { ImageType, ImageSizeType } from '@ajs/core/ds-resource/models';

export interface IProfileImageDetail {
    resourceId: number;
    sourceTypeId: number;
    source: string;
    imageType: ImageType;
    imageSizeType: ImageSizeType;
}
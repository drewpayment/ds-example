import { IProfileImageDetail } from './profile-image-detail.model';
import { IImage } from '@ajs/core/ds-resource/models';

export interface IProfileImage {
    employeeId: number;
    employeeGuid: string;
    clientId: number;
    clientGuid: string;
    sasToken: string;
    extraLarge?: IImage;
    large?: IImage;
    medium?: IImage;
    small?: IImage;
    profileImageInfo: IProfileImageDetail[];
}

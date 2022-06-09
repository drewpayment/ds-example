import { ImageType } from '@ds/core/resources/shared/image-type.model';
import { ImageSizeType } from '@ds/core/resources/shared/image-size-type.model';
import { IProfileImageDto } from '@ds/core/resources/shared/profile-image.model';
import { IEmployeeAvatars } from '@ds/core/employees/shared/employee-avatars.model';

export interface IImage {
  hasImage: boolean;
  url: string;
}

export interface IEmployeeImage {
  employeeId: number;
  employeeGuid: string;
  clientId: number;
  clientGuid: string;
  sasToken: string;
  extraLarge: IImage;
  large: IImage;
  medium: IImage;
  small: IImage;
  profileImageInfo: IProfileImageDto[];
  _employeeAvatar: IEmployeeAvatars;
}

export interface EmployeeImageUploadResult extends IEmployeeImage {
  imageHasChanges: boolean;
}

export interface ImageDto {
  employeeId: number;
  clientId: number;
  token: string;
  imageType: ImageType;
  imageSize: ImageSizeType;
  hasImage: boolean;
  source: string;

  // AzureViewDto properties from C#

  resourceId?: number;
  name?: string;
}

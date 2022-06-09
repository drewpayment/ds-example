import { Pipe, PipeTransform } from '@angular/core';
import { IEmployeeAvatars } from '@ds/core/employees/shared/employee-avatars.model';
import { IImage } from '@ds/core/resources';
import { IEmployeeImage } from '@ds/core/resources/shared/employee-image.model';
import { IProfileImageDto } from '@ds/core/resources/shared/profile-image.model';
import { IEmployeeSearchResult } from '@ds/employees/search/shared/models/employee-search-result';

@Pipe({
  name: 'mapToEmployeeImage',
})
export class MapEmpSearchResultToIEmployeeImagePipe implements PipeTransform {
  transform(e: IEmployeeSearchResult) {
    let result = {} as IEmployeeImage;
    if (e == null) return result;

    try {
      result = {
        employeeId: e.employeeId,
        clientId: e.clientId,
        _employeeAvatar: e.employeeAvatar
          ? ({
              employeeAvatarId: e.employeeAvatar.employeeAvatarId,
              employeeId: e.employeeId,
              clientId: e.clientId,
              avatarColor: e.employeeAvatar.avatarColor,
            } as IEmployeeAvatars)
          : null,
        extraLarge:
          e.profileImage.profileImageInfo != null &&
          e.profileImage.profileImageInfo.length
            ? ({
                hasImage: true,
                url: `${e.profileImage.profileImageInfo[0].source}${e.profileImage.sasToken}`,
              } as IImage)
            : null,
        profileImageInfo:
          e.profileImage.profileImageInfo &&
          e.profileImage.profileImageInfo.length
            ? (e.profileImage.profileImageInfo as IProfileImageDto[])
            : null,

        sasToken: e.profileImage.sasToken,
      } as IEmployeeImage;
    } catch (error) {
      console.error(error);
    }

    return result;
  }
}

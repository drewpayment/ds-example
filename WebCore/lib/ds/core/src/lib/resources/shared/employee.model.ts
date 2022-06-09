import { IProfileImageDto } from './profile-image.model';
import { IEmployeeImage } from './employee-image.model';

export interface IEmployee {
    employeeId: number,
    clientId: number,
    selectedImage: IProfileImageDto,
    azureSas: string,
    hasProfileImage: boolean,
    profileImage: IEmployeeImage    
}

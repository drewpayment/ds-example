import { Pipe, PipeTransform } from '@angular/core';
import { UserType } from '../shared';


@Pipe({ name: 'ToUserTypeString' })
export class UserTypePipe implements PipeTransform {


    transform(input: UserType) {
        switch (input) {
            case UserType.applicant:
                return 'Applicant';
            case UserType.employee:
                return 'Employee';
            case UserType.supervisor:
                return 'Supervisor';
            case UserType.companyAdmin:
                return 'Company Admin';
            case UserType.systemAdmin:
                return 'System Admin';
            default:
                return null;
        }
    }

}

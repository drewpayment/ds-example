import { IOnboardingEmployee } from 'apps/ds-company/src/app/models/onboarding-employee.model';
import { IEmployeeName } from 'apps/ds-company/src/app/models/employee-name.model';
import { coerceNumberProperty } from '@angular/cdk/coercion';
import { Injectable } from '@angular/core';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { IEmployeeSearchResult,    IEmployeeSearchResultResponseData, IEmployeeNavInfo } from '@ds/employees/search/shared/models/employee-search-result';
import { IEmployeeSearchFilter } from '@ds/employees/search/shared/models/employee-search-filter';
import { IEmployeeSearchFilterOption } from '@ds/employees/search/shared/models/employee-search-filter-option';
import { EmployeeSearchFilterType } from '@ds/employees/search/shared/models/employee-search-filter-type';
import { IEmployeeAvatars } from '@ds/core/employees/shared/employee-avatars.model';
import { ClientDepartment } from '@ds/core/employee-services/models/client-department.model';
import { isNullOrUndefined } from '../../../../../lib/utilties';

export const ONBOARDING_SELECTED_EMPLOYEES_KEY = "onboarding-selected-employees";

@Injectable({
    providedIn: 'root'
})
export class EmployeeDataService {

    constructor (private store:DsStorageService) {
    }

    // Unfiltered employee list from API, should not be altered.
    private _employeeList: IEmployeeName[] = [];
    get employeeList(): IEmployeeName[] {
        if(!this._employeeList || this._employeeList.length == 0 ){
            let result = this.store.get(ONBOARDING_SELECTED_EMPLOYEES_KEY);
            if(result.hasError || !result.data) this._employeeList = [];
            else this._employeeList = result.data;
        }
        return this._employeeList;
    }

    private _currentEmployeeIndex = -1;
    set currentEmployeeIndex(value: number) {
        this._currentEmployeeIndex = coerceNumberProperty(value);
    }
    get currentEmployeeIndex(): number {
        return this._currentEmployeeIndex;
    }

    public setEmployeeList(employees: any[]): void {
        if(!employees || employees.length == 0) this._employeeList = [];

        let list = (employees || []).map(x => <IEmployeeName>{ employeeId: x.employeeId,
            employeeNumber: x.employeeNumber,
            employeeFirstName: x.employeeFirstName,
            employeeLastName: x.employeeLastName,
            employeeMiddleName: x.employeeMiddleName,
            clientId: x.clientId,
        });

        this.store.set(ONBOARDING_SELECTED_EMPLOYEES_KEY, list);
        this._employeeList = list;
    }

    public selectEmployee(empId: number): void {
        this.currentEmployeeIndex = this.employeeList.map(x=>x.employeeId).indexOf(empId);
    }
    public deleteEmployee(empId: number): void {
        let delInx = this.employeeList.map(x=>x.employeeId).indexOf(empId);
        this._employeeList.splice(delInx,1);
        if(this._employeeList.length > delInx)
            this.currentEmployeeIndex = delInx;
        else if(this._employeeList.length == 0)
            this.currentEmployeeIndex = -1;
        else
            this.currentEmployeeIndex = delInx-1;

        this.setEmployeeList(this._employeeList);
    }

    getCurrentEmployeeId(): number {
        if( isNullOrUndefined( this.currentEmployeeIndex) || !this._employeeList || this._employeeList.length == 0 ) return null;

        let employeeData = this._employeeList[this.currentEmployeeIndex];
        return employeeData.employeeId;
    }

    mapOnboardingEmployeeToSearchResponse(emp:IOnboardingEmployee):IEmployeeSearchResultResponseData
    {
        let list:IEmployeeSearchResult[] = ([emp]).map<IEmployeeSearchResult>(this.mapEmployeeInfoToSearchResult);

        if(!list || list.length == 0) {
            return <IEmployeeSearchResultResponseData>{
                totalCount: 0,
                filterCount: 0,
                pageCount: 0,
                page: -1,
                pageSize: 0,
                results: [],
                nav: <IEmployeeNavInfo>{},
            };
        } else {
            if(!this.currentEmployeeIndex || this.currentEmployeeIndex<0) this.currentEmployeeIndex = 0;
            let prev = this.currentEmployeeIndex == 0 ? null :
                this.mapEmployeeNameToSearchResult( this.employeeList[this.currentEmployeeIndex-1] );
            let curr = this.mapEmployeeInfoToSearchResult( emp );
            let next = this.currentEmployeeIndex == this.employeeList.length-1 ? null :
                this.mapEmployeeNameToSearchResult( this.employeeList[this.currentEmployeeIndex+1] );

            return <IEmployeeSearchResultResponseData>{
                totalCount: this.employeeList.length,
                filterCount: this.employeeList.length,
                pageCount: 0,
                page: -1,
                pageSize: 0,
                results: list,
                nav: <IEmployeeNavInfo>{
                    current: curr,
                    prev:prev,
                    next:next,
                    first: null,
                    last: null,
                },
            };
        }
    }

    public mapEmployeeNameToSearchResult = (x: IEmployeeName) => {
        return <IEmployeeSearchResult>{
            employeeId:          x.employeeId,
            clientId:            x.clientId,
            employeeNumber:      x.employeeNumber,
            firstName:           x.employeeFirstName,
            middleInitial:       x.employeeMiddleName,
            lastName:            x.employeeLastName,
        }
    }

    public mapEmployeeInfoToSearchResult = (x: IOnboardingEmployee) => {
        x = this.mapEmployeeAdminInfo(x);
        x = this.mapEmployeeAvatar(x);
        return <IEmployeeSearchResult>{
            employeeId:          x.employeeId,
            clientId:            x.clientId,
            clientName:          x.clientName,
            employeeNumber:      x.employeeNumber,
            firstName:           x.employeeFirstName,
            middleInitial:       x.employeeMiddleName,
            lastName:            x.employeeLastName,
            isActive:            true,
            isTemp:              false,
            isNewEmployee:      !!x.rehireDate,
            jobTitle:           x.jobTitle,
            jobProfileId:       x.jobProfileId,
            jobCategory:        x.jobCategory,
            emailAddress:       x.emailAddress,
            homePhoneNumber:    x.homePhone,
            cellPhoneNumber:    '',
            hireDate:           x.hireDate,
            separationDate:     x.separationDate,
            rehireDate:         x.rehireDate,
            terminationReason:  null,
            statusType:         x.employeeStatusDescription,
            employeeStatusType: x.employeeStatus,
            payType:            x.payType,
            divisionName:       x.divisionName,
            departmentName:     x.departmentName,
            department:         <ClientDepartment>{clientDepartmentId: x.clientDepartmentId, name: x.departmentName},
            groups:              this.getEmployeeFilterGroups(x),
            profileImage:        x.profileImage,
            $groupIx:            null,
            $homePhone:          null,
            $cellPhone:          null,
            directSupervisor:    x.supervisor ? x.supervisor : '',
            competencyModel: null,
            employeeAvatar: x.employeeAvatar,
            getGroupInfo: (group: IEmployeeSearchFilter) => [],
            getWorkHistoryDisplayText: ():string => '',
            getLengthOfServiceDisplayText: ():string => '',
        }
    }

    public mapEmployeeAdminInfo = (emp: IOnboardingEmployee) => {
        emp.intEmployeeNumber = parseInt(emp.employeeNumber);
        emp.dateHireDate = emp.hireDate;

        if (emp.hireDate) {
            const millisecs = Date.parse(emp.hireDate.toString());
            const dateNew = new Date();
            dateNew.setTime(millisecs);
            emp.hireDate = dateNew;
        }
        //
        // Flags
        //
        // isSelfOnboardingComplete => employee has completed workflow.
        // isFinalized => employee has signed forms (on-line).
        // isWorkflowComplete => workflow has been completed by the administrator.
        // isI9AdminComplete => admin has sgined I9 form (on-line).
        // isI9Required => I9 is part of the employee workflow.
        emp.isSelfOnboardingComplete = false;
        emp.selfOnboardingTitle = 'Self-Onboarding Status';

        if (emp.pctComplete === 100) emp.selfOnboardingTitle = 'Finalize Self-Onboarding';

        if (emp.essActivated) {
            emp.isSelfOnboardingComplete = true;
            emp.selfOnboardingTitle = 'Self-Onboarding Finalized';
        }

        emp.isFinalized = false;
        // Check if workflow has been completed and I9 is NOT required.
        if (emp.isWorkflowComplete) {
            if (!emp.isI9Required && emp.isWorkflowComplete) {
                emp.isI9AdminComplete = true; // Force true if not required.
            }
        }

        if (emp.employeeSignoff) {
            emp.isFinalized = true;
        }

        if (emp.isWorkflowComplete == false || emp.employeeStarted == null) {
            emp.sortEmployee = 'Needs Attention';
        } else if (emp.pctComplete < 100 && emp.employeeStarted != null) {
            emp.sortEmployee = 'In Progress';
        } else if (emp.pctComplete >= 100 && emp.adminPctComplete < 100 && emp.employeeStarted != null) {
            emp.sortEmployee = 'Completed';
        } else if (emp.pctComplete == 100 && emp.adminPctComplete == 100) {
            emp.sortEmployee = 'Close Onboarding';
        }

        if (!emp.isWorkflowComplete || (emp.isWorkflowComplete && emp.invitationSent == null)) {
            emp.sortAdmin = 'Setup Incomplete';
        } else if (emp.isWorkflowComplete && emp.invitationSent != null && emp.adminPctComplete < 100) {
            emp.sortAdmin = 'In Progress';
        } else if (emp.adminPctComplete == 100) {
            emp.sortAdmin = 'Completed';
        }

        return emp;
    }

    public mapEmployeeAvatar = (emp: IOnboardingEmployee) => {

        if(!emp.profileImage) {
          emp.profileImage = {
                  extraLarge : {
                  hasImage: false,
                  url: '',
              }
          };
          emp.employeeAvatar = <IEmployeeAvatars>{ avatarColor : ''};
          return;
        }

        // get all of our profile image stuff here
        if (emp.profileImage.profileImageInfo.length) {
            const imageSizeType = {
                XL: 1,
                LG: 2,
                MD: 3,
                SM: 4
            };
            const images = emp.profileImage.profileImageInfo;

            for (const i in images) {
                const image = images[i];
                switch (image.imageSizeType) {
                    case imageSizeType.XL:
                        emp.profileImage.extraLarge = {
                            hasImage: true,
                            url: image.source + emp.profileImage.sasToken
                        };
                        break;
                    case imageSizeType.LG:
                        emp.profileImage.large = {
                            hasImage: true,
                            url: image.source + emp.profileImage.sasToken
                        };
                        break;
                    case imageSizeType.MD:
                        emp.profileImage.medium = {
                            hasImage: true,
                            url: image.source + emp.profileImage.sasToken
                        };
                        break;
                    case imageSizeType.SM:
                        emp.profileImage.small = {
                            hasImage: true,
                            url: image.source + emp.profileImage.sasToken
                        };
                        break;
                    default:
                        // extra large is default, since we usually want largest
                        emp.profileImage.extraLarge = {
                            hasImage: false,
                            url: image.source + emp.profileImage.sasToken
                        };
                        break;
                }
            }
        } else {
            // if the user doesn't have an image, we are going to assign it a dummy "extra large" image
            // that the frontend can rely on to check for images
            emp.profileImage.extraLarge = {
                hasImage: false,
                url: null
            };
        }
        emp.employeeAvatar = emp.profileImage._employeeAvatar
              ? ({
                  employeeAvatarId:
                  emp.profileImage._employeeAvatar.employeeAvatarId,
                  employeeId: emp.employeeId,
                  clientId: emp.clientId,
                  avatarColor: emp.profileImage._employeeAvatar.avatarColor,
              } as IEmployeeAvatars)
              : <IEmployeeAvatars>{ avatarColor : ''};
        return emp;
    }

    public getEmployeeFilterGroups = (e:IOnboardingEmployee): Array<IEmployeeSearchFilterOption> => {
        let groups = [];
        if(e.departmentName){
            let deptGroup:IEmployeeSearchFilterOption = <IEmployeeSearchFilterOption>{
                filterType : EmployeeSearchFilterType.Department,
                id: e.clientDepartmentId,
                name: e.departmentName,
            };
            groups.push(deptGroup);
        }
        if(e.divisionName){
            let diviGroup:IEmployeeSearchFilterOption = <IEmployeeSearchFilterOption>{
                filterType : EmployeeSearchFilterType.Division,
                id: -1,
                name: e.divisionName,
            };
            groups.push(diviGroup);
        }
        if(e.jobTitle){
            let titleGroup:IEmployeeSearchFilterOption = <IEmployeeSearchFilterOption>{
                filterType : EmployeeSearchFilterType.JobTitle,
                id: e.jobProfileId,
                name: e.jobTitle,
            };
            groups.push(titleGroup);
        }
        return groups;
    }
}

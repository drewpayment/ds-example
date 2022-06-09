import { IEmployeeSearchFilter } from './employee-search-filter';
import { IEmployeeSearchFilterOption } from './employee-search-filter-option';
import { IImageInfo } from './image-info';
import { PhoneNumber } from './phone-number';
import { IEmployeeImage } from '@ds/core/resources/shared/employee-image.model';
import { ICompetencyModelBasic } from '@ds/performance/competencies/shared/competency-model.model';
import { IEmployeeAvatars } from '@ds/core/employees/shared/employee-avatars.model';
import { ClientDepartment } from '@ds/core/employee-services/models/client-department.model';
import { IEmployeeTerminationReason } from '@ajs/employee/models/employee-termination-reason.model';


export interface IEmployeeSearchResultResponseData {
    totalCount: number;
    filterCount: number;
    pageCount: number;
    page: number;
    pageSize:number;
    results: IEmployeeSearchResult[];
    nav: IEmployeeNavInfo;
}

export interface IEmployeeNavInfo {
    current?: IEmployeeSearchResult;
    first?: IEmployeeSearchResult;
    last?: IEmployeeSearchResult;
    next?: IEmployeeSearchResult;
    prev?: IEmployeeSearchResult;
}

export interface IEmployeeSearchResult {
    employeeId:          number;
    clientId:            number;
    clientName:          string;
    employeeNumber:      string;
    firstName:           string;
    middleInitial:       string;
    lastName:            string;
    isActive:            boolean;
    isTemp:              boolean;
    isNewEmployee?:      boolean;
    jobTitle?:           string;
    jobProfileId?:       number;
    jobCategory?:        string;
    emailAddress?:       string;
    homePhoneNumber?:    string;
    cellPhoneNumber?:    string;
    hireDate?:           Date | any;
    separationDate?:     Date | any;
    rehireDate?:         Date | any;
    terminationReason?:  IEmployeeTerminationReason;
    statusType?:         string;
    payType?:            number;
    divisionName?:       string;
    departmentName?:     string;
    department?:         ClientDepartment,
    groups:              IEmployeeSearchFilterOption[];
    profileImage:        IEmployeeImage;
    $groupIx:            {[groupType:string]:IEmployeeSearchFilterOption[]}
    $homePhone:          PhoneNumber;
    $cellPhone:          PhoneNumber;
    directSupervisor:    string;
    competencyModel?:    ICompetencyModelBasic;
    employeeAvatar?:     IEmployeeAvatars;

    getGroupInfo(group: IEmployeeSearchFilter): IEmployeeSearchFilterOption[];
    getWorkHistoryDisplayText():string;
    getLengthOfServiceDisplayText():string;
}

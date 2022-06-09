

export interface ClientDepartment {
    clientDepartmentId:number;
    code:string;
    name:string;
    clientId:number;
    isActive:boolean;
    departmentHeadEmployeeId:number|null;
}

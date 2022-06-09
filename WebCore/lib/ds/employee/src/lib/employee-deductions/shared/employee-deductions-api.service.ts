import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpRequest } from '@angular/common/http';
import { IInsertEmployeeDeductionDto, IUpdateEmployeeDeductionDto, IInsertEmployeeBankDto, IUpdateEmployeeBankDto, IDeleteEmployeeDeductionDto, IInsertEmployeeBankDeductionDto, IDeleteEmployeeBankDeductionDto, IInsertEffectiveDateDto, IPlanDeduction } from '../models/Deductions';


@Injectable({
    providedIn: 'root'
})

export class EmployeeDeductionsApiService {

    constructor(private http: HttpClient) {}

    getEmployeeDeductionsList(employeeID: number) { //gets list of all deductions-> deduction, DD, and earnings to display in tables
        return this.http.get(`api/employee/deduction/GetEmployeeDeductionListByEmployeeID/${employeeID.toString()}`);
    }

    getClientDeductionInformation(clientID: number, userTypeID: number, employeeID: number, userID: number){ //gets information for vendor, max type, plan dropdowns, clientCostCenter Dropdowns, and prenote info
        return this.http.get(`api/clients/deductions/${clientID.toString()}/${userTypeID.toString()}/${0}/${employeeID.toString()}/${userID.toString()}` );
    }

    getClientDeductionThatAllowDDs(clientID: number){ //used to get a list of IDs that show which
        return this.http.get(`api/clients/deductions/allowDD/${clientID.toString()}` );
    }

    getEmployeeAmountType(clientID: number, clientDeductionID: number){ //gets information for AmountType Dropdown
        return this.http.get(`api/clients/deductions/AmountType/${clientID}/${clientDeductionID}`);
    }

    getDeductionDescriptionList(employeeId:number, clientDeductionID: number, payrollId: number, deductionType: number, clientId: number, subCheck: string){ //gets information for deductions dropdown
        return this.http.get(`api/clients/deductions/DeductionList/${employeeId}/${clientDeductionID}/${payrollId}/${deductionType}/${clientId}/${subCheck}`);
    }

    insertNewDeduction(insertDeductionDTO: IInsertEmployeeDeductionDto){
        return this.http.post('api/employee/deduction/InsertEmployeeDeduction', insertDeductionDTO);
    }

    updateExistingDeduction(updateDeductionDTO: IUpdateEmployeeDeductionDto){
        return this.http.post('api/employee/deduction/UpdateEmployeeDeduction', updateDeductionDTO);
    }

    insertEmployeeBankDto(insertEmployeeBankDto: IInsertEmployeeBankDto){
        return this.http.post('api/employee/deduction/InsertEmployeeBank', insertEmployeeBankDto);
    }

    updateEmployeeBankDto(updateEmployeeBankDto: IUpdateEmployeeBankDto){
        return this.http.post('api/employee/deduction/UpdateEmployeeBank', updateEmployeeBankDto);
    }

    deleteEmployeeDeduction(deleteEmployeeDeductionDto: IDeleteEmployeeDeductionDto){
        return this.http.post('api/employee/deduction/DeleteEmployeeDeduction', deleteEmployeeDeductionDto);
    }

    insertEmployeeBankDeduction(insertEmployeeBankDeductionDto: IInsertEmployeeBankDeductionDto){
        return this.http.post('api/employee/deduction/InsertEmployeeBankDeduction', insertEmployeeBankDeductionDto);
    }

    deleteEmployeeBankDeduction(deleteEmployeeBankDeductionDto: IDeleteEmployeeBankDeductionDto){
        return this.http.post('api/employee/deduction/DeleteEmployeeBankDeduction', deleteEmployeeBankDeductionDto);
    }

    insertEffectiveDate(insertEffectiveDateDto: IInsertEffectiveDateDto){
        return this.http.post('api/employee/deduction/InsertEffectiveDate', insertEffectiveDateDto);
    }

    getSupervisorHasBlockedDeductions(userId: number){
        return this.http.get(`api/employee/deduction/GetSupervisorHasBlockedDeductions/${userId}`)
    }

    getPlanListByClientDeductionId(clientId: number, clientDeductionId: number){
        return this.http.get<IPlanDeduction[]>(`api/clients/plansByDeduction/${clientId}/${clientDeductionId}`)
    }
}

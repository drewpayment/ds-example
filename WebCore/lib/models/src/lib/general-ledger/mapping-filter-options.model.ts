import { GeneralLedgerType } from './general-ledger-type.model';
import { ClientGLSubPayrollTransactions } from './client-gl-sub-payroll-transactions.model';

export interface MappingFilterOptions {
    controlExists: Boolean;
    splitByCostCenter: Boolean;
    splitByDepartment: Boolean;
    splitByCustomClass: Boolean;

    includeAccrual: Boolean;
    includeProject: Boolean;
    includeSequence: Boolean;
    includeClassGroups: Boolean;
    includeOffset: Boolean;
    includeDetail: Boolean;

    selectedClass: number;
    selectedCategory: number;
    selectedGeneralLedgerType: number;
    selectedSubPayrollTransaction: number;
    GLTypesFromCategory : GeneralLedgerType[];
    subPayrollTransactionsFromType : ClientGLSubPayrollTransactions[];
    cashGLTypes : GeneralLedgerType[];
    liabilityGLTypes : GeneralLedgerType[];
    expenseGLTypes : GeneralLedgerType[];
    paymentGLTypes : GeneralLedgerType[];
    subPayrollTransactions : ClientGLSubPayrollTransactions[];
}
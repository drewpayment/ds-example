import { Maybe } from '@ds/core/shared/Maybe';
import { IPayrollRequestItem } from '../../shared/payroll-request-item.model';
import { IPayrollRequest } from '../../shared/payroll-request.model';
import { meritIncreaseRequestType, bonusIncreaseRequestType } from './shared/report-display-data.model';
import { ClientSideFilters } from './payroll-request-report-args.model';

export interface PayrollRequestFilter {
    filter(items: Maybe<IPayrollRequest>): Maybe<IPayrollRequestItem[]>;
}

export class ProposalFilterStrategy implements PayrollRequestFilter {
    private includedApprovalStatus: {[id: number]: boolean} = {};

    constructor(statuses: number[]) {
        if (statuses != null) {
            this.includedApprovalStatus = statuses.reduce((prev, curr) => {
                prev[curr] = true;
                return prev;
            }, {})
        }
    }

    filter(payrollRequest: Maybe<IPayrollRequest>): Maybe<IPayrollRequestItem[]> {
        return payrollRequest.map(list => {
            return list.payrollRequestItems
        });
    }

}

export class PayrollRequestTypeFilterStrategy implements PayrollRequestFilter {

    constructor(private includeMerit: boolean, private includeOneTime: boolean) { }

    filter(items: Maybe<IPayrollRequest>): Maybe<IPayrollRequestItem[]> {
        return items.map(list => list.payrollRequestItems).map(payrollRequestItems => payrollRequestItems.filter(item => this.ShouldIncludeItem(item)))
    }

    private ShouldIncludeItem(item: IPayrollRequestItem): boolean {
        const isMerit = item.requestType === meritIncreaseRequestType;
        const isOneTime = item.requestType === bonusIncreaseRequestType;

        if(isMerit && !this.includeMerit){
            return false;
        }

        if(isOneTime && !this.includeOneTime){
            return false;
        }

        return true;
    }

}

export class PayrollRequestFilterFactory{

    getFilter(filterSettings: ClientSideFilters): PayrollRequestFilter{

        if(filterSettings.showTable === true){
            return new PayrollRequestTypeFilterStrategy(filterSettings.showMerit, filterSettings.showOneTime);
        }

        return new ProposalFilterStrategy(filterSettings.selectedApprovalStatus);
    }
}
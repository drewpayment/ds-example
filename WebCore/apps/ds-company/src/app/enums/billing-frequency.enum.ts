import { KeyValue } from '@models';


export enum BillingFrequency {
    EveryPayroll = 1,
    Monthly = 2,
    Quarterly = 3,
    Annual = 4,
    OddPayrollOut = 5
}

export namespace BillingFrequency {
    export function toString(item: BillingFrequency): string {
        let notFound = BillingFrequency[item]; // <-- name not value
        if (item ==  null) return '';
        switch(item) {
            case BillingFrequency.EveryPayroll:
                return 'Every Payroll';
            case BillingFrequency.Monthly:
                return 'Monthly';
            case BillingFrequency.Quarterly:
                return 'Quarterly';
            case BillingFrequency.Annual:
                return 'Annual';
            case BillingFrequency.OddPayrollOut:
                return 'Odd Payroll Out';
            default:
                return notFound;
        }
    }   // end of toString

    export function getModel(): KeyValue[] {
        let names: KeyValue[] = [];
        for (var elm in this) {
            const isInteger = parseInt(elm) >= 0;
            if (isInteger) {
                const desc = this.toString(+elm);
                const id   = +elm;
                names.push({id: id, description: desc});
            }
        }
        names.sort((x, y) => x.description.localeCompare(y.description));
        return names;
    }
}

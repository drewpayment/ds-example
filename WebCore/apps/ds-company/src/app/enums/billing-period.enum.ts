import { KeyValue } from '@models';

export enum BillingPeriod {
    NextPayroll = 1,
    NextNormalPayroll = 2,
    CreditCarryOver = 3
}

export namespace BillingPeriod {
    export function toString(item: BillingPeriod): string {
        let notFound = BillingPeriod[item] // <-- name not value
        if (item ==  null) return '';
        switch(item) {
            case BillingPeriod.NextPayroll:
                return 'Next Payroll';
            case BillingPeriod.NextNormalPayroll:
                return 'Next Normal Payroll';
            case BillingPeriod.CreditCarryOver:
                return 'Credit Carry Over';
            default:
                return notFound;
        }
    }   // end of toString()

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

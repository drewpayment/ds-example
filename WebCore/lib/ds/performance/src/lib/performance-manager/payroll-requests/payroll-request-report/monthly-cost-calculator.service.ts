import { IPayrollRequestItem } from '../../shared/payroll-request-item.model';
import { PayFrequencyTypeEnum } from '@ajs/applicantTracking/shared/models/qualifyapplicants-data.interface';

export interface MonthlyCostCalculator {
  DoesCalcApply(item: IPayrollRequestItem): boolean;
  ApplyCalculationFrom(item: IPayrollRequestItem): number;
  ApplyCalculationTo(item: IPayrollRequestItem): number;
}

export class HourlyStrategy implements MonthlyCostCalculator {

    private readonly HoursWorkedPerWeek = 40;
    private readonly WeeksInYear = 52;
    private readonly MonthsInYear = 12;
    
    DoesCalcApply(item: IPayrollRequestItem): boolean {
        return item.employeeClientRateId != null;
    }

    ApplyCalculationFrom(item: IPayrollRequestItem): number {
        return this.calculatePayout(item.payoutFrom)
    }

    ApplyCalculationTo(item: IPayrollRequestItem): number {
        return this.calculatePayout(item.payoutTo);
    }

    private calculatePayout(payout: number): number {
        return (payout * this.HoursWorkedPerWeek * this.WeeksInYear) / this.MonthsInYear;
    }

}

export class SalaryStrategy implements MonthlyCostCalculator {
    

    DoesCalcApply(item: IPayrollRequestItem): boolean {
        return item.employeeClientRateId == null;
    }

    ApplyCalculationFrom(item: IPayrollRequestItem): number {
        return this.calculatePayout(item.payoutFrom, item.payFrequencyId);
    }

    ApplyCalculationTo(item: IPayrollRequestItem): number {
        return this.calculatePayout(item.payoutTo, item.payFrequencyId);
    }

    private calculatePayout(payout: number, payFrequency: number): number {
        var yearlyPayout = 0;

        switch (payFrequency) {
            case PayFrequencyTypeEnum.Weekly:
                yearlyPayout = payout * 52;
                break;
            case PayFrequencyTypeEnum.AlternateBiWeekly:
            case PayFrequencyTypeEnum.BiWeekly:
                yearlyPayout = payout * 26;
                break;
            case PayFrequencyTypeEnum.SemiMonthly:
                yearlyPayout = payout * 24;
                break;
            case PayFrequencyTypeEnum.Monthly:
                yearlyPayout = payout * 12;
                break;
            case PayFrequencyTypeEnum.Quarterly:
                yearlyPayout = payout * 4;
                break;
            default:
                throw new Error(`Invalid Pay Frequency: ${payFrequency}`);
        }

        return yearlyPayout / 12;
    }
}
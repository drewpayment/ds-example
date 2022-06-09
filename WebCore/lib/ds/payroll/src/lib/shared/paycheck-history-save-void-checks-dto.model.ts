export interface IPaycheckHistorySaveVoidChecksDto
{
    genPaycheckHistoryId: number;
    employeeId: number;
    equals(other: IPaycheckHistorySaveVoidChecksDto): boolean;
}

export class PaycheckHistorySaveVoidChecksDto implements IPaycheckHistorySaveVoidChecksDto {
    genPaycheckHistoryId: number;
    employeeId: number;

    constructor(genPaycheckHistoryId: number, employeeId: number) {
        this.genPaycheckHistoryId = genPaycheckHistoryId;
        this.employeeId = employeeId;
    }

    equals(other: IPaycheckHistorySaveVoidChecksDto) {
        return this.genPaycheckHistoryId === other.genPaycheckHistoryId && this.employeeId === other.employeeId;
    }
}

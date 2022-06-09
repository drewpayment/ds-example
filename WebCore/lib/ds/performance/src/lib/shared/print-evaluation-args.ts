
export const PRINT_EVALUATION_KEY = 'print-evaluation';

export interface PrintEvaluationArgs {
    reviewedEmployeeId: number;
    reviewId: number;
    evaluationId: number;
    printForEmp: boolean;
}

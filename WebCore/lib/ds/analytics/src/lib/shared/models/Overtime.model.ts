export interface OvertimeData {
    employeeId: number;
    employeeName: string;
    supervisor: string;
    department: string;
    totalHoursScheduled: number[];
    totalHoursWorked: number[];
    overtimeHours: number;
    overtimeFrequency: number;
}
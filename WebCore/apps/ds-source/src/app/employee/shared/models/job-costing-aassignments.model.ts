import { JobCostingAssignment } from '@ds/core/employee-services/models';

export interface JobCostingIdAssignments {
    clientJobCostingId: number;
    availableAssignments: JobCostingAssignment[];
}

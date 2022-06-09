import { JobCosting } from './job-costing.model';

export interface ClientJobCostingCustom {
    clientJobCosting: JobCosting;
    clientJobCostingList: JobCosting[]; 
}
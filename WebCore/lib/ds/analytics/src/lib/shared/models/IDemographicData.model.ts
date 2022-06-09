import { DemographicInfo } from './DemographicData.model'
import { DateRange } from './DateRange.model';

export interface IDemographicData {
    demographicData: DemographicInfo[];
    featureName: string;
    dateRange: DateRange;
}
import { IncreaseType } from '../shared/increase-type';
import { BasedOn } from '../shared/based-on';
import { Measurement } from '../shared/measurement';

export interface IOneTimeEarningSettings {
    oneTimeEarningSettingsId: number;
    employeeId?: number;
    name: string;
    increaseType?: IncreaseType;
    increaseAmount?: number;
    basedOn?: BasedOn;
    measurement?: Measurement;
    displayInEss: boolean;
    isArchived: boolean;
}

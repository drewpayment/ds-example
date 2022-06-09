import { ClientFeatureOption } from './client-feature-option.model';

export interface FeatureOptionGroup {
    featureOptionsGroupId: number;
    description: string;
    featureOptions: ClientFeatureOption[];
}
import { RelativeTimeType } from './relative-time-type';

export interface IRelativeTimeDifference {
    displayText: string;
    relativeTimeType: RelativeTimeType;
    isUpcoming: boolean;
    isSoon: boolean;
    isInPast: boolean;
}
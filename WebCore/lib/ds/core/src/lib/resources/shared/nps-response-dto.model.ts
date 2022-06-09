import { UserType } from '@ds/core/shared';

export interface INpsResponseDto {
    //responseId: number;
    questionId: number;
    userId: number;
    userTypeId: UserType,
    clientId: number;
    responseDate: Date;
    score: number;
    feedback: string;
}

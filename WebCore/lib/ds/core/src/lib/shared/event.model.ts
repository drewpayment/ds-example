import { Moment } from 'moment'

export interface IEvent {
    employeeEventId: number,
    employeeId: number,
    clientId: number,
    clientTopicId: number,
    clientTopicDescription: string,
    clientSubTopicId: number,
    clientSubTopicDescription: string,
    eventDate: Date | Moment,
    event: string,
    duration: number,// Not included
    isEmployeeViewable: boolean,// Not included
    isEmployeeEditable: boolean,// Not included
    expirationDate: Date | Moment,
    isDirty?: boolean,
    hovered?: boolean,
}

export interface IClientSubTopic {
    clientTopicId: number;
    clientSubTopicId?: number;
    description: string;
    topicDescription: string;
}

export interface IClientTopic {
    clientId: number;
    clientTopicId: number;
    description: string;
    //modifiedBy: number;
    //clientSubTopics?: IClientSubTopic[];
    //subTopicDescription: IClientSubTopic;
}
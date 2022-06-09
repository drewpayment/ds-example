import { IMeetingAttendee } from "./meeting-attendee.model";
import { Moment } from "moment";

export interface IMeeting {
    meetingId: number;
    title: string;
    description?: string;
    location?: string;
    startDateTime: Date | string | Moment;
    endDateTime?: Date | string | Moment;
    attendees?: IMeetingAttendee[];
}
import { IContact } from "@ds/core/contacts";

export interface IMeetingAttendee extends IContact {
    meetingAttendeeId: number;
    meetingId: number;
}
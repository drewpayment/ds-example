export interface IScheduledReportFileFormat {
    scheduledReportFileFormatId              : scheduledReportFileFormatEnum;
    scheduledReportFileFormat                : string;
}

export enum scheduledReportFileFormatEnum {
    pdf = 1,
    html = 2,
    csv = 3,
    excel = 4
}

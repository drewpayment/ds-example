import { IEmployeeAttachment } from './employee-attachment.model';
import { IEmployeeAttachmentFolder } from './employee-attachment-folder.model';

export interface IEmployeeAttachmentFolderDetail extends IEmployeeAttachmentFolder {
    attachmentCount: number,
    isSystemFolder: boolean,
    isCompanyFolder: boolean,
    showAttachments: boolean,
    attachments: IEmployeeAttachment[]
}

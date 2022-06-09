import { EmployeeAttachmentFolder } from './employee-attachment-folder.model';
import { EmployeeAttachment } from './employee-attachment.model';

export interface EmployeeAttachmentFolderDetail extends EmployeeAttachmentFolder {
    attachmentCount: number;
    isSystemFolder: boolean;
    isCompanyFolder: boolean;
    showAttachments: boolean;
    isDefaultOnboardingFolder: boolean;
    isDefaultPerformanceFolder: boolean;
    attachments: EmployeeAttachment[];
}
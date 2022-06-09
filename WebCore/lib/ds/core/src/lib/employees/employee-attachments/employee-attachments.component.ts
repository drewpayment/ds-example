import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { DsEmployeeAttachmentModalService } from '../../../../../../../Scripts/ds/employee/attachments/addattachment-modal.service';
import { DsResourceApi } from '../../../../../../../Scripts/ds/core/ds-resource/ds-resource-api.service';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'ds-employee-attachments',
  templateUrl: './employee-attachments.component.html',
  styleUrls: ['./employee-attachments.component.scss']
})
export class EmployeeAttachmentsComponent implements OnInit {
  @Input() files: any;
  @Input() userAccount: any;
  @Output() editedAttachment = new EventEmitter();
  @Input()  folders: any[];
  constructor(
    public employeeAttachModalService: DsEmployeeAttachmentModalService,
    private dsResourceApi: DsResourceApi
  ) { }

  ngOnInit() {
  }

  // function from employee-attachments.controller
  getMappedExtension(extension) {
      extension = extension.replace('.', '').toUpperCase();
      if (extension == 'JFIF' || extension == 'JPG') {
          return 'JPEG';
      }
      return extension;
  }

  download(file) {
    this.dsResourceApi.downloadResource(file.resourceId, file.isAzure);
  }

  // modified function from employee-attachments.controller
  editModal(currAttachment) {
    let origFolderId = currAttachment.folderId;
    let origIsViewableByEmployee = currAttachment.isViewableByEmployee;
    let origName = currAttachment.name;
    let origExt = currAttachment.extension;
    let deleted = false;
    let currentFolder;
    this.folders.forEach( x => {
      if (x.employeeFolderId == currAttachment.folderId) {
        currentFolder = x;
      }
    });

    let currAttachmentCopy = Object.assign({}, currAttachment);
    this.employeeAttachModalService.openPageOtherThanEmployeeAttachments
      (currAttachmentCopy, this.userAccount, this.folders, false /*isCompanyAttachments false cause it's an employee attachment?*/, false).result.then( result => {
      // file edited
        // file edited
        if (origFolderId != currAttachmentCopy.folderId ||
            origIsViewableByEmployee != currAttachmentCopy.isViewableByEmployee ||
            origName != currAttachmentCopy.name ||
            origExt != currAttachmentCopy.extension
            )
        {
          let editedAttach = {
            attachment: currAttachmentCopy,
            editType:   1 // 1 is edited, 2 is deleted
          }
          this.editedAttachment.emit(editedAttach);
          return;
        }

        // file not edited or deleted
        let updatedFolder = null
        for (let i = 0; i < result.savedFolderList.length; i++) {
          if (origFolderId == result.savedFolderList[i].employeeFolderId){
            updatedFolder = result.savedFolderList[i];
            break;
          }
        }

        if (updatedFolder.attachments > 0) {
          updatedFolder.attachments.forEach( y => {
            if (y.resourceId = currAttachmentCopy.resourceId) {
              deleted = true;
            }
          });
        } else {
          deleted = true;
        }

        if (deleted) {
          let editedAttach = {
            attachment: currAttachmentCopy,
            editType:   2
          }
          this.editedAttachment.emit(editedAttach);
          return;
        }
        this.folders = result.savedFolderList;
    });
  }

  onEvent(event) {
    event.stopPropagation();
  }
}

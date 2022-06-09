import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { AttachmentsService } from '../shared/attachments.service';
import { EmployeeAttachmentFolderDetail, EmployeeAttachment } from '../shared/models';
import { Observable } from 'rxjs';import { FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { startWith, map, debounceTime, skip, delay } from 'rxjs/operators';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import { ResourceApiService } from '@ds/core/resources/shared/resources-api.service';
import { DomSanitizer } from '@angular/platform-browser';
import { Moment } from 'moment';
import * as moment from 'moment';
import { MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';

@Component({
  selector: 'ds-employee-attachment-viewer',
  templateUrl: './employee-attachment-viewer.component.html',
  styleUrls: ['./employee-attachment-viewer.component.scss']
})
export class EmployeeAttachmentViewerComponent implements OnInit {
  f: FormGroup = this.createForm();
  user: UserInfo;
  folders: EmployeeAttachmentFolderDetail[];
  selectedFolders: any[] = [];
  filteredFolders : Observable<EmployeeAttachmentFolderDetail[]>;
  selectedPDFAttachment: EmployeeAttachment;
  filteredAttachments: EmployeeAttachment[] = [];
  pdfAttachments: EmployeeAttachment[] = [];
  otherAttachments: EmployeeAttachment[] = [];
  isLoading: Boolean = true;
  showAttachmentCards: Boolean = false;
  isAttachmentsLoading: Boolean = true;
  source: string;
  safeSource: any;
  pdfColWidth: string = "col-lg-8";
  otherColWidth: string = "col-lg-4";
  otherColCardWidth: string[] = ["col-6", "col-lg-12", "col-xl-6"];

  /** Material Chip Variable */
  visible = true;

  /** Material Chip Variable */
  selectable = true;

  /** Material Chip Variable */
  removable = true;

  /** Material Chip Variable */
  addOnBlur = false;

  /** Material Chip Key codes */
  separatorKeysCodes: number[] = [ENTER, COMMA];

  /** New FormControl */
  folderCtrl = new FormControl();

  /** watching html input */
  @ViewChild('folderInput', {static: false}) folderInput: ElementRef<HTMLInputElement>;

  /** watching material input */
  @ViewChild('auto', {static: false}) matAutocomplete: MatAutocomplete;

  get startDate() { return this.f.get('startDate') as FormControl; }
  get endDate() { return this.f.get('endDate') as FormControl; }
  readonly momentFormatString = 'MM/DD/YYYY';
  constructor(
    private fb: FormBuilder,
    private api: AttachmentsService,
    private accountService: AccountService,
    private msg: DsMsgService,
    private resourceService: ResourceApiService,
    public sanitizer: DomSanitizer
  ) { }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe(user => {
      this.user = user;
      this.api.employeeFolders$.subscribe((data : EmployeeAttachmentFolderDetail[]) => {
        if (data == null) return;
        this.folders = data;

        this.filteredFolders = this.folderCtrl.valueChanges.pipe(
          debounceTime(250),
          // tslint:disable-next-line:deprecation
          startWith(''),
          map((folder: string | null) => folder ? this._filter(folder) : this._filter(''))
        );

        if (this.folders != null && this.folders.length > 0) {
          const defPerfFolder = this.folders.find((folder) => {
            return folder.isDefaultPerformanceFolder;
          });
          const hasPerfFolderExisting = this.selectedFolders.findIndex(f => f.isDefaultPerformanceFolder == true) > -1;
          if (defPerfFolder != null && !hasPerfFolderExisting) {
            this.selectedFolders.push(defPerfFolder);
            this.api.folderId = defPerfFolder.employeeFolderId;
            this.updateFilteredList();
          }
        }

        // reload the attachments
        if (this.selectedFolders.length && this.showAttachmentCards) {
          let folderIds = [];
          this.selectedFolders.forEach((sf : EmployeeAttachmentFolderDetail) => {
            folderIds.push(sf.employeeFolderId);
          });
          this.selectedFolders = this.folders.filter((f: EmployeeAttachmentFolderDetail) => {
            return this.selectedFolders.findIndex((sf: EmployeeAttachmentFolderDetail) => sf.employeeFolderId == f.employeeFolderId) > -1;
          });
          this.showAttachments();
        }

        this.isLoading = false;
      });
      this.api.fetchEmployeeFolders(this.user.selectedEmployeeId(), this.user.selectedClientId(), true);
    });
  }

  private createForm(): FormGroup {
    return this.fb.group({
        startDate: this.fb.control(null),
        endDate: this.fb.control(null),
    });
  }

  showAttachments() {

    if (this.f.invalid)
      return;

    this.showAttachmentCards = true;
    this.isAttachmentsLoading = true;

    this.filteredAttachments = [];
    this.pdfAttachments = [];
    this.otherAttachments = [];
    this.selectedPDFAttachment = null;

    this.selectedFolders.forEach((folder : EmployeeAttachmentFolderDetail ) => {
      if (folder.attachmentCount > 0)
      this.filteredAttachments = this.filteredAttachments.concat(folder.attachments);
    });

    this.filteredAttachments.forEach((att : EmployeeAttachment) => {
      /** Filtering out attachments that don't align with the correct dates */
      if (this.endDate.value != null && this.startDate.value != null) {
        if (moment(att.addedDate).isAfter(this.endDate.value, 'day') || moment(att.addedDate).isBefore(this.startDate.value, 'day'))
          return;
      } else if (this.endDate.value != null && this.startDate.value == null) {
        if (moment(att.addedDate).isAfter(this.endDate.value, 'day'))
          return;
      } else if (this.endDate.value == null && this.startDate.value != null) {
        if (moment(att.addedDate).isBefore(this.startDate.value, 'day'))
          return;
      }
      /** Separating PDFs from other documents */
      if (att.extension.toLowerCase() == '.pdf')
        this.pdfAttachments.push(att);
      else
        this.otherAttachments.push(att);

    });

    if (this.pdfAttachments.length == 0 && this.otherAttachments.length > 0) {
      this.otherColWidth = "col-lg-12";
      this.otherColCardWidth = ["col-6", "col-md-4", "col-lg-3"];
    } else if (this.otherAttachments.length == 0 && this.pdfAttachments.length > 0) {
      this.pdfColWidth = "col-lg-12";
    } else if (this.pdfAttachments != null && this.pdfAttachments.length && this.otherAttachments != null && this.otherAttachments.length) {
      this.pdfColWidth = "col-lg-8";
      this.otherColWidth = "col-lg-4";
      this.otherColCardWidth = ["col-6", "col-lg-12", "col-xl-6"];
    }

    if (this.pdfAttachments.length) this.selectAttachment(this.pdfAttachments[0]);

    this.isAttachmentsLoading = false;
  } // end of showAttachments

  selectAttachment(attachment: EmployeeAttachment) {
    this.selectedPDFAttachment = attachment;
    this.source = 'api/resources/' + this.selectedPDFAttachment.resourceId + "?wmode=transparent&isDownload=false";
    this.safeSource = this.sanitizer.bypassSecurityTrustResourceUrl(this.source);
  }

  getMappedExtension(extension : string) {
    extension = extension.replace('.','').toUpperCase();
    if (extension == "JFIF" || extension == "JPG")
        return 'JPEG';
    return extension;
  }

  download(attachment: EmployeeAttachment) {
    this.resourceService.downloadResource(attachment.resourceId, attachment.extension);
  }

  //--------------- All Angular Chip Methods Below ---------------//

  /****************************************
   * Add App event handler
   * @param event js event
   ***************************************/
  add(event: MatChipInputEvent): void {
    // Add folder only when MatAutocomplete is not open
    // To make sure this does not conflict with OptionSelected Event
    if (!this.matAutocomplete.isOpen) {
      // do nothing user cannot add folders
    }
  }

  /************************************************
   * Removes folder from selected list
   * @param folder detailed employee folder obj
   ***********************************************/
  remove(folder: EmployeeAttachmentFolderDetail): void {
    const index = this.selectedFolders.indexOf(folder);

    if (index >= 0) {
      this.selectedFolders.splice(index, 1);
      this.updateFilteredList();
    }
  }

  /**************************************************
   * This method is used to update the filtered list
   * in mat autocomplete when a selected or removed
   * event occurs
   *************************************************/
  updateFilteredList(): void {
    this.filteredFolders = this.filteredFolders.pipe(
      map(() => this._filter(''))
    );
  }

  /*****************************************
   * Selected folder event handler
   * @param event material event
   ****************************************/
  selected(event: MatAutocompleteSelectedEvent): void {
    this.selectedFolders.push(event.option.value);
    this.folderInput.nativeElement.value = '';
    this.folderCtrl.setValue(null);
    this.updateFilteredList();
  }

  /**********************************************
   * Filters input text from app list
   * @param text inputted filter text
   * @returns filtered list of folders
   *********************************************/
  private _filter(text: any): EmployeeAttachmentFolderDetail[] {
    const fs  = this.selectedFolders;
    const list = _.filter(this.folders, (f: EmployeeAttachmentFolderDetail) => {
      return _.findIndex(fs, {employeeFolderId: f.employeeFolderId}) === -1;
    });

    const results = text ? list.filter(this.createFilterFor(text)) : list.filter(this.createFilterFor(''));
    return results;
  }

  /***********************************************
   * Creating list based on user input
   * @param query inputted filter text
   * @returns filtered list of folders
   **********************************************/
  public createFilterFor(query: string) {
    const lowerCaseQuery = query.toString().toLowerCase();

    return function filterFn(folder: EmployeeAttachmentFolderDetail) {
      return (folder.description.toString().toLowerCase().indexOf(lowerCaseQuery) === 0);
    };
  }

  //--------------- All Angular Chip Methods Above ---------------//

  onEvent(event) {
    event.stopPropagation();
  }
}

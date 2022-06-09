import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { IEmployeeNotes, INoteSource, INoteTag, IEmpAttachmentChanges } from '../../shared/employee-notes-api.model';
import { EmployeeApiService } from '../../shared/employee-api.service';
import * as moment from 'moment';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { HttpErrorResponse } from '@angular/common/http';
import { FormControl, FormGroup } from '@angular/forms';
import { debounceTime, distinctUntilChanged, concatMap, tap, exhaustMap, map, startWith } from 'rxjs/operators';
import { switchMap } from 'rxjs/operators';
import { MatInput } from '@angular/material/input';
import { DsEmployeeAttachmentModalService } from '../../../../../../../../Scripts/ds/employee/attachments/addattachment-modal.service';
import { EmployeeAttachmentApiService } from '../../employee-attachments/employee-attachment-api.service';
import { AccountService } from '@ds/core/account.service';
import { EmployeeAttachmentsComponent } from '../../employee-attachments/employee-attachments.component';
import { Observable, forkJoin, Subject, of, concat, iif } from 'rxjs';
import { IEmployeeAttachment } from '../../employee-attachments/employee-attachment.model';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { UserType } from '@ds/core/shared';

@Component({
  selector: 'ds-employee-notes',
  templateUrl: './employee-notes.component.html',
  styleUrls: ['./employee-notes.component.scss'],
  providers: [EmployeeAttachmentsComponent],
})
export class EmployeeNotesComponent implements OnInit {
  private _isEssMode = false;
  @Input()
  get isEssMode(): boolean {
      return this._isEssMode;
  }
  set isEssMode(value: boolean) {
      this._isEssMode = coerceBooleanProperty(value);
  }
  @Input() employeeId:  number;
  @ViewChild('fromInput', { read: MatInput, static: false }) fromInput: MatInput;
  @ViewChild('toInput',   { read: MatInput, static: false }) toInput:   MatInput;
  newNote:              IEmployeeNotes;
  empNotes:             IEmployeeNotes[];
  archivedEmpNotes:     IEmployeeNotes[];
  displayedNotesCount:  number | 0;
  totalNotesCount:      number | 0;
  selectedSource:       number | 999;
  sources:              INoteSource[];
  displayedEmpNotes:    IEmployeeNotes[];
  fromDate:             any | null;
  toDate:               any | null;
  searchText:           string | '';
  searchTextFormCtrl =  new FormControl('', { updateOn: 'change' });
  displayArchive:       boolean | false;
  isHRBlocked:          boolean | false;
  ViewOnly = false;
  notSelectedFilterTags:INoteTag[] = [];
  clientNoteTags:       INoteTag[] = [];
  selectedFilterTags:   INoteTag[] = [];
  searchTagsInput = new FormControl();
  searchTagsCtrl = new FormControl('');
  tagsFormGroup:        FormGroup  = new FormGroup({
    searchTagsInput: this.searchTagsCtrl
  });
  filteredTags: Observable<any>;
  @ViewChild('searchTagsElm', {static: false}) searchTagsElm: ElementRef<HTMLInputElement>

  folders:              any[];
  userinfo:             any;
  currentAttachment:    any;
  displayedAttachments: any;
  loading:              boolean;
  forwardToGetArchived: Subject<any> = new Subject();

  subscriptionHandler$: Observable<any>;

  constructor(
    private api: EmployeeApiService,
    private confirmSvc: DsConfirmService,
    private msgSvc: DsMsgService,
    public modalSvc:        DsEmployeeAttachmentModalService,
    private attachmentApi:  EmployeeAttachmentApiService,
    private accountService: AccountService
  ) { }

  private _filter(value: string): INoteTag[] {
    return this.notSelectedFilterTags.filter(tag => tag.tagName.toLowerCase().indexOf(value.toLowerCase()) === 0);
  }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe( data => {
      this.userinfo = data;

      if(!this.employeeId) {
        if(this.isEssMode)  this.employeeId = this.userinfo.employeeId;
        else                this.employeeId = this.userinfo.lastEmployeeId || this.userinfo.employeeId;
      }
      this.initSubscriptions();
    });
  }
  initSubscriptions() {
    this.loading = true;

    this.subscriptionHandler$ = this.forwardToGetArchived.pipe(exhaustMap(x => this.api.getArchivedEmployeeNotes(this.employeeId).pipe(
      concatMap(x => {
        this.archivedEmpNotes = x;
        this.archivedEmpNotes.forEach(x => x.activity.sort(function(j, k) { return new Date(k.activityDate).getTime() - new Date(j.activityDate).getTime(); }));

        const calls: Observable<IEmployeeAttachment[]>[] = [];
        x.forEach(item => {
          calls.push(this.attachmentApi.getEmployeeAttachmentsByResourceIds(item.attachments.map(y => y.attachmentId), this.employeeId, this.userinfo.clientId, true));
        });

        return concat(this.sortArchivedNotes(calls), this.showArchive())
      })
    )));

    this.checkCurrentUser().pipe(
      switchMap( userInfo => this.attachmentApi.getEmployeeAttachmentFolderList(this.employeeId, this.userinfo.clientId, true))
        ,tap( dataFolders => {
          this.folders = dataFolders;

          this.currentAttachment = {
            resourceId: 0,
            clientId: this.userinfo.clientId,
            employeeId: this.employeeId,
            name: '',
            folderId: this.folders && this.folders.length ? this.folders[0].employeeFolderId : null,
            sourceType: 1,
            source: null,
            isViewableByEmployee: true,
            isNew: true
          };
        })).subscribe();

    this.checkCurrentUser().pipe(switchMap(userInfo =>
    this.api.getCurrentUserHRBlockedAndViewOnly().pipe(tap( x => {
      this.isHRBlocked = x.isHrBlocked;
      if(this.isEssMode)
        this.ViewOnly = false ;
      else
        this.ViewOnly = x.isEmployeeSelfServiceViewOnly && (this.userinfo.userTypeId != UserType.supervisor) ;

      if (!this.isHRBlocked) {
        this.api.getEmployeeNotes(this.employeeId)
          .subscribe(data => {
            data.forEach(x => x.activity.sort(function(j, k) { return new Date(k.activityDate).getTime() - new Date(j.activityDate).getTime(); }));
            this.empNotes = data;
            this.sortNotes();
            this.displayedEmpNotes = this.empNotes;
            this.displayedNotesCount = this.empNotes.length;
            this.totalNotesCount = this.empNotes.length;
            this.displayArchive = false;

                for (let i = 0; i < this.empNotes.length; i++) {
                  if (this.empNotes[i].attachments.length > 0) {
                    let attachmentIds = [];
                    for (let a = 0; a < this.empNotes[i].attachments.length; a++) {
                      attachmentIds.push(this.empNotes[i].attachments[a].attachmentId);
                    }

                    this.attachmentApi.getEmployeeAttachmentsByResourceIds(attachmentIds, this.employeeId, this.userinfo.clientId, true)
                    .subscribe(dataAtt => {
                      if (dataAtt.length > 1) {
                        dataAtt.sort((j, k) => {
                          if (j.name > k.name) {
                            return 1;
                          } else if (j.name < k.name) {
                            return -1;
                          }
                        });
                      }
                      this.empNotes[i].files = dataAtt;
                    });
                  }
                }
                this.loading = false;
            });
        }
    })))).subscribe();

    this.api.getNoteSources()
      .subscribe(data => {
          this.sources = data;
          this.sources.unshift({
            noteSource: 'All',
            noteSourceId: 999
          });
          this.selectedSource = 999;
      });

    this.api.getClientNoteTags().subscribe( tags => {
      this.clientNoteTags = tags;
      this.notSelectedFilterTags = tags;
      this.filteredTags = this.searchTagsCtrl.valueChanges.pipe(
          debounceTime(250),
          startWith<string | INoteTag>(''),
          map(val => typeof val === 'string' ? val : val.tagName),
          map((tag: string) => (tag ? this._filter(tag) : this.clientNoteTags.slice()))
      );
    });

    this.newNote = {
      remarkId: null,
      description: null,
      employeeId: this.employeeId,
      securityLevel: 1,
      addedBy: this.userinfo.userId,
      addedDate: null,
      noteSource: 'General',
      noteSourceId: 1,
      showToggle: false,
      isArchived: false,
      activity: null,
      attachments: null,
      files: null,
      tags: null,
      employeeViewable: false,
      directSupervisorViewable: false,
      usersViewable: ''
    };

    this.searchTextFormCtrl.valueChanges
      .pipe(
          debounceTime(500),
          distinctUntilChanged(),
      )
      .subscribe(search => {
          this.searchText = search;
          this.updateDisplayNotes();
      });
  }

  checkCurrentUser(): Observable<UserInfo>{
    return iif(() => this.userinfo == null,
        this.accountService.getUserInfo().pipe(tap(u => {
            this.userinfo = u;
        })),
        of(this.userinfo));
  }

  displayArchivedNotes() {
    this.msgSvc.loading(true);
    this.forwardToGetArchived.next();
  }

  showArchive(): Observable<any> {
    return of(null).pipe(tap(() => {
      this.displayArchive = true;
      this.displayedEmpNotes = this.archivedEmpNotes;
      this.displayedNotesCount = this.displayedEmpNotes.length;
      this.totalNotesCount = this.archivedEmpNotes.length;
      this.msgSvc.loading(false);
    }))
  }

  sortArchivedNotes(calls: Observable<IEmployeeAttachment[]>[]): Observable<any> {
    return forkJoin(...calls).pipe(
      tap(result => {
      result.forEach((attachmentDetails, i) => {
        if (attachmentDetails.length > 1) {
          attachmentDetails.sort((j, k) => {
            if (j.name > k.name) {
              return 1;
            } else if (j.name < k.name) {
              return -1;
            }
          });
        }
        this.archivedEmpNotes[i].files = attachmentDetails;
      })
    }),
    tap(() => {
      this.archivedEmpNotes.sort(function(x, y) {
        return new Date(y.addedDate).getTime() - new Date(x.addedDate).getTime();
      });
    }))
  }

  updateDisplayNotes() {
    if (this.displayArchive == false) {
      this.displayedEmpNotes = this.empNotes;
    }
    else {
      this.displayedEmpNotes = this.archivedEmpNotes;
    }
    if (this.fromDate != null && this.toDate != null) {
      this.filterDatesBetween();
    } else {
      if (this.fromDate != null) {
        this.filterDatesFrom();
      }
      if (this.toDate != null) {
        this.filterDatesTo();
      }
    }
    if (this.selectedSource != 999) {
      this.sourceFilter();
    }
    if (this.searchText != null || this.searchText == '') {
      this.search();
    }
    if (this.selectedFilterTags.length > 0) {
      this.filterTags();
    }
    this.displayedNotesCount = this.displayedEmpNotes.length;
  }

  resetForm() {
    this.fromInput.value = '';
    this.toInput.value = '';
    this.selectedSource = 999;
    this.searchText = '';
    this.searchTextFormCtrl.reset();
  }

  filterDatesFrom() {
    const tempEmpNotes: IEmployeeNotes[] = new Array;
    for (let i = 0; i < this.displayedEmpNotes.length; i++) {
      if (moment(this.displayedEmpNotes[i].addedDate).isSameOrAfter(this.fromDate)) {
        tempEmpNotes.push(this.displayedEmpNotes[i]);
      }
    }
    this.displayedEmpNotes = tempEmpNotes;
  }

  filterDatesTo() {
    const tempEmpNotes: IEmployeeNotes[] = new Array;
    for (let i = 0; i < this.displayedEmpNotes.length; i++) {
      // when formatted this way sets the time to 12:00am for that day
      const date = moment(this.displayedEmpNotes[i].addedDate).format('MM/DD/YYYY');
      if (moment(date).isSameOrBefore(this.toDate)) {
        tempEmpNotes.push(this.displayedEmpNotes[i]);
      }
    }
    this.displayedEmpNotes = tempEmpNotes;
  }

  filterDatesBetween() {
    const tempEmpNotes: IEmployeeNotes[] = new Array;
    for (let i = 0; i < this.displayedEmpNotes.length; i++) {
      let date = moment(this.displayedEmpNotes[i].addedDate).format('MM/DD/YYYY')
      if (moment(date).isBetween(this.fromDate, this.toDate) || moment(date).isSame(this.fromDate, 'day') || moment(date).isSame(this.toDate, 'day')) {
        tempEmpNotes.push(this.displayedEmpNotes[i]);
      }
    }
    this.displayedEmpNotes = tempEmpNotes;
  }

  sourceFilter() {
    const tempEmpNotes: IEmployeeNotes[] = new Array();
    for(let i = 0; i < this.displayedEmpNotes.length; i++) {
      if (this.selectedSource ==  this.displayedEmpNotes[i].noteSourceId) {
        tempEmpNotes.push(this.displayedEmpNotes[i]);
      }
    }
    this.displayedEmpNotes = tempEmpNotes;
  }

  search() {
    const textArray = this.searchText.split(' ');
    const tempEmpNotes: IEmployeeNotes[] = new Array();
    for (let i = 0; i < this.displayedEmpNotes.length; i++) {
      let notMatch = false;
      for (let k = 0; k < textArray.length; k++ ){
        if (this.displayedEmpNotes[i].description.toLowerCase().indexOf(textArray[k].toLowerCase()) == -1) {
          // search note source title like 'General'
          if (this.displayedEmpNotes[i].noteSource.toLowerCase().indexOf(textArray[k].toLowerCase()) == -1) {
            // search attachment title
            if (this.displayedEmpNotes[i].files == undefined) this.displayedEmpNotes[i].files = [];
            if (this.displayedEmpNotes[i].files.length > 0) {
              for (let j = 0; j < this.displayedEmpNotes[i].files.length; j++) {

                if (this.displayedEmpNotes[i].files[j].name.toLowerCase().indexOf(textArray[k].toLowerCase()) == -1) {
            	  notMatch = true;
                } else {
                  notMatch = false;
            	  break;
                }
              }
            } else {
              notMatch = true;
              break;
            }
          }
        }
      }
      if (!notMatch) {
        tempEmpNotes.push(this.displayedEmpNotes[i]);
      }
    }
    this.displayedEmpNotes = tempEmpNotes;
  }

  sortNotes() {
     this.empNotes.sort(function(x, y) { return new Date(y.addedDate).getTime() - new Date(x.addedDate).getTime(); });
  }

  attachmentModal(remarkId) {
    this.currentAttachment = {
      resourceId: 0,
      clientId: this.userinfo.clientId,
      employeeId: this.employeeId,
      name: '',
      folderId: this.folders && this.folders.length ? this.folders[0].employeeFolderId : null,
      sourceType: 1,
      source: null,
      isViewableByEmployee: true,
      isNew: true,
      data: null
    };
    const done = this.modalSvc.openPageOtherThanEmployeeAttachments(this.currentAttachment, this.userinfo, this.folders, false, false).result.then(() => {
      for (let i = 0; i < this.empNotes.length; i++) {
        if (this.empNotes[i].remarkId == remarkId) {
          this.attachmentApi.getEmployeeAttachmentFolderList(this.employeeId, this.userinfo.clientId, true)
          .subscribe( dataFolders => {
            this.folders = dataFolders;

            let attachments = [];
            this.folders.forEach((x) => {
              attachments = attachments.concat(x.attachments);
            });

            attachments.sort(function(x, y) { return y.resourceId - x.resourceId; });
            const file = attachments[0].name + attachments[0].extension;

            this.empNotes[i].activity.unshift({
              activityDesc: "Attachment '" + file + "' added by you just now",
            });

            const changes: IEmpAttachmentChanges = {
              attachmentId: attachments[0].resourceId,
              remarkId: remarkId,
              change: 'a',
              fileName: file
            }

            this.api.registerEmployeeAttachmentChanges(changes).subscribe();

            this.empNotes[i].attachments.push(attachments[0]);

            if (this.empNotes[i].files == undefined)
              this.empNotes[i].files = [];

            this.empNotes[i].files = this.empNotes[i].files.concat(attachments[0]);
            if (this.empNotes[i].files.length > 1) {
              this.empNotes[i].files.sort((j, k) => {
                if (j.name > k.name) {
                  return 1;
                } else if (j.name < k.name) {
                  return -1;
                }
              });
            }
          });
          break;
        }
      }
      this.currentAttachment = {
        resourceId: 0,
        clientId: this.userinfo.clientId,
        employeeId: this.employeeId,
        name: '',
        folderId: this.folders[0].employeeFolderId,
        sourceType: 1,
        source: null,
        isViewableByEmployee: true,
        isNew: true
      };
    });
  }

  attachmentEditClosed(resource, remarkId) {
    const file = resource.attachment.name + resource.attachment.extension;
    let attachment = resource.attachment;
    let localempNotes= this.empNotes;
    if (resource.editType == 2 /* deleted*/) {

      // find the relevant note and the attachment
      for (let i = 0; i < this.empNotes.length; i++) {
        if (this.empNotes[i].remarkId == remarkId) {
          for (let k = 0; k < this.empNotes[i].attachments.length; k++) {
            if (this.empNotes[i].attachments[k].attachmentId === attachment.resourceId || this.empNotes[i].attachments[k].resourceId === attachment.resourceId) {
              const changes: IEmpAttachmentChanges = {
                attachmentId: attachment.resourceId || attachment.attachmentId,
                remarkId: remarkId,
                change: 'd',
                fileName: file
              }

              this.api.registerEmployeeAttachmentChanges(changes).subscribe( x => {

                // update note files and employee attachments
                this.empNotes[i].attachments.splice(k, 1);
                for (let l = 0; l < this.empNotes[i].files.length; l++) {
                  if (this.empNotes[i].files[l].resourceId === attachment.resourceId) {
                    this.empNotes[i].files.splice(l, 1);
                  }
                }

                this.empNotes[i].activity.unshift({
                  activityDesc: "Attachment '" + file + "' deleted by you just now",
                });

                return;
              });
              break;
            }
          }
          break;
        }
      }
    }

    else {
      const changes: IEmpAttachmentChanges = {
        attachmentId: attachment.resourceId,
        remarkId: remarkId,
        change: 'e',
        fileName: file
      }
      this.api.registerEmployeeAttachmentChanges(changes).subscribe();
      for (let i = 0; i < this.empNotes.length; i++) {
        if (this.empNotes[i].remarkId == remarkId) {
          this.empNotes[i].activity.unshift({
            activityDesc: "Attachment '" + file + "' edited by you just now",
          });
          for (let a = 0; a < this.empNotes[i].files.length; a++) {
            if (this.empNotes[i].files[a].resourceId == changes.attachmentId) {
              this.empNotes[i].files[a] = resource.attachment;
            }
          }
          return;
        }
      }
    }
  }

  closedShare(shareResult) {
    if (shareResult != null && shareResult != undefined) {
      for (let i = 0; i < this.empNotes.length; i++) {
        if (this.empNotes[i].remarkId == shareResult.remarkId || shareResult.remarkId == null) {
          this.empNotes[i].directSupervisorViewable = shareResult.directSupervisorViewable;
          // this.empNotes[i].employeeViewable = shareResult.employeeViewable;
          this.empNotes[i].usersViewable = shareResult.usersViewable;
          this.empNotes[i].securityLevel = shareResult.securityLevel;
          this.empNotes[i].activity.unshift({
            activityDesc: 'Note permissions changed by you just now',
          });
          break;
        }
      }
    }
  }

  closedSaved(modalResult) {
    let sources: INoteSource[];
    this.api.getNoteSources()
    .subscribe(data => {
      sources = data;
      if (null == modalResult || modalResult == undefined) { return; }
      const sourceText = sources.find(function(x) { return x.noteSourceId === modalResult.noteSourceId; });
      for (let i = 0; i < this.empNotes.length; i++) {
        if (this.empNotes[i].remarkId === modalResult.remarkId || modalResult.remarkId == null) {
          this.empNotes[i].description  = modalResult.remark.description;
          this.empNotes[i].noteSourceId = modalResult.noteSourceId;
          this.empNotes[i].noteSource = sourceText.noteSource;
          this.empNotes[i].showToggle = true;
          this.empNotes[i].activity.unshift({
            activityDesc: 'Note edited by you just now',
          });
          this.empNotes[i].isArchived = false;
          this.sortNotes();
          return;
        }
      }

      const newNote = {
        remarkId: modalResult.remarkId,
        employeeId: modalResult.employeeId,
        securityLevel: modalResult.securityLevel,
        description: modalResult.remark.description,
        noteSource:  sourceText.noteSource,
        noteSourceId: modalResult.noteSourceId,
        showToggle: true,
        addedBy: this.userinfo.userId,
        addedDate: modalResult.remark.addedDate,
        isArchived: false,
	      activity: [],
        attachments: [],
        files: [],
	      tags: [],
	      employeeViewable: false,
        directSupervisorViewable: false,
        usersViewable: '[]'
      };

      this.empNotes.unshift(newNote);
      this.empNotes[0].activity.push({
            activityDesc: 'Note created by you just now',
            isArchived:       false,
          });
      this.totalNotesCount = this.empNotes.length;
      this.updateDisplayNotes();
    });
  }

  archiveNote(remarkId) {
    this.confirmSvc.show(null, {
        bodyText: 'Are you sure you want to archive this note?',
        swapOkClose: true,
        actionButtonText: 'Archive',
        closeButtonText: 'Cancel'
    }).then(result => {
        this.msgSvc.loading(true);
        this.api.archiveEmployeeNote(remarkId)
            .subscribe(() => {
                this.msgSvc.setTemporaryMessage('Note archived successfully');
                _.remove(this.empNotes, f => f.remarkId === remarkId);
                _.remove(this.displayedEmpNotes, f => f.remarkId === remarkId);
                this.displayedNotesCount = this.displayedEmpNotes.length;
                this.totalNotesCount = this.empNotes.length;
            }
            ,
            (error: HttpErrorResponse) => {
                this.msgSvc.showWebApiException(error.error);
            });
    });
  }

  restoreNote(remarkId) {
    this.api.restoreEmployeeNote(remarkId)
      .subscribe(() => {
          this.msgSvc.setTemporaryMessage('Note restored successfully');

              for (let a in this.archivedEmpNotes) {
                if (this.archivedEmpNotes[a].remarkId == remarkId) {
                  this.archivedEmpNotes[a].isArchived = false;
                  this.archivedEmpNotes[a].activity.unshift({
                      activityDesc: 'Note restored by you just now',
                      isArchived:       false,
                  })
                  this.empNotes.push(this.archivedEmpNotes[a]);
                  break;
                }
              }
          _.remove(this.archivedEmpNotes, f => f.remarkId === remarkId);
          this.displayedNotesCount = this.displayedEmpNotes.length;
          this.totalNotesCount = this.archivedEmpNotes.length;
          this.sortNotes();
      }
      ,
      (error: HttpErrorResponse) => {
          this.msgSvc.showWebApiException(error.error);
      });
  }

  closedAssignTags(event, remarkId) {
    if (event != undefined) {
      let newTags: number[] = [];
      let oldTags: number[] = [];
      let removed: boolean = false;
      let added: boolean = false;

      for (let i = 0; i < this.empNotes.length; i++) {
        if (this.empNotes[i].remarkId == remarkId) {
          if (this.empNotes[i].tags.length > 0 && event.length > 0) {
            event.forEach( x => {
              newTags.push(x.tagID);
            });

            this.empNotes[i].tags.forEach( x => {
              oldTags.push(x.tagID);
            });

            newTags.sort();
            oldTags.sort();

            for (let n = 0; n < newTags.length; n++) {
              if (!oldTags.includes(newTags[n])) {
                added = true
              }
            }

            for (let n = 0; n < oldTags.length; n++) {
              if (!newTags.includes(oldTags[n])) {
                removed = true
              }
            }
          } else if (this.empNotes[i].tags.length < 1 && event.length > 0) {
            removed = false;
            added = true;
          } else if (this.empNotes[i].tags.length > 0 && event.length < 1) {
            removed = true;
            added = false;
          }

          if (removed && added) {
            this.empNotes[i].activity.unshift({activityDesc: 'Note tags changed by you just now'});
          } else if (added) {
            this.empNotes[i].activity.unshift({activityDesc: 'Note tagged by you just now'});
          } else if (removed) {
            this.empNotes[i].activity.unshift({activityDesc: 'Note tags removed by you just now'});
          }

          this.empNotes[i].tags = event;
          break;
        }
      }
    }
  }

  removeFilterTag(unselectedTag) {
    for (let t = 0; t < this.selectedFilterTags.length; t++) {
      if (this.selectedFilterTags[t].tagID == unselectedTag.tagID) {
        this.notSelectedFilterTags.push(this.selectedFilterTags[t]);
        this.selectedFilterTags.splice(t, 1);
        break;
      }
    }
    this.updateDisplayNotes();
  }

  addFilterTag(selectedTag) {
    for (let t = 0; t < this.notSelectedFilterTags.length; t++) {
      if (this.notSelectedFilterTags[t].tagID == selectedTag.option.value.tagID) {
        this.selectedFilterTags.push(this.notSelectedFilterTags[t]);
        this.notSelectedFilterTags.splice(t, 1);
        break;
      }
    }
    this.searchTagsElm.nativeElement.value = '';
    this.searchTagsCtrl.setValue('');
    this.updateDisplayNotes();
  }

  filterTags() {
    let selectedTags: number[] = [];
    const tempEmpNotes: IEmployeeNotes[] = new Array();

    this.selectedFilterTags.forEach( x => {
      selectedTags.push(x.tagID);
    });
    for (let n = 0; n < this.displayedEmpNotes.length; n++) {
      let match: boolean = false;
      for (let t = 0; t < this.displayedEmpNotes[n].tags.length; t++) {
        console.log(selectedTags.includes(this.displayedEmpNotes[n].tags[t].tagID));
        if (selectedTags.includes(this.displayedEmpNotes[n].tags[t].tagID)) {
          match = true;
          break;
        }
      }
      if (match) {
        tempEmpNotes.push(this.displayedEmpNotes[n]);
      }
    }
    this.displayedEmpNotes = tempEmpNotes;
  }
}

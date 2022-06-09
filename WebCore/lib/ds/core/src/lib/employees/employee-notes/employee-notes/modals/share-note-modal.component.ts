import { Component, Input, Output, EventEmitter, Inject, Optional } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EmployeeApiService } from '../../../shared/employee-api.service';
import {  } from '../../../shared/employee-notes-api.model';
import { tap } from 'rxjs/operators';
import { IShareSettings, IEmployeeNotes } from '../../../shared/employee-notes-api.model';
import { IContact } from '@ds/core/contacts';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { HttpErrorResponse } from '@angular/common/http';
import { forkJoin } from 'rxjs';
import { coerceBooleanProperty } from '@angular/cdk/coercion';

@Component({
  selector: 'share-note-modal',
  template: `<button type="button" mat-menu-item (click)="openShareModal()">Share</button>`,
})
export class ShareNoteTriggerComponent {
  constructor(public dialog: MatDialog) {}
  private _isEssMode = false;
  @Input()
  get isEssMode(): boolean {
      return this._isEssMode;
  }
  set isEssMode(value: boolean) {
      this._isEssMode = coerceBooleanProperty(value);
  }
  @Input() selectedNote: IEmployeeNotes;
  @Output() savedSettings = new EventEmitter();
  openShareModal(): void {
    this.dialog.open(ShareNoteModalComponent, {
        width: '550px',
        data: { note:this.selectedNote, isEss : this.isEssMode }
    }).afterClosed().pipe(tap(x => this.savedSettings.emit(x))).subscribe();
  }
}

@Component({
  selector: 'mat-dialog-demo',
  templateUrl: './share-note-modal.component.html',
  providers: [ShareNoteTriggerComponent],
})
export class ShareNoteModalComponent {
  remarkId:            number;
  isDirectSupervisor:  boolean;
  // isEmployee:          boolean;
  shareSettings:       IShareSettings;
  accessType:          number;
  supervisors:         IContact[];
  selectedSupervisors: IContact[] = [];
  otherChecked =       false;
  employeeName =       '';
  userIds:             number[];
  data:                IEmployeeNotes;

  constructor(
      public dialogRef: MatDialogRef<ShareNoteModalComponent>,
      private api: EmployeeApiService,
      private msgSvc: DsMsgService,
      @Optional() @Inject(MAT_DIALOG_DATA) public dataInput: any
  ){}

  ngOnInit() {
    this.data = this.dataInput.note;
    this.remarkId = this.data.remarkId;
    this.isDirectSupervisor =  this.data.directSupervisorViewable;
    // this.isEmployee = this.data.employeeViewable;
    this.selectedSupervisors = [];
    this.accessType = this.data.securityLevel;

    forkJoin(this.api.getEmployeeProfileCard(this.data.employeeId),
      this.api.getSupervisorsForEmployee(this.data.employeeId, true))
      .subscribe(([data1,data2]) => {
        this.supervisors = data2;
        if(!!data1 && !this.dataInput.isEss &&
          this.supervisors.map(x=>x.employeeId).indexOf(data1.employeeId) == -1){
          this.supervisors.push(data1);
          this.supervisors.sort((a,b) => (a.firstName > b.firstName) ? 1 :
            ( (b.firstName > a.firstName) ? -1 :
              (a.lastName > b.lastName ? 1 : -1) ));
        }
        this.userIds = JSON.parse(this.data.usersViewable || '[]');
        for (let i = 0; i < this.userIds.length; i++) {
          for (let k = 0; k < this.supervisors.length; k++) {
            if (this.userIds[i] == this.supervisors[k].userId) {
              this.selectedSupervisors.push(this.supervisors[k]);
            }
          }
        }
        if (this.selectedSupervisors.length > 0) {
          this.otherChecked = true;
        }
      });

    //this.api.getEmployeeName(this.data.employeeId)
    //  .subscribe(data => {
    //      this.employeeName = data.firstName + ' ' + data.lastName;
    //  });
  }

  close() {
    this.dialogRef.close();
  }

  saveShareSettings() {
    const userIds = [];
    let  userIdsString = '[]';


    if (this.otherChecked == true && this.accessType == 3) {
      for (let i = 0; i < this.selectedSupervisors.length; i++) {
        userIds.push(this.selectedSupervisors[i].userId);
      }
      userIdsString = JSON.stringify(userIds);
    }

    if (this.accessType != 3) {
      this.isDirectSupervisor = false;
      // this.isEmployee = false;
    }

    this.shareSettings = {
      remarkId:                 this.remarkId,
      securityLevel:            this.accessType,
      directSupervisorViewable: this.isDirectSupervisor || false,
      employeeViewable:         false, // this.isEmployee || false,
      usersViewable:            userIdsString
    };
    this.api.saveShareSettings(this.shareSettings)
    .pipe(tap(x => this.dialogRef.close(x)))
      .subscribe( () => {
        this.msgSvc.setTemporaryMessage('Share settings saved successfully');
      },
      (error: HttpErrorResponse) => {
          this.msgSvc.showWebApiException(error.error);
      });
  }
}

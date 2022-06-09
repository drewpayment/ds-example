import { Component, Input, Output, EventEmitter, OnInit, Optional, Inject, ViewChild, ElementRef } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EmployeeApiService } from '../../../shared/employee-api.service';
import { IEmployeeNotes, INoteTag, IAssignTags } from '../../../shared/employee-notes-api.model';
import { tap, debounceTime, startWith, map } from 'rxjs/operators';
import { FormGroup, FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

@Component({
  selector: 'assign-tags-modal',
  template: `
  <button type="button" mat-menu-item (click)="openAssignTagsModal()">Assign Tags</button>
  `,
})
export class AssignTagsTriggerComponent {
  @Input() selectedNote: IEmployeeNotes;
  @Input() remarkId: number;
  @Output() assignedTags = new EventEmitter();
  constructor(public dialog: MatDialog) {}
  description: String;
  noteSourceId: number;

  openAssignTagsModal(): void {
    const data: IAssignTags =  {
      tags: this.selectedNote.tags,
      remarkId: this.remarkId
    };
    this.dialog.open(AssignTagsModalComponent, {
        width: '500px',
        data: data,
    }).afterClosed().pipe(tap(x => this.assignedTags.emit(x))).subscribe();
  }
}

@Component({
  selector: 'mat-dialog-demo2',
  templateUrl: './assign-tags-modal.component.html',
  providers: [AssignTagsTriggerComponent],
})
export class AssignTagsModalComponent implements OnInit {
  filteredClientNoteTags: INoteTag[] = [];
  selectedNoteTags: INoteTag[] = [];
  notSelectedNoteTags: INoteTag[];
  clientNoteTags = new FormControl();
  noteTagChanges: INoteTag[] = [];
  remarkId: number;
  allClientTags: INoteTag[];

  tagsFormGroup: FormGroup;
  @ViewChild('noteTagsInput', {static: false}) noteTagsInput: ElementRef<HTMLInputElement>

  noteTagsCtrl = new FormControl('');
  filteredNoteTags: Observable<any>;

  constructor(
    public dialogRef: MatDialogRef<AssignTagsModalComponent>,
    private api: EmployeeApiService,
    private msgSvc: DsMsgService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: IAssignTags
  ) {

   }

  private _filter(value: string): INoteTag[] {
    return this.notSelectedNoteTags.filter(tag => tag.tagName.toLowerCase().indexOf(value.toLowerCase()) === 0);
  }

  ngOnInit() {
    if (this.data.tags != null) {
      for (let i = 0; i < this.data.tags.length; i ++) {
        this.selectedNoteTags.push(this.data.tags[i]);
      }
    }

    this.remarkId = this.data.remarkId;

    this.api.getClientNoteTags().subscribe( tags => {
      this.allClientTags = tags;
      this.selectedNoteTags.forEach( selected => {
        for (let t = 0; t < tags.length; t++) {
          if (selected.tagID == tags[t].tagID) {
            tags.splice(t, 1);
            break;
          }
        }
      });

      this.notSelectedNoteTags = tags;

      this.filteredNoteTags = this.noteTagsCtrl.valueChanges.pipe(
        debounceTime(250),
        startWith<string | INoteTag>(''),
        map(val => typeof val === 'string' ? val : val.tagName),
        map((tag: string) => (tag ? this._filter(tag) : this.filteredClientNoteTags.slice())));
    });

    this.tagsFormGroup = new FormGroup({
      clientNoteTags: this.noteTagsCtrl
    });
  }

  addNewTag() {
    const newTag = this.noteTagsInput.nativeElement.value.trim();

    if (newTag == "") {
      return;
    }

    for (let k = 0; k < this.noteTagChanges.length; k++) {
      if (this.noteTagChanges[k].tagName.toLocaleLowerCase() == newTag.toLocaleLowerCase()) {
        return;
      }
    }

    for (let i = 0; i < this.allClientTags.length; i++) {
      if (this.allClientTags[i].tagName.toLocaleLowerCase() == newTag.toLocaleLowerCase()) {
        this.selectedNoteTags.push(this.allClientTags[i]);
        this.noteTagChanges.push(this.allClientTags[i]);

        this.noteTagsInput.nativeElement.value = '';
        this.noteTagsCtrl.setValue('');
        return;
      }
    }

    let noteTag: INoteTag = {
      noteTagId: null,
      tagID: null,
      tagName: newTag,
      change: null
    };

    this.selectedNoteTags.push(noteTag);
    this.noteTagChanges.push(noteTag);

    this.noteTagsInput.nativeElement.value = '';
    this.noteTagsCtrl.setValue('');
  }

  addNoteTag(selectedTag) {
    for (let t = 0; t < this.notSelectedNoteTags.length; t++) {
      if (this.notSelectedNoteTags[t].tagID == selectedTag.option.value.tagID) {
        if (this.notSelectedNoteTags[t].change == 'r') {
          for (let k = 0; k < this.noteTagChanges.length; k++) {
            if (this.noteTagChanges[k].tagID == selectedTag.option.value.tagID)
            this.noteTagChanges.splice(k, 1);
          }
        } else {
          this.notSelectedNoteTags[t].change = 'a';
          this.noteTagChanges.push(this.notSelectedNoteTags[t]);
        }

        this.selectedNoteTags.push(this.notSelectedNoteTags[t]);
        this.notSelectedNoteTags.splice(t, 1);
        break;
      }
    }
    this.noteTagsInput.nativeElement.value = '';
    this.noteTagsCtrl.setValue('');
  }

  removeNoteTag(unselectedTag) {
    if (unselectedTag.tagID == null){
      for (let i = 0; i < this.selectedNoteTags.length; i++) {
        if (this.selectedNoteTags[i].tagName == unselectedTag.tagName) {
          for (let k = 0; k < this.noteTagChanges.length; k++) {
            if (this.noteTagChanges[k].tagName == unselectedTag.tagName && this.noteTagChanges[k].tagID == null)
            this.noteTagChanges.splice(k, 1);
            break;
          }
          this.selectedNoteTags.splice(i, 1);
          break;
        }
      }
    } else {
      for (let t = 0; t < this.selectedNoteTags.length; t++) {
        if (this.selectedNoteTags[t].tagID == unselectedTag.tagID) {
          if (this.selectedNoteTags[t].change == 'a') {
            for (let k = 0; k < this.noteTagChanges.length; k++) {
              if (this.noteTagChanges[k].tagID == unselectedTag.tagID)
              this.noteTagChanges.splice(k, 1);
            }
          } else {
            this.selectedNoteTags[t].change = 'r';
            this.noteTagChanges.push(this.selectedNoteTags[t]);
          }

          this.notSelectedNoteTags.push(this.selectedNoteTags[t]);
          this.selectedNoteTags.splice(t, 1);
          break;
        }
      }
    }
  }

  close() {
    this.dialogRef.close();
  }

  saveTags() {
    let newTags: string[] = [];
    this.noteTagChanges.forEach( x => {
      if (x.tagID == null) {
        newTags.push(x.tagName);
      }
    });

    if (this.noteTagChanges.length > 0) {
      this.api.createClientNoteTag(newTags).subscribe( clientTags => {
          for (let t = 0; t < this.noteTagChanges.length; t++) {
            if (this.noteTagChanges[t].tagID == null) {
              for (let i = 0; i < clientTags.length; i++) {
                if (clientTags[i].tagName == this.noteTagChanges[t].tagName) {
                  this.noteTagChanges[t] = clientTags[i];
                  break;
                }
              }
            }
          }
          this.api.registerEmployeeNoteTagsChanges(this.noteTagChanges, this.remarkId)
          .pipe(tap(x => this.dialogRef.close(x)))
            .subscribe();
      });
    }
  }
}

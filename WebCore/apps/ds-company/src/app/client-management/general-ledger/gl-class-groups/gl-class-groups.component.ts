import { Component, OnInit } from '@angular/core';
import { CompanyManagementService } from '../../../services/company-management.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { MOMENT_FORMATS } from '@ds/core/shared/moment-formats';
import * as moment from 'moment';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from '@ds/core/shared';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';
import { map, tap } from 'rxjs/operators';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { ClientGLClassGroup } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';


@Component({
  selector: 'ds-gl-class-groups',
  templateUrl: './gl-class-groups.component.html',
  styleUrls: ['./gl-class-groups.component.css']
})
export class GlClassGroupsComponent implements OnInit {
  loaded: boolean = false;
  user: UserInfo;
  isSubmitted = false;
  form: FormGroup;
  showDelete: boolean = false;
  clientGLClassGroups: ClientGLClassGroup[];
  isSaving: boolean = false;
  selectedClassGroup: ClientGLClassGroup;
  confirmed: any;

  constructor(
    private fb: FormBuilder,
    private ngxMsgSvc: NgxMessageService,
    private CompanyManagementService: CompanyManagementService,
    private accountService: AccountService,
    private confirmDialog: ConfirmDialogService,
  ) { }

  ngOnInit() {
    this.showDelete = false;
    this.accountService.getUserInfo()
      .pipe(
        tap(u => this.user = u)).subscribe(() => {
          this.getClientGLClassGroups();
          this.initForm();
          this.loaded = true;
        });
  }

  getClientGLClassGroups(){
    this.CompanyManagementService.GetClientGLClassGroups().subscribe((data:any) => {
      this.clientGLClassGroups = data;
    });
  }

  initForm() {
    this.form = this.fb.group({
      classGroup: [0, Validators.required],
      name: [null, Validators.required],
      code: [null, Validators.required],
    });
    this.showDelete = false;
  }

  changeClassGroup() {
    this.isSubmitted = false;
    this.selectedClassGroup = this.clientGLClassGroups.find(s => s.clientGLClassGroupId == this.form.controls['classGroup'].value);

    if ( this.selectedClassGroup ) {
        this.showDelete = true;
        this.form.reset({
          classGroup: this.selectedClassGroup.clientGLClassGroupId,
          name: this.selectedClassGroup.classGroupDesc,
          code: this.selectedClassGroup.classGroupCode,
        });
    } else {
        this.initForm();
    }
  }

  save() {
    this.isSaving = true;
    this.isSubmitted = true;
    this.form.markAllAsTouched();

    if ( this.form.errors ) {
      this.isSaving = false;
      return;
    }

    //Check if name + code exists
    if ( this.checkDuplicate() ) return;

    //Set model for save
    this.selectedClassGroup = {
      clientGLClassGroupId: this.form.value.classGroup,
      clientId: this.user.clientId,
      classGroupCode: this.form.value.code,
      classGroupDesc: this.form.value.name,
      modified: moment(new Date).format(MOMENT_FORMATS.API),
      modifiedBy: this.user.userId,
    }

    //Save
    this.CompanyManagementService.SaveClientGLClassGroup(this.selectedClassGroup).subscribe((response: ClientGLClassGroup) => {
      this.ngxMsgSvc.setSuccessMessage("GL Class Group Saved Successfully");
      this.isSubmitted = false;
      this.isSaving = false;

      if ( response ) {
        let index = this.clientGLClassGroups.map(s => s.clientGLClassGroupId).indexOf(response.clientGLClassGroupId);
        if ( index == -1 ) {
          this.clientGLClassGroups.push(response); //insert
        }
        else{
          this.clientGLClassGroups[index] = response; //update
        }
        this.form.controls['classGroup'].patchValue(0); //clear form to allow user to insert another group
        this.changeClassGroup();
      }
    }, (error: HttpErrorResponse) => {
      this.isSaving = false;
      this.ngxMsgSvc.setErrorResponse(error);
    });
  }

  deleteClientGLClassGroup(confirmed) {
    if ( confirmed ) {
      if ( this.selectedClassGroup ) {
        this.selectedClassGroup.modified = moment(new Date).format(MOMENT_FORMATS.API);
        this.selectedClassGroup.modifiedBy = this.user.userId;
      }
      this.CompanyManagementService.DeleteClientGLClassGroup(this.selectedClassGroup).subscribe((res) => {
        this.ngxMsgSvc.setSuccessMessage("GL Class Group Deleted Successfully");
        this.clientGLClassGroups = this.clientGLClassGroups.filter((clientGLClassGroup) => clientGLClassGroup.clientGLClassGroupId != this.selectedClassGroup.clientGLClassGroupId); //remove from class groups array
        this.initForm(); //clear form
      },
      (error: HttpErrorResponse) => {
        this.ngxMsgSvc.setErrorResponse(error);
      });
    }
  }

  delete() {
      const options = {
          title: 'Are you sure you want to delete this Class Group?',
          confirm: "Delete"
      };
      this.confirmDialog.open(options);
      this.confirmDialog.confirmed().pipe(
          map(confirmation => this.confirmed = confirmation)
      ).subscribe(() => {
          this.deleteClientGLClassGroup(this.confirmed);
      });
  }

  checkDuplicate():boolean{
    let hasDuplicate = false;
    let duplicateNames = this.clientGLClassGroups.filter(cg => cg.classGroupDesc === this.form.value.name);
    if ( duplicateNames.length > 0 ) {
      duplicateNames.forEach((cg) => {
        //If duplicate name and code exist, and is not the currently selected Class Group. Throw error
        if (cg.classGroupCode == this.form.value.code && cg.clientGLClassGroupId != this.form.value.classGroup) {
          this.ngxMsgSvc.setErrorMessage("The specified Class Group already exists. Changes were not saved.");
          this.isSaving = false;
          hasDuplicate = true;
        };
      });
    }
    return hasDuplicate;
  }

}

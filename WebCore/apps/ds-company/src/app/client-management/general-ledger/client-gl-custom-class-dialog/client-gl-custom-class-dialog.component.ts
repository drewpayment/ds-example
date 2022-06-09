import { Component, OnInit } from '@angular/core';
import { UserInfo } from '@ds/core/shared';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { forkJoin } from 'rxjs';
import { ClientDivisionDto } from '@ajs/ds-external-api/models';
import * as _ from "lodash"
import { HttpErrorResponse } from '@angular/common/http';
import { AccountService } from '@ds/core/account.service';
import { GeneralLedgerService } from '../../../services/general-ledger.service';
import { ClientCostCenter, ClientDepartment, ClientGLCustomClass, ClientGroupDto } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-client-gl-custom-class-dialog',
  templateUrl: './client-gl-custom-class-dialog.component.html',
  styleUrls: ['./client-gl-custom-class-dialog.component.scss']
})
export class ClientGlCustomClassDialogComponent implements OnInit {
  user: UserInfo;
  isLoading: boolean = true;
  formSubmitted: boolean = false;
  form: FormGroup = this.createForm();
  customClasses: ClientGLCustomClass[];
  customClass: ClientGLCustomClass;
  costCenters: ClientCostCenter[];
  groups: ClientGroupDto[];
  divisions: ClientDivisionDto[];
  departments: ClientDepartment[];
  divisionSelected = false;

  constructor(
    private api: GeneralLedgerService,
    private accountAPI: AccountService,
    private msg: NgxMessageService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ClientGlCustomClassDialogComponent>) { }

  ngOnInit() {
    this.form.get('divisions').valueChanges.subscribe(val => this.divisionChanged(val));
    this.form.get('customClasses').valueChanges.subscribe(val => this.classChanged(val));

    this.accountAPI.getUserInfo().subscribe((u: UserInfo) => {
      this.user = u;
      forkJoin(
        this.api.getClientGLCustomClass(),
        this.api.getClientCostCenters(this.user.clientId),
        this.api.getClientGroups(this.user.clientId),
        this.api.getClientDivisionsWithDepartments()
      ).subscribe(([customClass, costCenters, groups, divisions]) => {
        this.customClasses = customClass;
        this.costCenters   = costCenters;
        this.groups        = groups;
        this.divisions     = divisions;

        this.isLoading = false;
      });
    });
  }

  createForm() {
    return this.fb.group({
      customClasses : [''],
      description   : [''],
      divisions     : ['', Validators.required],
      groups        : ['', Validators.required],
      departments   : ['', Validators.required],
      costCenters   : ['', Validators.required]
    });
  }

  classChanged(classId: number) {

    const clientGLCustomClass = _.find(this.customClasses, (c) => {
      return c.clientGLCustomClassId == classId;
    });

    if(clientGLCustomClass != null) {
      this.form.patchValue({
        description   : clientGLCustomClass.description,
        divisions     : clientGLCustomClass.clientDivisionId,
        groups        : clientGLCustomClass.clientGroupId,
        departments   : clientGLCustomClass.clientDepartmentId,
        costCenters   : clientGLCustomClass.clientCostCenterId
      });

    } else {
      this.form.reset();
    }

  }

  divisionChanged(divisionId: number) {

    const division = this.divisions.find(d => d.clientDivisionId == divisionId);

    if(division) {
      this.departments = division.departments;
      this.divisionSelected = true;
      this.formSubmitted = false;
    } else {
      this.divisionSelected = false;
    }

  }

  save() {
    this.formSubmitted = true;

    if(this.form.valid) {
      this.prepareClass();
      this.api.saveClientGLCustomClass(this.customClass).subscribe((data) => {
        this.msg.setSuccessMessage("Custom Class saved successfully.");
        this.dialogRef.close(null);
      }, (error: HttpErrorResponse) => {
        this.msg.setErrorMessage(error.message)
      });
    }

  }

  prepareClass() {

    const costCenter = _.find(this.costCenters, (cc) => {
      return cc.clientCostCenterId == this.form.get('costCenters').value;
    });

    const group = _.find(this.groups, (cc) => {
      return cc.clientGroupId == this.form.get('groups').value;
    });

    const department = _.find(this.departments, (cc) => {
      return cc.clientDepartmentId == this.form.get('departments').value;
    });

    this.customClass = {
      clientId              : this.user.clientId,
      clientGLCustomClassId : (this.form.get('customClasses').value == '') ? 0 : this.form.get('customClasses').value,
      clientDivisionId      : this.form.get('divisions').value,
      clientGroupId         : this.form.get('groups').value,
      clientDepartmentId    : this.form.get('departments').value,
      clientCostCenterId    : this.form.get('costCenters').value,
      description           : group.code + department.code + costCenter.code
    }
  }

  cancel() {
    this.dialogRef.close(null);
  }

}

import { Component, OnInit, ViewChild } from '@angular/core';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { ITermsAndConditionsVersion } from '@ds/core/shared/terms-and-conditions-version.model';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { skip } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from '@ds/core/shared';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'ds-terms-and-conditions',
  templateUrl: './terms-and-conditions.component.html',
  styleUrls: ['./terms-and-conditions.component.scss']
})
export class TermsAndConditionsComponent implements OnInit {

  user: UserInfo;
  termsNConditions: ITermsAndConditionsVersion[];
  isLoading = true;
  displayedColumns: string[] = ['Filename', 'Effective Date', 'End Date'];
  matList: any;
  paginator: MatPaginator;
  fileToUpload: File = null;
  isSaving = false;
  formSubmitted: boolean = false;


  form: FormGroup = this.createForm()
  @ViewChild(MatPaginator, {static: false}) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.setDataSourceAttr();
  }

  constructor(
    private accountService: AccountService,
    private msg: DsMsgService,
    private fb: FormBuilder

  ) { }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe((u) => {
      this.user = u;
      this.accountService.fetchTermsAndConditionsVersions();
    });

    this.accountService.tncItems$.pipe(skip(1)).subscribe((tnc) => {
      this.termsNConditions = tnc;
      this.matList = new MatTableDataSource<ITermsAndConditionsVersion>(tnc);
      this.isLoading = false;
    });
  }

  createForm() {
    return this.fb.group({
      fileInput: [null, Validators.required]
    });
  }

  setDataSourceAttr() {
    if (this.matList) this.matList.paginator = this.paginator;
  }

  downloadTermsNConditions(filePath, filename) {
    this.accountService.getTermsAndConditionsVersionDownload(filePath, filename).subscribe((res) => { });
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
  }

  upload() {
    this.formSubmitted = true;
    if (this.form.valid) {
      this.accountService.uploadTermsAndConditionVersion(this.fileToUpload, this.matList.data.length).subscribe((data) => {
        this.accountService.fetchTermsAndConditionsVersions();
        this.msg.setTemporarySuccessMessage("Service Agreement uploaded successfully.", 5000);
        this.isSaving = false;
      }, (error : HttpErrorResponse) => {
        this.msg.setTemporaryMessage(error.message, MessageTypes.error, 5000);
        this.isSaving = false;
      });
    } else {
      this.isSaving = false;
    }
  }

}

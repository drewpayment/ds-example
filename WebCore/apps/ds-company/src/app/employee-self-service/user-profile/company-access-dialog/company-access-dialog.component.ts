import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserInfo } from '@ds/core/shared';
import { UserClientAccess } from '@models/user-client-access.model';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MessageService } from '../../../services/message.service';
import { UserProfileService } from '../user-profile.service';

interface DialogData {
  access: UserClientAccess[];
}

@Component({
  selector: 'ds-company-access-dialog',
  templateUrl: './company-access-dialog.component.html',
  styleUrls: ['./company-access-dialog.component.scss'],
})
export class CompanyAccessDialogComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject();
  form: FormGroup = this.createForm();

  get companies(): FormArray {
    return this.form.get('companies') as FormArray;
  }
  oldCompanies: UserClientAccess[] = [];

  constructor(
    private dialogRef: MatDialogRef<CompanyAccessDialogComponent, any>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private fb: FormBuilder,
    private msg: MessageService,
    private service: UserProfileService,
  ) {}

  ngOnInit(): void {
    this.form.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(() => {
      const companies = this.companies.value as UserClientAccess[];
      const hasAccessCount = companies.filter(c => c.hasAccess).length;

      if (hasAccessCount < 1) {
        this.msg.setWarningMessage('Must have at least one company.', 5000);
      }
    });

    this.oldCompanies = [...this.data.access];
    this.patchForm(this.data.access);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
  }

  save() {
    if (this.form.invalid || !this.form.value ||
      !this.form.value.companies || !this.form.value.companies.length) return;

    const userId = this.form.value.companies[0].userId;

    this.service.saveCompanyAdminAccess(userId, this.form.value.companies)
      .subscribe(access => {
        this.dialogRef.close(access);
      });
  }

  cancel() {
    this.dialogRef.close();
  }

  private createForm(): FormGroup {
    return this.fb.group({
      companies: this.fb.array([]),
    });
  }

  private patchForm(companies: UserClientAccess[]) {
    if (!companies) return;

    companies.forEach((c) =>
      this.companies.push(
        this.fb.group({
          clientId: c.clientId,
          clientName: c.clientName,
          hasAccess: c.hasAccess,
          isBenefitAdmin: c.isBenefitAdmin,
          isClientAdmin: c.isClientAdmin,
          userId: c.userId,
        })
      )
    );
  }
}

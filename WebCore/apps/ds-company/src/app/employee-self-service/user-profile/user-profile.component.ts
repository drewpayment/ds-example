import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { AccountService } from '@ds/core/account.service';
import { UserInfo, UserType } from '@ds/core/shared';
import { UserProfile, UserSupervisorAccessInfo } from '@models';
import { Subject } from 'rxjs';
import { filter, take, takeUntil, tap } from 'rxjs/operators';
import { UserProfileService } from './user-profile.service';

@Component({
  selector: 'ds-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss'],
})
export class UserProfileComponent implements OnInit, OnDestroy {

  user: UserInfo;
  userSupAccess: UserSupervisorAccessInfo;
  userSupCanEnableEmps = true;
  destroy$ = new Subject();
  includeTerminatedSearch = this.fb.control(false);
  isAddUserView = false;
  lastUserProfile: UserProfile;

  constructor(
    private service: UserProfileService,
    private fb: FormBuilder,
  ) { }

  ngOnInit(): void {
    this.service.lastUserProfile$.pipe(takeUntil(this.destroy$)).subscribe(prof => this.lastUserProfile = prof);

    this.service.user$.pipe(takeUntil(this.destroy$), filter(u => !!u))
      .subscribe(user => {
        this.user = user;

        if (this.user.userTypeId === UserType.supervisor) {
          this.service.userSupervisorAccess$.pipe(takeUntil(this.destroy$))
            .subscribe(access => this.userSupAccess = access);

          this.service.userSupCanEnableEmps$.pipe(takeUntil(this.destroy$))
            .subscribe(canEnable => this.userSupCanEnableEmps = canEnable);
        }
      });

    this.service.isAddUserview$.pipe(takeUntil(this.destroy$))
      .subscribe(isAdd => {
        this.isAddUserView = isAdd;
      });

    this.service.includeTerminatedSearch$.pipe(take(1))
      .subscribe(includeTerm =>
        this.includeTerminatedSearch.setValue(includeTerm, { emitEvent: false, emitViewToModelChange: false }));

    this.includeTerminatedSearch.valueChanges.subscribe(() =>
      this.service.setIncludeTerminated(this.includeTerminatedSearch.value));
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.service.destroy();
  }

  addUser() {
    this.service.setAddUserViewState(true);
  }

  cancel() {
    this.service.cancelForm();
  }

  canResetPws(): boolean {
    return (this.user && this.user.isRole(UserType.systemAdmin, UserType.companyAdmin)) ||
      (this.user && this.user.userTypeId === UserType.supervisor &&
      this.userSupAccess && this.userSupAccess.canSendPasswords);
  }

  private checkUserTypePermissions(user: UserInfo) {
    const userType = user.userTypeId;

    // if (userType == UserType.systemAdmin && user.) {

    // }
  }

}

import { Component, OnInit, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { IEmergencyContact } from '@ds/employees/profile/shared/emergency-contact.model';
import { UserInfo } from '@ds/core/shared';
import { EmergencyContactsApiService } from '../shared/emergency-contacts-api.service';
import { Observable, Subscription } from 'rxjs';
import { tap, switchMap, skipWhile, distinctUntilChanged, catchError, map } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AccountService } from '@ds/core/account.service';

@Component({
  selector: 'ds-emergency-contacts',
  templateUrl: './emergency-contacts.component.html',
  styleUrls: ['./emergency-contacts.component.scss']
})
export class EmergencyContactsComponent implements OnInit {
  currentUser: UserInfo;
  emergencyContacts: IEmergencyContact[];
  employeeId:         number;
  count = 1;
  isLoading$: Observable<boolean>;

  constructor(
    private api: EmergencyContactsApiService,
    private acctService: AccountService,
    private sb: MatSnackBar,
  ) { }

    ngOnInit() {
        this.isLoading$ = this.acctService.getUserInfo()
            .pipe(
                tap(user => this.currentUser = user),
                switchMap(_ => {
                    return this.api.getEmergencyContactsByEmployeeId(this.currentUser.employeeId);
                }),
                map(result => {
                    this.emergencyContacts = result;
                    return false;
                }, err => {
                    //if (err?.error?.errors?.length) {
                    if (err.error && err.error.errors && err.error.errors.length) {
                        this.sb.open(err.error.errors[0].msg, 'dismiss', { duration: 5000 });
                    } else {
                        this.sb.open(err.message, 'dismiss', { duration: 5000 });
                    }
                    return false;
                })
            );
    }
}

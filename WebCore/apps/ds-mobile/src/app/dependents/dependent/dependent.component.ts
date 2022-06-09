import { Component, OnInit } from '@angular/core';
import { EmployeeDependentsApiService } from '../../shared/employee-dependents-api.service';
import {  UserInfo } from '@ds/core/shared';
import { IEmployeeDependent } from '@ds/employees/profile/shared/employee-dependent.model';
import { Observable } from 'rxjs';
import { tap, switchMap, skipWhile, distinctUntilChanged, catchError, map } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AccountService } from '@ds/core/account.service';

@Component({
  selector: 'ds-dependent',
  templateUrl: './dependent.component.html',
  styleUrls: ['./dependent.component.scss']
})
export class DependentComponent implements OnInit {
  currentUser: UserInfo;
  employeeDependents  :   IEmployeeDependent[];
  isLoading$: Observable<boolean>;
  constructor(
    private employeeDependentsApiService: EmployeeDependentsApiService,
    private acctService: AccountService,
    private sb: MatSnackBar
  ) { }

    ngOnInit() {

        this.isLoading$ = this.acctService.getUserInfo()
            .pipe(
                tap(user => this.currentUser = user),
                switchMap(_ => {
                    return this.employeeDependentsApiService.getDependents(this.currentUser.employeeId);
                }),
                map(result => {
                    this.employeeDependents = result;
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

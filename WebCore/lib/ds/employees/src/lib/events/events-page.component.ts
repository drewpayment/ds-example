import { Component, OnInit } from '@angular/core';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from "@ds/core/account.service";
import {  tap } from "rxjs/operators";
import { Router } from '@angular/router';

@Component({
  selector: 'ds-events-page',
  templateUrl: './events-page.component.html'
})
export class EventsPageComponent implements OnInit {
  userinfo: UserInfo;
  clientId: number;
  employeeId: number;
  isLoading: boolean = true;
  isHRBlocked: boolean = false;

  constructor(private accountService: AccountService,private router: Router,) { }

  ngOnInit() {
    this.accountService.getUserInfo().pipe(
      tap(
        (user : UserInfo) => {
          this.userinfo = user;
          this.employeeId = user.lastEmployeeId || user.employeeId;
          this.isLoading = false;
          this.isHRBlocked = user.isHrBlocked;
        }
      ))
      .subscribe(x=>{
        if(this.isHRBlocked) 
        this.router.navigate(['error'],  { queryParams:  {showButton: false, showHelpMessage: false, message:'You do not have access to this information.'},  
          queryParamsHandling: "merge" });
      });
  }

}
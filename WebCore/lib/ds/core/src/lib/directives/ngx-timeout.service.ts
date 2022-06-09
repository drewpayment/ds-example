import { Injectable } from '@angular/core';
import {
  Directive,
  HostListener,
  Inject,
  ElementRef,
  OnDestroy,
} from "@angular/core";
import { AccountService } from "../account.service";
import { APP_CONFIG, AppConfig } from "../app-config/app-config";
import { tap, takeUntil, filter, switchMap } from "rxjs/operators";
import { Subject } from "rxjs";
import { DOCUMENT } from "@angular/common";
import { ConfigUrlType } from "../shared/config-url.model";
import { WINDOW } from "../core.tokens";
import { UserInfo, UrlParts } from "../shared";
import { State } from "../app-config/ngrx-store/reducers/menu.reducer";
import { InitiateLogout } from "../app-config/ngrx-store/actions/menu.actions";

@Injectable({
  providedIn: 'root',
})
export class NgxTimeoutService {
  destroy$ = new Subject();
  timeoutMinutes: number;
  idleTimer: NodeJS.Timer;
  logoutLink: string;
  user: UserInfo;

  constructor(
    private accountService: AccountService,
    @Inject(APP_CONFIG) private config: AppConfig,
    @Inject(DOCUMENT) private document: Document
  ) {

  }

  listen() {
    this.accountService
      .getUserInfo()
      .pipe(
        takeUntil(this.destroy$),
        tap((user) => (this.user = user)),
        switchMap((user) => {
          this.user = user;
          return this.accountService.getTimeoutDuration();
        }),
        tap((timeout) => {
          if (this.user.timeoutMinutes > 0) {
            this.timeoutMinutes = this.user.timeoutMinutes;
          } else {
            this.timeoutMinutes = timeout.timeoutMinutes;
          }
          switch (this.config.baseSite.siteType) {
            case ConfigUrlType.Ess:
            case ConfigUrlType.Company:
              this.logoutLink = new UrlParts("").joinWithSlash(
                this.config.baseSite.url,
                "Logout?directLogout"
              );
              break;
            case ConfigUrlType.Payroll:
            default:
              this.logoutLink = new UrlParts("").joinWithSlash(
                this.config.baseSite.url,
                "Logout.aspx"
              );
              break;
          }

          this.addListeners();
        })
      )
      .subscribe(() => {
        if (this.timeoutMinutes > 0) {
          this.setTimer();
        }
      });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.removeListeners();
  }

  addListeners() {
    this.document.addEventListener("click", () => this.resetTimer());
    this.document.addEventListener("keypress", () => this.resetTimer());
    this.document.addEventListener("scroll", () => this.resetTimer());
  }

  removeListeners() {
    this.document.removeEventListener("click", () => this.resetTimer());
    this.document.removeEventListener("keypress", () => this.resetTimer());
    this.document.removeEventListener("scroll", () => this.resetTimer());
  }

  logout() {
    if (localStorage) localStorage.clear();
    if (window.onbeforeunload) {
      window.onbeforeunload = () => { };
    }

    this.document.location.href = this.logoutLink;
  }

  setTimer() {
    this.idleTimer = setTimeout(() => this.logout(), this.timeoutMinutes);
  }

  resetTimer() {
    if (this.idleTimer) clearTimeout(this.idleTimer);
    this.setTimer();
  }
}

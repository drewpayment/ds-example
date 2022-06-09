import { Component, OnInit, Inject, OnDestroy } from "@angular/core";
import { AccountService } from "@ds/core/account.service";
import { UserInfo } from "@ds/core/shared";
import { Observable, of, Subject } from "rxjs";
import { tap, switchMap, map, takeUntil, filter } from "rxjs/operators";
import { UserType, WindowConfig } from "@ds/core/shared";
import { MatDialog } from "@angular/material/dialog";
import { NgClientSelectorComponent } from "../ng-client-selector/ng-client-selector.component";
import { ClientService } from "@ds/core/clients/shared";
import { IClientData } from "@ajs/onboarding/shared/models";
import { DOCUMENT } from "@angular/common";
import { ConfigUrlType } from "@ds/core/shared/config-url.model";
import { ClientStatus } from "@ds/core/employee-services/enums";
import { Store } from "@ngrx/store";
import { getUser, UserState } from "@ds/core/users/store/user.reducer";

export abstract class BaseComponent {}
export abstract class HeaderBaseComponent {}

@Component({
  selector: "ds-header",
  templateUrl: "./header.component.html",
  styleUrls: ["./header.component.scss"],
})
export class HeaderComponent implements HeaderBaseComponent, OnDestroy {
  user: UserInfo;
  destroy$ = new Subject();
  user$ = this.getUserInfo();
  canChangeClient = false;
  selectedClient: IClientData;

  constructor(
    private accountService: AccountService,
    private dialog: MatDialog,
    private clientService: ClientService,
    @Inject(DOCUMENT) private document: Document,
    private store: Store<UserState>
  ) {}

  ngOnDestroy() {
    this.destroy$.next();
  }

  clearLocalStorage() {
    if (localStorage) localStorage.clear();
  }

  getUserInfo(): Observable<UserInfo> {
    return this.store.select(getUser).pipe(
      takeUntil(this.destroy$),
      filter((u) => u != null && u.userId > 0),
      tap((u) => (this.user = u)),
      switchMap((user) => {
        this.canChangeClient = this.checkUserAdminRights(user.userTypeId);
        return this.clientService.getClientById(user.lastClientId);
      }),
      map((client) => {
        this.selectedClient = client;
        return this.user;
      })
    );
  }

  checkUserAdminRights(userType: number): boolean {
    return (
      userType === UserType.systemAdmin || userType === UserType.companyAdmin
    );
  }

  openClientSelector() {
    this.dialog
      .open(NgClientSelectorComponent, {
        position: {
          top: "15vh",
        },
        width: "600px",
        data: {
          selectedClient$: this.clientService.getClientById(
            this.user.selectedClientId()
          ),
        },
        autoFocus: false,
      })
      .afterClosed()
      .subscribe((result) => {
        if (!result) return;
      });
  }

  showEmployeeSelector() {
    const width = 600;
    const height = 800;

    const options: WindowConfig = {
      width: width,
      height: height,
      status: "no",
      toolbar: "no",
      menubar: "no",
      location: "no",
      scrollbars: "yes",
      resizable: "yes",
      fullscreen: "no",
    };

    this.accountService.getSiteUrls().subscribe((urls) => {
      const source = urls.find((u) => u.siteType === ConfigUrlType.Payroll);
      const url = `${source.url}ChangeEmployeePopup.aspx?Clock=0&SSID=0`;
      this.showWindow(url, options);
    });
  }

  showReportQueue() {
    this.accountService.getSiteUrls().subscribe((urls) => {
      const source = urls.find((u) => u.siteType === ConfigUrlType.Payroll);
      const url = `${source.url}ReportQueuePopup.aspx`;

      const options: WindowConfig = {
        width: 670,
        height: 450,
        status: "no",
        toolbar: "no",
        menubar: "no",
        location: "no",
        scrollbars: "yes",
        resizable: "yes",
        fullscreen: "no",
      };

      this.showWindow(url, options, "Report Queue");
    });
  }

  private showWindow(url: string, options: WindowConfig, title?: string) {
    const dualScreenLeft =
      window.screenLeft !== undefined ? window.screenLeft : window.screenX;
    const dualScreenTop =
      window.screenTop !== undefined ? window.screenTop : window.screenY;

    const width = window.innerWidth
      ? window.innerWidth
      : document.documentElement.clientWidth
      ? document.documentElement.clientWidth
      : screen.width;
    const height = window.innerHeight
      ? window.innerHeight
      : document.documentElement.clientHeight
      ? document.documentElement.clientHeight
      : screen.height;

    const systemZoom = width / window.screen.availWidth;
    const left = (width - options.width) / 2 / systemZoom + dualScreenLeft;
    const top = (height - options.height) / 2 / systemZoom + dualScreenTop;

    options.left = left;
    options.top = top;

    let params = "";
    let first = true;
    for (const p in options) {
      if (first) {
        params += p + "=" + options[p];
        first = false;
      } else {
        params += "," + p + "=" + options[p];
      }
    }

    const newWindow = window.open(url, title, params);

    if (window.focus) newWindow.focus();
  }
}

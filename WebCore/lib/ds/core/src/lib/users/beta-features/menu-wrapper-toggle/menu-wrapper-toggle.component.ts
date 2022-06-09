import { Component, OnInit, Inject, OnDestroy, Input, AfterViewInit } from "@angular/core";
import { UserInfo } from "@ds/core/shared";
import { AccountService } from "@ds/core/account.service";
import {
  BetaFeatureType,
  UserBetaFeature,
  UrlParts,
  UserType,
  WindowConfig,
} from "@ds/core/shared";
import { DOCUMENT } from "@angular/common";
import { tap, switchMap, takeUntil, map } from "rxjs/operators";
import { Observable, Subscription, iif, of, BehaviorSubject, combineLatest, forkJoin, Subject } from "rxjs";
import { MenuApiService } from "@ds/core/app-config/shared/menu-api.service";
import { ConfigUrlType } from "@ds/core/shared/config-url.model";
import { Moment } from "moment";
import * as moment from "moment";
import { APP_CONFIG, AppConfig } from "@ds/core/app-config/app-config";
import {
  MatDialogRef,
  MatDialog,
  MatDialogConfig,
} from "@angular/material/dialog";
import {
  ISystemFeedbackData,
  SystemFeedbackType,
} from "./toggle-feedback-dialog/toggle-feedback-dialog-data.model";
import {
  ToggleFeedbackDialogComponent,
  ToggleThankYouComponent,
} from "./toggle-feedback-dialog/toggle-feedback-dialog.component";
import { CORE_ENVIRONMENT } from '@ds/core/core.tokens';

@Component({
  selector: "ds-menu-wrapper-toggle",
  templateUrl: "./menu-wrapper-toggle.component.html",
  styleUrls: ["./menu-wrapper-toggle.component.scss"],
})
export class MenuWrapperToggleComponent implements OnInit, OnDestroy {
  user$: Observable<UserInfo>;
  user: UserInfo;
  private _hasMenuWrapper$ = new BehaviorSubject<boolean>(false);
  hasMenuWrapper: boolean;
  feature: UserBetaFeature;
  doNotShowToggle = false;
  isEmployeeOrApplicant = true;
  isSupervisor = true;
  showCheckDate$: Observable<boolean>;
  isShowEmployeeSelector$: Observable<boolean>;
  menuWrapperFeedback: ISystemFeedbackData;
  subs: Subscription[];

  // private _ssid: string;
  // @Input() set ssid(value: string) {
  //   value = value != null ? value.replace(/\"/g, "") : null;
  //   this._ssid = value;
  // }
  // get ssid(): string {
  //   return this._ssid;
  // }

  private _checkDate: Moment | Date | string;
  @Input("checkdate") set checkDate(value: Moment) {
    const momentValue = moment(value);
    if (!momentValue.isValid) return;
    this._checkDate = momentValue;
  }
  get checkDate(): Moment {
    return this._checkDate != null ? moment(this._checkDate) : null;
  }

  hideTooltip$ = this.accountService.hideMenuWrapperTooltip$;
  private destroy$ = new Subject();

  constructor(
    private accountService: AccountService,
    @Inject(DOCUMENT) public document: Document,
    private menuApi: MenuApiService,
    @Inject(APP_CONFIG) private config: AppConfig,
    private feedbackDialog: MatDialog,
    @Inject(CORE_ENVIRONMENT) private environment: any,
  ) {}

  ngOnInit() {
    this.showCheckDate$ = this.menuApi.showCheckDate$.asObservable();
    this.isShowEmployeeSelector$ = this.menuApi.showEmployeeSelector$.asObservable();
    this.subs = [];

    this.user$ = this.accountService.getUserInfo().pipe(
      tap((user) => {
        this.user = user;
        //this.doNotShowToggle = this.user.userTypeId > UserType.supervisor;
        this.doNotShowToggle =
          this.document.location.href
            .toLowerCase()
            .includes(this.config.baseSite.url.toLowerCase() + "benefits") ||
          this.user.userTypeId > UserType.supervisor;
        this.feature = this.user.betaFeatures.find(
          (b) => b.betaFeatureId == BetaFeatureType.MenuWrapper
        );

        this.isEmployeeOrApplicant =
          this.user.userTypeId == 3 || this.user.userTypeId == 5;
        this.isSupervisor = this.user.userTypeId == 4;

        let subscript = this.accountService
          .getSystemFeedback(this.user.userId, SystemFeedbackType.MenuWrapper)
          .subscribe((x) => (this.menuWrapperFeedback = x));
        this.subs.push(subscript);
      })
    );
  }

  ngOnDestroy() {
    this.destroy$.next();
    if (this.subs && this.subs.length)
      this.subs.forEach((s) => s.unsubscribe());
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
      const sourceUrl =
        source && source.url !== undefined && source.url.length > 0
          ? source.url.charAt(source.url.length - 1) === "/"
            ? source.url
            : `${source.url}/`
          : "";
      const url = `${sourceUrl}ChangeEmployeePopup.aspx?Clock=0`;
      this.showWindow(url, options);
    });
  }

  showReportQueue() {
    this.accountService.getSiteUrls().subscribe((urls) => {
      const source = urls.find((u) => u.siteType === ConfigUrlType.Payroll);
      let sourceUrl = source && source.url ? source.url : "";
      sourceUrl =
        sourceUrl.charAt(sourceUrl.length - 1) === "/"
          ? sourceUrl
          : sourceUrl + "/";
      const url = sourceUrl + "ReportQueuePopup.aspx";

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

  showPersonnelReportViewer() {
    this.accountService.getSiteUrls().subscribe((urls) => {
      const source = urls.find((u) => u.siteType === ConfigUrlType.Payroll);
      const url = `${source.url}PreparePersonalReport.aspx`;

      const options: WindowConfig = {
        menubar: "menubar",
      };

      this.showWindow(url, options);
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
      if (p === "menubar") {
        if (first) {
          params += p;
          first = false;
        } else {
          params += "," + p;
        }
        continue;
      }

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

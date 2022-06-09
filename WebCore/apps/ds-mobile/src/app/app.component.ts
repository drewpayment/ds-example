import { Component, OnInit, HostListener, Inject } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { UserInfo, UserType } from '@ds/core/shared';
import { tap, map } from 'rxjs/operators';
import { DOCUMENT } from '@angular/common';
import { AppService } from './app.service';
import { AssetHelperService } from '@ds/core/ui/ui-helper';
import { AccountService } from '@ds/core/account.service';
import { NPSSurveyDialogComponent } from '@ds/admin/nps/nps-survey/nps-survey-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';

@Component({
    selector: 'ds-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    title = 'ds-mobile';
    shouldRun = false;
    user: UserInfo;
    timeoutMinutes: number;
    idleTimer: NodeJS.Timer;
    lastRouteId: number;
    baseUrl: string;
    logoutLink: string;

    constructor(
        private breakpointObserver: BreakpointObserver,
        private accountService: AccountService,
        @Inject(DOCUMENT) private document: Document,
        private service: AppService,
        private assets: AssetHelperService,
        private dialog: MatDialog,
        private sb: MatSnackBar,
    ) {
        this.breakpointObserver.observe([
            Breakpoints.Handset
        ]).subscribe(result => this.shouldRun = result.matches);
    }

    ngOnInit() {
        this.service.baseUrl$.pipe(map(url => this.baseUrl = url))
            .subscribe(url => {
                this.logoutLink = this.assets.resolveBaseUrl('logout.aspx');
            });

        this.accountService.getUserInfo()
            .pipe(tap(user => this.user = user))
            .subscribe(user => {
                if (this.user.userTypeId <= UserType.companyAdmin) {
                    this.accountService.getSiteConfig(ConfigUrlType.Company).subscribe(result => {
                        this.document.location.href = result.url + 'service/home?RequestDesktop=1';
                    });
                }

                this.initializeTimeout(this.user.timeoutMinutes);
            });
    }

    logout() {
        if (window.onbeforeunload) window.onbeforeunload = () => {};
        this.document.location.href = this.logoutLink;
    }

    getDesktopUrl(): string {
        return this.assets.resolveBaseUrl('default.aspx?RequestDesktop=1');
    }

    @HostListener('document:keypress', ['$event'])
    onKeyPress(event: KeyboardEvent) {
        this.resetTimer();
    }

    @HostListener('document:click', ['$event'])
    onClick(event: MouseEvent) {
        this.resetTimer();
    }

    setTimer() {
        this.idleTimer = setTimeout(() => this.logout(), this.timeoutMinutes * 60000);
    }

    resetTimer() {
        clearTimeout(this.idleTimer);
        this.setTimer();
    }

    getAssetsPath(path: string): string {
        return this.assets.resolveAsset(path);
    }

    private initializeTimeout(timeout?: number) {
        this.timeoutMinutes = timeout;
        if (this.timeoutMinutes < 1) this.timeoutMinutes = 15;
        this.setTimer();
    }

    openNPSSurvey() {
        const dialogRef = this.dialog.open(NPSSurveyDialogComponent);

        dialogRef.afterClosed().subscribe(result => {
            if (result == null) {

            } else {
                if (result) {
                    this.sb.open('Thank you for your feedback. We value your input.', 'dismiss', {duration: 2000});
                } else {
                    this.sb.open('There was an error that occurred while saving your input.', 'dismiss', {duration: 2000});
                }
            }
        });
    }

}

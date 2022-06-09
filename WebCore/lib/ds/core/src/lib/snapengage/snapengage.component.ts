import { Component, AfterViewChecked, Inject, Directive, OnInit, ChangeDetectionStrategy, AfterViewInit, Input, ElementRef, OnDestroy } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { AccountService } from '@ds/core/account.service';
import { takeUntil, switchMap } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { UserType, UserInfo } from '@ds/core/shared';


@Component({
    selector: 'ds-snapengage',
    template: ``,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SnapengageComponent implements AfterViewInit, OnDestroy {
    done = false;
    calledShow = false;
    destroy$ = new Subject();
    user: UserInfo;
    isMenuWrapperOn = true;
    constructor(
        @Inject(DOCUMENT) private document: Document,
        @Inject('window') private window: any,
        private el: ElementRef,
        private accountService: AccountService,
    ) {}

    ngAfterViewInit() {
        this.accountService.getUserInfo()
            .pipe(
                takeUntil(this.destroy$),
                switchMap(user => {
                    this.user = user;

                    return this.accountService.hasMenuWrapperFeature();
                })
            )
            .subscribe(isMenuWrapperOn => {
                this.isMenuWrapperOn = isMenuWrapperOn;
                if (this.user.userTypeId < UserType.employee) {
                    if (!this.calledShow) this.showSnapEngage();
                }
            });
    }

    ngOnDestroy() {
        this.destroy$.next();
    }

    showSnapEngage() {
        let done = this.done;
        this.calledShow = true;
        const se = this.document.createElement('script') as any;
        se.type = 'text/javascript';
        se.async = true;
        se.src = '//commondatastorage.googleapis.com/code.snapengage.com/js/1fcf5e82-84ce-493f-a354-91166a77ee74.js';
        se.onload = se.onreadystatechange = function(event) {
            if (!done && (!event.readyState || event.readyState === 'loaded' || event.readyState === 'complete')) {
                done = true;
                // Place your SnapEngage JS API code below
                // SnapEngage.allowChatSound(true); // Example JS API: Enable sounds for Visitors.

                (window as any).SnapEngage.hideButton();
            }
        };
        const s = document.getElementsByTagName('script')[0];
        s.parentNode.insertBefore(se, s);
    }

}

import { Component, OnInit, AfterViewInit, InjectionToken, Inject } from '@angular/core';
import { DsEssAppAjsModule } from 'apps/ds-ess/ajs/ds-ess-app.module';
import { UpgradeModule } from '@angular/upgrade/static';
import { BootstrapService } from '../bootstrap.service';
import { BetaFeatureType } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { first, tap } from 'rxjs/operators';
import { defer } from 'rxjs';
import { Router } from '@angular/router';


@Component({
    selector: 'ess-outlet',
    templateUrl: './outlet.component.html',
})
export class OutletComponent implements OnInit {
    isInitialPageLoad = true;
    hasMenuWrapper = false;

    constructor(
        private upgrade: UpgradeModule,
        private bootService: BootstrapService,
        private account: AccountService,
        private router: Router
    ) { }

    ngOnInit() {
        /** If AJS has not been bootstrapped, we are going to bootstrap it to the document */
        // if (!this.bootService.isBootstrapped) {
        //     const ajsRoot = document.body;
        //     this.upgrade.bootstrap(ajsRoot, [DsEssAppAjsModule.AjsModule.name], { strictDi: true });
        //     this.bootService.isBootstrapped = true;
        // }
    }

}

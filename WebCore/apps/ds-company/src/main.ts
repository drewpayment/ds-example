import * as angular from 'angular';
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { DsCompanyAppNgxModule } from './app/app.module';
import { DsCompanyAppAjsModule } from '../ajs/ds-company-app.module';
import { environment } from './environments/environment';

import 'hammerjs';
// ajs dependencies
import { setAngularJSGlobal, UpgradeModule } from '@angular/upgrade/static';
import { fetchSiteUrls, AppConfig } from '@ds/core/app-config/app-config';

// animations library for Angular Material
import 'hammerjs';
import { Router, UrlHandlingStrategy } from '@angular/router';
import { RootInjector } from 'lib/utilties/root.injector';
import { hideConsole } from 'lib/utilties/hide-console';

if (environment.production) {
  enableProdMode();
}
hideConsole(environment.production);

setAngularJSGlobal(angular);
fetchSiteUrls().then(config => {
    if (config && config.baseSite && config.baseSite.url)
        environment.baseUrl = config.baseSite.url;
    platformBrowserDynamic([ { provide: AppConfig, useValue: config } ])
        .bootstrapModule(DsCompanyAppNgxModule)
        .then(ref => {
          DsCompanyAppAjsModule.AjsModule.factory('ng2Injector', () => ref.injector)
            .factory('ng2UrlHandlingStrategy', () => ref.injector.get(UrlHandlingStrategy))
            .factory('ng2Router', () => ref.injector.get(Router));
          const upgrade = ref.injector.get(UpgradeModule);
          upgrade.bootstrap(document.documentElement,
            [DsCompanyAppAjsModule.AjsModule.name],
            { strictDi: true });

          RootInjector.setInjector(ref.injector);
        })
        .catch(err => console.log(err));
});


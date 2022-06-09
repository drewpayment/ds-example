import * as angular from 'angular';
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { DsSourceAppModule } from './app/app.module';
import { environment } from './environments/environment';
import '../../../lib/utilties';

import 'hammerjs';
// ajs dependencies
import 'angular';
import { setAngularJSGlobal, setAngularLib } from '@angular/upgrade/static';
import '@ds/core/shared/array-extensions';
import { fetchSiteUrls, AppConfig } from '@ds/core/app-config/app-config';
import { hideConsole } from 'lib/utilties/hide-console';

if (environment.production) {
  enableProdMode();
}
hideConsole(environment.production);

setAngularJSGlobal(angular);
fetchSiteUrls().then(config => {
    platformBrowserDynamic([ { provide: AppConfig, useValue: config } ])
        .bootstrapModule(DsSourceAppModule)
        .catch(err => console.log(err));
});

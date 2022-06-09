import * as angular from 'angular';
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

import 'hammerjs';
import 'angular';
import '@ds/core/shared/array-extensions';
import { setAngularJSGlobal } from '@angular/upgrade/static';
import { AppConfig, fetchSiteUrls } from '@ds/core/app-config/app-config';
import { hideConsole } from 'lib/utilties/hide-console';

if (environment.production) {
    enableProdMode();
}
hideConsole(environment.production);

setAngularJSGlobal(angular);
fetchSiteUrls().then(config => {
    platformBrowserDynamic([{ provide: AppConfig, useValue: config }])
        .bootstrapModule(AppModule)
        .catch(err => console.log(err));
});

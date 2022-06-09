import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";
import { setAngularLib } from "@angular/upgrade/static";
import * as angular from "angular";
import { DsDesignAppModule } from "./app/ds-design-app.module";

setAngularLib(angular);
platformBrowserDynamic().bootstrapModule(DsDesignAppModule);
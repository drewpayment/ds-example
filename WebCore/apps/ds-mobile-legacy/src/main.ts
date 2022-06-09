import * as angular from "angular";
import { DsMobileAppAjsModule } from "../ajs/ds-mobile-app.module";

angular.bootstrap(document.documentElement, [DsMobileAppAjsModule.AjsModule.name], { strictDi: true })


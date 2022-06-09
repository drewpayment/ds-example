import { Injectable } from '@angular/core';
import { UpgradeModule } from '@angular/upgrade/static';


@Injectable({
    providedIn: 'root'
})
export class AjsCompanyLoaderService {

    constructor(private readonly upgrade: UpgradeModule) { }
    private elementPartOfAjsApp: HTMLElement;
    
    load(el: HTMLElement) {
        this.elementPartOfAjsApp = el;
        import ('../../ajs/ds-company-app.module').then(app => {
            try {
                this.upgrade.bootstrap(this.elementPartOfAjsApp, [app.DsCompanyAppAjsModule.AjsModule.name], { strictDi: true });
            } catch (e) {
                console.error(e);
            }
        });
    }
    
    destroy() {
            ($(this.elementPartOfAjsApp) as any ).scope().$root.$destroy();
    }
}

import { UpgradeModule } from '@angular/upgrade/static';
import { BootstrapService } from '../bootstrap.service';
import { DsDesignModule } from '../ds-design-app.module.ajs';
import { Component, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
    template:''
})
export class NgUpgradeComponent {
    constructor(
        private upgrade:UpgradeModule, 
        private bootService:BootstrapService
    ) {}
    
    ngOnInit() {
        /** If AJS has not been bootstrapped, we are going to bootstrap it to the document */
        if(!this.bootService.isBootstrapped) {
            let ajsRoot = document.documentElement;
            this.upgrade.bootstrap(ajsRoot, [DsDesignModule.AjsModule.name], { strictDi: true });      
            this.bootService.isBootstrapped = true;
        }

    }
}

@NgModule({
    imports: [RouterModule.forChild([
      {path: '**', component: NgUpgradeComponent}
    ]),
    UpgradeModule
    ],
    declarations: [NgUpgradeComponent]
  })
export class NgUpgradeModule {
}
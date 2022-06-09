import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { UpgradeModule } from "@angular/upgrade/static";
import { NgxMigrationDemoModule } from "./ngx-migration/demo/demo.module";
import { DowngradeModule } from "./downgrade.module";
import { DsProgressModule } from "@ds/core/ui/ds-progress";
import { Routes, RouterModule } from "@angular/router";
import { LayoutComponent } from "./layout/layout.component";
import { FormsModule } from "./forms/forms.module";
import { NgUpgradeModule } from "./ng-upgrade/ng-upgrade.module";
import { HttpClientModule, HttpClient } from "@angular/common/http";
import { MarkdownModule } from "ngx-markdown";
import { DsComponentsModule } from "./ds-components/ds-components.module";
import { DocsMaterialModule } from "./material.module";
import { TypographyModule } from "./typography/typography.module";
import { HomeDocsComponent } from "./ds-home/home-docs/home-docs.component";
import { DsHomeModule } from "./ds-home/ds-home.module";
import { environment } from '../environments/environment';

const routes: Routes = [
  { path: '', component: HomeDocsComponent },
];

@NgModule({
  imports: [
    BrowserModule,
    DocsMaterialModule,
    // UpgradeModule,
    // NgxMigrationDemoModule,
    // DowngradeModule,
    DsProgressModule,
    FormsModule,
    DsComponentsModule,
    // NgUpgradeModule,
    HttpClientModule,
    TypographyModule,
    DsHomeModule,
    MarkdownModule.forRoot({ loader: HttpClient }),
    RouterModule.forRoot(routes, {
      enableTracing: !environment.production, /* For debugging only */
    }),
  ],
  declarations: [LayoutComponent],
  bootstrap: [LayoutComponent],
})
export class DsDesignAppModule {
  // constructor(private upgrade: UpgradeModule) { }
  // ngDoBootstrap() {
  //     this.upgrade.bootstrap(document.documentElement, [DsDesignModule.AjsModule.name], { strictDi: true });
  // }
}

import * as angular from "angular";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HeroDetailComponent } from "./hero-detail/hero-detail.component";
import { NgxMigrationDemoAjsModule } from "./demo.module.ajs";
import { downgradeComponent } from "@angular/upgrade/static";

@NgModule({
    imports:[CommonModule],
    declarations:[HeroDetailComponent],
    entryComponents:[HeroDetailComponent]
})
export class NgxMigrationDemoModule {}

angular
    .module(NgxMigrationDemoAjsModule.AjsModule.name)
    .directive('heroDetail', downgradeComponent({component:HeroDetailComponent}) as angular.IDirectiveFactory);

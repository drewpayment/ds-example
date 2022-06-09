import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OnLoadDirective } from './on-load.directive';


@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
      OnLoadDirective
    ],
    exports: [
      OnLoadDirective
    ]
})
export class DsOnLoadModule {}
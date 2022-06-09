import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { MaterialModule } from '../material';
import { AngularMaterialDateboxComponent } from './angular-material-datebox/angular-material-datebox.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MomentDateModule } from '@angular/material-moment-adapter';

export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};

@NgModule({
    imports: [
        MaterialModule,
        CommonModule,
        MomentDateModule,
        NgxMaskModule.forRoot(options),
        ReactiveFormsModule,
    ],
    declarations: [
        AngularMaterialDateboxComponent,
    ],
    entryComponents: [
        AngularMaterialDateboxComponent,
    ],
    exports: [
        AngularMaterialDateboxComponent,
        CommonModule,
        FormsModule,
    ]
})
export class AngularMaterialDateboxModule { }

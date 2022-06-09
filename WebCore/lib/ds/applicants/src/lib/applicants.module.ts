import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { SignUpComponent } from './newuser/signup.component';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { NgxMaskModule, IConfig } from 'ngx-mask';
export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};

@NgModule({
    declarations: [
        SignUpComponent,
    ],
    imports: [
        BrowserModule,
        CommonModule,
        MaterialModule,
        FormsModule,
        ReactiveFormsModule,
        DsCardModule,
        NgxMaskModule.forRoot(options)
    ],
    exports: [
        SignUpComponent,
    ],
    entryComponents: [
    ]
})
export class ApplicantsModule { }

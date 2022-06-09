import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxLinkComponent } from '@ds/core/ngx-downgrades/ngx-link/ngx-link.component';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MaterialModule } from '../ui/material';
import { DsCustomOutletModule } from '../ui/ds-custom-outlet/ds-custom-outlet.module';
import { WorkNumberModule } from '../popup/equifax/work-number/work-number.module';
import { AvatarModule } from '../ui/avatar/avatar.module';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        MaterialModule,
        RouterModule,
        WorkNumberModule,
        AvatarModule
    ],
    declarations: [
        NgxLinkComponent
    ],
    entryComponents: [
        NgxLinkComponent
    ],
    exports: [
        NgxLinkComponent
    ]
})
export class NgxDowngradesModule { }

export * from './ngx-link/ngx-link.component';

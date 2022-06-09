import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsRoutingModule } from '@ds/docs/forms/forms-routing.module';
import { FormsDocsComponent } from '@ds/docs/forms/forms-docs/forms-docs.component';
import { ChangeTrackModule } from '@ds/docs/forms/change-track/change-track.module';
import { AutoFocusModule } from './auto-focus/auto-focus.module';
import { FormValidationModule } from '@ds/docs/forms/form-validation/form-validation.module';
import { MarkdownModule } from 'ngx-markdown';

@NgModule({
    declarations: [FormsDocsComponent],
    imports: [
        CommonModule,
        FormsRoutingModule,
        ChangeTrackModule,
        FormValidationModule,
        AutoFocusModule,
        MarkdownModule.forChild()
    ]
})
export class FormsModule { }

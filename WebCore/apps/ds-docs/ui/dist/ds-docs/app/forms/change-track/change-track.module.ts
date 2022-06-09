import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChangeTrackDocsComponent } from './change-track-docs/change-track-docs.component';
import { TemplateFormChangeTrackComponent } from './template-form-change-track/template-form-change-track.component';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ReactiveFormChangeTrackComponent } from './reactive-form-change-track/reactive-form-change-track.component';
import { CustomChangeTrackComponent } from './custom-change-track/custom-change-track.component';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { MarkdownModule } from 'ngx-markdown';

@NgModule({
    declarations: [
        ChangeTrackDocsComponent, 
        TemplateFormChangeTrackComponent, 
        ReactiveFormChangeTrackComponent, CustomChangeTrackComponent
    ],
    imports: [
        CommonModule,
        DsCardModule,
        DsCoreFormsModule,
        FormsModule,
        ReactiveFormsModule,
        MarkdownModule.forChild()
    ],
    exports: [
        ChangeTrackDocsComponent
    ]
})
export class ChangeTrackModule { }

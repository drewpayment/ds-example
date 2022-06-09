import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FormsDocsComponent } from './forms-docs/forms-docs.component';
import { ChangeTrackDocsComponent } from './change-track/change-track-docs/change-track-docs.component';
import { CheckDeactivatorGuard } from '@ds/core/ui/change-track';
import { FormValidationDocsComponent } from './form-validation/form-validation-docs/form-validation-docs.component';
import { AutoFocusDocsComponent } from './auto-focus/auto-focus-docs/auto-focus-docs.component';

const routes: Routes = [
    {
        path: 'forms',
        children:[
            {
                path: '',
                component: FormsDocsComponent
            },
            {
                path: 'auto-focus',
                component: AutoFocusDocsComponent
            },
            {
                path: 'change-track',
                component: ChangeTrackDocsComponent,
                canDeactivate: [CheckDeactivatorGuard]
            },
            {
                path: 'validation',
                component: FormValidationDocsComponent
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FormsRoutingModule { }

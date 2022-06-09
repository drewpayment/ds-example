import { ScrollingModule } from '@angular/cdk/scrolling';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Route, RouterModule } from '@angular/router';
import { UpgradeModule } from '@angular/upgrade/static';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { MaterialModule } from '@ds/core/ui/material';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Route[] = [{
    path: 'en',
    children: [
        {
            path: 'dashboard',
            component: DashboardComponent,
            pathMatch: 'full'
        }
    ]
}];

@NgModule({
    imports: [
        CommonModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule,
        MaterialModule,
        MatDialogModule,
        DsCardModule,
        DsCoreFormsModule,
        UpgradeModule,
        ScrollingModule,
        LoadingMessageModule,

        RouterModule.forChild(routes),
    ],
    declarations: [
        DashboardComponent,
    ],
})
export class EmployeeNavigatorModule {}

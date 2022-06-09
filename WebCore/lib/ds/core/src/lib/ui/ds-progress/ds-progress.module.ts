import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DsProgressComponent } from '@ds/core/ui/ds-progress/ds-progress.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@NgModule({
    imports: [
        CommonModule,
        MatProgressBarModule
    ],
    declarations: [DsProgressComponent],
    exports: [DsProgressComponent]
})
export class DsProgressModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchFilterDialogComponent } from '@ds/core/employees/search-filter-dialog/search-filter-dialog.component';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { DsCardModule } from '@ds/core/ui/ds-card/ds-card.module';

@NgModule({
    imports: [
        CommonModule,
        MaterialModule,
        FormsModule,
        ReactiveFormsModule,
        DsCoreFormsModule,
        DsCardModule,
    ],

    declarations: [
        SearchFilterDialogComponent
    ],
    exports: [
        SearchFilterDialogComponent
    ],
    entryComponents: [
        SearchFilterDialogComponent
    ]
})
export class DsCoreEmployeesModule { }

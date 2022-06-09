import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControlValidatorDirective } from '@ds/core/ui/forms/form-control-validator/form-control-validator.directive';
import { MinDirective } from '@ds/core/ui/forms/min/min.directive';
import { MaxDirective } from '@ds/core/ui/forms/max/max.directive';
import { RoutingNumberDirective } from '@ds/core/ui/forms/routing-number/routing-number.directive';
import { TrackFormChangesDirective } from '@ds/core/ui/forms/change-track/track-form-changes.directive';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ChangeTrackModule } from '@ds/core/ui/change-track';
import { CustomControlValidatorDirective } from './custom-control-validator/custom-control-validator.directive';
import { DateSelectorComponent } from './date-selector/date-selector.component';
import { DayOfMonthSelectorComponent } from './day-of-month-selector/day-of-month-selector.component';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { MaterialModule } from '../material';
import { GenericAutocompleteComponent } from './generic-autocomplete/generic-autocomplete.component';
import { DecimalFormatDirective } from './decimal-formatter/decimal-formatter.directive'
import { AccountAmountDirective } from './account-amount/account-amount.directive';
import { AutoFocusDirective } from './focus/auto-focus.directive';
import { DsOnLoadModule } from '@ds/core/directives/on-load/on-load.module';

export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ChangeTrackModule,
        NgxMaskModule.forRoot(options),
        MaterialModule,
        DsOnLoadModule
    ],
    declarations: [
        FormControlValidatorDirective,
        MinDirective,
        MaxDirective,
        RoutingNumberDirective,
        TrackFormChangesDirective,
        CustomControlValidatorDirective,
        DateSelectorComponent,
        DayOfMonthSelectorComponent,
        GenericAutocompleteComponent,
        DecimalFormatDirective,
        AccountAmountDirective,
        AutoFocusDirective
    ],
    exports: [
        FormControlValidatorDirective,
        CustomControlValidatorDirective,
        RoutingNumberDirective,
        TrackFormChangesDirective,
        ChangeTrackModule,
        DateSelectorComponent,
        DayOfMonthSelectorComponent,
        MaxDirective,
        GenericAutocompleteComponent,
        DecimalFormatDirective,
        AccountAmountDirective,
        AutoFocusDirective,
        DsOnLoadModule
    ]
})
export class DsCoreFormsModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MomentTimeInputDirective } from '@ds/core/ui/datetime/moment-time-input.directive';
import { MomentFormatPipe } from '@ds/core/ui/datetime/moment-format.pipe';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
        MomentTimeInputDirective,
        MomentFormatPipe
    ],
    exports: [
        MomentTimeInputDirective,
        MomentFormatPipe
    ]
})
export class DateTimeModule {
}

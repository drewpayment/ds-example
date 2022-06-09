import { NgModule } from '@angular/core';
import { PunchService } from './punch.service';


@NgModule({
    exports: [
        PunchService
    ]
})
export class TimeClockModule {}

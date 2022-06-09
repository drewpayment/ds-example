import { Component, OnInit, Input } from "@angular/core";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";

import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import * as moment from "moment";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { AccountService } from '@ds/core/account.service';
import { MOMENT_FORMATS } from '@ds/core/shared';

@Component({
  selector: 'ds-exceptions',
  templateUrl: './exceptions.component.html',
  styleUrls: ['./exceptions.component.css']
})
export class ExceptionsComponent implements OnInit {
    @Input() employeeIds: Number[];
    @Input() dateRange: DateRange;

    loaded: boolean;
    infoData: InfoData;
    emptyState: boolean = false;
    
    cardType: string = "graph";
    title: string = "Exceptions";

    dataArray: any[] = [];
    scheduleEmployeeIds: any[] = [];

    constructor(
        private analyticsApi: AnalyticsApiService,
        private accountService: AccountService
    ) {}

    ngOnInit() {
        this.accountService.getUserInfo().subscribe((user) => {this.analyticsApi.GetClockExceptionsByRange(user.clientId,moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE),moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE),this.employeeIds)
            .subscribe((data: any) => {
                    if (data == null || data == [] || data.length <= 0){
                        this.emptyState = true;
                    }
                    else{
                        var a = this.uniqueBy(data, "clockException");
                        for(const exception of a){
                            this.dataArray.push(data.filter(x => x.clockException == exception));
                        }
                        this.dataArray.sort((a,b) => a[0].clockException.toString() > b[0].clockException.toString() ? 1 : -1);         
                    }
                    this.loaded = true;
                });
        });
    }

    uniqueBy(arr, prop){
        return arr.reduce((a, d) => {
            if (!a.includes(d[prop])) { a.push(d[prop]); }
            return a;
        }, []);
    }
}


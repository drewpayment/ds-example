import { Component, OnInit, Input } from "@angular/core";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import * as moment from "moment";
import { AccountService } from '@ds/core/account.service';
import { MOMENT_FORMATS } from '@ds/core/shared';

@Component({
  selector: 'ds-demographics',
  templateUrl: './demographics.component.html',
  styleUrls: ['./demographics.component.css']
})
export class DemographicsComponent implements OnInit {
    @Input() employeeIds: Number[];
    @Input() dateRange: DateRange;

    loaded: boolean;
    emptyState: boolean = false;

    isList: boolean;
    isGraph: boolean;

    cardType: string = "graph";

    infoData: InfoData;

    public title: string = "Demographics"

    public graphData = [];

    public availableChartTypes: string[] = ['bar', 'list'];

    constructor(
        private analyticsApi: AnalyticsApiService,
        private accountService: AccountService
    ) {}

    ngOnInit() {
        this.accountService.getUserInfo().subscribe((user) => { this.analyticsApi.GetEmployeeDemographicInformation(user.clientId,moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE),
            moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE), this.employeeIds)
                .subscribe((data: any) => {
                  if (data == null || data.length <= 0 || data == []){
                    this.emptyState = true;
                  }
                  this.graphData = data;

                  //Set loaded to true
                  this.loaded = true;
                  this.isList = true;
                });
        });
    }

    chartChange(e){
      if(e.value == "list"){
        this.isGraph = false;
        this.isList = true;
      }
      else{
        this.isGraph = true;
        this.isList = false;
      }
    }

}

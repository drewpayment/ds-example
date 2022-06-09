import { Component, OnInit, AfterViewInit, AfterViewChecked } from '@angular/core';
import { DsStyleLoaderService, IStyleAsset } from '@ajs/ui/ds-styles/ds-styles.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { map } from 'rxjs/operators';
import * as _ from "lodash";
import { ICompetency } from '@ds/performance/competencies';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { EmployeePerformanceConfiguration } from "@ds/performance/competencies/shared/employee-performance-configuration.model";
import { IOneTimeEarningSettings } from "@ds/performance/competencies/shared/onetime-earning-settings";
import { IncreaseType } from "@ds/performance/competencies/shared/increase-type";
import { BasedOn } from "@ds/performance/competencies/shared/based-on";
import { Measurement } from "@ds/performance/competencies/shared/measurement";
@Component({
    selector: 'ds-performance',
    templateUrl: './performance.component.html',
    styleUrls: ['./performance.component.scss']
})
export class PerformanceComponent implements OnInit, AfterViewChecked {

    mainStyle:IStyleAsset;
    competencies:ICompetency[];
    user: UserInfo;
    hasEarningsData: boolean;
    oneTimeEarningSettings: IOneTimeEarningSettings = <IOneTimeEarningSettings>{};
    hasAdditionalEarnings: boolean;
    public increaseType = IncreaseType;
    public increaseTypeKeys: any;
    public basedOn = BasedOn;
    public basedOnKeys: any;
    public measurement = Measurement;
    public measurementKeys: any;

    constructor(private styles: DsStyleLoaderService, private service: PerformanceReviewsService, private accountService: AccountService) {
        this.increaseTypeKeys = Object.keys(this.increaseType).filter(k => !isNaN(Number(k))).map(k => Number(k));
        this.basedOnKeys = Object.keys(this.basedOn).filter(k => !isNaN(Number(k))).map(k => Number(k));
        this.measurementKeys = Object.keys(this.measurement).filter(k => !isNaN(Number(k))).map(k => Number(k));
    }

    ngOnInit() {
        this.hasEarningsData = false;
        this.competencies = [],
        this.accountService.getUserInfo().subscribe(u => {
            this.user = u;

            this.service.getCompetenciesByEmployee(u.selectedClientId(), u.employeeId)
                .subscribe(comps => {
                    this.sortCompetencies(comps);
                });
            this.service.getOneTimeEarningSettings(u.selectedClientId(), u.employeeId)
                .subscribe(result => {
                    if (result) {
                        this.hasAdditionalEarnings = result.hasAdditionalEarnings;
                        if (result.oneTimeEarningSettings && result.oneTimeEarningSettings.displayInEss) {
                            this.hasEarningsData = true;
                            this.oneTimeEarningSettings = result.oneTimeEarningSettings;
                        }
                        else {
                            this.hasEarningsData = false;
                            this.initiateOnetimeSettings();
                        }
                    }
                    else {
                        this.hasAdditionalEarnings = false;
                        this.initiateOnetimeSettings();
                    }

                });
        });
    }

    getBasedOnLabel(basedOn: BasedOn): string {
        switch(basedOn){
          case BasedOn.Salary:
            return "Base Pay"
            default:
              throw new Error("Invalid Based On value");
        }
      }

    /**
     * We tell DsStyleLoaderService that this component should use main stylesheet AFTER OnInit and AfterViewInit
     * because we need to make sure that everything is resolved above this component. The DsStyleLoaderService is not
     * instantiated until after OutletComponent is finished loading.
     */
    ngAfterViewChecked() {
        this.styles.useMainStyleSheet();
    }

    private sortCompetencies(comps:ICompetency[]):void {
        this.competencies = _.sortBy(comps, ['name']);
    }

    private initiateOnetimeSettings() {
        this.oneTimeEarningSettings = {
            oneTimeEarningSettingsId: 0,
            employeeId: 0,
            name: "",
            increaseType: 0,
            increaseAmount: 0,
            basedOn: 0,
            measurement: 0,
            displayInEss: false,
            isArchived: false
        };
    }

}

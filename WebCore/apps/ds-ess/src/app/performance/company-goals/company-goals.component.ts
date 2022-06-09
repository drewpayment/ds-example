import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { GoalService } from '@ds/performance/goals/shared/goal.service';
import { PERFORMANCE_ACTIONS } from '@ds/performance/shared/performance-actions';

@Component({
    selector: 'ds-ess-company-goals',
    templateUrl: './company-goals.component.html',
    styleUrls: ['./company-goals.component.scss']
})
export class CompanyGoalsComponent implements OnInit {

    user:UserInfo;
    isLoading:boolean = true;
    hasGoals:boolean = false;
    canReadGoals:boolean = false;

    constructor(private dialog:MatDialog, private goalService:GoalService, private msg:DsMsgService, private accountService:AccountService) {
        /**
         * We subscribe to the companyGoals subject on GoalService to check if there are goals.
         * However, we do not explicitly make the API call to get goals for the company goals list,
         * because we can wait for the CompanyGoalsListComponent to make that API call and we will
         * get the result. This will save us an unnecessary API call.
         */
        this.goalService.companyGoals$.subscribe(goals => {
            this.isLoading = false;
            this.hasGoals = goals.length > 0;
        });
    }

    ngOnInit() {
        this.accountService.getAllowedActions().subscribe(permissions => {
            permissions.forEach((p, i, a) => {
                if(p != PERFORMANCE_ACTIONS.GoalTracking.ReadGoals) return;
                this.canReadGoals = true;
            });

            this.accountService.getUserInfo().subscribe(user => {
                this.user = user;
            });
        });
    }

}

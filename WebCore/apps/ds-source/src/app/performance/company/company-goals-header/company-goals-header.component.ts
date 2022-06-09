import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { CompanyGoalsListComponent } from '@ds/performance/goals/company-goals-list/company-goals-list.component';
import { GoalService } from '@ds/performance/goals/shared/goal.service';
import { AddGoalDialogComponent } from '@ds/performance/goals/add-goal-dialog/add-goal-dialog.component';
import { PERFORMANCE_ACTIONS } from '@ds/performance/shared/performance-actions';

@Component({
    selector: 'ds-company-goals-header',
    templateUrl: './company-goals-header.component.html',
    styleUrls: ['./company-goals-header.component.scss']
})
export class CompanyGoalsHeaderComponent implements OnInit {

    user: UserInfo;
    isLoading:boolean = true;
    hasGoals:boolean = false;
    @ViewChild('companyGoalList', { static: true }) goalListComponent:CompanyGoalsListComponent;
    archiveButtonText:string;
    viewArchive:boolean = false
    canAddGoals: boolean = false;

    constructor(
        private accountService:AccountService, 
        private dialog:MatDialog, 
        private goalService:GoalService,
        private msg:DsMsgService
    ) { 
        /**
         * We subscribe to the companyGoals subject on GoalService to check if there are goals. 
         * However, we do not explicitly make the API call to get goals for the company goals list, 
         * because we can wait for the CompanyGoalsListComponent to make that API call and we will 
         * get the result. This will save us an unnecessary API call. 
         */
        this.goalService.companyGoals$.subscribe(goals => {
            this.isLoading = false;
        }); 
        this.goalService.hasGoals$.subscribe(hasGoals=>{
            this.hasGoals = hasGoals;
        });
        this.archiveButtonText = this.viewArchive ? "Active Goals":"View Archive";
    }

    ngOnInit() {        
        this.accountService.getUserInfo().subscribe(user => {
            this.user = user;
        });

        this.accountService.canPerformActions(PERFORMANCE_ACTIONS.GoalTracking.WriteGoals).subscribe((x: boolean) => {
            this.canAddGoals = x === true;
        });
    }

    addCompanyGoal(): void {
        this.dialog.open(AddGoalDialogComponent, {
            width: '500px',
            disableClose: true,
            data: {
                user: this.user,
                isCompanyGoal: true
            }
        })
        .afterClosed()
        .subscribe(result => {
            if(result == null) return;
            this.msg.loading(true);
            this.goalService
                .saveClientGoal(result, this.user.selectedClientId());
        });
    }

    swapDisplay(){
        this.viewArchive = !this.viewArchive;
        this.goalListComponent.swapDisplay();
        this.archiveButtonText = this.viewArchive ? "Active Goals":"View Archive";
    }
}

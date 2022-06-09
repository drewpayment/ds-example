import { Component, OnInit, AfterViewInit, AfterViewChecked } from '@angular/core';
import { DsStyleLoaderService } from '@ajs/ui/ds-styles/ds-styles.service';
import { AccountService } from '@ds/core/account.service';
import { PERFORMANCE_ACTIONS } from '@ds/performance/shared/performance-actions';

@Component({
    selector: 'app-goals',
    templateUrl: './goals.component.html',
    styleUrls: ['./goals.component.scss']
})
export class GoalsComponent implements OnInit {

    canReadGoals = false;

    constructor(private accountService: AccountService) { }

    ngOnInit() {
        this.accountService.getAllowedActions().subscribe(permissions => {
            permissions.forEach((p, i, a) => {
                if(p != PERFORMANCE_ACTIONS.GoalTracking.ReadGoals) return;
                this.canReadGoals = true;
            });
        });
    }

}

import { Component, OnInit } from '@angular/core';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { ReviewStatusType, IReviewSearchOptions, IReviewGroupStatus } from '@ds/performance/performance-manager';
import { PerformanceManagerService } from '@ds/performance/performance-manager/performance-manager.service';

@Component({
    selector: 'ds-archive-outlet',
    templateUrl: './archive-outlet.component.html',
    styleUrls: ['./archive-outlet.component.scss']
})
export class ArchiveOutletComponent implements OnInit {
    statuses = [ReviewStatusType.Closed];
    searchOptions: IReviewSearchOptions;
    reviewGroups: IReviewGroupStatus[];
    displayedColumns;

    constructor(
        private manager: PerformanceManagerService,
        private msgSvc: DsMsgService) { }

    ngOnInit() {
        this.manager.activeReviewSearchOptions$.subscribe(options => {
            this.searchOptions = options || {};

            this.updateReviewStatusList();
        });
    }

    updateReviewStatusList(successMsg?:string) {
        this.manager.searchReviewStatus(this.searchOptions).subscribe(groups => {
            this.reviewGroups = groups;

            if (successMsg)
                this.msgSvc.setTemporarySuccessMessage(successMsg);
        });
    }

    reviewChanged() {
        this.updateReviewStatusList("Review updated successfully.");
    }
}

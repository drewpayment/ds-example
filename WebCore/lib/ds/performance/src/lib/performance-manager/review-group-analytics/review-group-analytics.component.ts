import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { IReviewGroupStatus } from '../shared/review-group-status.model';
import { IProfileImage } from '@ds/core/contacts';
import { IScoreGroup } from '@ds/performance/evaluations/shared/score-group.model';
import { IReviewStatus } from '@ds/performance/performance-manager/shared/review-status.model';
import { EvaluationRoleType } from '@ds/performance/evaluations';
import { EmployeeSearchFilterType, IEmployeeSearchResult } from '@ajs/employee/search/shared/models';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';


interface IAnalyticsReviewItem {
    firstName: string;
    lastName: string;
    score: number;
    profileImage: IProfileImage;

    [key: string]: any;
}


@Component({
    selector: 'ds-review-group-analytics',
    templateUrl: './review-group-analytics.component.html',
    styleUrls: ['./review-group-analytics.component.scss']
})
export class ReviewGroupAnalyticsComponent implements OnInit {

    private _defaultColumns = ['avatar', 'name'];
    private _defaultPageSizeOptions = [10, 25, 50, 100 ];
    private _group: IReviewGroupStatus;
    private _data: IAnalyticsReviewItem[];
    private _isScoringEnabled: boolean = false;
    datasource = new MatTableDataSource<IAnalyticsReviewItem>([]);
    displayColumns = [];

    additionalColumns: { [key:string]:string };


    @Input()
    set group (group: IReviewGroupStatus) {
        this._group = group;
        this.updateDatasource();
    }

    @ViewChild(MatSort, { static: true }) 
    sort: MatSort;
    @ViewChild(MatPaginator, { static: true })
    paginator: MatPaginator;

    get pageSizeOptions() {
        return this._data && this._data.length > 100 ? [...this._defaultPageSizeOptions, this._data.length] : this._defaultPageSizeOptions;
    }
    get pagingLength() {
        return this._data ? this._data.length : 0;
    }
    
    constructor(
        private service: PerformanceReviewsService
    ) { }

    ngOnInit() {

        this.datasource.sort = this.sort;
        this.datasource.paginator = this.paginator;

        this.datasource.sortingDataAccessor = (item, sortColumn) => {
            switch (sortColumn) {
                case 'name':
                    return `${item.lastName} ${item.firstName}`;
                case 'supervisor_evaluator':
                case 'employee_evaluator': 
                    let contact = item[this.sort.active];
                    return contact ? `${contact.lastName} ${contact.firstName}` : null;
                case 'avatar': 
                    return 0;
                default: 
                    return item[this.sort.active];
            }
        };
    }

    updateDatasource() {
        this._data = [];

        if (!this._group)
            return;

        this.service.isScoringEnabledForReviewTemplate(this._group.reviewTemplateId).subscribe(x => {
            this._isScoringEnabled = x.data;

            this.additionalColumns = {};
            this.additionalColumns.employeeNumber = "Number";

            if (this._isScoringEnabled) {
                this.additionalColumns.score = "Overalll Score";
            }

            this._group.reviews.forEach(r => {

                let item = {
                    firstName: r.employee.firstName,
                    lastName: r.employee.lastName,
                    employeeNumber: +r.employee.employeeNumber,
                    score: r.score ? r.score.score : null,
                    profileImage: r.review.reviewedEmployeeContact.profileImage
                };

                if (this._isScoringEnabled) {
                    this.setScoreColumns(item, r.score);
                }

                this.setEvalColumns(item, r);
                this.setEmployeeInfoColumns(item, r.employee);

                this._data.push(item);
            });

            this._data.forEach(d => {
                Object.keys(this.additionalColumns).forEach(k => {
                    if (typeof d[k] === 'undefined')
                        d[k] = null;
                })
            })

            this.displayColumns = [...this._defaultColumns, ...Object.keys(this.additionalColumns)];
            this.datasource.data = this._data;
        });
    }

    getColumnType(columnKey: string) {
        if (columnKey.indexOf("score") > -1) {
            return 'score';
        }
        else if (columnKey.indexOf("_evaluator") > -1) {
            return 'evaluator';
        }
        else if (columnKey.indexOf("_date") > -1) {
            return 'date';
        }
        return null;
    }

    private setScoreColumns(dataItem: IAnalyticsReviewItem, group: IScoreGroup, parentPropKey:string = null) {
        if (group && group.items) {
            (<IScoreGroup[]>group.items).forEach((g, idx) => {
                if (g.items) {
                    let propKey = `${parentPropKey ? parentPropKey : "score"}_${idx}`;
                    dataItem[propKey] = g.score;
                    this.additionalColumns[propKey] = g.name;

                    this.setScoreColumns(dataItem, g, propKey);
                }
            });
        }
    }

    private setEvalColumns(dataItem: IAnalyticsReviewItem, reviewStatus: IReviewStatus) {
        if (reviewStatus.review.evaluations) {
            reviewStatus.review.evaluations.forEach(ev => {
                switch(ev.role) {
                    case EvaluationRoleType.Manager: {

                        if (ev.evaluatedByContact) {
                            let evaluatorProp = "supervisor_evaluator";
                            this.additionalColumns[evaluatorProp] = "Supervisor Evaluator";
                            dataItem[evaluatorProp] = ev.evaluatedByContact;
                        }

                        let completeProp = "supervisor_complete_date";
                        this.additionalColumns[completeProp] = "Supervisor Submitted";
                        dataItem[completeProp] = ev.completedDate;
                        break;
                    }
                    case EvaluationRoleType.Self: {
                        // let evaluatorProp = "employee_evaluator";
                        // this.additionalColumns[evaluatorProp] = "Employee Evaluator";
                        // dataItem[evaluatorProp] = ev.evaluatedByContact;

                        let completeProp = "employee_complete_date";
                        this.additionalColumns[completeProp] = "Employee Submitted";
                        dataItem[completeProp] = ev.completedDate;
                        break;
                    }
                }
            });
        }
    }

    private setEmployeeInfoColumns(dataItem: IAnalyticsReviewItem, employee: IEmployeeSearchResult) {
        employee.groups.forEach(g => {
            switch(g.filterType) {
                case EmployeeSearchFilterType.JobProfile:
                    dataItem.jobTitle = g.name;
                    this.additionalColumns.jobTitle = "Job Title";
                    break;
            }
        })
    }
}

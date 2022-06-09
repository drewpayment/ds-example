import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { IReviewSearchOptions, IReviewStatusSearchOptions,REVIEW_SEARCH_OPTIONS_KEY } from './shared/review-search-options.model';
import { HttpClient, HttpParams, HttpRequest } from '@angular/common/http';
import { map, filter } from 'rxjs/operators';
import { ProfileImageLoaderPipe, ContactProfileImageLoader, ContactsProfileImageLoader, IContact, IContactSearchResult } from '@ds/core/contacts';
import { IReviewGroupStatus } from '@ds/performance/performance-manager/shared/review-group-status.model';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { SelectionModel } from '@angular/cdk/collections';
import { IEmployeeDirectSupervisorLink } from '@ds/performance/performance-manager/shared/employee-direct-supervisor-link.model';
import { Maybe } from '@ds/core/shared/Maybe';
import { IReviewTemplate } from '../review-policy';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { IFeedbackResponseData } from '@ds/performance/feedback';
import { IPayrollRequest } from './shared/payroll-request.model';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { StoreActionNoType } from '@ds/core/shared/store-builder';
import { FormGroup } from '@angular/forms';
import { IEmployeeSearchResult, IEmployeeSearchResultResponseData } from '@ds/employees/search/shared/models/employee-search-result';
import { EmployeeSearchFilterType } from '@ds/employees/search/shared/models/employee-search-filter-type';
import { IEmployeeSearchFilter } from '@ajs/employee/search/shared/models';

@Injectable({
    providedIn: 'root'
})
export class PerformanceManagerService {

    private readonly API = 'api/performance/manager';

    private _activeReviewSearchOptions = new BehaviorSubject<IReviewSearchOptions>(null);

    // This contains the user's currently stored search filters in the browser's localstorage
    activeReviewSearchOptions$ = this._activeReviewSearchOptions.asObservable();

    /** Internal Use --- DO NOT USE */
    private _selectedReviewTemplate = new BehaviorSubject<IReviewTemplate>(null);
    set selectedReviewTemplate(value:IReviewTemplate) {
        this._selectedReviewTemplate.next(value);
    }
    selectedReviewTemplate$ = this._selectedReviewTemplate.asObservable();

    private _filterEnabled:boolean = false;
    set filterEnabled(value:boolean) {
        this._filterEnabled = coerceBooleanProperty(value);
    }
    get filterEnabled():boolean {
        return this._filterEnabled;
    }

    private _employeeSearchResponse = new BehaviorSubject<IEmployeeSearchResultResponseData>(null);
    employeeSearchResponse$ = this._employeeSearchResponse.asObservable();
    set employeeSearchResponse(value:IEmployeeSearchResultResponseData) {
        this._employeeSearchResponse.next(value);
    }

    filterOptionsFormGroup: FormGroup;


    constructor(private http:HttpClient, private store: DsStorageService) { }

    setReviewSearchOptions(options: IReviewSearchOptions) {
        this._activeReviewSearchOptions.next(options);
        this.store.set(REVIEW_SEARCH_OPTIONS_KEY, options);
    }

    searchEmployees(options?:IReviewSearchOptions):Observable<IEmployeeSearchResultResponseData> {
        options = options || {};

        if (options.reviewTemplateId == null) {
            this.employeeSearchResponse = <IEmployeeSearchResultResponseData>{};
            return of(this.employeeSearchResponse);
        }

        let params = new HttpParams()
            .set('reviewTemplateId', options.reviewTemplateId == null ? '' : options.reviewTemplateId.toString())
            .set('sortBy', options.sortBy)
            .set('sortDirection', options.sortDirection.toString())
            .set('page', options.page == null ? '' : options.page.toString())
            .set('pageSize', options.pageSize == null ? '' : options.pageSize.toString())
            .set('isActiveOnly', (!!options.isActiveOnly).toString())
            .set('isExcludeTemps', (!!options.isExcludeTemps).toString())
            .set('searchText', options.searchText == null ? '' : options.searchText)
            .set('startDate', convertToMoment(options.startDate).format('YYYY-MM-DDTHH:mm:ss'))
            .set('endDate', convertToMoment(options.endDate).format('YYYY-MM-DDTHH:mm:ss'));

        options.filters && options.filters.length && options.filters.forEach(f => {
          if (isNaN(f as any) && f.$selected) {
            const selectedId = f.$selected.id;
            params = params.append(`${f.filterType}`, `${selectedId}`);

          } else if (!isNaN(f as any)) {
            const filterId = f as any;
            params = params.append(filterId, options.filters[filterId].toString());

          }
        });

        return this.http.get<IEmployeeSearchResultResponseData>(`${this.API}/employees`, { params: params })
            .pipe(
                map(response => {
                    response.results.forEach(employee => {
                        ProfileImageLoaderPipe.EmployeeProfileImageLoader(employee);
                        this.populateFilterData(employee);
                    });

                    this.employeeSearchResponse = response;

                    return response;
                })
            );
    }

    retrieveFeedbacks(options?:IReviewSearchOptions):Observable<Array<IFeedbackResponseData>> {
        options = options || {};

        if (options.reviewTemplateId == null) {
            return of(<Array<IFeedbackResponseData>>[]);
        }

        let params = new HttpParams()
            .set('reviewTemplateId', options.reviewTemplateId == null ? '' : options.reviewTemplateId.toString())
            .set('searchText', options.searchText == null ? '' : options.searchText)
            .set('isActiveOnly', (!!options.isActiveOnly).toString())
            .set('isExcludeTemps', (!!options.isExcludeTemps).toString())
            .set('startDate', convertToMoment(options.startDate).format('YYYY-MM-DDTHH:mm:ss'))
            .set('endDate', convertToMoment(options.endDate).format('YYYY-MM-DDTHH:mm:ss'));

        options.filters && options.filters.length && options.filters.forEach(f => {
          if (isNaN(f as any) && f.$selected) {
            const selectedId = f.$selected.id;
            params = params.append(`${f.filterType}`, `${selectedId}`);

          } else if (!isNaN(f as any)) {
            const filterId = f as any;
            params = params.append(filterId, options.filters[filterId].toString());

          }
        });

        //if (typeof options.includeScores !== 'undefined')
        //    params = params.append('includeScores', (!!options.includeScores).toString());

        return this.http.get<Array<IFeedbackResponseData>>(`${this.API}/feedbacks`, { params: params });
    }

    private populateFilterData(employee:IEmployeeSearchResult) {
        employee.groups.forEach(g => {
            switch(g.filterType) {
                case EmployeeSearchFilterType.Division:
                    employee.divisionName = g.name;
                    break;
                case EmployeeSearchFilterType.Department:
                    employee.departmentName = g.name;
                    break;
                case EmployeeSearchFilterType.JobTitle:
                    employee.jobTitle = g.name;
                    break;
                case EmployeeSearchFilterType.PayType:
                    employee.payType = g.id;
                    break;
                case EmployeeSearchFilterType.CompetencyModel:
                    employee.competencyModel = { competencyModelId: g.id, name: g.name };
                    break;
                default:
                    break;

            }
        });
    }

    searchReviewStatus(options?:IReviewStatusSearchOptions):Observable<IReviewGroupStatus[]> {
        options = options || {};

        if (options.reviewTemplateId == null) {
            return of(<Array<IReviewGroupStatus>>[]);
        }

        let params = new HttpParams()
            .set('reviewTemplateId', options.reviewTemplateId == null ? '' : options.reviewTemplateId.toString())
            .set('searchText', options.searchText == null ? '' : options.searchText)
            .set('isActiveOnly', (!!options.isActiveOnly).toString())
            .set('isExcludeTemps', (!!options.isExcludeTemps).toString())
            .set('startDate', convertToMoment(options.startDate).format('YYYY-MM-DDTHH:mm:ss'))
            .set('endDate', convertToMoment(options.endDate).format('YYYY-MM-DDTHH:mm:ss'));

        options.filters && options.filters.length && options.filters.forEach(f => {
          if (isNaN(f as any) && f.$selected) {
            const selectedId = f.$selected.id;
            params = params.append(`${f.filterType}`, `${selectedId}`);

          } else if (!isNaN(f as any)) {
            const filterId = f as any;
            params = params.append(filterId, options.filters[filterId].toString());

          }
        });

        if (typeof options.includeScores !== 'undefined')
            params = params.append('includeScores', (!!options.includeScores).toString());

        return this.http.get<IReviewGroupStatus[]>(`${this.API}/review-status`, { params: params })
            .pipe(
                map(groups => {
                    var groupsM = new Maybe(groups);
                    groupsM.map(groups => groups.forEach(group => {
                        group.reviews.forEach(reviewStatus => {
                            //TODO: this is copied from ReviewService ... consolidate
                            let review = reviewStatus.review;
                            ContactProfileImageLoader(review.reviewedEmployeeContact);
                            ContactProfileImageLoader(review.reviewOwnerContact);

                            if(review.evaluations) {
                                review.evaluations.forEach(evl => ContactProfileImageLoader(evl.evaluatedByContact));
                            }

                            if(review.meetings) {
                                /** the typescript error was bugging me... Drew */
                                review.meetings.forEach(meeting => ContactsProfileImageLoader(meeting.attendees as IContact[]));
                            }
                        });
                    }));
                    return groupsM.value();
                })
            );
    }



    downloadReviewStatusCsvUrl(options?:IReviewStatusSearchOptions):string {
        options = options || {};

        let params = new HttpParams()
            .set('reviewTemplateId', options.reviewTemplateId == null ? '' : options.reviewTemplateId.toString())
            .set('searchText', options.searchText == null ? '' : options.searchText)
            .set('isActiveOnly', (!!options.isActiveOnly).toString())
            .set('isExcludeTemps', (!!options.isExcludeTemps).toString());

        options.filters && options.filters.length && options.filters.forEach(f => {
          if (isNaN(f as any) && f.$selected) {
            const selectedId = f.$selected.id;
            params = params.append(`${f.filterType}`, `${selectedId}`);

          } else if (!isNaN(f as any)) {
            const filterId = f as any;
            params = params.append(filterId, options.filters[filterId].toString());

          }
        });

        if (typeof options.includeScores !== 'undefined')
            params = params.append('includeScores', (!!options.includeScores).toString());

        return new HttpRequest("GET", `${this.API}/review-status/csv`, { params: params }).urlWithParams;
    }

    saveEmloyeeDirectSupervisor(dto: IEmployeeDirectSupervisorLink) {
        return this.http.post<IEmployeeDirectSupervisorLink>(`${this.API}/${dto.employeeId}/directSupervisor/${dto.directSupervisorId}`, dto);
    }

    /**
     * Get the list of direct supervisors for current client.
     */
    getDirectSupervisors(overrideGetSubords?: Boolean):Observable<IContact[]> {

        var params = new HttpParams();
        params = params.set('onlySupsWithActiveEmployee', true.toString());
        params = params.set('excludeTimeClockOnly', true.toString());
        if (overrideGetSubords == null || !overrideGetSubords)
            params = params.set('ifSupervisorGetSubords', true.toString());

        return this.http.get<IContact[]>(`${this.API}/supervisors`, {params: params});
    }

}

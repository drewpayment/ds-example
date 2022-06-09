import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { TimeAndAttendanceService } from './time-and-attendance.service';
import { Observable, Subject, of, combineLatest, defer, fromEvent, from, Subscription, merge } from 'rxjs';
import { ComponentData, MappedComponentData } from './shared/ComponentData.model';
import { withLatestFrom, tap, concatMap, map, switchMap, filter } from 'rxjs/operators';
import { UserInfo, UserType } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { Maybe } from '@ds/core/shared/Maybe';
import { MappedInitData, InitData } from './shared/InitData.model';
import { ClockFilter } from './shared/ClockFilter.model';
import { DataRow } from './shared/DataRow.model';
import { ClientService } from '@ds/core/clients/shared';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { ApproveHourSettings } from './shared/ApproveHourSettings.model';
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';
import { MatDialog } from '@angular/material/dialog';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';
import { DsPopupService } from '@ajs/ui/popup/ds-popup.service';

@Component({
    selector: 'ds-time-and-attendance',
    templateUrl: './time-and-attendance.component.html',
    styleUrls: ['./time-and-attendance.component.scss']
})
export class TimeAndAttendanceComponent implements OnInit, OnDestroy {
    params$: Observable<any>;

    constructor(
        public timeAndAttSvc: TimeAndAttendanceService,
        private acctSvc: AccountService,
        private clientService: ClientService,
        private route: ActivatedRoute,
        private dialog: MatDialog,
        private DsPopup: DsPopupService,
    ) {}

    showPunchImportBtn = false;
    data$: Observable<MappedComponentData> = combineLatest(
        this.timeAndAttSvc.data$,
        this.acctSvc.getUserInfo(),
        defer(() => this.params$))
        .pipe(
            map(x => this.select(x[0], x[1], x[2]))
    );
    imperativeHook: Subject<any> = new Subject();
    saveBtn2: Subject<any> = new Subject();
    save: Subject<any> = new Subject();
    childForm: Subject<any> = new Subject();
    startDate: string;
    endDate: string;
    btnListenter$: Subscription;
    currentPage = 0;
    private lastSearchVals: any;
    hasSearchedEmps = false;
    @Output() popUpClosed: EventEmitter<any> = new EventEmitter();

    ngOnInit() {

        this.params$ = this.route.paramMap.pipe(
            map((params: ParamMap) => {
                const supervisor = params.get('supervisorId');
                const isApproved = params.get('isApproved');

                return {
                    supervisor,
                    isApproved
                };
            }));

        const enterPress = fromEvent(document, 'keydown').pipe(filter((k: KeyboardEvent) => k.keyCode === 13));
        const buttonPress = this.imperativeHook.asObservable();
        this.btnListenter$ = merge(enterPress, buttonPress).pipe(
            withLatestFrom(this.childForm),
            withLatestFrom(this.data$),
            map(x => {
                const tableData = new Maybe(x[1]).map(x => x.tableData);
                const forms = new Maybe(x[0][1])
                    .map(x => Object.keys(x).map(id => +id))
                    .map(x => {
                        const result: DataRow[] = [];
                        x.forEach(y => {
                            const found = tableData.map(data => data.table).map(rows => rows.find(row => row.id === y)).value();
                            if (found != null)
                                result.push(found);
                        });
                        return result;
                    });
                const result = {
                    form: x[0][1],
                    dataRows: forms.value(),
                    startDate: this.startDate,
                    endDate: this.endDate
                };
                return result;
            }),
            tap(x => {
                this.timeAndAttSvc.save(x);
            })).subscribe();

        this.acctSvc.getUserInfo().pipe(
            switchMap(user => {
                return this.clientService.getClientAccountFeatureByFeatureId(user.selectedClientId(), Features.PunchImport);
            })
        ).subscribe(punchImportFeature => {
            this.showPunchImportBtn = punchImportFeature != null;
        });
    }

    ngOnDestroy() {
      if (this.btnListenter$) this.btnListenter$.unsubscribe();
    }

    private select(data: ComponentData, userInfo: UserInfo, angularRouteParams: any): MappedComponentData {
        const result: MappedComponentData = {
            initData: new Maybe(data).map(x => (<MappedInitData>{
                clockFilterCategory1: this.getClockFilterCategoryShallowCopy(this.getInitDataFromSource(x))
                    .map(y => this.addDefaultCategory1Options(y, userInfo)).value(),
                clockFilterCategory2: this.getClockFilterCategoryShallowCopy(this.getInitDataFromSource(x))
                    .map(this.addDefaultCategory2Options).value(),
                clientJobCostingInfoResults: this.getInitDataFromSource(x).map(y => y.clientJobCostingInfoResults).value(),
                clockEmployeeApproveHoursOptions: this.getInitDataFromSource(x).map(y => y.clockEmployeeApproveHoursOptions).value(),
                clockEmployeeApproveHoursSettings: this.getInitDataFromSource(x).map(y => y.clockEmployeeApproveHoursSettings).value(),
                clockFilterCategory: this.getClockFilterCategoryShallowCopy(this.getInitDataFromSource(x)).value(),
                clockPayrollList: this.getInitDataFromSource(x).map(y => y.clockPayrollList).value(),
                clientNotes: this.getInitDataFromSource(x).map(y => y.clientNotes).value()
            })).value(),
            tableData: new Maybe(data).map(x => x.tableData).value(),
            filterValues: new Maybe(data).map(x => x.filterValues).value(),
            angularRouteParams: angularRouteParams
        };

        return result;
    }

    private getInitDataFromSource(source: ComponentData): Maybe<InitData> {
        return new Maybe(source).map(x => x.initData);
    }

    private getClockFilterCategoryShallowCopy(source: Maybe<InitData>): Maybe<ClockFilter[]> {
        return source.map(y => y.clockFilterCategory).map(y => y.slice());
    }

    private readonly addDefaultCategory1Options = (data: ClockFilter[], userInfo: UserInfo) => {
        data.unshift({ clockFilterID: 0, description: 'All Employees', whereClause: null, idx: null, value: null });
        data.unshift({ clockFilterID: 7, description: 'Select Employee', whereClause: null, idx: null, value: null });
        if (userInfo.userTypeId === UserType.companyAdmin || userInfo.userTypeId === UserType.systemAdmin)
            data.unshift({ clockFilterID: -2, description: 'Unassigned', whereClause: null, idx: null, value: null });

        return data;
    }

    private readonly addDefaultCategory2Options = (data: ClockFilter[]) => {
        data.unshift({ clockFilterID: 0, description: '--No 2nd Filter--', whereClause: null, idx: null, value: null });
        return data;
    }

    searchEmps(formData: any): void {
        this.hasSearchedEmps = true;
        this.startDate = formData.startDate;
        this.endDate = formData.endDate;
        this.lastSearchVals = formData;
        this.timeAndAttSvc.searchEmps(formData);
    }

    loadFilterIds(formVal: any): void {
        this.timeAndAttSvc.loadFilterOptions(formVal);
    }

    loadFilter2Options(formVal: any): void {
        this.timeAndAttSvc.loadFilter2Options(formVal);
    }

    saveDisplaySettings(formVal: ApproveHourSettings): void {
        this.timeAndAttSvc.saveDisplaySettings(formVal);
    }

    loadPage(page: number): void {
        this.currentPage = this.lastSearchVals.currentPage = page;
        this.searchEmps(this.lastSearchVals);
    }

    openImportPunchModal() {
      // this.openModal('ModalContainer.aspx?URL=PunchImport.aspx',
      //       'test', 'height=500px,width=650px,toolbar=no,menubar=no,scrollbars=yes,location=no,status=no')
      //       .onbeforeunload = () => { this.emitPopUpClosed(); };
      this.openModal('ModalContainer.aspx?URL=PunchImport.aspx');

    }

    private emitPopUpClosed(): void {
        this.popUpClosed.emit(null);
    }

    openModal(url: String) {
      const w = window,
        d = document,
        e = d.documentElement,
        g = d.getElementsByTagName("body")[0],
        x = 723,
        y = 550,
        xt = w.innerWidth || e.clientWidth || g.clientWidth,
        yt = w.innerHeight || e.clientHeight || g.clientHeight;
  
      const left = (xt - x) / 2;
      const top = (yt - y) / 4;
  
      this.acctSvc.getSiteUrls()
        .subscribe(sites => {
          const baseUrl = sites.find(s => s.siteType == ConfigUrlType.Payroll);
          url = `${baseUrl.url}${url}`;
          const modal = this.DsPopup.open(url, "_blank", {
            height: y,
            width: x,
            top: top,
            left: left,
          });
        });
    }
}

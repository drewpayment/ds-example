import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { PerformanceReviewsService } from '../../shared/performance-reviews.service';
import { AccountService } from "@ds/core/account.service";
import { Observable, forkJoin, fromEvent, throwError, Subject } from 'rxjs';
import { ICompetencyModel } from '../shared/competency-model.model';
import { concatMap, map, exhaustMap, catchError, tap, merge, takeUntil, take, share } from 'rxjs/operators';
import { ICompetency } from '../shared/competency.model';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { EmployeePerformanceConfiguration } from '../shared/employee-performance-configuration.model';
import { PERFORMANCE_ACTIONS } from '@ds/performance/shared/performance-actions';
import { UserInfo, UserType } from '@ds/core/shared';
import { IncreaseType } from '../shared/increase-type';
import { BasedOn } from '../shared/based-on';
import { Measurement } from '../shared/measurement';
import { IOneTimeEarningSettings } from '../shared/onetime-earning-settings';
import { Maybe } from '@ds/core/shared/Maybe';

@Component({
  selector: 'ds-competency-model-assign-employee',
  templateUrl: './competency-model-assign-employee.component.html',
  styleUrls: ['./competency-model-assign-employee.component.scss']
})
export class CompetencyModelAssignEmployeeComponent implements OnInit {
  private _currentUser: UserInfo;
  private _canEdit = false;
  private _employeeId: number;
  hasAdditionalEarnings: boolean = false;
  showAdditionalEarning: boolean = false;
  public models$: Observable<ICompetencyModel[]>;
  public origEmpConfig$: Observable<EmployeePerformanceConfiguration>;
  public coreComps$: Observable<ICompetency[]>;
  public additionalEarnings$: Observable<boolean>;
  public data$: Observable<[ICompetencyModel[], ICompetency[], EmployeePerformanceConfiguration, boolean]>;
  public selectedComp: ICompetencyModel;
  public increaseType= IncreaseType;
  public increaseTypeKeys: any;
  public basedOn = BasedOn;
  public basedOnKeys: any;
  public measurement = Measurement;
  public measurementKeys: any;
  public oneTimeEarningSettings: IOneTimeEarningSettings = <IOneTimeEarningSettings>{};
  public compSettings: EmployeePerformanceConfiguration = <EmployeePerformanceConfiguration>{};

  public get selectedCompName() {
    return this.selectedComp && this.selectedComp.name ? this.selectedComp.name : "Not Assigned";
  }
  public get canEditModelSelection() {
    return this._currentUser && this._canEdit;
  }

  public get CAAndSupervisorIsViewingOwnRecord(){
    return this._currentUser && (this._currentUser.userTypeId === UserType.supervisor || this._currentUser.userTypeId === UserType.companyAdmin) && this._currentUser.selectedEmployeeId() === this._currentUser.userEmployeeId;
  }

  @ViewChild('saveBtn', { static: true }) saveBtn: ElementRef;

  constructor(
    private perfService: PerformanceReviewsService,
    private msg: DsMsgService,
    private accountSvc: AccountService) {
      this.increaseTypeKeys = Object.keys(this.increaseType).filter(k => !isNaN(Number(k))).map(k=> Number(k));
      this.basedOnKeys = Object.keys(this.basedOn).filter(k => !isNaN(Number(k))).map(k => Number(k));
      this.measurementKeys = Object.keys(this.measurement).filter(k => !isNaN(Number(k))).map(k => Number(k));
  }

  getBasedOnLabel(basedOn: BasedOn): string {
    switch(basedOn){
      case BasedOn.Salary:
        return "Base Pay"
        default:
          throw new Error("Invalid Based On value");
    }
  }

  ngOnInit() {
    this.additionalEarnings$ = this.perfService.getEmployeePerformanceSettings().pipe(take(1));
    this.origEmpConfig$ = this.perfService.getEmployeePerformanceConfiguration().pipe(take(1));
    this.models$ = this.perfService.getCompetencyModelsForCurrentClient().pipe(map(models => [<ICompetencyModel>{}].concat(models.sort((a, b) => {
      return (a.name || '').toLowerCase().localeCompare((b.name || '').toLowerCase());
    }))), take(1));



    this.coreComps$ = this.accountSvc.getUserInfo()
      .pipe(
        concatMap(userInfo => {
          this._currentUser = userInfo;
          return this.perfService.getPerformanceCompetencies(userInfo.lastClientId || userInfo.clientId)
        }),
        map(competencies => competencies.filter(competency => competency.isCore)), take(1)
      );
    // add observables to display temporary message while we are getting our data.
    var tmpHandler = fromEvent<MouseEvent>(this.saveBtn.nativeElement, 'click').subscribe(evt => {
      this.msg.setTemporaryMessage('Please wait for competencies to finish loading.', MessageTypes.info, 3000);
    });

    this.data$ = forkJoin(this.models$, this.coreComps$, this.origEmpConfig$, this.additionalEarnings$).pipe(tap(value => {
      this.showAdditionalEarning = value[3];
      value[0].forEach(element => {
            element.competencies = (element.competencies || []).concat(value[1]);
            element.competencies.sort((a, b) => (a.name || '').toLowerCase().trim().localeCompare((b.name || '').toLowerCase().trim()));

            if (element.competencyModelId == (value[2] || <EmployeePerformanceConfiguration>{}).competencyModelId) {
            this.selectedComp = element;
            }
            if (!(null == value[0] || null == value[2])) {
                this._employeeId = value[2].employeeId;
                this.hasAdditionalEarnings = new Maybe(value[2]).map(x => x.oneTimeEarningSettings).map(x => !x.isArchived).valueOr(false);
                //this.hasAdditionalEarnings = value[2].hasAdditionalEarnings;
                this.oneTimeEarningSettings = this.getNonNullOneTimeEarningSettings(new Maybe(value[2]).map(x => x.oneTimeEarningSettings).value());
            }
            else {
                this._employeeId = this._currentUser.lastEmployeeId;

                this.hasAdditionalEarnings = new Maybe(value[2]).map(x => x.oneTimeEarningSettings).map(x => !x.isArchived).valueOr(false);
                this.oneTimeEarningSettings = {
                    oneTimeEarningSettingsId:0,
                    employeeId: this._currentUser.lastEmployeeId,
                    name: "",
                    increaseType: 0,
                    increaseAmount: 0,
                    basedOn: BasedOn.Salary,
                    measurement: Measurement.GoalCompletion,
                    displayInEss: false,
                    isArchived: true};
            }

            if(this._currentUser.selectedEmployeeId() == this._currentUser.employeeId){
                this.accountSvc.canPerformActions(PERFORMANCE_ACTIONS.Performance.AdministrateOwnPerformanceSetup)
                .subscribe(result => {
                    this._canEdit = (result === true);
                });
            }else{
                this.accountSvc.canPerformActions(PERFORMANCE_ACTIONS.Performance.AssignEmployeeCompetencyModel)
                .subscribe(result => {
                    this._canEdit = (result === true);
                });
            }
      });

      tmpHandler.unsubscribe();
      this.assignSaveBtnHandler();
    }));

  }

  getNonNullOneTimeEarningSettings(settings: IOneTimeEarningSettings): IOneTimeEarningSettings {
    return new Maybe(settings).valueOr({
      oneTimeEarningSettingsId:0,
      employeeId: this._currentUser.lastEmployeeId,
      name: "",
      increaseType: 0 as IncreaseType,
      increaseAmount: 0,
      basedOn: 0,
      measurement: 0,
      displayInEss: false,
      isArchived: true
} as IOneTimeEarningSettings);
  }

  private assignSaveBtnHandler(): void {

      var saveBtn$ = fromEvent<MouseEvent>(this.saveBtn.nativeElement, 'click');

      saveBtn$.pipe(exhaustMap(evt => {
          this.compSettings.competencyModelId = this.selectedComp.competencyModelId;
            new Maybe(this.oneTimeEarningSettings).map(x => x.isArchived = !this.hasAdditionalEarnings);
          this.compSettings.oneTimeEarningSettings = this.oneTimeEarningSettings;
          this.compSettings.employeeId = this._employeeId;
          this.compSettings.hasAdditionalEarnings = this.hasAdditionalEarnings;
          return this.perfService.assignCompetencyModelToEmployee(this.compSettings);
      }),
      catchError(e => {
        this.assignSaveBtnHandler();
        this.msg.setTemporaryMessage('Sorry, changes could not be saved.', MessageTypes.error, 3000);
        return throwError(e);
      })).subscribe(undefinedVal => {
        this.msg.setTemporarySuccessMessage("Changes saved successfully.");
      });
  }

}

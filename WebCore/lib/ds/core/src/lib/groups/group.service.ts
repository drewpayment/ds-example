import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject, forkJoin, from, of, throwError, ReplaySubject } from 'rxjs';
import { Group } from './shared/group.model';
import { StoreBuilder, StoreAction } from '../shared/store-builder';
import { AccountService } from '../account.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { applyMsgSvcToSaveHandler, applyMsgSvcToErrorHandler, PASaveHandler } from '../shared/shared-api-fn';
import { MatDialog } from '@angular/material/dialog';
import { filter, map, concatMap, exhaustMap, first, catchError, tap, withLatestFrom, mergeMap, finalize, take } from 'rxjs/operators';
import { GroupDialogComponent } from './group-dialog/group-dialog.component';
import { ExternalApiService } from '@ajs/ds-external-api/ds-external-api.svc';
import { ClientDivisionDto, JobProfileDto } from '@ajs/ds-external-api/models';
import { DsOnboardingAdminApiService } from '@ajs/onboarding/shared/ds-admin-api.service';
import { PayrollService } from '@ds/payroll/shared/payroll.service';
import { IGroupDialogData } from './group-dialog/group-dialog-data.model';
import { ReviewPolicyService } from '@ds/performance/review-policy/review-policy-setup-form/review-policy.service';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { Maybe } from '../shared/Maybe';
import { ClientService } from '../clients/shared/client.service';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { IReviewTemplate } from '@ds/core/groups/shared/review-template.model';
import { sortTemplates } from './shared/sort-templates.pipe';
import { IClientCostCenterDto, ClientDepartmentDto } from '@ajs/labor/punch/api';

const url = 'api/group'

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  private readonly AddGroupDispatcher = new Subject<undefined>();
  private readonly DeleteGroupDispatcher = new Subject<number>();
  private readonly UpdateGroupDispatcher = new Subject<number>();
  private readonly TemplateUpdated = new Subject<Group[]>();
  /**
   * A hook into our store streams.  This allows users of this store to pass in this dependency.
   *
   * Doing this allows us to avoid injecting the dependency and causing a circular dependency.
   */
  private readonly ServiceEmitter = new ReplaySubject<ReviewPolicyService>(1);

readonly groups$: Observable<Group[]>

  constructor(
    dialog: MatDialog,
    http: HttpClient,
    acctSvc: AccountService,
    msgSvc: DsMsgService,
    perfSvc: PerformanceReviewsService,
    apiService: ExternalApiService,
    onboardingApiSvc: DsOnboardingAdminApiService,
    payrollApiService: PayrollService,
    confirmSvc: DsConfirmService,
    clientSvc: ClientService) {
      const factory = applyMsgSvcToSaveHandler(msgSvc);
      const attachErrorHandler = applyMsgSvcToErrorHandler(msgSvc);
    const builder = new StoreBuilder<Group[]>();
    const initData$ = acctSvc.PassUserInfoToRequest(userInfo => http.get<Group[]>(`${url}/client/${userInfo.selectedClientId()}`));
    builder.setDataSource(initData$, attachErrorHandler, 'Get Client Groups');

    const addGroup = builder.scaffoldAction<Group, undefined>();

    addGroup.dispatcher$ = this.AddGroupDispatcher;
    addGroup.setupFn = x => x.pipe(
      exhaustMap(x => forkJoin(
        perfSvc.getCompetencyModelsForCurrentClient(),
        acctSvc.PassUserInfoToRequest(userInfo => from(<PromiseLike<ClientDivisionDto[]>> apiService.getClientDivision(userInfo.selectedClientId()))),
        acctSvc.PassUserInfoToRequest(userInfo => clientSvc.getClientDepartmentList(userInfo.selectedClientId())),
        acctSvc.PassUserInfoToRequest(userInfo => from(<PromiseLike<JobProfileDto[]>>apiService.getJobProfiles(userInfo.selectedClientId()))),
        acctSvc.PassUserInfoToRequest(userInfo => from(<PromiseLike<IClientCostCenterDto[]>>onboardingApiSvc.getClientCostCentersList(userInfo.selectedClientId()))),
        payrollApiService.getPayType(),
        this.ServiceEmitter.pipe(mergeMap(x => x.allTemplatesForClient$.pipe(
          map(templates => templates.filter(template => !template.isArchived)),
          map(templates => sortTemplates(templates)))),take(1)),
        of(x.currentValue)
        )),
      map(z => ({
        data: <IGroupDialogData>{
          compModels: z[0],
          divisions: z[1],
          departments: z[2],
          jobProfiles: z[3],
          costCenters: z[4],
          payTypes: z[5],
          templates: z[6]
        },
        currentValue: z[z.length - 1]
      })),
      concatMap(z => dialog.open<GroupDialogComponent, IGroupDialogData, Group>(GroupDialogComponent, {
        data: z.data
      })
    .afterClosed()
    .pipe(
      filter(x => x != null),
      map(y => ({item: y, currentValue: z.currentValue, idOrNewValue: undefined})))));
      addGroup.operation = 'Create Group';
      addGroup.normalizeSaveHandler = factory;
      addGroup.updateState = (result, staleCache) => {
        staleCache.push(result);
        return  staleCache;
      };
      addGroup.effect = (item, id) => acctSvc.PassUserInfoToRequest(userInfo => {
        item.clientId = userInfo.selectedClientId();
        return this.ServiceEmitter.pipe(mergeMap(policySvc => this.updateTemplates(http.post<Group>(url, item), policySvc)), take(1))
      });

    this.groups$ = builder
    .addAction(addGroup)
    .addAction(this.createDeleteAction(builder, factory, confirmSvc, http, acctSvc))
    .addAction(this.createEditAction(dialog, builder, acctSvc, perfSvc, payrollApiService, apiService, onboardingApiSvc, http, factory))
    .addAction(this.createTemplateUpdatedAction(builder, this.TemplateUpdated, factory))
    .Build();
   }

   private updateTemplates(groupChanged$: Observable<Group>, policySvc: ReviewPolicyService): Observable<Group>{
return groupChanged$.pipe(withLatestFrom(policySvc.allTemplatesForClient$),
tap(x => {
const existingTemplates = new Maybe(x[1]);
const group = x[0];
existingTemplates.valueOr([] as IReviewTemplate[]).forEach(template => {
    const groups = new Maybe(template.groups).valueOr([] as number[]);
    const templateRefToGroup = groups.find(temp => group.groupId === temp);
    const groupRefToTemplate = group.reviewTemplates.find(tempGroup => template.reviewTemplateId === tempGroup);

    if(templateRefToGroup == null && groupRefToTemplate != null){
        template.groups.push(group.groupId)
    }

    if(templateRefToGroup != null && groupRefToTemplate == null){
        const locationToRemove = template.groups.indexOf(group.groupId);
        template.groups.splice(locationToRemove, 1);
    }
});
policySvc.ReactToGroupChange(existingTemplates.value());
}),
map(x => x[0]))
   }

   private createTemplateUpdatedAction(
     builder: StoreBuilder<Group[]>,
     dispatcher: Observable<Group[]>,
     factory: PASaveHandler): StoreAction<Group[], Group[], Group[]> {
      const templateUpdatedAction = builder.scaffoldAction<Group[], Group[]>();
      templateUpdatedAction.dispatcher$ = dispatcher;
      templateUpdatedAction.effect = (x, groups: Group[]) => of(groups);
      templateUpdatedAction.normalizeSaveHandler = factory;
      templateUpdatedAction.operation = 'Update Groups';
      templateUpdatedAction.setupFn = builder.nullSetupFn();
      templateUpdatedAction.updateState = (result, oldState) => result;
      return templateUpdatedAction;
     }
whichOfYaDoneBroke<T>(data: Observable<T>, id: number): Observable<T> {
  return data.pipe(
    tap(x => console.log('Next!' + ' ' + id)),
  finalize(() => console.log('Done!' + ' ' + id)));
}
  private createEditAction(
    dialog: MatDialog,
    builder: StoreBuilder<Group[]>,
    acctSvc: AccountService,
    perfSvc: PerformanceReviewsService,
    payrollApiService: PayrollService,
    apiService: ExternalApiService,
    onboardingApiSvc: DsOnboardingAdminApiService,
    http: HttpClient,
    factory: PASaveHandler): StoreAction<Group, Group[], number> {
    const action = builder.scaffoldAction<Group, number>();

    action.setupFn = x => x.pipe(exhaustMap(x => forkJoin(
      perfSvc.getCompetencyModelsForCurrentClient(),
      acctSvc.PassUserInfoToRequest(userInfo => from(<PromiseLike<ClientDivisionDto[]>>apiService.getClientDivision(userInfo.selectedClientId()))),
      acctSvc.PassUserInfoToRequest(userInfo => from(<PromiseLike<ClientDepartmentDto[]>>apiService.getClientDepartments(userInfo.selectedClientId()))),
      acctSvc.PassUserInfoToRequest(userInfo => from(<PromiseLike<JobProfileDto[]>>apiService.getJobProfiles(userInfo.selectedClientId()))),
      acctSvc.PassUserInfoToRequest(userInfo => from(<PromiseLike<IClientCostCenterDto[]>>onboardingApiSvc.getClientCostCentersList(userInfo.selectedClientId()))),
      payrollApiService.getPayType(),
      this.ServiceEmitter.pipe(mergeMap(svc => svc.allTemplatesForClient$.pipe(
        map(templates => templates.filter(template => !template.isArchived)),
        map(templates => sortTemplates(templates)))), take(1)),
      of(x.currentValue.find(group => group.groupId === x.idOrNewValue)),
      of(x.currentValue)
    )),
      map(z => ({
        data: <IGroupDialogData>{
          compModels: z[0],
          divisions: z[1],
          departments: z[2],
          jobProfiles: z[3],
          costCenters: z[4],
          payTypes: z[5],
          templates: z[6],
          group: z[7]
        },
        currentValue: z[z.length - 1]
      })),
      concatMap(z => dialog.open<GroupDialogComponent, IGroupDialogData, Group>(GroupDialogComponent, {
        data: z.data,
        width: "700px"
      }).afterClosed()
        .pipe(
          filter(x => x != null),
          map(y => ({ item: y, currentValue: z.currentValue, idOrNewValue: undefined })))));

      action.effect = (item, id) => acctSvc.PassUserInfoToRequest(userInfo => {
        item.clientId = userInfo.selectedClientId();
        return this.ServiceEmitter.pipe(mergeMap(policySvc => this.updateTemplates(http.put<Group>(url, item), policySvc)), take(1))
      });

      action.dispatcher$ = this.UpdateGroupDispatcher;
      action.normalizeSaveHandler = factory;
      action.operation = 'Edit Group';
      action.updateState = (result, oldState) => {
        const location = oldState.findIndex(group => group.groupId === result.groupId);
        if(location !== -1){
          oldState[location] = result;
        }

        return oldState;
      }

    return action;
  }

   private createDeleteAction(
     builder: StoreBuilder<Group[]>,
     handler: PASaveHandler,
     confirmSvc: DsConfirmService,
     client: HttpClient,
     acctSvc: AccountService): StoreAction<number, Group[], number> {
const action = builder.scaffoldAction<number, number>()

action.dispatcher$ = this.DeleteGroupDispatcher;
action.normalizeSaveHandler = handler;
action.operation = 'Delete Group';
action.setupFn = builder.nullSetupFn();
action.effect = (item, id: number) => from(<PromiseLike<any>>confirmSvc.show(null, {
  bodyText: 'Are you sure you want to delete this group?',
  swapOkClose: true,
  actionButtonText: 'Delete',
  closeButtonText: 'Cancel'
})).pipe(
  catchError(err => {
    if(this.WasCancelled(err)){
      return of(err);
    } else {
      return throwError(err);
    }
  }),
  filter((x) => !this.WasCancelled(x)),
  concatMap(() => acctSvc.PassUserInfoToRequest(userInfo => client.delete<null>(`${url}/group/${id}/client/${userInfo.selectedClientId()}`))),
  map(x => id)
  );
  action.updateState = (result, oldState) => {
return oldState.filter(x => x.groupId !== result);
  }

return action;
   }

   private WasCancelled(err: any): boolean{
     return this.compareString('cancel', err) || this.compareString('backdrop click', err);
   }

   private compareString(msg: string, err: any){
    return msg.localeCompare(new Maybe(err).map(x => x.toLowerCase ? x.toLowerCase() : '').valueOr('')) === 0
   }


   readonly AddGroup = (policySvc: ReviewPolicyService) => {
    this.ServiceEmitter.next(policySvc);
     this.AddGroupDispatcher.next();
   }
   readonly DeleteGroup = (id: number, policySvc: ReviewPolicyService) => {
    this.ServiceEmitter.next(policySvc);
     this.DeleteGroupDispatcher.next(id);
   }
   readonly UpdateGroup = (id: number, policySvc: ReviewPolicyService) => {
    this.ServiceEmitter.next(policySvc);
     this.UpdateGroupDispatcher.next(id);
   }
   readonly ReactToTemplateUpdated = (groups: Group[], policySvc: ReviewPolicyService) => {
    this.ServiceEmitter.next(policySvc);
     this.TemplateUpdated.next(groups);
   }
}

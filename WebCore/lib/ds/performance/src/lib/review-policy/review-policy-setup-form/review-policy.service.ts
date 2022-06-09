import { Injectable, Inject } from '@angular/core';
import { ReviewPolicyApiService } from '../review-policy-api.service';
import { IReviewTemplate } from '../../../../../core/src/lib/groups/shared/review-template.model';
import { Observable, Subject, throwError, forkJoin, of, merge, OperatorFunction, ReplaySubject } from 'rxjs';
import { AccountService } from '@ds/core/account.service';
import { map, withLatestFrom, tap, publishReplay, refCount, concatMap, filter, switchMap, first } from 'rxjs/operators';
import { Maybe } from '@ds/core/shared/Maybe';
import { ReviewTemplateDialogService } from '../review-template-dialog/review-template-dialog.service';
import { PASaveHandlerToken, PASaveHandler, AttachErrorHandlerFn, AttachErrorHandler, applyMsgSvcToSaveHandler, applyMsgSvcToErrorHandler,  } from '@ds/core/shared/shared-api-fn';
import { ReferenceDate } from '../../../../../core/src/lib/groups/shared/schedule-type.enum';
import { SetupFn, StoreBuilder, ComposableSetupFn, StoreAction } from '@ds/core/shared/store-builder';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { GroupService } from '@ds/core/groups/group.service';



/**
 * Configures a flux store that can be used to get and manage Review Templates
 */
@Injectable({
  providedIn: 'root'
})
export class ReviewPolicyService {

  public allTemplatesForClient$: Observable<IReviewTemplate[]>
  private copy = new Subject<number>();
  private archive = new Subject<number>();
  private edit = new Subject<number>();
  private addRecurring = new Subject<null>();
  private addNewHire = new Subject<null>();
  private reActivate = new Subject<number>();
  private reactToGroupChange = new Subject<IReviewTemplate[]>();
  /**
 * Group service subject added to fix circular dependency problem...  TODO find better solution to solve this (maybe register all of the dependencies of services with some third party
 *  and then manually inject them after they have all been created)
 */
  private groupSvc = new ReplaySubject<GroupService>(1);

  constructor(
    private apiSvc: ReviewPolicyApiService,
    private acctSvc: AccountService,
    private dialogSvc: ReviewTemplateDialogService,
    msgSvc: DsMsgService) {
      const factory = applyMsgSvcToSaveHandler(msgSvc);
      const attachErrorHandler = applyMsgSvcToErrorHandler(msgSvc);

    const originalSource$ = this.acctSvc.PassUserInfoToRequest(userInfo => apiSvc.getReviewTemplatesByClientId(userInfo.selectedClientId(), true));
    var storeBuilder = new StoreBuilder<IReviewTemplate[]>();

    const archiveAction = storeBuilder.scaffoldAction<IReviewTemplate, number>();
    archiveAction.dispatcher$ = this.archive;
    archiveAction.effect = x => {
      return this.apiSvc.archiveReviewTemplate(x.reviewTemplateId);
    };
    archiveAction.updateState = (result, cache) => {
      const index = cache.findIndex(template => template.reviewTemplateId === result.reviewTemplateId);
        cache[index] = result;
      return cache;
    };
    archiveAction.operation = 'Archive Review';
    archiveAction.normalizeSaveHandler = factory;
    archiveAction.setupFn = this.findExisting;

    const editAction = storeBuilder.scaffoldAction<IReviewTemplate, number>();
    editAction.dispatcher$ = this.edit;
    editAction.effect = x => {
      const deepCopyExisting = <IReviewTemplate>JSON.parse(JSON.stringify(x));
      return of(null).pipe(
        withLatestFrom(this.groupSvc),
        map(val => val[1]),
        concatMap(svc => this.apiSvc.saveReviewTemplate(deepCopyExisting, svc, this)));
    };
    editAction.updateState = (result, cache) => {
      if (result != null) {
        const index = cache.findIndex(template => template.reviewTemplateId === result.reviewTemplateId);
        cache[index] = result;
      }

      return cache;
    };
    editAction.operation = 'Save Review';
    editAction.normalizeSaveHandler = factory;
    editAction.setupFn = this.IgnoreCancelledModal(this.openDialogWithTemplate(this.findExisting));

    const addRecurringAction = this.creatAddTemplateAction(storeBuilder, this.addRecurring, factory, true);
    const addNewHireAction = this.creatAddTemplateAction(storeBuilder, this.addNewHire, factory, false);

    const copyAction = storeBuilder.scaffoldAction<IReviewTemplate, number>();
copyAction.dispatcher$ = this.copy;
copyAction.effect = x => {
  const copy = <IReviewTemplate>JSON.parse(JSON.stringify(x));
  copy.reviewTemplateId = 0;
  new Maybe(copy.evaluations).map(y => y.map(z => z.reviewTemplateId = null))
  copy.name = "Copy of: " + copy.name;
  return of(null).pipe(
    withLatestFrom(this.groupSvc),
    map(val => val[1]),
    concatMap(svc => this.apiSvc.saveReviewTemplate(copy, svc, this)));
};
copyAction.normalizeSaveHandler = factory;
copyAction.operation = 'Copy Review';
copyAction.setupFn = this.findExisting;
copyAction.updateState = (result, cache) => {
  cache.push(result);
  return cache;
};

    // this.CreateActionStream(
    //   originalSource$,
    //   this.copy,
    //   x => {
    //     const copy = <IReviewTemplate>JSON.parse(JSON.stringify(x));
    //     copy.reviewTemplateId = 0;
    //     new Maybe(copy.evaluations).map(y => y.map(z => z.reviewTemplateId = null))
    //     copy.name = "Copy of: " + copy.name;
    //     return this.apiSvc.saveReviewTemplate(copy);
    //   },
    //   (result, cache) => {
    //     cache.push(result);
    //     return cache;
    //   },
    //   'Copy Review',
    //   factory,
    //   this.findExisting
    // )

    const reActivateTemplate = storeBuilder.scaffoldAction<IReviewTemplate, number>();
    reActivateTemplate.dispatcher$ = this.reActivate;
    reActivateTemplate.normalizeSaveHandler = factory;
    reActivateTemplate.setupFn = this.findExisting;
    reActivateTemplate.operation = 'Restore Review';
    reActivateTemplate.updateState = (result, oldState) => {
      const index = oldState.findIndex(template => template.reviewTemplateId === result.reviewTemplateId);
      oldState[index] = result;
      return oldState;
    };
    reActivateTemplate.effect = template => {
      const deepCopyExisting = <IReviewTemplate>JSON.parse(JSON.stringify(template));
      deepCopyExisting.isArchived = false;
      return of(null).pipe(
        withLatestFrom(this.groupSvc),
        map(val => val[1]),
        concatMap(svc => this.apiSvc.saveReviewTemplate(deepCopyExisting, svc, this)));
    }

    this.allTemplatesForClient$ = storeBuilder
      .setDataSource(originalSource$, attachErrorHandler, 'Get client reviews')
      .addAction(archiveAction)
      .addAction(editAction)
      .addAction(addRecurringAction)
      .addAction(addNewHireAction)
      .addAction(reActivateTemplate)
      .addAction(copyAction)
      .addAction(this.createTemplateUpdatedAction(storeBuilder, this.reactToGroupChange, factory))
      .Build();
  }

  private creatAddTemplateAction(
    builder: StoreBuilder<IReviewTemplate[]>,
    dispatcher: Observable<null>,
    factory: PASaveHandler,
    openRecurringView: boolean): StoreAction<IReviewTemplate, IReviewTemplate[], null> {
      const addAction = builder.scaffoldAction<IReviewTemplate, null>();
    addAction.dispatcher$ = dispatcher;
    addAction.effect = x => {
      return of(null).pipe(
        withLatestFrom(this.groupSvc),
        map(val => val[1]),
        concatMap(svc => this.apiSvc.saveReviewTemplate(x, svc, this)));
    };
    addAction.updateState = (result, cache) => {
      cache.push(result);
      return cache;
    };
    addAction.operation = 'Save Review';
    addAction.normalizeSaveHandler = factory;
    addAction.setupFn = this.IgnoreCancelledModal(this.openDialogWithNoTemplate(openRecurringView));
    return addAction
    }

  private createTemplateUpdatedAction(
    builder: StoreBuilder<IReviewTemplate[]>,
    dispatcher: Observable<IReviewTemplate[]>,
    factory: PASaveHandler): StoreAction<IReviewTemplate[], IReviewTemplate[], IReviewTemplate[]> {
     const templateUpdatedAction = builder.scaffoldAction<IReviewTemplate[], IReviewTemplate[]>();
     templateUpdatedAction.dispatcher$ = dispatcher;
     templateUpdatedAction.effect = (x, groups: IReviewTemplate[]) => of(groups);
     templateUpdatedAction.normalizeSaveHandler = factory;
     templateUpdatedAction.operation = 'Update Groups';
     templateUpdatedAction.setupFn = builder.nullSetupFn();
     templateUpdatedAction.updateState = (result, oldState) => result;
     return templateUpdatedAction;
    }



  public readonly CopyTemplate = (id: number, groupSvc: GroupService) => {
    this.groupSvc.next(groupSvc);
    this.copy.next(id);
  }

  public readonly ArchiveTemplate = (id: number, groupSvc: GroupService) => {
    this.groupSvc.next(groupSvc);
    this.archive.next(id);
  }

  public readonly EditTemplate = (id: number, groupSvc: GroupService) => {
    this.groupSvc.next(groupSvc);
    this.edit.next(id);
  }

  public readonly AddRecurringTemplate = (groupSvc: GroupService) => {
    this.groupSvc.next(groupSvc);
    this.addRecurring.next(null);
  }

  public readonly AddNewHireTemplate = (groupSvc: GroupService) => {
    this.groupSvc.next(groupSvc);
    this.addNewHire.next(null);
  }

  public readonly ReActivateTemplate = (id: number, groupSvc: GroupService) => {
    this.groupSvc.next(groupSvc);
    this.reActivate.next(id);
  }

  public readonly ReactToGroupChange = (templates: IReviewTemplate[]) => {
    this.reactToGroupChange.next(templates);
  }

    /**
   * When the user cancels out of the modal we don't want to continue down the pipeline
   */
  private IgnoreCancelledModal: ComposableSetupFn<IReviewTemplate, IReviewTemplate[], any> = (fn) => {
    return (input) => fn(input).pipe(
      filter(x => null != x.item))
  }

  private findExisting:  SetupFn<IReviewTemplate, IReviewTemplate[], number> = (input) => input.pipe(map(x => {
    return {
      item: new Maybe(x.currentValue).map(y => y.find(z => z.reviewTemplateId === x.idOrNewValue)),
      cache: x.currentValue,
      idOrNewValue: x.idOrNewValue
    };
  }),
  concatMap(x => {
      return x.item.map(() => of(({item: x.item.value(), currentValue: x.cache, idOrNewValue: x.idOrNewValue}))).valueOr(throwError("Could not find that Review Template."))
  }));

/**
 * Setup function to use when modifying an existing ReviewTemplate
 */
  private openDialogWithTemplate: ComposableSetupFn<IReviewTemplate, IReviewTemplate[], number> = (fn) =>
   (input) => fn(input).pipe(
  concatMap(x => {
    return this.acctSvc.PassUserInfoToRequest(userInfo => this.dialogSvc.openReview(x.item, null, userInfo, null, false).afterClosed()).pipe(
      map(result => ({ item: result, currentValue: x.currentValue, idOrNewValue: x.idOrNewValue })));
  }));

  /**
   * Setup function to use when creating a new ReviewTemplate
   */
  private openDialogWithNoTemplate: (openRecurringView: boolean) => SetupFn<IReviewTemplate, IReviewTemplate[], number> = (openRecurring) => (input) => input.pipe(
    map(x => {
      return {
        item: null,
        cache: x.currentValue,
        idOrNewValue: x.idOrNewValue
      };
    }),
    concatMap(data => this.acctSvc.PassUserInfoToRequest(userInfo =>
      this.dialogSvc.openReview(null, null, userInfo, null, openRecurring, data.idOrNewValue).afterClosed()).pipe(
        map(x => ({ item: x, currentValue: data.cache, idOrNewValue: data.idOrNewValue })))));
}

import { Component, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CompetencyModelEditDialogComponent } from '../competency-model-edit-dialog/competency-model-edit-dialog.component';
import { PerformanceReviewsService } from '../../shared/performance-reviews.service';
import { AccountService } from '@ds/core/account.service';
import { Observable, fromEvent, Subscription, Subject, merge, throwError, from, forkJoin } from 'rxjs';
import { concatMap, map, share, shareReplay, exhaustMap, takeUntil, mapTo, scan, catchError, take, tap } from 'rxjs/operators';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { JobProfilesApiService } from '@ajs/job-profiles/shared/job-profiles-api.service';
import { IJobDetailData } from '@ajs/employee/add-employee/shared/models';
import { IJobProfileDisplayable } from '../shared/job-profile-displayable.model';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { IDsConfirmOptions } from '@ajs/ui/confirm/ds-confirm.interface';
import * as _ from 'lodash';
import { ICompetencyModel, ICompetency } from '..';

/**
 * @author Justin Scott
 * @description A 'smart component' that allows the user to add, edit, and delete competency models for the current client.
 */
@Component({
  selector: 'ds-competency-model',
  templateUrl: './competency-model.component.html',
  styleUrls: ['./competency-model.component.scss']
})
export class CompetencyModelComponent implements OnInit, OnDestroy {

  coreCompCount: number;
  models$: Observable<ICompetencyModel[]>;
  private displayModalHandlerSubscription: Subscription;
  private displayMessageHandlerSubscription: Subscription;
  private modifiedModelEmitter: Subject<ICompetencyModel>;
  private deleteModelEmitter: Subject<ICompetencyModel>;
  private editModelEmitter: Subject<ICompetencyModel>;
  private deleteModelBtnEmitter: Subject<number>;
  allJobProfiles: IJobProfileDisplayable[];



  constructor(private dialog: MatDialog,
    private perfService: PerformanceReviewsService,
    private accountSvc: AccountService,
    private msg: DsMsgService,
    private jobProfileService: JobProfilesApiService,
    private confirmService: DsConfirmService) { }
  @ViewChild('addModelButton', { static: true }) addModel: ElementRef;

  ngOnInit() {
    this.modifiedModelEmitter = new Subject();
    this.editModelEmitter = new Subject();
    this.deleteModelBtnEmitter = new Subject();
    this.deleteModelEmitter = new Subject();

    this.assignShowDialogBtnHandlers();
    this.assignDeleteModelBtnHandler();
    this.assignModelObservable();
  }

  private assignModelObservable(): void {
    let gettingModels$ = this.perfService.getCompetencyModelsForCurrentClient().pipe(shareReplay(1), 
    // remove any archived competencies
    map(val => {
      (val || []).forEach(compModel => {
        compModel.competencies = 
        (compModel.competencies || []).filter(comp => !comp.isArchived);
      });
      return val;
    }));
    this.models$ = merge(
      // IMPORTANT -- Make sure you are modifying the original array in the scan, otherwise these handlers will be out of sync
      merge(gettingModels$, this.modifiedModelEmitter.asObservable().pipe(map(x => [x]))).pipe(scan((existing, newOrModified) => {
        let newOrModifiedVal = newOrModified[0];
        newOrModifiedVal.competencies = (newOrModifiedVal.competencies || []).filter(comp => !comp.isArchived);
        let isFound = this.findAndInvokeIfFound(existing, newOrModifiedVal, (foundVal, index) => {
          existing[index] = newOrModifiedVal;
        });
        if (!isFound) {
          existing.push(newOrModifiedVal);
        }
        return existing;
      })),
      // IMPORTANT -- Make sure you are modifying the original array in the scan, otherwise these handlers will be out of sync
      merge(gettingModels$, this.deleteModelEmitter.asObservable().pipe(map(x => [x]))).pipe(scan((existing, modelToRemove) => {
        this.findAndInvokeIfFound(existing, modelToRemove[0], (found, index) => {
          const jobProfilesToAdd: {[key: string]: IJobProfileDisplayable} = {};
          (found.jobProfiles || []).forEach(val => {
            jobProfilesToAdd[val.jobProfileId] = val;
          });
          (this.allJobProfiles || []).forEach(val => {
            if (jobProfilesToAdd[val.jobProfileId] != null) {
              val.competencyModelId = null;
            }
          });
          existing.splice(index, 1);
        });
        return existing;
      }))).pipe(map(x => x.sort((x, y) => {
        const name1 = (x.name || '').toLowerCase().trim();
        const name2 = (y.name || '').toLowerCase().trim();
        return name1.localeCompare(name2);
      })), share(), catchError(x => {
        this.assignModelObservable();
        return throwError(x);
      }));
  }

  deleteCompModel(competencyModelId: number): void {
    const modalOptions: IDsConfirmOptions = {};
    modalOptions.bodyText = 'Are you sure you want to remove this model?  All employees assigned to this model will have it removed.';
    modalOptions.swapOkClose = true;
    modalOptions.closeButtonText = 'Cancel';
    modalOptions.actionButtonText = 'Yes, Remove'
    this.confirmService.show(null, modalOptions).then(yes => this.deleteModelBtnEmitter.next(competencyModelId));
  }

  private assignDeleteModelBtnHandler(): void {
    this.deleteModelBtnEmitter.pipe(exhaustMap(modelId => {
      return this.perfService.deleteCompetencyModelForCurrentClient(modelId);
    }),
      catchError(e => {
        this.assignDeleteModelBtnHandler();
        this.msg.setTemporaryMessage('Sorry, this competency model was unable to be removed.', MessageTypes.error, 3000);
        return throwError(e);
      })).subscribe(x => {
        this.msg.setTemporaryMessage(`The '${x.name}' competency model was successfully removed.`, MessageTypes.success, 6000);
        this.deleteModelEmitter.next(x);
      });
  }

  private assignShowDialogBtnHandlers(): void {
    // create observable to get the data we need
    let gettingCompetencyData$ = this.perfService.getPerformanceCompetenciesForCurrentClient(false);

    let gettingJobProfiles$ = this.accountSvc.getUserInfo().pipe(concatMap(userInfo => {
      return from(<PromiseLike<IJobDetailData[]>>this.jobProfileService.getJobDetailsByClientId(userInfo.lastClientId || userInfo.clientId))
        .pipe(map(profiles => profiles.map((x: any) => <IJobProfileDisplayable>{ name: x.description, jobProfileId: x.jobProfileId, competencyModelId: x.competencyModelId })));
    }), take(1));


    let gettingCompModelsAndJobProfiles$ = forkJoin(gettingCompetencyData$, gettingJobProfiles$);
    // add observables to show edit/add competency dialog after we get our competencies from the db
    this.displayModalHandlerSubscription = gettingCompModelsAndJobProfiles$.pipe(
      tap(x => this.coreCompCount = (x[0] || []).filter(y => y.isCore && !y.isArchived).length),
      concatMap(compsAndJobProfiles =>
        merge(
          fromEvent<MouseEvent>(this.addModel.nativeElement, 'click').pipe(mapTo(null)),
          this.editModelEmitter.asObservable())
          .pipe(
            map(x => { 
              this.allJobProfiles = compsAndJobProfiles[1];
              return { model: x, competencies: compsAndJobProfiles[0], jobProfiles: compsAndJobProfiles[1] }; 
            })
            )
      )).subscribe(data => {
        this.showAddCompetencyDialog(data.model, data.competencies, data.jobProfiles);
      }, err => {
        this.coreCompCount = 0;
      });

    // add observables to display temporary message while we are getting our data.
    this.displayMessageHandlerSubscription = merge(fromEvent<MouseEvent>(this.addModel.nativeElement, 'click'), this.editModelEmitter.asObservable())
      .pipe(takeUntil(gettingCompModelsAndJobProfiles$)).subscribe(evt => {
        this.msg.setTemporaryMessage('Please wait for competencies and job profiles to finish loading.', MessageTypes.info, 3000);
      });
  }


  /**
   * Displays the modal to either create a new ICompetencyModel or edit an existing ICompetencyModel
   * @param { ICompetencyModel } compModel  The competency model to edit
   * @param { ICompetency[] } competencies All of the available competencies
   */
  private showAddCompetencyDialog(compModel: ICompetencyModel, competencies: ICompetency[], jobProfiles: IJobProfileDisplayable[]): void {
    this.dialog.open(CompetencyModelEditDialogComponent, {
      width: '800px',
      data: { model: compModel == null ? null : _.cloneDeep(compModel), competencies: Object.assign([], competencies), jobProfiles: jobProfiles }
    }).afterClosed().subscribe(compModelResult => {
      if (compModelResult != null) {
        this.modifiedModelEmitter.next(compModelResult);
      }
    });
  }

  /**
   * Copies the competency model
   * @param model They model to copy
   */
  public copy(model: ICompetencyModel): void {
    let objToCopy = Object.assign({}, model);
    objToCopy.jobProfiles = null;
    objToCopy.empPerfConfigs = null;
    objToCopy.name = 'Copy of ' + objToCopy.name;
    let copyModel = this.perfService.createCompetencyModelForCurrentClient(objToCopy);
    copyModel.subscribe(x => {
      this.modifiedModelEmitter.next(x);
      this.msg.setTemporaryMessage(`Copy of '${model.name}' has been created.`, MessageTypes.success, 6000);
    });
  }

  /**
   * Searches for a value in the array and invokes a function if a value is found.
   * @param existing The array to search
   * @param modelToFind The model to find
   * @param callbackFn The function to call when a value is found.  The function is passed the found value and where it is in the array
   * @returns { boolean } A boolen indicating that a value was found (true) or no value was found (false)
   */
  private findAndInvokeIfFound(existing: ICompetencyModel[], modelToFind: ICompetencyModel, callbackFn: (found: ICompetencyModel, indexToUpdate: number) => void): boolean {
    let indexToUpdate;
    let foundVal = existing.find((val, index) => {
      indexToUpdate = index;
      return val.competencyModelId == modelToFind.competencyModelId;
    });

    if (foundVal != null) {
      callbackFn(foundVal, indexToUpdate);
      return true;
    }
    return false;
  }

  ngOnDestroy(): void {
    this.displayModalHandlerSubscription.unsubscribe();
    this.displayMessageHandlerSubscription.unsubscribe();
  }
}

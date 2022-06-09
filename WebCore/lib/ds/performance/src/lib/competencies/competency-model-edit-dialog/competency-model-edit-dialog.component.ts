import { Component, OnInit, Inject, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { fromEvent, Subscription, Observable, Subject, of, merge } from 'rxjs';
import { exhaustMap, catchError, flatMap, startWith, map, debounceTime, switchMap } from 'rxjs/operators';
import { FormBuilder, FormControl, FormGroup, FormArray, Validators } from '@angular/forms';
import { ICompetencyModel } from '@ds/performance/competencies/shared/competency-model.model';
import { AccountService } from '@ds/core/account.service';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import * as _ from 'lodash';
import { ICompetencyModelUpdate } from '@ds/performance/competencies/shared/competency-model-update.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatAutocompleteSelectedEvent, MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { ICompetency, IJobProfileDisplayable } from '..';


@Component({
  selector: 'ds-competency-model-edit-dialog',
  templateUrl: './competency-model-edit-dialog.component.html',
  styleUrls: ['./competency-model-edit-dialog.component.scss']
})
export class CompetencyModelEditDialogComponent implements OnInit, OnDestroy {

  private btnSubscription: Subscription;
  private checkBoxSub: Subscription;
  compFormGroup: FormGroup;
  private submit$: Subject<any> = new Subject();
  private btnSubmitSub: Subscription;
  private origModel: ICompetencyModel;
  private addedCompetencies: { [id: number]: ICompetency };
  private removdCompetencies: { [id: number]: ICompetency };
  private addedJobProfiles: { [id: number]: IJobProfileDisplayable };
  private removedJobProfiles: { [id: number]: IJobProfileDisplayable };
  private originallySelectedComps: { [id: number]: ICompetency };
  private originallySelectedjobProfs: { [id: number]: IJobProfileDisplayable };
  public submitted: boolean;
  jobProfileCtrl = new FormControl('');
  selectedjobProfiles: IJobProfileDisplayable[] = [];
  allJobProfiles: IJobProfileDisplayable[] = [];
  filteredJobProfiles: Observable<IJobProfileDisplayable[]>;

  private isJobProfileAdded = (id: number) => this.addedJobProfiles[id] != null;
  private isJobProfileRemoved = (id: number) => this.removedJobProfiles[id] != null;
  private isJobProfileOriginal = (id: number) => this.originallySelectedjobProfs[id] != null;

  dialogMode = 'Add';

  @ViewChild('cancel', { static: true }) cancelBtnRef: ElementRef;
  @ViewChild('close', { static: true }) closeBtnRef: ElementRef;
  @ViewChild('jobProfileInput', { static: true }) jobProfileInput: ElementRef<HTMLInputElement>


  get name() { return this.compFormGroup.get('name'); }
  get jobProfiles() { return this.compFormGroup.get('jobProfiles'); }

  constructor(
    public dialogRef: MatDialogRef<CompetencyModelEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { model: ICompetencyModel, competencies: ICompetency[], jobProfiles: IJobProfileDisplayable[] },
    private fb: FormBuilder,
    private pefService: PerformanceReviewsService,
    private msg: DsMsgService) {

    // TODO fix the first value the user types is sometimes ignored.......
    // To reproduce:
    // 1) type the name of a job profile that is already in the chip list
    // 2) select the job profile
    // 3) type the first character of the name of that same job profile
    this.filteredJobProfiles = this.jobProfileCtrl.valueChanges.pipe(
      debounceTime(250),
      startWith<string | IJobProfileDisplayable>(''),
      map(val => typeof val === 'string' ? val : val.name),
      map((profile: string) => (profile ? this._filter(profile) : this.allJobProfiles.slice())
        .filter(profile => (profile.competencyModelId == null && !this.isJobProfileAdded(profile.jobProfileId)) ||
          (profile.competencyModelId != null && this.isJobProfileRemoved(profile.jobProfileId)))));
  }

  ngOnInit() {

    this.submitted = false;
    this.addedCompetencies = {};
    this.removdCompetencies = {};
    this.addedJobProfiles = {};
    this.removedJobProfiles = {};
    var selectedJobProfilesDictionary: { [id: number]: IJobProfileDisplayable } = {};
    // function to determine whether a jobprofile should be available to be selected
    var filterFn: (jobProfile: IJobProfileDisplayable) => boolean;
    if (this.data.model != null) {
      this.dialogMode = 'Edit';
      this.origModel = _.cloneDeep(this.data.model);
      // include all job profiles without a competency model and the job profiles with the current competency model
      filterFn = (jobProfile: IJobProfileDisplayable) =>
        jobProfile.competencyModelId == null || selectedJobProfilesDictionary[jobProfile.jobProfileId] != null;
    } else {
      this.data.model = <ICompetencyModel>{ competencies: [] };
      filterFn = (jobProfile: IJobProfileDisplayable) => jobProfile.competencyModelId == null;
    }

    this.originallySelectedComps = {};
    this.originallySelectedjobProfs = {};

    let coreComps = this.data.competencies.filter(x => x.isCore).sort(this.sortFn);
    let nonCoreComps = this.data.competencies.filter(x => !x.isCore).sort(this.sortFn);

    this.data.competencies = coreComps.concat(nonCoreComps);

    (this.data.model.competencies || []).forEach(element => {
      this.originallySelectedComps[element.competencyId] = element;
    });

    (this.data.model.jobProfiles || []).forEach(element => {
      this.originallySelectedjobProfs[element.jobProfileId] = element;
    });

    // populate dictionary
    (this.data.model.jobProfiles || []).forEach(x => {
      selectedJobProfilesDictionary[x.jobProfileId] = x;
    });

    //update the names of the job profiles
    if (Object.keys(selectedJobProfilesDictionary).length > 0) {
      (this.data.jobProfiles || []).forEach(x => {
        if (null != selectedJobProfilesDictionary[x.jobProfileId])
          selectedJobProfilesDictionary[x.jobProfileId].name = x.name;
      });
    }

    this.allJobProfiles = this.data.jobProfiles.filter(filterFn);
    this.selectedjobProfiles = this.data.model.jobProfiles || [];


    this.createForm();
    this.initializeCheckboxes();
    this.assignCloseBtnHandler();
    this.assignSubmitFormHandler();
  }

  ngOnDestroy(): void {
    this.btnSubscription.unsubscribe();
    this.btnSubmitSub.unsubscribe();
  }

  private createForm(): void {
    this.compFormGroup = this.fb.group({
      name: new FormControl(this.data.model.name, {
        validators: Validators.compose([Validators.maxLength(256), Validators.required]),
        updateOn: 'blur'
      }),
      jobProfiles: this.jobProfileCtrl,
      competencies: this.fb.array([''])
    });
  }

  /**
   * Puts the core competencies first and creates a form control for each checkbox in the table of
   * competencies.  There will be exactly as many form controls as there are competencies for the current client.
   */
  private initializeCheckboxes(): void {

    this.data.competencies = this.data.competencies == null ? [] : this.data.competencies;

    // add a control for each check box in the table
    var formArr = this.compFormGroup.get('competencies') as FormArray;;
    this.data.competencies.forEach(x => {
      var value = null;

      // check to see if the competency was selected for this model
      if (x.isCore || this.originallySelectedComps[x.competencyId] != null) {
        value = x;
      }

      const control = new FormControl({ value: value, disabled: false });

      if (x.isCore) {
        control.disable();
      }

      formArr.push(control);
    });
    formArr.removeAt(0);
  }

  private assignCloseBtnHandler(): void {
    // register close modal handlers
    this.btnSubscription = merge(
      fromEvent(this.cancelBtnRef.nativeElement, 'click'),
      fromEvent(this.closeBtnRef.nativeElement, 'click')
    ).subscribe(event => {
      this.dialogRef.close();
    }, this.assignCloseBtnHandler);
  }

  /**
   * Handles submit events.  If a CompetencyModelId is found in the competency model passed into the component then it will
   * update that competency model.  Otherwise it creates a new one.  Submit events will be ignored if we are currently trying
   * to save a competency model
   */
  private assignSubmitFormHandler(): void {
    this.btnSubmitSub = this.submit$.pipe(exhaustMap(() => {
      if (this.data.model.competencyModelId == null) {
        return this.createCompetencyModel();
      } else {
        return this.updateCompetencyModel();
      }
    })).subscribe(x => {
      this.msg.setTemporaryMessage(`The '${x.name}' Competency Model saved successfully`, MessageTypes.success, 6000);
      (this.data.jobProfiles || []).forEach(item => {
        if (this.addedJobProfiles[item.jobProfileId] != null) {
          item.competencyModelId = x.competencyModelId;
        }
        if (this.removedJobProfiles[item.jobProfileId] != null) {
          item.competencyModelId = null;
        }
      })
      this.dialogRef.close(x);
    }, y => {
      this.msg.setTemporaryMessage('Sorry, this operation failed: \'Save Competency Model\'', MessageTypes.error, 6000);
      this.assignSubmitFormHandler();
    });
  }

  public submit(): void {
    this.submitted = true;
    if (this.compFormGroup.invalid) return;
    this.submit$.next();
  }

  /**
   * Adds the competency to the appropriate collections.
   * @param control The control we are setting a value on
   */
  handleChange(control: FormControl, competency: ICompetency): void {
    if (control.value) {
      // competency was changed to selected
      control.setValue(competency);
      this.updateDictionaries(
        this.originallySelectedComps[competency.competencyId] == null,
        this.addedCompetencies,
        this.removdCompetencies,
        competency.competencyId,
        competency);
    } else {
      // competency was changed to de-selected
      control.setValue(null);
      this.updateDictionaries(
        this.originallySelectedComps[competency.competencyId] != null,
        this.removdCompetencies,
        this.addedCompetencies,
        competency.competencyId,
        competency
      );
    }
  }

  /**
   * Updates the competency model based on the values found in the form
   */
  private updateCompetencyModel(): Observable<ICompetencyModel> {
    var addedCompArray: ICompetency[] = this.ExtractValues(this.addedCompetencies);
    var removedCompArray: ICompetency[] = this.ExtractValues(this.removdCompetencies);
    var addedJobProfilArray: IJobProfileDisplayable[] = this.ExtractValues(this.addedJobProfiles);
    var removedJobProfileArray: IJobProfileDisplayable[] = this.ExtractValues(this.removedJobProfiles);

    var nameToUpdate = (this.origModel.name || '')
      .localeCompare((this.compFormGroup.controls.name.value || '')) === 0 ?
      undefined : this.compFormGroup.controls.name.value;

    if (nameToUpdate == null &&
      removedCompArray == null &&
      addedCompArray == null &&
      addedJobProfilArray == null &&
      removedJobProfileArray == null) return of(this.origModel);

    var compModelToUpdate: ICompetencyModelUpdate = {
      clientId: null, // this will be set for us
      competencyModelId: this.origModel.competencyModelId,
      addedCompetencies: addedCompArray,
      removedCompetencies: removedCompArray,
      name: nameToUpdate,
      addedJobProfiles: addedJobProfilArray,
      removedJobProfiles: removedJobProfileArray
    }

    return this.pefService.updateCompetencyModelForCurrentClient(compModelToUpdate);


  }

  /**
   * Creates a competency model using the values found in the form.
   */
  private createCompetencyModel(): Observable<ICompetencyModel> {
    var compModel: ICompetencyModel = {
      competencies: this.ExtractValues(this.addedCompetencies),
      name: this.compFormGroup.get('name').value,
      clientId: null,
      description: "",
      empPerfConfigs: null,
      jobProfiles: this.ExtractValues(this.addedJobProfiles)
    }
    return this.pefService.createCompetencyModelForCurrentClient(compModel);
  }

  /**
   * Removes the provided job profile from the list of selected job profiles for the current competency model.
   * @param { IJobProfileDisplayable } jobProfile The job profile the user wants to remove from the list
   */
  removeJobProfile(jobProfile: IJobProfileDisplayable) {
    var jobProfileId = jobProfile.jobProfileId;


    if (this.isJobProfileAdded(jobProfileId) ||
    (!(this.isJobProfileRemoved(jobProfileId) || this.isJobProfileAdded(jobProfileId)) && this.isJobProfileOriginal(jobProfileId))) {
      this.selectedjobProfiles = this.selectedjobProfiles.filter((val) => jobProfile.jobProfileId != val.jobProfileId);
      this.updateDictionaries(
        this.isJobProfileOriginal(jobProfileId),
        this.removedJobProfiles,
        this.addedJobProfiles,
        jobProfile.jobProfileId,
        jobProfile
      );
    }
    this.jobProfileCtrl.setValue('');
  }

  /**
   * Adds the provided job profile (contained within the event) to the list of selected job profiles for the current competency model.
   * @param event The event containing the job profile the user wants to add to the list.
   */
  addJobProfile(event: MatAutocompleteSelectedEvent): void {
    var jobProfileId = event.option.value.jobProfileId;

    if (this.isJobProfileRemoved(jobProfileId) ||
      !((this.isJobProfileRemoved(jobProfileId) || this.isJobProfileAdded(jobProfileId)) || this.isJobProfileOriginal(jobProfileId))) {
      this.selectedjobProfiles.push(event.option.value);
      this.updateDictionaries(
        !this.isJobProfileOriginal(jobProfileId),
        this.addedJobProfiles,
        this.removedJobProfiles,
        jobProfileId,
        event.option.value
      );
    }
    this.jobProfileInput.nativeElement.value = '';
    this.jobProfileCtrl.setValue('');
  }




  /**
 * Gets the values from the provided dictionary.  IE does not support Object.values so this is a workaround.
 * @param { { [id: number]: T } } dictionary The dictionary to get the values from
 * @returns { T[] | undefined } An array of T if a value is found for a key in the dictionary, otherwise undefined
 */
  private ExtractValues<T>(dictionary: { [id: number]: T }): T[] | undefined {
    var result: T[]
    Object.keys(dictionary).forEach(key => {
      var value = dictionary[key];
      if (value != null) {
        if (result == null) {
          result = [];
        }
        result.push(value);
      }
    });
    return result;
  }

  private _filter(value: string): IJobProfileDisplayable[] {
    return this.allJobProfiles.filter(profile => profile.name.toLowerCase().indexOf(value.toLowerCase()) === 0 &&
      ((profile.competencyModelId == null && !this.isJobProfileAdded(profile.jobProfileId)) ||
        (profile.competencyModelId != null && this.isJobProfileRemoved(profile.jobProfileId))));
  }

  private sortFn = (a: ICompetency, b: ICompetency) => a.name.localeCompare(b.name);

  private updateDictionaries(shouldAdd: boolean, dictionaryToAdd: object, dictionaryToRemove: object, key: number, value: any): void {
    if (shouldAdd) {
      dictionaryToAdd[key] = value;
    }
    dictionaryToRemove[key] = undefined;
  }

  selectOption(e: Event, trigger: MatAutocompleteTrigger) {
    e.stopPropagation();
    trigger.openPanel();
  }
}



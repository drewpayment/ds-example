import { Component, OnInit, ElementRef, ViewChild, OnDestroy, LOCALE_ID } from "@angular/core";
import { Observable, of, ReplaySubject, Subject, fromEvent, Subscription, BehaviorSubject, forkJoin, merge, concat } from "rxjs";
import { AccountService } from "@ds/core/account.service";
import { MatDialog } from "@angular/material/dialog";
import * as _ from 'lodash';
import { PerformanceReviewsService } from "../../shared/performance-reviews.service";
import { IReviewRating } from "../";
import { RatingsEditDialogComponent } from "../ratings-edit-dialog/ratings-edit-dialog.component";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { UserInfo } from "@ds/core/shared";
import { ScoreMethodType } from "../shared/score-method-type.model";
import { ScoreModel } from "../shared/score-model.model";
import { mustBePercentValidator } from "@ds/performance/shared/shared-performance-fns";
import { FormBuilder, FormGroup, FormArray, FormControl, AbstractControl, ValidatorFn, Validators, AbstractControlOptions } from "@angular/forms";
import { map, tap, exhaustMap, take, skip, share, concatMap, catchError } from "rxjs/operators";
import { ScoreRangeLimit } from "../shared/score-range-limit.model";
import { MatTableDataSource } from '@angular/material/table';
import { formatNumber } from "@angular/common";
import { locale } from "moment";

const RATING_DEFAULTS:IReviewRating[] = <IReviewRating[]>[
  { rating: 5, label: 'Outstanding', description: 'Performance consistently exceeds the job requirements', clientId: null },
  { rating: 4, label: 'Exceeds Expectations', description: 'Performance exceeds job requirements most of the time', clientId: null },
  { rating: 3, label: 'Meets Expectations', description: 'Performance is on target with job requirements', clientId: null },
  { rating: 2, label: 'Satisfactory', description: 'Performance is below job requirements most of the time', clientId: null },
  { rating: 1, label: 'Needs Improvements', description: 'Performance does not meet job requirements and improvement is necessary', clientId: null }
]

const enableScoring = 'enableScoring';
const viewableByEmp = 'viewableByEmp';
const displayPrefs = 'displayPrefs';
const scoreRange = 'scoreRange';
const enableDefinitions = 'enableDefinitions';
const scoreModelId = 'scoreModelId';
const decimalFormatString = '1.2-2';
const maximumInput = 'maximum';
const minimumInput = 'minimum';
const meritPercentInput = 'meritPercent';
const additionalEarnings = 'additionalEarnings';

@Component({
  selector: 'ds-ratings-edit',
  templateUrl: './ratings-edit.component.html',
  styleUrls: ['./ratings-edit.component.scss']
})
export class RatingsEditComponent implements OnInit, OnDestroy { 
    user:UserInfo;
    tableColumns = ['rating', 'label', 'description', 'actions'];
    scoreDefColumns = ['minimum', 'maximum', 'label', 'description', 'meritPercent' , 'actions'];
    dataSource$:Subject<IReviewRating[]> = new ReplaySubject(1);
    dataSource:Observable<IReviewRating[]>;
    reviewRatings:IReviewRating[];
    scoringDefs:any;
    methodTypes$: Observable<ScoreMethodType>;
    scoreModel$: Observable<{data: ScoreModel}>;
    @ViewChild('save', { static: true }) saveBtn: ElementRef<HTMLButtonElement>;
    saveBtnSub: Subscription;
    scoringFormGroup: FormGroup;
    tableDataStream = new MatTableDataSource<AbstractControl>([]);
    private controlSubscription: Subscription;
    private displayPrefsSubscription: Subscription;
    rangePercentText = '';
    showFormOverride = false;
    defaultValueAddedToForm = false;
    hideMenu = false;
    submitted = false;

    constructor(
        private service:PerformanceReviewsService, 
        private accountService:AccountService,
        private dialog:MatDialog,
        private msg:DsMsgService,
        fb:FormBuilder
    ) {
        this.dataSource = this.dataSource$.asObservable();
        this.scoringFormGroup = fb.group({});
        this.scoringFormGroup.addControl(scoreModelId, fb.control(null));
        this.scoringFormGroup.addControl(enableScoring, fb.control(null));
        this.scoringFormGroup.addControl(viewableByEmp, fb.control(null));
        this.scoringFormGroup.addControl(displayPrefs, fb.control(null));
        this.scoringFormGroup.addControl(scoreRange, fb.array([], this.formValidator()));
        this.scoringFormGroup.addControl(enableDefinitions, fb.control(false));
        this.scoringFormGroup.addControl(additionalEarnings, fb.control(false));

    }

    /**
     * LIFECYCLE HOOKS
     */

    ngOnInit() {
        this.accountService.getUserInfo().subscribe((user:UserInfo) => {
            this.user = user;
            this.service.getPerformanceReviewRatings(this.user.lastClientId || this.user.clientId)
                .subscribe(ratings => {
                    if(!ratings || !ratings.length) {
                        let data = RATING_DEFAULTS.map((r:IReviewRating) => {
                            r.clientId = this.user.lastClientId || this.user.clientId;
                            return r;
                        });
                        this.dataSource$.next(data);

                        // need to save defaults to this client... 
                        this.service.savePerformanceReviewRatings(this.user.lastClientId || this.user.clientId, RATING_DEFAULTS)
                            .subscribe((ratings:IReviewRating[]) => {
                                this.reviewRatings = ratings;
                                this.dataSource$.next(ratings);
                            });
                    } else {
                        this.reviewRatings = ratings;
                        this.dataSource$.next(ratings);
                    }
                });
        });

        var handleFirstEmission$ = this.scoringFormGroup.get(enableScoring).valueChanges.pipe(take(1), tap(next => {
            if (true === next && this.scoringFormGroup.get(scoreModelId).value == null) {
                this.defaultValueAddedToForm = true;
                this.scoringFormGroup.get(enableScoring).setValue(true);
                this.scoringFormGroup.get(viewableByEmp).setValue(true);
                this.scoringFormGroup.get(enableDefinitions).setValue(false);
                this.scoringFormGroup.get(additionalEarnings).setValue(false);
                this.insertDefaultRanges(null);
                this.tableDataStream.data = (<FormArray>this.scoringFormGroup.controls[scoreRange]).controls;
                this.scoringFormGroup.get(displayPrefs).setValue("1", {emitEvent: false});
                this.setTableDisabled(false);
            }
        }));

        var activateScoringListener$ = merge(handleFirstEmission$, this.scoringFormGroup.get(enableScoring).valueChanges);

        this.methodTypes$ = this.service.getScoreMethodTypes();
        this.scoreModel$ = this.service.getScoreModelForCurrentClient().pipe(tap(opResult => this.initScoreModelForm(opResult)), share()); 
        concat(
            this.scoreModel$,
            merge(activateScoringListener$.pipe(
                tap(next => { this.showFormOverride = next; })),
                this.scoringFormGroup.get(enableDefinitions).valueChanges.pipe(
                    tap(next => this.setTableDisabled(next))),
                this.getDisplayPrefs.valueChanges.pipe(
                    tap(next => {
                        var ctrl = this.scoreRangeArray.controls[0].get(maximumInput);
                        var convertFunc = null;
                        if (next == '2') {
                            ctrl.setValidators(this.mustEqualOneHundredValidator());
                            this.removeError(ctrl, 'mustBe5');
                            ctrl.updateValueAndValidity();
                            this.rangePercentText = '%';
                            convertFunc = this.convertToPercent;
                        } else {
                            ctrl.setValidators(this.mustEqualFiveValidator());
                            this.removeError(ctrl, 'mustBe100');
                            ctrl.updateValueAndValidity();
                            this.rangePercentText = '';
                            convertFunc = this.convertToDecimal;
                        }
                        var i;
                        for (i = 0; i < this.scoreRangeArray.controls.length; i++) {
                            var form = this.scoreRangeArray.controls[i];
                            if (form.get(minimumInput).value != null) {
                                form.get(minimumInput).setValue(convertFunc(+form.get(minimumInput).value));
                            }
                            if (form.get(maximumInput).value != null) {
                                form.get(maximumInput).setValue(convertFunc(+form.get(maximumInput).value));
                            }
                        }
                    }))
            )
        ).subscribe();

        this.saveBtnSub = fromEvent(this.saveBtn.nativeElement, 'click').subscribe(() => this.saveChanges());
    }
    
    ngOnDestroy(): void {
        if(this.saveBtnSub){
            this.saveBtnSub.unsubscribe();
        }
        if(this.controlSubscription){
            this.controlSubscription.unsubscribe();
        }
        if(this.displayPrefsSubscription){
            this.displayPrefsSubscription.unsubscribe();
        }
    }

    get getDisplayPrefs() {
        return this.scoringFormGroup.controls[displayPrefs] as FormControl;
    }

    get scoreRangeArray() {
        return this.scoringFormGroup.controls[scoreRange] as FormArray;
    }

    private saveChanges(): void {
        this.submitted = true;
        this.scoreRangeArray.updateValueAndValidity();
        if (this.scoringFormGroup.valid) {
            var result = this.scoringFormGroup.value;
            var scoreModel: ScoreModel;
            var i;
            var ranges: ScoreRangeLimit[] = [];
            for (i = 0; i < (result.scoreRange || []).length; i++) {
                var range = result.scoreRange[i];
                ranges.push({
                    description: range.description,
                    label: range.label,
                    maxScore: +range.maximum,
                    scoreModelId: range.scoreModelId,
                    scoreModelRangeId: range.scoreModelRangeId,
                    meritPercent: range.meritPercent
                });
            }
            this.accountService.getUserInfo().pipe(
                map(userInfo => {
                    scoreModel = {
                        clientId: userInfo.lastClientId || userInfo.clientId,
                        hasScoreRange: result.enableDefinitions,
                        isScoreEmployeeViewable: result.viewableByEmp,
                        name: '',
                        scoreMethodType: {
                            scoreMethodTypeId: +result.displayPrefs ? +result.displayPrefs : 1,
                            description: null,
                            name: null
                        },
                        scoreModelId: result.scoreModelId,
                        scoreRangeLimits: ranges,
                        isActive: !!result.enableScoring,
                        additionalEarnings: result.additionalEarnings
                    }
                    return scoreModel;
                }),
                concatMap(data => this.service.savePerformanceScoring(data).pipe(tap(() => this.msg.setTemporarySuccessMessage('Successfully updated scoring.')))
                )).subscribe();
        }
    }

    private convertToPercent(number: number): number {
        return number * 100 / 5;
    }

    private convertToDecimal(percent: number): number {
        return percent * 5 / 100;
    }

    private initScoreModelForm(opResult: { data: ScoreModel }): { data: ScoreModel } {
        if (opResult.data != null) {
            this.scoringFormGroup.controls[enableScoring].setValue(opResult.data.isActive);
            this.showFormOverride = opResult.data.isActive;
            this.scoringFormGroup.controls[viewableByEmp].setValue(opResult.data.isScoreEmployeeViewable);
            this.scoringFormGroup.controls[scoreModelId].setValue(opResult.data.scoreModelId);
            var i;
            this.scoringFormGroup.get(enableDefinitions).setValue(opResult.data.hasScoreRange);
            this.scoringFormGroup.get(additionalEarnings).setValue(opResult.data.additionalEarnings);
            opResult.data.scoreRangeLimits = opResult.data.scoreRangeLimits.sort((a, b) => b.maxScore - a.maxScore);
            if (opResult.data.scoreRangeLimits == null || opResult.data.scoreRangeLimits.length == 0) {
                this.insertDefaultRanges(opResult.data.scoreMethodType.scoreMethodTypeId + '');
            } else {
                for (i = 0; i < opResult.data.scoreRangeLimits.length; i++) {
                    var nextMax = 0;
                    if (i < opResult.data.scoreRangeLimits.length - 1) {
                        nextMax = opResult.data.scoreRangeLimits[i + 1].maxScore;
                    }
                    var rangeLimit = opResult.data.scoreRangeLimits[i];
                    var form = this.createTableForm((nextMax == 0 ? nextMax : nextMax + .01), rangeLimit); // add .01 so user can save form without modifying min value
                    if (i == 0) {
                        if (opResult.data.scoreMethodType.scoreMethodTypeId + '' == '2') {
                            form.get(maximumInput).setValidators(this.mustEqualOneHundredValidator());
                            this.rangePercentText = '%';
                        } else {
                            form.get(maximumInput).setValidators(this.mustEqualFiveValidator());
                        }
                    } else if (i == opResult.data.scoreRangeLimits.length - 1) {
                        form.get(minimumInput).setValidators(this.mustEqualZeroValidator());
                    }
                    form.get(meritPercentInput).setValidators([Validators.required, mustBePercentValidator()]);
                    (<FormArray>this.scoringFormGroup.controls[scoreRange]).controls.push(form);
                    nextMax = rangeLimit.maxScore;
                }
            }

            this.setTableDisabled(opResult.data.hasScoreRange);
            this.getDisplayPrefs.setValue(opResult.data.scoreMethodType.scoreMethodTypeId + '');
            this.tableDataStream.data = (<FormArray>this.scoringFormGroup.controls[scoreRange]).controls;
        }
        return opResult;
    }

    private setTableDisabled(value: any): void {
        if (true === value) {
            this.scoreRangeArray.enable();
        } else {
            this.scoreRangeArray.disable();
        }
    }

    openEditDialog(element:IReviewRating):void {
        const dialogRef = this.dialog.open(RatingsEditDialogComponent, {
            width: '400px',
            data: { rating: element, allRatings: this.reviewRatings }
        });

        dialogRef.afterClosed().subscribe((result:IReviewRating) => {
            // if result is null, user canceled changes from dialog
            if(result == null) return;

            // save changes to rating
            this.service.savePerformanceReviewRatings(this.user.lastClientId || this.user.clientId, result, result.reviewRatingId)
                .subscribe(ratings => {
                    if(ratings == null || !ratings.length) return;
                    let rating = ratings[0];
                    
                    /** Find the updated rating that was returned from API and replace in list */
                    this.reviewRatings.forEach((r, i, a) => {
                        if(r.reviewRatingId != rating.reviewRatingId) return;
                        a[i] = rating;
                    });
                    
                    this.dataSource$.next(this.reviewRatings);
                    this.msg.setTemporarySuccessMessage('Successfully updated ratings.');
                });
        });
    }

    removeRange(index: number): void {
        this.scoreRangeArray.removeAt(index);
        this.setMaxAndMinValidators();
        this.tableDataStream.data = this.scoreRangeArray.controls;
    }

    addRowAbove(index: number): void {
        this.scoreRangeArray.insert(index, this.createTableForm(null, <any>{}));
        this.setMaxAndMinValidators();
        this.tableDataStream.data = this.scoreRangeArray.controls;
    }

    addRowBelow(index: number): void {
        if(index == this.scoreRangeArray.controls.length - 1){
            this.scoreRangeArray.push(this.createTableForm(null, <any>{}));
        } else {
            this.scoreRangeArray.insert(index + 1, this.createTableForm(null, <any>{}));
        }
        this.setMaxAndMinValidators();
        this.tableDataStream.data = this.scoreRangeArray.controls;
    }

    private setMaxAndMinValidators(): void {
        var i;
        for (i = 0; i < this.scoreRangeArray.controls.length; i++) {
            const range = this.scoreRangeArray.controls[i];
            const p = i == 0;
            const q = i == this.scoreRangeArray.controls.length - 1;

            if (p) {
                const validator = this.getDisplayPrefs.value == '2' ? this.mustEqualOneHundredValidator() : this.mustEqualFiveValidator();
                range.get(maximumInput).setValidators(validator);
                range.get(minimumInput).setValidators([]);
            }

            if (q) {
                range.get(minimumInput).setValidators(this.mustEqualZeroValidator());
                range.get(maximumInput).setValidators([]);
            }

            if (!p && !q) {
                range.get(maximumInput).setValidators([]);
                range.get(minimumInput).setValidators([]);
            }
            range.get(meritPercentInput).setValidators([Validators.required, mustBePercentValidator()]);
            range.get(meritPercentInput).updateValueAndValidity();
            range.get(maximumInput).updateValueAndValidity();
            range.get(minimumInput).updateValueAndValidity();
        }
    }


    private createTableForm(min: number | null, rangeLimit: ScoreRangeLimit): FormGroup {
        var locale = (<any>navigator).browserLanguage != null ? (<any>navigator).browserLanguage : navigator.language;
        var minCtrl = new FormControl(min != null ? formatNumber(min, locale, decimalFormatString) : null, {updateOn: "blur", validators: Validators.required });
        minCtrl.valueChanges.subscribe(next => {
            this.scoreRangeArray.updateValueAndValidity();
            if(next != null){
                this.checkAndFormatDecimal(next, minCtrl, locale);
            }
        });
        var maxCtrl = new FormControl(rangeLimit.maxScore != null ? formatNumber(rangeLimit.maxScore, locale, decimalFormatString) : null, {updateOn: "blur", validators: Validators.required });
        maxCtrl.valueChanges.subscribe(next => {
            this.scoreRangeArray.updateValueAndValidity();
            if(next != null){
                this.checkAndFormatDecimal(next, maxCtrl, locale);
            }
        });
       return new FormGroup({
            minimum: minCtrl,
            maximum: maxCtrl,
            label: new FormControl(rangeLimit.label, {updateOn: "blur", validators: Validators.required}),
            description: new FormControl(rangeLimit.description, {updateOn: "blur" }),
            scoreModelRangeId: new FormControl(rangeLimit.scoreModelRangeId),
            meritPercent: new FormControl(rangeLimit.meritPercent)
        });
    }

    checkAndFormatDecimal(value: number, control: FormControl, locale: string): void {
        if (value.toString().match(/^[0-9]+\.[0-9]{2}$/) == null) {
            control.setValue(formatNumber(value, locale, decimalFormatString));
        }
    }

    private removeError(ctrl: AbstractControl, errorCode: string): void {
        if(ctrl.hasError(errorCode)) {
            var errorsList = Object.keys(ctrl.errors);
            var errorObj = {};
            var i;
            for(i = 0; i < errorsList.length; i++){
                var errorCode = errorsList[i];
                if(errorCode.localeCompare(errorCode) != 0)
                errorObj[errorsList[i]] = {};
            }
            ctrl.setErrors(Object.keys(errorObj).length == 0 ? null : errorObj);
            ctrl.updateValueAndValidity();
        }
    }

    private insertDefaultRanges(val: string): void {
        var max = val == '2' ? 100 : 5;
        (<FormArray>this.scoringFormGroup.get(scoreRange)).controls.push(this.createTableForm(null, <ScoreRangeLimit>{maxScore: max}));
        (<FormArray>this.scoringFormGroup.get(scoreRange)).controls.push(this.createTableForm(0, <ScoreRangeLimit>{}));
    }

    /**
     * VALIDATOR FUNCTIONS
     */

    mustEqualFiveValidator(): ValidatorFn {
        return (control: FormControl): {[key: string]: any} | null => {
            return control.value == 5.00 ? null : {'mustBe5': {}};
        }
    }

    mustEqualZeroValidator(): ValidatorFn {
        return (control: FormControl): {[key: string]: any} | null => {
            return control.value == 0.00 ? null : {'mustBe0': {}};
        }
    }

    mustEqualOneHundredValidator(): ValidatorFn {
        return (control: FormControl): {[key: string]: any} | null => {
            return control.value == 100.00 ? null : {'mustBe100': {}};
        }
    }

    formValidator(): ValidatorFn {
        return (control: FormArray): {[key: string]: any} | null => {
            var i;
            for(i = 0; i < control.controls.length; i++){
                var currentMin = <FormGroup>control.controls[i].get(minimumInput);
                const minVal = +currentMin.value;
                const maxVal = +<FormGroup>control.controls[i].get(maximumInput).value;
                if(null != minVal && null != maxVal){
                    if(minVal > maxVal) {
                        currentMin.setErrors({'minGtrThnMax': {}});
                    } else {
                        this.removeError(currentMin, 'minGtrThnMax');
                    }
                }
                
                if(i < control.controls.length - 1){
                    const ctrl = control.controls[i + 1].get(maximumInput); // check value of next input
                    if(ctrl.value >= minVal){
                        ctrl.setErrors({'nextMaxGtrThnMin': {}});
                    } else {
                        this.removeError(ctrl, 'nextMaxGtrThnMin');
                    }
                }
    
                if(i > 0){ 
                    const ctrl = control.controls[i - 1].get(minimumInput); //check value of previous input
                    if(ctrl.value < maxVal){
                        ctrl.setErrors({'prevMinLessThnMax': {}});
                    } else {
                        this.removeError(ctrl, 'prevMinLessThnMax');
                    }
                }
            }
            return null;
        }
    }
}

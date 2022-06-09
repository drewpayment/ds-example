import { Component, OnInit, Input, ViewChild, AfterViewInit } from '@angular/core';
import { IMeritIncreaseView } from '../shared/merit-increase-view.model';
import { IMeritIncreaseCurrentPayAndRates } from '../shared/merit-increase-current-pay-and-rates.model';
import { MeritIncreaseType } from '../shared/merit-increase-type.enum';
import { MatSidenav } from '@angular/material/sidenav';
import { IRemark } from '@ds/core/shared';
import { IMeritIncrease } from '../shared/merit-increase.model';
import { IMeritLimit } from '../shared/merit-limit.model';
import { Maybe } from '@ds/core/shared/Maybe';
import { MatTableDataSource } from '@angular/material/table';
import { ActiveEvaluationService } from '../shared/active-evaluation.service';
import { OneTimeEarning } from '@ds/performance/competencies/shared/one-time-earning.model';
import { Observable, Subject, iif, ReplaySubject, forkJoin, of, defer, combineLatest, from, merge } from 'rxjs';
import { AccountService } from '@ds/core/account.service';
import { ApprovalStatus } from '../shared/approval-status.enum';
import { IncreaseType } from '@ds/performance/competencies/shared/increase-type';
import { map, switchMap, catchError, tap, startWith, filter} from 'rxjs/operators';
import { MeritIncreaseService } from '@ds/performance/performance-manager/shared/merit-increase.service';
import { FormGroup, NgModel, Validators } from '@angular/forms';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { RecommendedBonus } from '../shared/recommended-bonus';
import { formatNumber } from '@angular/common';
import { DailyRulesModalService } from '@ajs/labor/company/rules/modal/daily-rules-modal.service';
import { IClientEarningDto } from '@ajs/labor/models/client-earning.model';
import { FormBuilder, FormArray, AbstractControl, ValidatorFn } from '@angular/forms';

const meritTable = 'meritTable';
const meritForm = 'meritForm';

@Component({
    selector: 'ds-evaluation-payroll-request',
    templateUrl: './evaluation-payroll-request.component.html',
    styleUrls: ['./evaluation-payroll-request.component.scss']
})
export class EvaluationPayrollRequestComponent implements OnInit, AfterViewInit {

    private _score: number;
    private _defaultMeritIncreaseInfo: IMeritIncreaseView;
    additionalEarningData: ReplaySubject<OneTimeEarningRow[]> = new ReplaySubject(1);
    /** A stream that calculates the resulting bonus amount.  Calculation is all done server side to prevent unauthorized users from seeing the employee's paycheck history */
    calcPayout$: Observable<any>;
    private modelChangedHook: Subject<any> = new Subject();
    canViewBonus$: Observable<boolean>;
    typeChangedListener$: Observable<boolean>;
    currentIncreaseBuilder: IncreaseTypeBuilder;
    isEarningChecked: boolean = false;
    listenToIncreaseType$: Observable<any>;
    private myModelSubject = new ReplaySubject<NgModel>(1);
    private typeModelSubject = new ReplaySubject<NgModel>(1);
    private recommendationSubject = new ReplaySubject<RecommendedBonus>(1);
    private earningClicked = new Subject();
    @Input() form: FormGroup;
    @Input() Submitted: boolean;

    @Input() set defaultMeritIncreaseInfo(val: IMeritIncreaseView) {
        this._defaultMeritIncreaseInfo = val;
    }

    @ViewChild('f', { static: false })
    set myForm(value: any) {
        if (value != null && !this.form.controls['control']) {
            this.form.addControl('control', value.control);
        }
    }

    get defaultMeritIncreaseInfo() {
        return this._defaultMeritIncreaseInfo == null ? {} as IMeritIncreaseView : this._defaultMeritIncreaseInfo;
    }
    @Input()
    set defaultOneTimeEarning(val: OneTimeEarning) {
        this._defaultOneTimeEarning = val;
    }
    get defaultOneTimeEarning() {
        return this._defaultOneTimeEarning;
    }

    @Input()
    set score(val: number) {
        this._score = val;
        this.updateMeritRecommendation();
    }

    get increaseAmountArray() {
        return (<FormArray>this.meritFormGroup.controls[meritTable]);
    }
    get hasMeritRecommendation() {
        return this.meritPercent !== null;
    }

    get IncreaseType() {
        return IncreaseType;
    }

    constructor(
        private activeEvaluationSvc: ActiveEvaluationService,
        acctSvc: AccountService,
        private meritIcreaseSvc: MeritIncreaseService,
        private msgSvc: DsMsgService,
        perfSvc: PerformanceReviewsService,
        laborMgmtSvc: DailyRulesModalService,
        private fb: FormBuilder,
    ) {

        this.meritFormGroup = this.fb.group({});
        this.meritFormGroup.addControl(meritTable, this.fb.array([]));

        this.listenToIncreaseType$ = combineLatest(this.typeModelSubject, this.myModelSubject, this.recommendationSubject).pipe(
            filter(x => x[0] != null && x[1] != null),
            tap(x => {
                const models = { typeModel: x[0] as NgModel, myModel: x[1] as NgModel };
                this.typeChangedListener$ = models.typeModel.control.valueChanges.pipe(
                    startWith(models.typeModel.control.value),
                    tap(x => {
                        if (x != null && x.getIncreaseType() === this.recommendedBonus.targetIncreaseType
                        && this.recommendedBonus.targetIncreaseAmount != null) {
                            models.myModel.control
                                .setValidators([Validators.required, Validators.max(this.recommendedBonus.targetIncreaseAmount)]);
                            models.myModel.control.setValue(models.myModel.control.value);
                        } else if (x != null && x.getIncreaseType() !== this.recommendedBonus.targetIncreaseType) {
                            models.myModel.control.setValidators(Validators.required);
                            models.myModel.control.setValue(models.myModel.control.value);
                        }
                    })
                );
            }));

        this.increaseTypeOptions.push(new PercentBuilder(this.meritIcreaseSvc, this.msgSvc));
        this.increaseTypeOptions.push(new FlatRateBuilder(this.meritIcreaseSvc));

        this.clientEarnings$ = acctSvc.PassUserInfoToRequest(userInfo =>
            from(<PromiseLike<IClientEarningDto[]>>laborMgmtSvc.getClientEarnings(userInfo.selectedClientId())).pipe(
                map(x => {
                    const filtered = x.filter(x => x.isActive);
                    return filtered.sort((a, b) => a.description.trim().toLowerCase().localeCompare(b.description.trim().toLowerCase()));
                }))
        );


        this.canViewBonus$ = this.shouldShowBonus(perfSvc);

        this.calcPayout$ = this.modelChangedHook.pipe(
            // startWith(null),
            switchMap(x => {
                const doCalc$ = defer(() => this.bonusCalculator
                    .calculateBonus(this.defaultMeritIncreaseInfo.oneTimeEarning.increaseType,
                        this.defaultMeritIncreaseInfo.oneTimeEarning.increaseAmount, this.employee, this.review).pipe(
                    tap(x => {
                        if (this.defaultMeritIncreaseInfo && this.defaultMeritIncreaseInfo.oneTimeEarning && typeof x === 'number') {
                            this.defaultMeritIncreaseInfo.oneTimeEarning.proposedTotalAmount = x;
                        }
                    }),
                    map(x => {
                        if (typeof x === 'number' && !isNaN(x)) {
                            return x.toFixed(2); // formatNumber(x, navigator.language || (<any>navigator).browserLanguage, '1.2-2')
                        }
                    })
                ));

                const notApplicable$ = of('N/A');

                return iif(() => this.defaultMeritIncreaseInfo.oneTimeEarning.increaseType != null
                    && this.defaultMeritIncreaseInfo.oneTimeEarning.increaseAmount != null
                    && this.defaultMeritIncreaseInfo.canViewRates && this.bonusCalculator != null,
                    doCalc$,
                    notApplicable$);
            }));
    }
    meritFormGroup: FormGroup;

    private _defaultOneTimeEarning: OneTimeEarning;

    proposedTotal: string;

    @Input() readOnly = false;
    @Input() savedMeritIncreaseInfo: IMeritIncrease[] = [] as IMeritIncrease[];
    @Input() sideNavRef: MatSidenav;
    @Input() employee: number;
    @Input() review: number;

    recommendedBonus: RecommendedBonus;
    dataSource = new MatTableDataSource<AbstractControl>([]);

    meritIncreaseColumns: column[] = [
        { def: 'include', showWhenUserCannotViewRates: true },
        { def: 'rates', showWhenUserCannotViewRates: true },
        { def: 'current', showWhenUserCannotViewRates: false },
        { def: 'increaseType', showWhenUserCannotViewRates: true },
        { def: 'amount', showWhenUserCannotViewRates: true },
        { def: 'proposed', showWhenUserCannotViewRates: false },
        { def: 'applyMeritIncreaseOn', showWhenUserCannotViewRates: true }
        // { def: 'comments', showWhenUserCannotViewRates: true },
    ];

    oneTimeEarningColumns: column[] = [
        { def: 'include', showWhenUserCannotViewRates: true },
        { def: 'earnings', showWhenUserCannotViewRates: true },
        { def: 'increaseType', showWhenUserCannotViewRates: true },
        { def: 'amount', showWhenUserCannotViewRates: true },
        { def: 'payout', showWhenUserCannotViewRates: false },
        { def: 'mayBeIncludedInPayroll', showWhenUserCannotViewRates: true }
    ];
    recommendation: number;

    readonly increaseTypeOptions: IncreaseTypeBuilder[] = [];

    canView = true;

    meritPercent: number = null;

    clientEarnings$: Observable<IClientEarningDto[]>;
    origOneTimeEarning: OneTimeEarning;

    bonusCalculator: CalculatePayoutStrategy;
    ngAfterViewInit(): void {

    }

    isSelected(builderOne: IncreaseTypeBuilder, builderTwo: IncreaseTypeBuilder): boolean {
        return new Maybe(builderOne).map(x => x.getIncreaseType()).value() === new Maybe(builderTwo).map(x => x.getIncreaseType()).value();
    }



    /**
     * The resulting observable will indicate whether the Bonus table should show.
     */
    private shouldShowBonus(perfSvc: PerformanceReviewsService): Observable<boolean> {
        const empOneTimeEarningIsOn$ = perfSvc.getEmployeePerformanceConfiguration().pipe(
            map(x => new Maybe(x).map(settings => settings.oneTimeEarningSettings).map(settings => !settings.isArchived).valueOr(false))
        );

        const clientOneTimeEarningIsOn$ = perfSvc.getScoreModelForCurrentClient().pipe(
            map(x => new Maybe(x).map(x => x.data).map(x => x.additionalEarnings).valueOr(false))
        );

        return forkJoin(empOneTimeEarningIsOn$, clientOneTimeEarningIsOn$).pipe(
            map(x => x[0] && x[1]));
    }

    getMax(item: any): number {
        return (this.recommendedBonus != null && this.recommendedBonus.targetIncreaseAmount != null
            && this.currentIncreaseBuilder != null && this.currentIncreaseBuilder.getIncreaseType()
            === this.recommendedBonus.targetIncreaseType ? this.recommendedBonus.targetIncreaseAmount : item.increaseAmount);
    }


    private getNonNullOneTimeEarning(): OneTimeEarningRow {
        return new Maybe(this.defaultOneTimeEarning).map(x => {
            const result = x as OneTimeEarningRow;
            result.selected = x.approvalStatusID !== ApprovalStatus.Rejected;
            return result;
        }).valueOr({
            approvalStatusID: ApprovalStatus.Pending,
            clientEarningId: null,
            employeeId: 0,
            increaseAmount: 0,
            increaseType: null,
            mayBeIncludedInPayroll: this.defaultMeritIncreaseInfo.payrollRequestEffectiveDate,
            proposedTotalAmount: 0,
            selected: false
        } as OneTimeEarning);
    }

    ngOnInit() {
        if (this.defaultMeritIncreaseInfo) {

            if (!this.form.controls[meritForm])
                this.form.addControl(meritForm, this.meritFormGroup);

            this.defaultMeritIncreaseInfo.currentPayInfo.forEach(x => {
                this.calculateProposedAmount(x);
                this.increaseAmountArray.controls.push(this.createTableForm(x));
            });
            this.dataSource.data = this.increaseAmountArray.controls;

            this.updateMeritRecommendation();
        }

        // when the user reopens an evaluation that has a rejected bonus, don't show the bonus data in the table but keep the original data
        if (new Maybe(this.defaultOneTimeEarning).map(x => x.approvalStatusID === ApprovalStatus.Rejected).valueOr(true)) {
            this.origOneTimeEarning = this.getNonNullOneTimeEarning();
            this.defaultMeritIncreaseInfo.oneTimeEarning = {};
        } else {
            this.origOneTimeEarning = this.defaultMeritIncreaseInfo.oneTimeEarning = this.getNonNullOneTimeEarning();
        }
        this.defaultMeritIncreaseInfo;

        if (this.defaultMeritIncreaseInfo.oneTimeEarning.increaseType === IncreaseType.Flat) {
            this.currentIncreaseBuilder = new FlatRateBuilder(this.meritIcreaseSvc);
        } else if (this.defaultMeritIncreaseInfo.oneTimeEarning.increaseType === IncreaseType.Percentage) {
            this.currentIncreaseBuilder = new PercentBuilder(this.meritIcreaseSvc, this.msgSvc);
        }


        this.updateEarningTable();
        setTimeout(() => {
            if (!this.isEmptyObject(this.defaultMeritIncreaseInfo.oneTimeEarning)) {
                this.isEarningChecked = true;
            }
        });

        if (this.defaultMeritIncreaseInfo && this.defaultMeritIncreaseInfo.currentPayInfo && this.savedMeritIncreaseInfo) {
            this.savedMeritIncreaseInfo.forEach(x => {
                const merit = this.defaultMeritIncreaseInfo.currentPayInfo.find(y => y.employeeClientRateId === x.employeeClientRateId);
                if (merit) {
                    merit.currentAmount = x.currentAmount;
                    merit.increaseAmount = +x.increaseAmount.toFixed(2);
                    //
                    formatNumber(x.increaseAmount, navigator.language || (<any>navigator).browserLanguage, '1.2-2');
                    merit.increaseType = x.type;
                    merit.selected = true;
                    merit.proposedTotal = x.proposedAmount;
                    merit.proposalId = x.proposalId;
                    merit.applyMeritIncreaseOn = x.applyMeritIncreaseOn || this.defaultMeritIncreaseInfo.payrollRequestEffectiveDate;
                    if (x.comments != null && x.comments.length > 0) {
                        const firstComment = x.comments[0];
                        merit.comments[0] = {
                            addedBy: firstComment.addedBy,
                            addedDate: firstComment.addedDate,
                            description: firstComment.description,
                            remarkId: firstComment.remarkId,
                            isSystemGenerated: false
                        };
                    }
                }
            });
        }
        new Maybe(this.defaultMeritIncreaseInfo).map(x => x.currentPayInfo).map(x => x.forEach(y => {
            y.applyMeritIncreaseOn = new Maybe(y).map(y => y.applyMeritIncreaseOn)
                .valueOr(new Maybe(this.defaultMeritIncreaseInfo).map(z => z.payrollRequestEffectiveDate).value());
        }));
    }

    setTarget(dto: RecommendedBonus): void {
        this.recommendedBonus = dto;
        this.recommendationSubject.next(dto);
        if (this.isEmptyObject(this.defaultMeritIncreaseInfo.oneTimeEarning) && this.origOneTimeEarning.oneTimeEarningId == null) {
            new Maybe(this.recommendedBonus).map(x => x.targetIncreaseType).map(x => {
                this.origOneTimeEarning.increaseType = x;
            });

            if (this.recommendedBonus && this.recommendedBonus.targetIncreaseAmount
                && this.recommendedBonus.complete != null && this.recommendedBonus.total > 0) {
                const numToFix = this.recommendedBonus.targetIncreaseAmount * (this.recommendedBonus.complete / this.recommendedBonus.total);
                this.origOneTimeEarning.increaseAmount = +numToFix.toFixed(2);
                // +formatNumber(this.recommendedBonus.targetIncreaseAmount * (this.recommendedBonus.complete / this.recommendedBonus.total), navigator.language || (<any>navigator).browserLanguage, '1.2-2');
            }
        } else if (!this.isEmptyObject(this.defaultMeritIncreaseInfo.oneTimeEarning)
            && this.defaultMeritIncreaseInfo.oneTimeEarning.oneTimeEarningId != null) {
            new Maybe(this.recommendedBonus).map(x => x.targetIncreaseType).map(x => {
                if (this.defaultMeritIncreaseInfo.oneTimeEarning.increaseType == null) {
                    this.defaultMeritIncreaseInfo.oneTimeEarning.increaseType = x;
                }
            });

            if (this.recommendedBonus && this.recommendedBonus.targetIncreaseAmount
                && this.recommendedBonus.complete != null
                && this.recommendedBonus.total > 0 && this.defaultMeritIncreaseInfo.oneTimeEarning.increaseAmount === 0) {
                const numToFix = this.recommendedBonus.targetIncreaseAmount * (this.recommendedBonus.complete / this.recommendedBonus.total);
                this.defaultMeritIncreaseInfo.oneTimeEarning.increaseAmount = +numToFix.toFixed(2);
                // +formatNumber(this.recommendedBonus.targetIncreaseAmount * (this.recommendedBonus.complete / this.recommendedBonus.total), navigator.language || (<any>navigator).browserLanguage, '1.2-2');
            }

            if (this.defaultMeritIncreaseInfo.oneTimeEarning.increaseAmount != null
                && this.defaultMeritIncreaseInfo.oneTimeEarning.increaseType != null && this.isEarningChecked) {
                if (this.defaultMeritIncreaseInfo.oneTimeEarning.increaseType === IncreaseType.Flat) {
                    this.currentIncreaseBuilder = new FlatRateBuilder(this.meritIcreaseSvc);
                } else if (this.defaultMeritIncreaseInfo.oneTimeEarning.increaseType === IncreaseType.Percentage) {
                    this.currentIncreaseBuilder = new PercentBuilder(this.meritIcreaseSvc, this.msgSvc);
                }

                if (this.defaultMeritIncreaseInfo.oneTimeEarning.increaseType === IncreaseType.Flat
                    || this.defaultMeritIncreaseInfo.oneTimeEarning.increaseType === IncreaseType.Percentage) {
                    this.setCalculationStrategy(this.currentIncreaseBuilder);
                }
                this.calcBonus(this.defaultMeritIncreaseInfo.oneTimeEarning, true);
            }
        }
    }

    calcBonus(item: OneTimeEarning, formIsValid: boolean): void {
        this.modelChangedHook.next({ item, isValid: formIsValid });
    }

    setIncreaseTypeOnItem(item: OneTimeEarning, increaseType: IncreaseType): void {
        item.increaseType = increaseType;
    }


    increaseTypeChanged(item: OneTimeEarning, builder: IncreaseTypeBuilder, isValid: boolean): void {
        this.setIncreaseTypeOnItem(item, builder.getIncreaseType());
        this.setCalculationStrategy(builder);
        this.calcBonus(item, isValid);
    }

    private createTableForm(record: IMeritIncreaseCurrentPayAndRates): FormGroup {
        const locale = (<any>navigator).browserLanguage != null ? (<any>navigator).browserLanguage : navigator.language;
        const hasCommentsToDisplay = record.comments != null && record.comments.length > 0;
        if (!hasCommentsToDisplay) record.comments = [<IRemark>{}];

        if (!record.increaseType) record.increaseType = MeritIncreaseType.Percent;

        const selCtl = this.fb.control({ value: record.selected, disabled: this.readOnly });
        const incTypeCtl = this.fb.control({ value: record.increaseType, disabled: (!record.selected || this.readOnly) });
        const incAmtCtl = this.fb.control({ value: record.increaseAmount, disabled: (!record.selected || this.readOnly) });

        merge(selCtl.valueChanges, incTypeCtl.valueChanges, incAmtCtl.valueChanges)
            .subscribe(next => { this.increaseAmountArray.updateValueAndValidity(); });

        const k = this.fb.group({
            selectedCtl: selCtl,
            increaseTypeCtl: incTypeCtl,
            increaseAmountCtl: incAmtCtl,
            commentsCtl: this.fb.control({ value: record.comments[0].description, disabled: (!record.selected || this.readOnly) }),
            applyMeritIncreaseOnCtl: this.fb.control({ value: record.applyMeritIncreaseOn || this.defaultMeritIncreaseInfo.payrollRequestEffectiveDate, disabled: (!record.selected || this.readOnly) }),
            // Data
            rateDesc: record.rateDesc,
            isSalaryRow: record.isSalaryRow,
            payFrequencyDesc: record.payFrequencyDesc,
            currentAmount: record.currentAmount,
            proposedTotal: record.proposedTotal,
        }, { validators: this.validateIncrease(), updateOn: 'change' });

        return k;
    }

    copyFormGroupToModel(fgItem: FormGroup, item: IMeritIncreaseCurrentPayAndRates): void {
        item.selected = fgItem.controls.selectedCtl.value;
        item.increaseType = fgItem.controls.increaseTypeCtl.value;
        item.increaseAmount = fgItem.controls.increaseAmountCtl.value;
        item.comments = fgItem.controls.commentsCtl.value;
        item.applyMeritIncreaseOn = fgItem.controls.applyMeritIncreaseOnCtl.value;

        this.enableFormGroupControls(fgItem, (!item.selected || this.readOnly));
    }

    enableFormGroupControls(fgItem: FormGroup, disabled: boolean) {
        if (disabled) {
            fgItem.controls.increaseTypeCtl.disable();
            fgItem.controls.increaseAmountCtl.disable();
            fgItem.controls.commentsCtl.disable();
            fgItem.controls.applyMeritIncreaseOnCtl.disable();
        } else {
            fgItem.controls.increaseTypeCtl.enable();
            fgItem.controls.increaseAmountCtl.enable();
            fgItem.controls.commentsCtl.enable();
            fgItem.controls.applyMeritIncreaseOnCtl.enable();
        }
    }

    validateIncrease(): ValidatorFn {
        return (fg: FormGroup): { [key: string]: any } | null => {
            if (fg.controls.selectedCtl.value) {
                if (!fg.controls.increaseAmountCtl.value) return { 'required': {} };

                if (fg.controls.increaseTypeCtl.value === MeritIncreaseType.Percent) {
                    return (fg.controls.increaseAmountCtl.value >= -100.00 &&
                        fg.controls.increaseAmountCtl.value <= 100.00) ? null : { 'mustBePercent': {} };
                }
            }
            // FG VALID
            return null;
        };
    }

    modelChanged(k: FormGroup, index: number): void {
        const item: IMeritIncreaseCurrentPayAndRates = this.defaultMeritIncreaseInfo.currentPayInfo[index];
        // As model changes are unkown copy all values from form
        this.copyFormGroupToModel(k, item);
        this.calculateProposedAmount(item);

        k.controls.proposedTotal.setValue(item.proposedTotal);
    }

    calculateProposedAmount(item: IMeritIncreaseCurrentPayAndRates) {
        let proposedAmt: number;

        switch (+item.increaseType) {
            case MeritIncreaseType.Flat:
                proposedAmt = +item.currentAmount + +item.increaseAmount;
                break;
            case MeritIncreaseType.Percent:
                proposedAmt = +item.currentAmount * (1 + (+item.increaseAmount / 100));
                break;
            default:
                proposedAmt = +item.currentAmount;
                break;
        }

        item.proposedTotal = proposedAmt;
        item.amountIncreased = +item.proposedTotal - +item.currentAmount;
    }

    isActive(item: IMeritIncreaseCurrentPayAndRates) {
        return item.selected;
    }

    bonusSelected(item: OneTimeEarningRow, isValid: boolean) {
        if (this.isEmptyObject(item)) {
            this.defaultMeritIncreaseInfo.oneTimeEarning = this.origOneTimeEarning;

            const currentIncreaseType = this.defaultMeritIncreaseInfo.oneTimeEarning.increaseType;

            if (currentIncreaseType === IncreaseType.Flat) {
                this.currentIncreaseBuilder = new FlatRateBuilder(this.meritIcreaseSvc);
            } else if (currentIncreaseType === IncreaseType.Percentage) {
                this.currentIncreaseBuilder = new PercentBuilder(this.meritIcreaseSvc, this.msgSvc);
            }

            if (currentIncreaseType === IncreaseType.Flat || currentIncreaseType === IncreaseType.Percentage) {
                this.setCalculationStrategy(this.currentIncreaseBuilder);
            }

            this.earningClicked.next(null);
            this.updateEarningTable();
            setTimeout(() => this.calcBonus(item, isValid));

        } else {
            this.origOneTimeEarning = item;
            this.bonusCalculator = null;
            this.defaultMeritIncreaseInfo.oneTimeEarning = {};
            this.earningClicked.next(null);
            this.updateEarningTable();
        }

    }

    rateSelected(k: FormGroup, index: number): void {
        const item: IMeritIncreaseCurrentPayAndRates = this.defaultMeritIncreaseInfo.currentPayInfo[index];

        const incAmount = k.controls.increaseAmountCtl.value;
        if ((incAmount == null || incAmount === 0) && k.controls.increaseTypeCtl.value === MeritIncreaseType.Percent) {
            this.meritPercent = this.activeEvaluationSvc
            .findRecommendedIncrease(this._score, this.defaultMeritIncreaseInfo.meritRecommendations);
            item.increaseAmount = +this.meritPercent.toFixed(2);
            // formatNumber(this.meritPercent, navigator.language || (<any>navigator).browserLanguage, '1.2-2');
            k.controls.increaseAmountCtl.setValue(item.increaseAmount);
        }

        this.modelChanged(k, index);
    }

    isEmptyObject(val: any): boolean {
        return new Maybe(val).map(x => Object.keys(x)).map(x => x.length === 0).valueOr(true);
    }

    updateMeritRecommendation() {
        this.meritPercent = null;
        if (!this.defaultMeritIncreaseInfo || !this.defaultMeritIncreaseInfo.meritRecommendations || this._score === null) {
            return;
        }
        this.safeLoop(this.defaultMeritIncreaseInfo.meritRecommendations, x => {
            if (this._score > x.minScore && this._score <= x.maxScore) {
                this.meritPercent = x.meritPercent || 0;
            }
        });
    }

    setCalculationStrategy(option: IncreaseTypeBuilder): void {
        this.bonusCalculator = option.getCalculationStrategy();
    }

    private safeLoop(array: IMeritLimit[], action: (value: IMeritLimit) => void): void {
        if (array == null) return;
        array.forEach(action);
    }

    getDisplayedColumns(columns: column[]): string[] {
        return columns
            .filter(cd => this.defaultMeritIncreaseInfo.canViewRates || cd.showWhenUserCannotViewRates)
            .map(cd => cd.def);
    }

    closeDrawer(): void {
        this.sideNavRef.close();
    }

    private updateEarningTable(): void {
        this.additionalEarningData.next([this.defaultMeritIncreaseInfo.oneTimeEarning]);
    }
}

interface CalculatePayoutStrategy {
    calculateBonus(increaseType: IncreaseType, increaseAmount: number, employeeId: number, reviewId: number): Observable<string | number>;
}

class PercentageStrategy implements CalculatePayoutStrategy {

    myForm: any;


    constructor(
        private meritIcreaseSvc: MeritIncreaseService,
        private msgSvc: DsMsgService) { }

    calculateBonus(increaseType: IncreaseType, increaseAmount: number, employeeId: number, reviewId: number): Observable<string | number> {
        return this.meritIcreaseSvc.CalculateBonus(increaseType, increaseAmount, employeeId, reviewId).pipe(
            catchError(y => {
                return of('N/A');
            }),
            map(x => new Maybe(this.myForm).map(form => form.valid).valueOr(false) ? 'N/A' : x));
    }

}

class FlatRateStrategy implements CalculatePayoutStrategy {

    constructor(private meritIcreaseSvc: MeritIncreaseService) { }

    calculateBonus(increaseType: IncreaseType, increaseAmount: number, employeeId: number, reviewId: number): Observable<string | number> {
        return of(increaseAmount);
    }

}

interface IncreaseTypeBuilder {
    getCalculationStrategy(): CalculatePayoutStrategy;
    getIncreaseType(): IncreaseType;
    getName(): string;
}

class PercentBuilder implements IncreaseTypeBuilder {
    getName(): string {
        return 'Percent';
    }

    constructor(
        private meritIcreaseSvc: MeritIncreaseService,
        private msgSvc: DsMsgService) { }

    getCalculationStrategy(): CalculatePayoutStrategy {
        return new PercentageStrategy(this.meritIcreaseSvc, this.msgSvc);
    }
    getIncreaseType(): IncreaseType {
        return IncreaseType.Percentage;
    }
}

class FlatRateBuilder implements IncreaseTypeBuilder {
    getName(): string {
        return 'Flat';
    }

    constructor(private meritIcreaseSvc: MeritIncreaseService) { }

    getCalculationStrategy(): CalculatePayoutStrategy {
        return new FlatRateStrategy(this.meritIcreaseSvc);
    }
    getIncreaseType(): IncreaseType {
        return IncreaseType.Flat;
    }


}


interface OneTimeEarningRow extends OneTimeEarning {
    selected?: boolean;
}

interface column { def: string; showWhenUserCannotViewRates: boolean; }

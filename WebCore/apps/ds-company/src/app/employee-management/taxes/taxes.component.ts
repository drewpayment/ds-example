import { UserType } from "@ajs/user";
import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
  OnInit,
} from "@angular/core";
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { AccountService } from "@ds/core/account.service";
import {
  AppConfig,
  APP_CONFIG,
} from "@ds/core/app-config/app-config";
import { ClientService } from "@ds/core/clients/shared";
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import {
  BehaviorSubject,
  EMPTY,
  forkJoin,
  NEVER,
  Observable,
  of,
  Subject,
  throwError,
} from "rxjs";
import {
  catchError,
  filter,
  map,
  switchMap,
  takeUntil,
  tap,
} from "rxjs/operators";
import { AdditionalAmountType } from "../../enums/additional-amount-type-enum";
import { EmployeeTaxesService } from "../../services/employee-taxes.service";
import { Features } from "@ds/admin/client-statistics/shared/models/featureEnum";
import { ConfigUrl, ConfigUrlType } from "@ds/core/shared/config-url.model";
import { UserInfo } from "@ds/core/shared";
import * as moment from "moment";
import { MatTableDataSource } from "@angular/material/table";
import { MatDialog } from "@angular/material/dialog";
import { ConfigureCostCentersDialogComponent } from "./configure-cost-centers-dialog/configure-cost-centers-dialog.component";
import { Store } from "@ngrx/store";
import {
  EmployeeState,
  getEmployeeState,
} from "@ds/employees/header/ngrx/reducer";
import { IEmployeeSearchResult } from "@ds/employees/search/shared/models/employee-search-result";
import { AddTaxDialogComponent } from "./add-tax-dialog/add-tax-dialog.component";
import { ClientAccountFeature } from '@ds/core/clients/shared/client-account-feature.model';
import { DecimalPipe } from "@angular/common";
import { IBasicTax, IEmployeeTaxAdmin, IEmployeeTaxSetup, IEmployeeNonFederalTax, KeyValue } from '@models';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: "ds-taxes",
  templateUrl: "./taxes.component.html",
  styleUrls: ["./taxes.component.scss"],
})
export class TaxesComponent implements OnInit, OnDestroy {
  destroy$ = new Subject();
  baseUrl = this.config.baseSite.url;
  breadcrumb: string;
  essPay: string;
  form: FormGroup = this.createForm();
  reminderDateForm: FormGroup = this.createReminderDateForm();
  minDate: Date;
  formSubmitted: boolean;
  taxInfoLoaded = false;
  hasError: boolean;
  sutaStates: IBasicTax[] = [];
  wotcReasons: KeyValue[] = [];
  clientId: number;
  employeeId: number;
  essViewOnly: boolean;
  defaultKeyValue: KeyValue = { id: null, description: "" };
  taxInfo: IEmployeeTaxAdmin;
  hasPennsylvaniaLocalTax: boolean;
  hasYtdTaxOrWages: boolean;
  showReimburseColumn: boolean;
  isSupervisorAccessing: boolean;
  isSupervisorOnHimself: boolean;
  allowIncomeWageExemptOption: boolean;
  stateTaxCount: number;
  splitTaxesByCostCenter: boolean;
  userInfo: UserInfo;
  empMustHaveEmpPaySetup = false;
  empPayUrl = '';

  get NonFederalTaxes() {
    return this.form.get("nonFederalTaxes") as FormArray;
  }

  get Using2020FederalW4Setup() {
    return this.form.get("federalTax.using2020FederalW4Setup") as FormControl;
  }

  get FederalWithholdingOption() {
    return this.form.get("federalTax.additionalAmountTypeId") as FormControl;
  }

  get FederalAdditionalPercent() {
    return this.form.get("federalTax.additionalPercent") as FormControl;
  }

  get FederalAdditionalAmount() {
    return this.form.get("federalTax.additionalAmount") as FormControl;
  }

  get SutaClientTaxId() {
    return this.form.get("generalInfo.sutaClientTaxId") as FormControl;
  }

  get Is1099Exempt() {
    return this.form.get("generalInfo.is1099Exempt") as FormControl;
  }

  get PsdCode() {
    return this.form.get("generalInfo.psdCode") as FormControl;
  }

  get IsReminderDateChecked() {
    return this.reminderDateForm.get('isReminderDateChecked') as FormControl;
  }

  get ReminderDate() {
    return this.reminderDateForm.get("reminderDate") as FormControl;
  }

  newFederalTaxDisplayColumns: string[] = [
    "filingStatus",
    "taxCredit",
    "otherIncome",
    "wageDeductions",
    "withholdingOptions",
    "percent",
    "flat",
  ];
  oldFederalTaxDisplayColumns: string[] = [
    "filingStatus",
    "numberOfExemptions",
    "numberOfDependents",
    "withholdingOptions",
    "percent",
    "flat",
  ];
  nonFederalTaxDisplayColumns: string[] = [
    "tax",
    "filingStatus",
    "numberOfExemptions",
    "numberOfDependents",
    "withholdingOptions",
    "percent",
    "flat",
    "reimburse",
    "edit",
  ];

  federalTaxDatasource = new MatTableDataSource<IEmployeeTaxSetup>([]);
  nonFederalTaxDatasource = new MatTableDataSource<IEmployeeNonFederalTax>([]);

  payrollUrl: ConfigUrl;
  companyUrl: ConfigUrl;

  selectedEmployee$ = this.store.select(
    getEmployeeState((x) => x.selectedEmployee)
  ) as any as Observable<IEmployeeSearchResult>;

  constructor(
    private fb: FormBuilder,
    private employeeTaxesService: EmployeeTaxesService,
    private accountService: AccountService,
    private clientService: ClientService,
    private ngxMsg: NgxMessageService,
    private dialog: MatDialog,
    private confirmDialog: ConfirmDialogService,
    @Inject(APP_CONFIG) private config: AppConfig,
    private store: Store<EmployeeState>,
    private decimal: DecimalPipe,
  ) {}

  ngOnInit() {
    this.selectedEmployee$
      .pipe(
        takeUntil(this.destroy$),
        filter((x) => !!x && x.employeeId != this.employeeId),
        tap(() => {
          if (this.empMustHaveEmpPaySetup) {
            this.taxInfoLoaded = false;
            this.empMustHaveEmpPaySetup = false;
          }
        }),
        switchMap((ee) =>
          forkJoin([
            this.accountService.getSiteUrls(),
            this.accountService.getUserInfo(),
            of(ee),
          ])
        ),
        map(
          ([sites, user, ee]: [
            ConfigUrl[],
            UserInfo,
            IEmployeeSearchResult
          ]) => {
            this.payrollUrl = sites.find(
              (s) => s.siteType === ConfigUrlType.Payroll
            );
            let essUrl = sites.find((s) => s.siteType === ConfigUrlType.Ess);
            this.companyUrl = sites.find(
              (s) => s.siteType === ConfigUrlType.Company
            );
            this.breadcrumb = `${this.payrollUrl.url}ChangeEmployee.aspx?SubMenu=Employee&Force=True&URL=${this.companyUrl.url}manage/taxes`;
            this.essPay = `${essUrl.url}pay`;
            this.empPayUrl = `${this.payrollUrl.url}EmployeePay.aspx`;
            this.minDate = new Date();
            this.minDate.setHours(0, 0, 0, 0);
            return [user, ee];
          }
        ),
        switchMap(([user, ee]: [UserInfo, IEmployeeSearchResult]) => {
          this.userInfo = user;
          this.isSupervisorAccessing = user.userTypeId === UserType.supervisor;
          this.isSupervisorOnHimself =
            this.isSupervisorAccessing && user.employeeId === ee.employeeId;
          this.clientId = user.selectedClientId();
          this.employeeId = ee.employeeId;
          this.essViewOnly = user.isEmployeeSelfServiceViewOnly;

          // if the user doesn't have an employee selected, redirect them to the employee select list
          if (this.employeeId == null || this.employeeId < 1) {
            document.location.href = this.breadcrumb;
            return EMPTY;
          }

          return this.employeeTaxesService
            .getEmployeeTaxInfo(this.clientId, this.employeeId)
            .pipe(catchError(err => {
              this.taxInfoLoaded = true;
              this.empMustHaveEmpPaySetup = true;
              return EMPTY;
            }));
        }),
        filter(x => !!x),
        switchMap((taxes: IEmployeeTaxAdmin) => {
          return forkJoin([
            of(taxes),
            this.clientService.getClientAccountFeatureByFeatureId(this.clientId, Features.SplitTaxesByCostCenter),
            this.employeeTaxesService.getClientSutaStateList(this.clientId, taxes.generalInfo.sutaClientTaxId),
            !this.wotcReasons.length ? this.employeeTaxesService.getWotcReasons() : this.wotcReasons, // Only get WOTC reasons if we haven't gotten them already
          ]);
        }),
        tap(([taxes, feature, sutaStates, wotcReasons]: [IEmployeeTaxAdmin, ClientAccountFeature, IBasicTax[], KeyValue[]]) => {
          this.empMustHaveEmpPaySetup = false;
          // do some stuff with taxes
          this.taxInfo = taxes;
          this.hasYtdTaxOrWages = taxes.generalInfo.hasYtdTaxOrWages;
          this.showReimburseColumn = taxes.generalInfo.clientHasReimbursableEarning;
          this.allowIncomeWageExemptOption = this.userInfo.isInMinimumRole(UserType.systemAdmin) || taxes.generalInfo.allowIncomeWageExemptOption;

          this.setStateTaxCount();
          this.setHasPennsylvaniaLocalTax();

          this.splitTaxesByCostCenter = feature != null;

          const sutaTaxIds = new Set(this.sutaStates.map(x => x.taxId));
          this.sutaStates = this.sutaStates != null && this.sutaStates.length
            ? [...this.sutaStates, ...sutaStates.filter(x => !sutaTaxIds.has(x.taxId))]
            : sutaStates;

          if (!this.wotcReasons.length) { // Set the WOTC reasons if we haven't already set them. Causes WOTC Reason to not be selected properly when changing EEs
            this.wotcReasons = [this.defaultKeyValue, ...wotcReasons];
          }

        }),
        tap(() => {
          this.federalTaxDatasource.data[0] = this.taxInfo.federalTax;
          this.nonFederalTaxDatasource.data = this.taxInfo.nonFederalTaxes;
          this.patchForm(this.taxInfo);
          this.disableFormControl(this.ReminderDate);
          this.taxInfoLoaded = true;
        }),
      )
      .subscribe();
    this.reminderDate();
  }
  ngOnDestroy() {
    this.destroy$.next();
  }

  private createForm() {
    return this.fb.group({
      employeeId: this.fb.control(null),
      clientId: this.fb.control(null),
      federalTax: this.fb.group({
        employeeTaxId: this.fb.control(null),
        employeeId: this.fb.control(null),
        clientTaxId: this.fb.control(null),
        filingStatusId: this.fb.control(null),
        numberOfExemptions:
          this.fb.control(
            null,
            [Validators.required, Validators.pattern('[0-9]*')]
          ),
        numberOfDependents:
          this.fb.control(
            null,
            [Validators.required, Validators.pattern('[0-9]*')]
          ),
        additionalAmountTypeId: this.fb.control(null),
        additionalAmount:
          this.fb.control(
            this.decimal.transform(0, '1.2-2'),
            {
              updateOn: 'blur',
              validators: Validators.required
            }
          ),
        additionalPercent:
          this.fb.control(
            this.decimal.transform(0, '1.4-4'),
            {
              updateOn: 'blur',
              validators: Validators.required
            }
          ),
        taxCredit:
          this.fb.control(
            this.decimal.transform(0, '1.2-2'),
            {
              updateOn: 'blur',
              validators: Validators.required
            }
          ),
        otherTaxableIncome:
          this.fb.control(
            this.decimal.transform(0, '1.2-2'),
            {
              updateOn: 'blur',
              validators: Validators.required
            }
          ),
        wageDeduction:
          this.fb.control(
            this.decimal.transform(0, '1.2-2'),
            {
              updateOn: 'blur',
              validators: Validators.required
            }
          ),
        hasMoreThanOneJob: this.fb.control(null),
        using2020FederalW4Setup: this.fb.control(null),
      }),
      nonFederalTaxes: this.fb.array([]),
      generalInfo: this.fb.group({
        employeePayId: this.fb.control(null),
        sutaClientTaxId: this.fb.control(null, [Validators.required]),
        psdCode: this.fb.control(null,
          [Validators.pattern("^[0-9]*$"),]
        ),
        wotcReasonId: this.fb.control(null),
        deferEmployeeSocSecTax: this.fb.control(null),
        is1099Exempt: this.fb.control(null),
        isFicaExempt: this.fb.control(null),
        isFutaExempt: this.fb.control(null),
        isSutaExempt: this.fb.control(null),
        isSocSecExempt: this.fb.control(null),
        isStateTaxExempt: this.fb.control(null),
        isIncomeTaxExempt: this.fb.control(null),
        isHireActQualified: this.fb.control(null),
        hireActStartDate: this.fb.control(null),
      })
    })
  }

  private patchFederalTax(federalTax: IEmployeeTaxSetup) {
    const fedTax = {
      employeeTaxId: federalTax.employeeTaxId,
      employeeId: federalTax.employeeId,
      clientTaxId: federalTax.clientTaxId,
      filingStatusId: federalTax.filingStatusId,
      numberOfExemptions: federalTax.numberOfExemptions,
      numberOfDependents: federalTax.numberOfDependents,
      additionalAmountTypeId: federalTax.additionalAmountTypeId,
      additionalAmount: this.decimal.transform(federalTax.additionalAmount, '1.2-2'),
      additionalPercent: this.decimal.transform(federalTax.additionalPercent, '1.4-4'),
      taxCredit: this.decimal.transform(federalTax.taxCredit, '1.2-2'),
      otherTaxableIncome: this.decimal.transform(federalTax.otherTaxableIncome, '1.2-2'),
      wageDeduction: this.decimal.transform(federalTax.wageDeduction, '1.2-2'),
      hasMoreThanOneJob: federalTax.hasMoreThanOneJob,
      using2020FederalW4Setup: federalTax.using2020FederalW4Setup
    };

    return fedTax;
  }

  private patchForm(taxInfo: IEmployeeTaxAdmin): void {
    this.form.patchValue({
      employeeId: taxInfo.employeeId,
      clientId: taxInfo.clientId,
      //federalTax: taxInfo.federalTax,
      federalTax: this.patchFederalTax(taxInfo.federalTax),
      generalInfo: taxInfo.generalInfo,
    });

    this.createNonFederalTaxesFormArray(taxInfo.nonFederalTaxes);

    this.toggleEnabledStateOfAdditionalTaxControls(
      this.FederalWithholdingOption.value,
      false,
      this.FederalAdditionalPercent,
      this.FederalAdditionalAmount
    );

    if (this.Is1099Exempt.value) {
      this.disableFormControl(this.SutaClientTaxId); // disable SUTA state control
      this.disableOtherTaxWithholdingsTable(); // disable all controls on Other Tax Withholdings table
    } else {
      this.enableFormControl(this.SutaClientTaxId)
      this.toggleEnabledStateOfNonFederalTaxes();
    }
  }

  private patchNonFederalTaxes(nonFederalTaxes: IEmployeeNonFederalTax[]): void {
    this.createNonFederalTaxesFormArray(nonFederalTaxes);
    this.toggleEnabledStateOfNonFederalTaxes();
  }

  private toggleEnabledStateOfNonFederalTaxes() {
    for (let control of this.NonFederalTaxes.controls) {
      let additionalAmountTypeId = +control.get("additionalAmountTypeId")
        .value;
      let blockOverrides = control.get("blockOverrides").value;
      let additionalPercent = control.get("additionalPercent") as FormControl;
      let additionalAmount = control.get("additionalAmount") as FormControl;

      this.toggleEnabledStateOfAdditionalTaxControls(
        additionalAmountTypeId,
        blockOverrides,
        additionalPercent,
        additionalAmount
      );
    }
  }

  private createNonFederalTaxesFormArray(
    nonFederalTaxes: IEmployeeNonFederalTax[]
  ): void {

    let nonFedTaxArray = this.fb.array([]);

    nonFederalTaxes.forEach((tax) => {
      let group =
        this.fb.group({
          employeeTaxId: this.fb.control(tax.employeeTaxId),
          clientTaxId: this.fb.control(tax.clientTaxId),
          stateTaxId: this.fb.control(tax.stateTaxId),
          localTaxId: this.fb.control(tax.localTaxId),
          disabilityTaxId: this.fb.control(tax.disabilityTaxId),
          description: this.fb.control(tax.description),
          filingStatusId: this.fb.control(tax.filingStatusId),
          numberOfExemptions:
            this.fb.control(
              tax.numberOfExemptions,
              {
                updateOn: 'blur',
                validators: [Validators.required, Validators.pattern('[0-9]*')]
              }

            ),
          numberOfDependents:
            this.fb.control(
              tax.numberOfDependents,
              {
                updateOn: 'blur',
                validators: [Validators.required, Validators.pattern('[0-9]*')]
              }

            ),
          additionalAmountTypeId: this.fb.control(tax.additionalAmountTypeId),
          additionalPercent:
            this.fb.control(
              this.decimal.transform(tax.additionalPercent, '1.4-4'),
              {
                updateOn: 'blur',
                validators: Validators.required
              },
            ),
          additionalAmount:
            this.fb.control(
              this.decimal.transform(tax.additionalAmount, '1.2-2'),
              {
                updateOn: 'blur',
                validators: Validators.required
              }
            ),
          isResident: this.fb.control(tax.isResident),
          isActive: this.fb.control(tax.isActive),
          isReimbursable: this.fb.control(tax.isReimbursable),
          reimburse: this.fb.control(tax.reimburse),
          blockOverrides: this.fb.control(tax.blockOverrides),
          stateInfo: this.fb.control(tax.stateInfo),
          hasTaxHistory: this.fb.control(tax.taxHasHistory),
          localTaxCode: this.fb.control(tax.localTaxCode),
          taxTypeId: this.fb.control(tax.taxTypeId),
          filingStatuses: this.fb.control(tax.filingStatuses),
          withholdingOptions: this.fb.control(tax.withholdingOptions),
        })

      nonFedTaxArray.push(group);

    });

    this.form.setControl('nonFederalTaxes', nonFedTaxArray);
  }

  private createReminderDateForm() {
    return this.fb.group({
      isReminderDateChecked: this.fb.control(false),
      reminderDate: this.fb.control(moment().add(1, 'day'))
    });
  }

  saveTaxInfo() {
    this.formSubmitted = true;
    this.form.markAllAsTouched();

    if (this.form.invalid) return;

    // Check if one state tax is active
    if (!this.checkOneStateTaxActive(this.NonFederalTaxes.value)) {
      this.displayNoActiveStateTaxWarning();
      return;
    }

    // Check to see if 1099 exempt was changed and EE has YTD tax and/or wages
    if (this.Is1099Exempt.value != this.taxInfo.generalInfo.is1099Exempt && this.hasYtdTaxOrWages) {
      this.display1099Warning();
      return;
    }

    if (this.IsReminderDateChecked.value) {
      const options = {
        title: `Are you sure you wish to save these taxes with a Reminder Date of ${moment(
          this.ReminderDate.value
        ).format(
          "MM/DD/YYYY"
        )}? You will be able to apply these changes at a later date.`,
        confirm: "Save",
      };

      this.confirmDialog.open(options);

      this.confirmDialog
        .confirmed()
        .pipe(
          switchMap((confirmed) => {
            if (confirmed) {
              this.ngxMsg.setSuccessMessage("Saving reminder dates...");
              return this.employeeTaxesService.saveReminderDates(
                moment(this.ReminderDate.value).format("YYYY-MM-DD"),
                this.form.getRawValue()
              );
            } else {
              return NEVER;
            }
          }),
          catchError((error) => {
            this.displayErrorMessage(error);
            return throwError(error);
          })
        )
        .subscribe((result) => {
          this.federalTaxDatasource.data[0] = this.taxInfo.federalTax;
          this.nonFederalTaxDatasource.data = this.taxInfo.nonFederalTaxes;
          this.patchForm(this.taxInfo);

          this.ngxMsg.setSuccessMessage("Reminder dates saved successfully");
          this.formSubmitted = false;
        });
    } else {
      this.ngxMsg.setSuccessMessage("Saving employee tax information...")
      this.employeeTaxesService
        .saveEmployeeTaxes(this.form.getRawValue())
        .pipe(
          catchError((error) => {
            this.displayErrorMessage(error);
            return throwError(error);
          }),
          tap((x) => {
            let sutaTaxChanged =
              this.taxInfo.generalInfo.sutaClientTaxId !=
              x.generalInfo.sutaClientTaxId;
            this.taxInfo = x;
            this.nonFederalTaxDatasource.data = x.nonFederalTaxes;

            if (sutaTaxChanged) {
              this.patchForm(x);
            }

            this.form.reset({...this.form.getRawValue()});
            this.ngxMsg.setSuccessMessage("Employee taxes saved successfully");
            this.formSubmitted = false;
          })
        )
        .subscribe();
    }
  }

  toggleFederalW4Setup() {
    this.Using2020FederalW4Setup.setValue(!this.Using2020FederalW4Setup.value);
  }

  federalTaxWithholdingOptionChanged(id: number) {
    this.FederalWithholdingOption.setValue(id);

    this.toggleEnabledStateOfAdditionalTaxControls(
      id,
      false,
      this.FederalAdditionalPercent,
      this.FederalAdditionalAmount
    );
  }

  nonFederalTaxWithholdingOptionChanged(index: number, id: number) {
    let tax = this.NonFederalTaxes.at(index) as FormGroup;
    let isStateTax = tax.get("stateTaxId").value != null;
    let isLocalTax = tax.get("localTaxId").value != null;
    let withholdingOption = tax.get("additionalAmountTypeId").value;

    // if changing withholding option for a state tax and changing to Stop Tax & Wages, check that at least one state tax is active (i.e. not Stop Tax & Wages)
    if (isStateTax) {
      if (
        !this.Is1099Exempt.value &&
        isStateTax &&
        withholdingOption == AdditionalAmountType.StopTaxAndWages &&
        !this.checkOneStateTaxActive(this.NonFederalTaxes.value)
      ) {
        this.displayNoActiveStateTaxWarning();
      } else {
        // set every other state tax to Stop Tax & Wages
        this.setOtherStateTaxesToStopTaxAndWages(tax, withholdingOption);
      }
    } else if (isLocalTax) {
      this.checkOppositeLocalTax(tax);
    }

    let blockOverrides = tax.get("blockOverrides").value;
    let additionalPercent = tax.get("additionalPercent") as FormControl;
    let additionalAmount = tax.get("additionalAmount") as FormControl;
    let additionalAmountTypeId = tax.get(
      "additionalAmountTypeId"
    ) as FormControl;
    let isActive = tax.get("isActive") as FormControl;

    additionalAmountTypeId.setValue(id);
    isActive.setValue(id != AdditionalAmountType.StopTaxAndWages);

    this.toggleEnabledStateOfAdditionalTaxControls(
      id,
      blockOverrides,
      additionalPercent,
      additionalAmount
    );
  }

  checkOneStateTaxActive(taxesToCheck: IEmployeeNonFederalTax[]) {
    for(let i = 0; i < taxesToCheck.length; i++ ) {
      let tax = taxesToCheck[i];
        if (tax.stateTaxId && +tax.additionalAmountTypeId != AdditionalAmountType.StopTaxAndWages) {
        return true;
      }
    }

    return false;
  }

  /**
   * Sets all other state taxes to Stop Tax and Wages.
   * If a disability tax is active that isn't associated with the state, it is deactivated.
   * Likewise, if a disability tax is associated with the state, it is activated.
   * @param changedTax: the form group for the state tax that was changed
   * @param withholdingOption: that withholding option that the state tax was changed to.
   */
  setOtherStateTaxesToStopTaxAndWages(
    changedTax: FormGroup,
    withholdingOption: AdditionalAmountType
  ) {
    let employeeTaxId = changedTax.get("employeeTaxId").value;
    let clientTaxId = changedTax.get("clientTaxId").value;
    let stateInfo = changedTax.get("stateInfo").value as KeyValue;

    // loop thru each non-federal tax
    for (let tax of this.NonFederalTaxes.controls) {
      let eeTaxId = tax.get("employeeTaxId").value;
      let cTaxId = tax.get("clientTaxId").value;
      let isStateTax = tax.get("stateTaxId").value != null;
      let isDisabilityTax = tax.get("disabilityTaxId").value != null;
      let state = tax.get("stateInfo").value as KeyValue;
      let additionalAmountTypeId = tax.get(
        "additionalAmountTypeId"
      ) as FormControl;
      let isActive = tax.get("isActive") as FormControl;
      let blockOverrides = tax.get("blockOverrides").value as boolean;
      let additionalPercent = tax.get("additionalPercent") as FormControl;
      let additionalAmount = tax.get("additionalAmount") as FormControl;

      if (isStateTax && cTaxId != clientTaxId) {
        // if this is a state tax and is not the state tax that was changed, set it to Stop Tax & Wages and deactivate
        additionalAmountTypeId.setValue(AdditionalAmountType.StopTaxAndWages);
        isActive.setValue(false);
        this.toggleEnabledStateOfAdditionalTaxControls(
          additionalAmountTypeId.value,
          blockOverrides,
          additionalPercent,
          additionalAmount
        );
      }

      if (isDisabilityTax && state.id != 40) {
        // if this is a disability tax and not PA disability
        if (state.id != stateInfo.id) {
          // if the disability tax belongs to a different state than the state tax that was changed, set it to Stop Tax & Wages and deactivate
          additionalAmountTypeId.setValue(AdditionalAmountType.StopTaxAndWages);
          isActive.setValue(false);
          this.toggleEnabledStateOfAdditionalTaxControls(
            additionalAmountTypeId.value,
            blockOverrides,
            additionalPercent,
            additionalAmount
          );
        } else {
          if (
            blockOverrides &&
            (withholdingOption == AdditionalAmountType.Override ||
              withholdingOption == AdditionalAmountType.StopTax)
          ) {
            additionalAmountTypeId.setValue(AdditionalAmountType.Additional);
          } else {
            // the disability tax belongs to the same state as the state tax that was changed, set it to the same withholding option and activate it.
            additionalAmountTypeId.setValue(withholdingOption);
          }

          isActive.setValue(true);
          this.toggleEnabledStateOfAdditionalTaxControls(
            additionalAmountTypeId.value,
            blockOverrides,
            additionalPercent,
            additionalAmount
          );
        }
      }
    }
  }

  /**
   * Deactivates the opposite local tax (i.e. the non-resident version of the tax if the resident version is changed).
   * EXAMPLE: GR resident and GR non-resident taxes cannot be setup at the same time for the same employee.
   * @param tax
   */
  checkOppositeLocalTax(tax: FormGroup) {
    const localTaxId = tax.get("localTaxId").value;
    const localTaxCode = tax.get("localTaxCode").value;
    const stateInfo = tax.get("stateInfo").value as KeyValue;

    let oppositeLocalTax = this.NonFederalTaxes.controls.filter(
      (tax) =>
        tax.get("localTaxCode").value === localTaxCode &&
        tax.get("stateInfo").value.id === stateInfo.id &&
        (tax.get("taxTypeId").value == 6 || tax.get("taxTypeId").value == 7) && // Local Resident OR Local Non-Resident
        tax.get("additionalAmountTypeId").value !=
          AdditionalAmountType.StopTaxAndWages &&
        tax.get("localTaxId").value &&
        tax.get("localTaxId").value != localTaxId
    );

    if (oppositeLocalTax.length > 0) {
      let oppoTax = oppositeLocalTax[0] as FormGroup;
      let additionalAmountTypeId = oppoTax.get(
        "additionalAmountTypeId"
      ) as FormControl;
      let additionalPercent = oppoTax.get("additionalPercent") as FormControl;
      let additionalAmount = oppoTax.get("additionalAmount") as FormControl;
      let blockOverrides = oppoTax.get("blockOverrides").value;
      let isActive = oppoTax.get("isActive") as FormControl;

      additionalAmountTypeId.setValue(AdditionalAmountType.StopTaxAndWages);
      isActive.setValue(false);
      this.toggleEnabledStateOfAdditionalTaxControls(
        additionalAmountTypeId.value,
        blockOverrides,
        additionalPercent,
        additionalAmount
      );
    }
  }

  /**
   * Enables or disables the Percent and Amount fields for a given tax
   * @param additionalAmountTypeId
   * @param blockOverrides
   * @param additionalPercent
   * @param additionalAmount
   */
  toggleEnabledStateOfAdditionalTaxControls(
    additionalAmountTypeId: number,
    blockOverrides: boolean,
    additionalPercent: FormControl,
    additionalAmount: FormControl
  ) {
    if (
      additionalAmountTypeId == AdditionalAmountType.StopTax ||
      additionalAmountTypeId == AdditionalAmountType.StopTaxAndWages ||
      blockOverrides === true
    ) {
      this.disableFormControl(additionalPercent);
      this.disableFormControl(additionalAmount);
    } else {
      this.enableFormControl(additionalPercent);
      this.enableFormControl(additionalAmount);
    }
  }

  // Disabled reminder date input if reminder date isn't on
  reminderDate() {
    this.reminderDateForm.get('isReminderDateChecked').valueChanges.subscribe(value => {
      if (value) {
        this.reminderDateForm.get('reminderDate').enable();
      } else {
        this.reminderDateForm.get('reminderDate').disable();
      }
    })
  }

  enableFormControl(fc: FormControl) {
    fc.enable();
  }

  disableFormControl(fc: FormControl) {
    fc.disable();
  }

  disableOtherTaxWithholdingsTable() {
    // for each non federal tax
    for (let i = 0; i < this.NonFederalTaxes.length; i++) {
      this.disableFormControl(
        this.NonFederalTaxes.at(i).get("filingStatusId") as FormControl
      ); // disable Filing Status control
      this.disableFormControl(
        this.NonFederalTaxes.at(i).get("numberOfExemptions") as FormControl
      ); // disable Exemptions control
      this.disableFormControl(
        this.NonFederalTaxes.at(i).get("numberOfDependents") as FormControl
      ); // disable Dependents control
      this.disableFormControl(
        this.NonFederalTaxes.at(i).get("additionalAmountTypeId") as FormControl
      ); // disable Withholding Option control
      this.disableFormControl(
        this.NonFederalTaxes.at(i).get("additionalPercent") as FormControl
      ); // disable Percent control
      this.disableFormControl(
        this.NonFederalTaxes.at(i).get("additionalAmount") as FormControl
      ); // disable Flat control
      this.disableFormControl(
        this.NonFederalTaxes.at(i).get("reimburse") as FormControl
      ); // disable Reimburse control
    }
  }

  displayErrorMessage(error: any) {
    const errorMsg =
      error.error.errors != null && error.error.errors.length
        ? error.error.errors[0].msg
        : error.message;
    this.ngxMsg.setErrorMessage(errorMsg);
    this.hasError = true;
  }

  independentContractor1099Changed() {
    if (this.Is1099Exempt.value != this.taxInfo.generalInfo.is1099Exempt && this.hasYtdTaxOrWages) {
      this.display1099Warning();
    } else {
      if (this.Is1099Exempt.value) {
        this.IsReminderDateChecked.setValue(false);
        this.setSutaRequiredValidation(false); // remove 'Required' validator
        this.disableFormControl(this.SutaClientTaxId); // disable SUTA state control

        this.federalTaxWithholdingOptionChanged(
          AdditionalAmountType.StopTaxAndWages
        ); // set federal tax as Stop Tax & Wages and disable Percent and Amount controls

        for (let tax of this.NonFederalTaxes.controls) {
          let additionalAmountTypeId = tax.get(
            "additionalAmountTypeId"
          ) as FormControl;
          let additionalPercent = tax.get("additionalPercent") as FormControl;
          let additionalAmount = tax.get("additionalAmount") as FormControl;
          let isActive = tax.get("isActive") as FormControl;

          additionalAmountTypeId.setValue(AdditionalAmountType.StopTaxAndWages);
          isActive.setValue(false);
          this.toggleEnabledStateOfAdditionalTaxControls(
            additionalAmountTypeId.value,
            null,
            additionalPercent,
            additionalAmount
          );

          this.disableFormControl(additionalAmountTypeId); // disable Withholding Option control

          // set tax as Stop Tax & Wages and disable Percent and Amount controls
          this.disableFormControl(tax.get("filingStatusId") as FormControl); // disable Filing Status control
          this.disableFormControl(tax.get("numberOfExemptions") as FormControl); // disable Exemptions control
          this.disableFormControl(tax.get("numberOfDependents") as FormControl); // disable Dependents control
          this.disableFormControl(tax.get("reimburse") as FormControl); // disable Reimburse control
        }
      } else {
        this.setSutaRequiredValidation(); // add 'Required' validator
        this.enableFormControl(this.SutaClientTaxId); // enable SUTA state drop down

        for (let tax of this.NonFederalTaxes.controls) {
          // for each non federal tax
          this.enableFormControl(tax.get("filingStatusId") as FormControl); // enable Filing Status control
          this.enableFormControl(tax.get("numberOfExemptions") as FormControl); // enable Exemptions control
          this.enableFormControl(tax.get("numberOfDependents") as FormControl); // enable Dependents control
          this.enableFormControl(
            tax.get("additionalAmountTypeId") as FormControl
          ); // enable Withholding Option control
          this.enableFormControl(tax.get("reimburse") as FormControl); // enable Reimburse control
        }
      }
    }
  }

  setSutaRequiredValidation(isRequired: boolean = true) {
    if (isRequired) {
      this.SutaClientTaxId.setValidators(Validators.required);
    } else {
      this.SutaClientTaxId.clearValidators();
    }

    this.SutaClientTaxId.updateValueAndValidity();
  }

  display1099Warning() {
    let txt: string = this.taxInfo.generalInfo.is1099Exempt ? "unmarking" : "marking";
    let msg: string = `Employee has wages and/or taxes. You must contact Dominion Systems to proceed with ${txt} this employee 1099 Exempt.`;
    this.ngxMsg.setErrorMessage(msg);
  }

  displayNoActiveStateTaxWarning() {
    let msg: string = "At least one state must have taxes setup.";
    this.ngxMsg.setErrorMessage(msg);
  }

  deleteEmployeeTax(index: number) {
    let tax = this.NonFederalTaxes.at(index);
    let employeeTaxId = tax.get("employeeTaxId").value;
    let stateTaxId = tax.get("stateTaxId").value;
    let desc = tax.get("description").value;
    let hasTaxHistory = tax.get("hasTaxHistory").value;

    if (stateTaxId != null && this.stateTaxCount == 1) {
      //Deleting a state tax, but there is only 1 state tax setup
      this.displayStateTaxCountWarning();
    } else if (stateTaxId != null && !this.checkOneStateTaxActive(this.NonFederalTaxes.value.filter((t: { employeeTaxId: number; }) => t.employeeTaxId != employeeTaxId))) {
      this.displayNoActiveStateTaxWarning();
    } else {
      if (employeeTaxId > 0) {
        if (hasTaxHistory) {
          this.displayCannotDeleteWarning(desc);
        } else {
          const options = {
            title: `Are you sure you want to delete ${desc} tax?`,
            confirm: "Delete",
          };

          this.confirmDialog.open(options);

          this.confirmDialog
            .confirmed()
            .pipe(
              switchMap((confirmed) => {
                if (confirmed) {
                  this.ngxMsg.setSuccessMessage(`Deleting ${desc}`);
                  return this.employeeTaxesService.deleteEmployeeTax(
                    employeeTaxId
                  );
                } else {
                  return NEVER;
                }
              }),
              catchError((error) => {
                this.displayErrorMessage(error);
                return throwError(error);
              })
            )
            .subscribe((result) => {
              this.handleSuccessfulDelete(index, desc);
            });
        }
      } else {
        this.handleSuccessfulDelete(index, desc);
      }
    }
  }

  handleSuccessfulDelete(index: number, taxDesc: string) {
    this.taxInfo.nonFederalTaxes.splice(index, 1);
    this.NonFederalTaxes.removeAt(index);
    this.nonFederalTaxDatasource.data = this.taxInfo.nonFederalTaxes;
    this.setStateTaxCount();
    this.ngxMsg.setSuccessMessage(`Deleted ${taxDesc} successfully!`);
  }

  displayCannotDeleteWarning(desc: string) {
    let msg: string = `${desc} has history and cannot be deleted.`;
    this.ngxMsg.setErrorMessage(msg);
  }

  displayStateTaxCountWarning() {
    let msg: string =
      "At least one state must have taxes setup. This tax cannot be deleted.";
    this.ngxMsg.setErrorMessage(msg);
  }

  setStateTaxCount() {
    this.stateTaxCount = this.taxInfo.nonFederalTaxes.filter(
      (tax) => tax.stateTaxId != null
    ).length;
  }

  openCostCenterDialog(index: number) {
    let employeeTaxId =
      this.NonFederalTaxes.at(index).get("employeeTaxId").value;
    const dialogReg = this.dialog.open(ConfigureCostCentersDialogComponent, {
      width: "600px",
      data: {
        clientId: this.clientId,
        employeeId: this.employeeId,
        employeeTaxId: employeeTaxId,
      },
    });
  }

  openAddTaxDialog() {
    const dialogRef = this.dialog.open(AddTaxDialogComponent, {
      width: "600px",
      data: {
        clientId: this.clientId,
        employeeId: this.employeeId,
      },
    });

    dialogRef.afterClosed().subscribe((result: IEmployeeNonFederalTax[]) => {
      if(result != null) { // result will be null if modal is cancelled
        this.taxInfo.nonFederalTaxes = result
        this.nonFederalTaxDatasource.data = this.taxInfo.nonFederalTaxes;
        this.patchNonFederalTaxes(this.taxInfo.nonFederalTaxes);
        this.setStateTaxCount();
        this.setHasPennsylvaniaLocalTax();
        this.form.reset({...this.form.getRawValue()});
      }
    });
  }

  setHasPennsylvaniaLocalTax() {
    this.hasPennsylvaniaLocalTax = this.taxInfo.nonFederalTaxes.filter(t => t.localTaxId && t.stateInfo.id == 40).length > 0;
  }
}

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ArService } from '../shared/ar.service';
import { IArReport } from '../shared/ar-report.model';
import { IFileFormat } from '../shared/ar-file-format.model';
import * as moment from 'moment';
import { IArReportParameter } from '../shared/ar-report-parameter.model';
import { IArManualInvoice } from '../shared/ar-manual-invoice.model';
import { IArReportPayroll } from '../shared/ar-report-payroll.model';
import { IArBillingItemDesc } from '../shared/ar-billing-item-desc.model';
import { ArReportType } from '../shared/ar-report-type';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { IOption } from '../shared/option.model';
import { tap } from 'rxjs/operators';
import { Observable, merge } from 'rxjs';

@Component({
  selector: 'ds-ar-reporting',
  templateUrl: './ar-reporting.component.html',
  styleUrls: ['./ar-reporting.component.scss']
})
export class ArReportingComponent implements OnInit {

    reportFormGroup: FormGroup;
    reports: IArReport[] = [];
    clients: IOption[] = [];
    fileFormats: IFileFormat[] = [];
    manualInvoices: IArManualInvoice[] = [];
    payrolls: IArReportPayroll[] = [];
    billingItemDescriptions: IArBillingItemDesc[] = [];
    selectedReport: IArReport;
    formSubmitted = false;
    selectedStartDate: Date;
    selectedEndDate: Date;
    selectedClientId: number;
    formControlObject = {
        dateRange: false,
        clientDropdown: false,
        billingItemDropdown: false,
        agingControls: false,
        reportTypeDropdown: false,
        gainsLossesControls: false,
        payrollDropdown: false,
        csvExportButton: false,
        manualInvoiceDropdown: false
    };

    changeClient$: Observable<any>;
    defaultOption: IOption = {
        filter: "ALL",
        id: 0
    };

    initLists$: Observable<any>;

    constructor(private fb: FormBuilder, private arService: ArService, private msg: DsMsgService) {
        this.initLists$ = merge(
            this._initList(this.arService.getReports(), this.reports),
            this._initList(this.arService.getClientList(), this.clients),
            this._initList(this.arService.getFileFormats(), this.fileFormats)
        );

        this.reportFormGroup = fb.group({
            StartDate: fb.control(null),
            EndDate: fb.control(null),
            report: new FormControl(null),
            client: new FormControl(null),
            manualInvoice: new FormControl(null),
            billingItem: new FormControl(null),
            payroll: new FormControl(null),
            reportType: new FormControl(1),
            agingDate: new FormControl(null),
            agingPeriod: new FormControl(0),
            lookbackDate: new FormControl(null),
            gainsLosses: new FormControl(0),
            orderBy: new FormControl(0),
            fileFormat: new FormControl(1),
            saveReport: new FormControl('false'),
        });

        this.changeClient$ = this.reportFormGroup.get('client').valueChanges.pipe(tap(x => this.changeClient(x)));
    }

    get StartDate() { return this.reportFormGroup.controls.StartDate as FormControl; }
    get EndDate() { return this.reportFormGroup.controls.EndDate as FormControl; }

    ngOnInit() {}

    private _initList<T>(source: Observable<T[]>, target: T[]) {
        return source.pipe(
            tap(data => {
                for (let item of data) {
                    target.push(item);
                }
            })
        )
    }

    setSelectedReport(){
        this.selectedReport = this.reports.find(x => x.arReportId == this.reportFormGroup.controls['report'].value);
    }

    // Show/hide controls based on the selected report
    changeReport(){
        // Select "All Clients" option by default, if no client is already selected
        if (this.selectedClientId == null) {
            this.reportFormGroup.get('client').setValue(this.defaultOption);
        }

        this.msg.clearMessage();

        this.setSelectedReport();

        this.clearForm();

        this.formControlObject.csvExportButton = this.selectedReport.arReportId == ArReportType.DepositDetail; //Only show Export CSV button when Deposit Detail selected

        switch (this.selectedReport.arReportId){ //turn booleans on if they are needed form controls for a given report
            case ArReportType.BillingDetail:
                this.formControlObject.dateRange = true;
                this.formControlObject.clientDropdown = true;
                this.setStartDateEndDateRequired();
                this.showAndPopulateBillingItemDescs();
                break;
            case ArReportType.BaseRevenueList:
                this.formControlObject.dateRange = true;
                this.formControlObject.clientDropdown = true;
                this.setStartDateEndDateRequired();
                break;
            case ArReportType.DepositDetail:
                this.formControlObject.dateRange = true;
                this.formControlObject.clientDropdown = true;
                this.setStartDateEndDateRequired();
                break;
            case ArReportType.DepositSummary:
                this.formControlObject.dateRange = true;
                this.formControlObject.clientDropdown = true;
                this.setStartDateEndDateRequired();
                break;
            case ArReportType.AgingReport:
                this.formControlObject.agingControls = true;
                this.formControlObject.clientDropdown = true;
                this.setControlRequiredValidator('agingDate');
                break;
            case ArReportType.SalesByDate:
                this.formControlObject.dateRange = true;
                this.formControlObject.clientDropdown = true;
                this.formControlObject.reportTypeDropdown = true;
                this.setStartDateEndDateRequired();
                this.EndDate.setValue(moment());
                this.StartDate.setValue(moment().subtract(1, 'month'));
                break;
            case ArReportType.ClientSales:
                this.formControlObject.dateRange = true;
                this.formControlObject.clientDropdown = true;
                this.formControlObject.reportTypeDropdown = true;
                this.setStartDateEndDateRequired();
                this.EndDate.setValue(moment());
                this.StartDate.setValue(moment().subtract(1, 'month'));
                break;
            case ArReportType.ManualBillingInvoice:
                this.formControlObject.clientDropdown = true;
                if (this.selectedClientId > 0){
                    this.showAndPopulateManualInvoiceControl();
                }
                break;
            case ArReportType.CreditListing:
                this.formControlObject.dateRange = true;
                this.formControlObject.clientDropdown = true;
                this.setStartDateEndDateRequired();
                break;
            case ArReportType.GainsAndLosses:
                this.formControlObject.gainsLossesControls = true;
                this.setControlRequiredValidator('lookbackDate')
                this.reportFormGroup.get('lookbackDate').setValue(moment(1, "DD")); // The 1st day of the current month
                break;
            case ArReportType.BillingInvoice:
                this.formControlObject.clientDropdown = true;
                if (this.selectedClientId > 0){
                    this.showAndPopulatePayrollControl();
                }
                break;
        }
    }

    clearForm(){
        Object.keys(this.formControlObject).forEach(key => this.formControlObject[key] = false); //loop through all the booleans and set them to false
        this.removeRequiredValidators();
    }

    removeRequiredValidators(){
        this.removeControlValidators('StartDate');
        this.removeControlValidators('EndDate');
        this.removeControlValidators('agingDate');
        this.removeControlValidators('lookbackDate');
    }

    removeControlValidators(control: string){
        this.reportFormGroup.get(control).clearValidators();
        this.reportFormGroup.get(control).updateValueAndValidity();
    }

    setControlRequiredValidator(control: string){
        this.reportFormGroup.get(control).setValidators([Validators.required]);
        this.reportFormGroup.get(control).updateValueAndValidity();
    }

    setStartDateEndDateRequired(){
        this.setControlRequiredValidator('StartDate');
        this.setControlRequiredValidator('EndDate');
    }

    changeClient(client: IOption){

        if(!client){
            return;
        }

        this.selectedClientId = client.id;

        //Selected report is Manual Billing Invoice or Billing Invoice
        if (this.selectedReportRequiresSpecificClient)
        {
            if (this.selectedClientId > 0)
            {
                this.msg.clearMessage();
                switch (this.selectedReport.arReportId)
                {
                    case ArReportType.ManualBillingInvoice:
                        this.showAndPopulateManualInvoiceControl();
                        break;
                    case ArReportType.BillingInvoice:
                        this.showAndPopulatePayrollControl();
                        break;
                }
            }
            else
            {
                this.formControlObject.manualInvoiceDropdown = false;
                this.formControlObject.payrollDropdown = false;
            }
        }
    }

    showAndPopulateManualInvoiceControl(){
        this.formControlObject.manualInvoiceDropdown = true;
        this.getManualInvoices();
    }

    getManualInvoices(){
        this.arService.getClientManualInvoices(this.selectedClientId).subscribe(manualInvoices => {
            for (let manualInvoice of manualInvoices) {
                this.manualInvoices.push(manualInvoice)
            }

            // Pre-select the first manual invoice in the list
            if (this.manualInvoices.length > 0) {
                this.reportFormGroup.get('manualInvoice').setValue(this.manualInvoices[0].arManualInvoiceId);
            }
        });
    }

    showAndPopulatePayrollControl(){
        this.formControlObject.payrollDropdown = true;
        this.getPayrolls();
    }

    getPayrolls(){
        this.arService.getClientPayrolls(this.selectedClientId).subscribe(payrolls => {
            for (let payroll of payrolls) {
                this.payrolls.push(payroll)
            }

            // Pre-select the first payroll in the list
            if (this.payrolls.length > 0) {
                this.reportFormGroup.get('payroll').setValue(this.payrolls[0].payrollId);
            }
        });
    }

    showAndPopulateBillingItemDescs(){
        this.formControlObject.billingItemDropdown = true;
        this.getBillingItemDescs();
    }

    getBillingItemDescs(){
        this.arService.getAllBillingItems().subscribe(billingItemDescs => {
            for (let billingItemDesc of billingItemDescs) {
                this.billingItemDescriptions.push(billingItemDesc)
            }

            // Pre-select the first billing item in the list
            if (this.billingItemDescriptions.length > 0) {
                this.reportFormGroup.get('billingItem').setValue(this.billingItemDescriptions[0].billingItemDescriptionId);
            }
        });
    }

    generateReport(){
        this.formSubmitted = true;

        this.checkValidDateControls();

        if (this.reportFormGroup.invalid) return;

        if (this.selectedReportRequiresSpecificClient() && this.selectedClientId <= 0) {
            this.msg.setMessage('A specific client must be selected.', MessageTypes.error);
            return;
        }

        this.msg.loading(true);
        var reportParams = this.getReportParameters();
        var saveReport = this.reportFormGroup.get('saveReport').value == 'true';

        this.arService.generateReport(reportParams, saveReport).subscribe((res) => { },
            (error) => console.error(error),
            () => { this.msg.loading(false); });
    }

    getReportParameters(): IArReportParameter {
        var reportParams: IArReportParameter =
        {
            arReportId: this.selectedReport.arReportId,
            fileFormat: this.fileFormats.find(x => x.scheduledReportFileFormatId == this.reportFormGroup.get('fileFormat').value).scheduledReportFileFormat, //this.reportFormGroup.get('fileFormat').value,
            clientId: this.formControlObject.clientDropdown == true
                      ? this.selectedClientId
                      : null,
            invoiceId: this.reportFormGroup.get('manualInvoice').value,
            arDepositId: 0,
            startDate: this.StartDate.value,
            endDate: this.EndDate.value,
            billingItemCode: this.formControlObject.billingItemDropdown == true
                             ? this.billingItemDescriptions.find(x => x.billingItemDescriptionId == this.reportFormGroup.get('billingItem').value).code
                             : null,
            agingDate: this.reportFormGroup.get('agingDate').value,
            agingPeriod: this.reportFormGroup.get('agingPeriod').value,
            reportType: this.reportFormGroup.get('reportType').value,
            lookBackDate: this.reportFormGroup.get('lookbackDate').value,
            gainsLossesToggle: this.reportFormGroup.get('gainsLosses').value,
            orderBy: this.reportFormGroup.get('orderBy').value,
            payrollId: this.reportFormGroup.get('payroll').value
        }

        return reportParams;
    }

    generateAchDepositCsv() {
        // Check to make sure startDate and endDate are valid?
        this.msg.loading(true);
        this.arService.generateAchDepositCsv(this.StartDate.value, this.EndDate.value)
            .subscribe((res) => {},
                error => console.error(error),
                () => { this.msg.loading(false) });
    }

    // Determines if the date controls showing are valid
    checkValidDateControls() {

        if (this.formControlObject.dateRange == true) {
            this.validateDate('StartDate');
            this.validateDate('EndDate');
        }

        if (this.formControlObject.agingControls == true) {
            this.validateDate('agingDate');
        }

        if (this.formControlObject.gainsLossesControls == true) {
            this.validateDate('lookbackDate');
        }
    }

    // Determines if the date in a given date control is a valid date.
    // If not, it addes the 'required' error to the control.
    validateDate(dateControl: string) {
        var isValid = this.reportFormGroup.get(dateControl).value.isValid();

        if (!isValid) {
            this.reportFormGroup.get(dateControl).setErrors({ required: true });
        }
    }

    selectedReportRequiresSpecificClient(): Boolean {
        return this.selectedReport &&
               (this.selectedReport.arReportId == ArReportType.ManualBillingInvoice ||
                this.selectedReport.arReportId == ArReportType.BillingInvoice);
    }
}

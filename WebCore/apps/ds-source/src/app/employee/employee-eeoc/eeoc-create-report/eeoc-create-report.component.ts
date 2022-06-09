import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { EmployeeEEOCApiService } from '@ds/core/employees/shared/employee-eeoc-api.service';
import { IEEOCOrganizationData } from '@ajs/job-profiles/shared/models/eeoc-organization-data.interface';
import { EmpEeocService } from '../emp-eeoc.service';
import { filter, switchMap, map, catchError } from 'rxjs/operators';
import { of, Observable, combineLatest } from 'rxjs';
import { IEEOCLocationDataPerMultiClient } from '@ajs/job-profiles/shared/models/eeoc-location-data-per-client.interface';
import { IClientPay } from '@ajs/job-profiles/shared/models/client-payroll.interface';
import { IPersonData } from '@ajs/onboarding/shared/models/i9-data.interface';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';


@Component({
    selector: 'ds-eeoc-create-report',
    templateUrl: './eeoc-create-report.component.html',
    styleUrls: ['./eeoc-create-report.component.scss']
})
export class EEOCCreateReportComponent implements OnInit {

    fourthFormGroup: FormGroup;
    organizationInfo: IEEOCOrganizationData;
    @Input() FEIN: number;
    formInit: Observable<any>;
    eeocLocations: IEEOCLocationDataPerMultiClient[];
    clientIds: number[] = [];
    clientPayrollList: IClientPay[] = [];

    constructor(
        private eeocService: EmployeeEEOCApiService,
        private service: EmpEeocService,
        private msg: DsMsgService
    ) { }

    ngOnInit() {
        this.initializefourthForm();
        this.initializeListener();
    }

    initializefourthForm(): void {
        this.fourthFormGroup = new FormGroup({
            location: new FormControl(null, Validators.required),
            naicsCode: new FormControl(null, [Validators.required, Validators.pattern('[0-9]*')]),
            norcUserId: new FormControl(null),
            companyNumber: new FormControl(null, Validators.required),
            questionB2C: new FormControl(null, Validators.required),
            questionC1: new FormControl(null, Validators.required),
            questionC2: new FormControl(null, Validators.required),
            questionC3: new FormControl(null, Validators.required),
            dunBradstreetNumber: new FormControl(null, Validators.pattern('[a-zA-Z0-9]*')),
            firstName: new FormControl(null, Validators.required),
            middleInitial: new FormControl(null),
            lastName: new FormControl(null, Validators.required),
            jobTitle: new FormControl(null, Validators.required),
            phoneNumber: new FormControl(null, Validators.required),
            email: new FormControl(null, Validators.required),
        });
    }

    initializeListener(): void {
        this.formInit = combineLatest(this.service.feinNumber, this.service.locationsChanged).pipe(
            filter(res => !!res[0]), //dont continue if the FEIN(res[0]]) given is null
            switchMap(result => {
               this.FEIN = result[0];
                return this.eeocService.getEeocOrganizationInfo(result[0]).pipe(
                    catchError(() => of({})),
                );
            }
            ),
            map((res: IEEOCOrganizationData) => {
                if (!!res) {
                    this.organizationInfo = res[0];
                    this.fourthFormGroup.patchValue({
                        naicsCode: this.organizationInfo.naicsCode,
                        norcUserId: this.organizationInfo.norcUserID,
                        companyNumber: this.organizationInfo.companyNumber,
                        questionB2C: this.organizationInfo.questionB2C ? this.organizationInfo.questionB2C.toString() : '2',
                        questionC1: this.organizationInfo.questionC1 ? this.organizationInfo.questionC1.toString() : '2',
                        questionC2: this.organizationInfo.questionC2 ? this.organizationInfo.questionC2.toString() : '2',
                        questionC3: this.organizationInfo.questionC3 ? this.organizationInfo.questionC3.toString() : '2',
                        dunBradstreetNumber: this.organizationInfo.dunAndBradstreetNumber,
                        firstName: this.organizationInfo.certifyingOfficial ? this.organizationInfo.certifyingOfficial.firstName : null,
                        middleInitial: this.organizationInfo.certifyingOfficial ? this.organizationInfo.certifyingOfficial.middleInitial : null,
                        lastName: this.organizationInfo.certifyingOfficial ? this.organizationInfo.certifyingOfficial.lastName : null,
                        jobTitle: this.organizationInfo.certifyingOfficial ? this.organizationInfo.certifyingOfficial.title : null,
                        phoneNumber: this.organizationInfo.certifyingOfficial ? this.organizationInfo.certifyingOfficial.phone : null,
                        email: this.organizationInfo.certifyingOfficial ? this.organizationInfo.certifyingOfficial.emailAddress : null,
                    });
                }
            }),
            switchMap(_ => this.service.selectedClientIdsList),
            map((clients) => this.clientIds = clients),
            switchMap(_ => this.eeocService.getEeocLocationsListMultipleClients(this.clientIds, true, true)),
            map((eeocLocations) => this.eeocLocations = eeocLocations),
            map(() => {
                var hq: IEEOCLocationDataPerMultiClient;
                if (this.eeocLocations.length == 1)
                {
                    hq = this.eeocLocations[0];
                }
                else if (this.eeocLocations.length > 1){
                    var headQuarters = this.eeocLocations.filter(eeocLocation => eeocLocation.isHeadquarters);
                    hq = headQuarters.length == 1 ? headQuarters[0] : null
                }

                this.fourthFormGroup.patchValue({
                    location: hq ? hq.eeocLocationId : null
                });
            }),
            switchMap(_ => this.service.clientPayrollList),
            map((clientPayrollList) => {
                this.clientPayrollList = clientPayrollList;
                return true;
            }),
            catchError(err => {
                console.error(err);
                return of(false);
            }),
        );
    }

    downloadCSV(): void {
        if(this.fourthFormGroup.invalid) return;

        // SAVE THE FORM IF THE USER HAS CLICKED THE CSV BUTTON
        // THE SAVED VALUES ARE NEEDED FOR THE CSV
        this.saveForm();
        this.eeocService.getEeocCSV(this.clientPayrollList, this.FEIN).subscribe((res) => { },
            (error) => console.error(error));
    }

    downloadPDF(): void {
        if(this.fourthFormGroup.invalid) return;
        
        this.saveForm();
        this.eeocService.getEeeocPDF(this.clientPayrollList).subscribe((res) => { },
            (error) => console.error(error));
    }

    saveForm(): void {
        this.fourthFormGroup.markAllAsTouched();
        if(this.fourthFormGroup.invalid) return;

        const certifyingOfficialToSend: IPersonData = {
            personId: this.organizationInfo == null || this.organizationInfo.certifyingOfficialId == null ? 0 : this.organizationInfo.certifyingOfficialId,
            firstName: this.fourthFormGroup.controls['firstName'].value,
            middleInitial: this.fourthFormGroup.controls['middleInitial'].value,
            lastName: this.fourthFormGroup.controls['lastName'].value,
            title: this.fourthFormGroup.controls['jobTitle'].value,
            phone: this.fourthFormGroup.controls['phoneNumber'].value,
            emailAddress: this.fourthFormGroup.controls['email'].value,
        }
        const orgInfoToSend: IEEOCOrganizationData = {
            eeocOrganizationID: this.organizationInfo == null ? 0 : (this.organizationInfo.eeocOrganizationID == null ? 0 : this.organizationInfo.eeocOrganizationID),
            fein: this.organizationInfo == null ? this.FEIN : this.organizationInfo.fein,
            naicsCode: this.fourthFormGroup.controls['naicsCode'].value,
            norcUserID: this.fourthFormGroup.controls['norcUserId'].value,
            companyNumber: this.fourthFormGroup.controls['companyNumber'].value,
            questionB2C: this.fourthFormGroup.controls['questionB2C'].value,
            questionC1: this.fourthFormGroup.controls['questionC1'].value,
            questionC2: this.fourthFormGroup.controls['questionC2'].value,
            questionC3: this.fourthFormGroup.controls['questionC3'].value,
            dunAndBradstreetNumber: this.fourthFormGroup.controls['dunBradstreetNumber'].value,
            certifyingOfficialId: this.organizationInfo == null || this.organizationInfo.certifyingOfficialId == null ? 0 : this.organizationInfo.certifyingOfficialId,
            certifyingOfficial: certifyingOfficialToSend
        };

        // location: new FormControl(null, Validators.required),
        // companyNumber: new FormControl(null, Validators.required),
        // questionC1: new FormControl(null, Validators.required),
        // hundredEmployees2: new FormControl(null, Validators.required),
        // dunBradstreetNumber: new FormControl(null, Validators.required),
        // //following is for certifying official information
        // firstName: new FormControl(null, Validators.required),
        // middleInitial: new FormControl(null, Validators.required),
        // lastName: new FormControl(null, Validators.required),
        // jobTitle: new FormControl(null, Validators.required),
        // phoneNumber: new FormControl(null, Validators.required),
        // email: new FormControl(null, Validators.required),
        const hq: IEEOCLocationDataPerMultiClient = this.eeocLocations.length == 1 ? this.eeocLocations[0] : this.eeocLocations.find(loc => loc.eeocLocationId == this.fourthFormGroup.controls['location'].value);
        
        if (hq != null){
            this.eeocService.updateLocationHeadquarters(this.clientIds, hq.eeocLocationDescription).subscribe(() => {});
        }
        
        this.eeocService.saveEeocOrganizationInfo(orgInfoToSend).subscribe(() => {
            this.msg.setTemporarySuccessMessage('Your EEOC Information has been saved successfully.', 5000);
        });
    }
}

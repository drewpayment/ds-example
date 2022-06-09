import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { DsStyleLoaderService, IStyleAsset } from '@ajs/ui/ds-styles/ds-styles.service';
import { MatDialog } from "@angular/material/dialog";

import { EmployeeTaxFormComponent } from "../employee-tax-form/employee-tax-form.component";
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';

import { IFilingStatus } from '../shared/filing-status.model';
import { IEmployeeTax } from '../shared/employee-tax.model';
import { EmployeeTaxExemptionService } from '../shared/employee-tax-api.service';

import { UserInfo } from '@ds/core/shared';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import * as _ from "lodash";
import { AccountService } from '@ds/core/account.service';

@Component({
  selector: 'ds-employee-tax-list',
  templateUrl: './employee-tax-list.component.html',
  styleUrls: ['./employee-tax-list.component.scss']
})
export class EmployeeTaxListComponent implements OnInit, AfterViewChecked {
    mainStyle: IStyleAsset;
    user: UserInfo;
    allTaxes: IEmployeeTax[];
    fedTaxes: IEmployeeTax[];
    stateTaxes: IEmployeeTax[];
    filingStatuses: IFilingStatus[];
    hasViewPermissions: boolean;
    hasEditPermissions: boolean;
    employeeUsing2020FederalW4Setup = false;
    isLoading = true;

    constructor(
        private styles: DsStyleLoaderService,
        private service: EmployeeTaxExemptionService,
        private accountService: AccountService,
        private msgSvc: DsMsgService,
        private dialog: MatDialog,
        private confirmService: DsConfirmService
    ) { }

    ngOnInit() {
        this.getUserInfoAndTaxes();
    }

    getUserInfoAndTaxes(): void {
        this.allTaxes = [],
        this.fedTaxes = [],
        this.stateTaxes = [];
        this.filingStatuses = [],
        this.hasViewPermissions = false,
        this.hasEditPermissions = false,
        this.employeeUsing2020FederalW4Setup = false;


        this.accountService.getUserInfo().subscribe(user => {
            this.user = user;

            this.accountService.canPerformActions("Tax.GetEmployeeTax").subscribe(x => {
                if (x === true) {
                    this.hasViewPermissions = true;
                }

                this.service.getTaxes(this.user.employeeId).subscribe(taxes => {
                    this.allTaxes = taxes;
                    for( let tax of this.allTaxes){
                        if( tax.description == "Federal"){
                            //clean up description for some filing statuses
                            if(tax.filingStatusId == 17 || tax.filingStatusId == 18){
                                tax.filingStatusDescription = "Single";
                            }
                            else if(tax.filingStatusId == 19 || tax.filingStatusId == 20){
                                tax.filingStatusDescription = "Married";
                            }
                            else if(tax.filingStatusId == 21 || tax.filingStatusId == 22){
                                tax.filingStatusDescription = "Head of Household";
                            }
                            this.fedTaxes.push(tax);
                            if(tax.using2020FederalW4Setup){
                                this.employeeUsing2020FederalW4Setup = true;
                            }
                        }
                        else{
                            this.stateTaxes.push(tax);
                        }
                    }

                    this.service.getFilingStatuses().subscribe(filingStatuses => {
                        this.filingStatuses = filingStatuses;

                        if (this.allTaxes.length > 0) {
                            this.accountService.canPerformActions("Tax.RequestUpdateEmployeeTax").subscribe(y => {
                                if (y === true) {
                                    this.hasEditPermissions = true;
                                }
                            });
                        }
                    });

                    this.isLoading = false;
                });
            });
        });
    }

    showFileNewFederalW4Dialog(): void {
        let employeeTaxObj:IEmployeeTax = null;
        if( this.fedTaxes.length  > 0 ){
            employeeTaxObj = this.fedTaxes[0];
        }
        this.dialog.open(EmployeeTaxFormComponent, {
            width: '600px',
            data: {
                user: this.user,
                maritalStatusList: this.filingStatuses,
                employeeTax: employeeTaxObj,
                useFederal2020Form: true
            },
        })
        .afterClosed()
        .subscribe(
            res2 => {
                if( res2 != null ){
                    this.msgSvc.sending(true);
                    this.service.updateEmployeeOnboardingW4AndTaxWithNotification(res2.tax).subscribe(
                        eTax => {
                            this.service.postEmployeeFormUpdatesWithoutFinalize(this.user.employeeId, res2.forms).subscribe(result =>{
                                this.msgSvc.setTemporarySuccessMessage("Successfully submitted your employee tax change.", 5000);
                                this.getUserInfoAndTaxes();
                            });
                        }
                    );

                }
            }
        )
    }

    showEditTaxDialog(employeeTax: IEmployeeTax): void {
        this.dialog.open(EmployeeTaxFormComponent, {
            width: '410px',
            data: {
                user: this.user,
                employeeTax: employeeTax,
                maritalStatusList: this.filingStatuses,
                useFederal2020Form: false
            }
        })
        .afterClosed()
        .subscribe(result => {
            if (result == null || result == undefined) return;
            if (result.tax == undefined || result.tax == null) return;
            this.msgSvc.sending(true);
            this.service.updateTax(result.tax)
                .subscribe(eTax => {
                    this.msgSvc.setTemporarySuccessMessage('Successfully submitted your employee tax change request.', 5000);
                });
        });

        //Will be used in a future case to make state tax changes like the federal changes on this component
        // .subscribe(result => {
        //     if (result == null) return;
        //     console.log(result);
        //     this.msgSvc.sending(true);
        //     this.service.updateEmployeeOnboardingW4AndTaxWithNotification(result.tax).subscribe(eTax => {
        //         this.service.postEmployeeFormUpdatesWithoutFinalize(this.user.employeeId, result.forms).subscribe(res =>{
        //             this.msgSvc.setTemporarySuccessMessage('Successfully submitted your employee tax change.', 5000);
        //             this.getUserInfoAndTaxes();
        //         });
        //     });
        // });
    }

    /**
     * We tell DsStyleLoaderService that this component should use main stylesheet AFTER OnInit and AfterViewInit
     * because we need to make sure that everything is resolved above this component. The DsStyleLoaderService is not
     * instantiated until after OutletComponent is finished loading.
     */
    ngAfterViewChecked() {
        this.styles.useMainStyleSheet();
    }
}

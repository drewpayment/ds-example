import { Component, OnInit, Input, Output, Inject, ViewChild } from '@angular/core';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { IEmployeeOnboardingData, II9DocumentData, ICountryData, IStateData } from "@ajs/onboarding/shared/models";

@Component({
  selector: 'ds-certify-i9-trigger',
  templateUrl: './certify-I9-trigger.component.html',
  styleUrls: ['./certify-I9-trigger.component.scss']
})
export class CertifyI9TriggerComponent implements OnInit {
    clientId: number;
    companyRootUrl: string;
    isLinkedToOnboarding: boolean;

    @Input() user: UserInfo;
    @Input() documents: Array<II9DocumentData>;
    @Input() countries: Array<ICountryData>;
    @Input() states: Array<IStateData>;
    @Input() employee: IEmployeeOnboardingData;

    constructor( private dialog: MatDialog,) {
        
    }
    ngOnInit() {
        
    }

    popupDialog(){
        let config = new MatDialogConfig<any>();
        config.width = "700px";
        config.data = { user     : this.user, 
                        employee : this.employee ,
                        documents : this.documents ,
                        countries : this.countries ,
                        states  : this.states};

        
        return this.dialog.open<CertifyI9DialogComponent, any, string>(CertifyI9DialogComponent, config)
        .afterClosed()
        .subscribe();
    }    
}

@Component({
    selector: 'ds-certify-i9-dialog',
    template: `
        <div mat-dialog-header>
            <h2 class="dialog-title">
                Certify I-9 For {{employee.employeeFirstName + ' ' + employee.employeeLastName}}
            </h2>
            <button type="button" class="close" (click)="clear()">
                <mat-icon>clear</mat-icon>
            </button>
        </div>
        <div mat-dialog-content>
            <ds-certify-i9-form 
            [isModal]='true'
            (i9StatusChange)='statusChange($event)'
            [user]='user' 
            [employee]='employee'
            [documents]='documents'
            [countries]='countries' 
            [states]='states'></ds-certify-i9-form>
        </div>
        <div mat-dialog-actions>
            <button 
                id="btnCertifyI9"
                type="button" 
                class="btn btn-primary"
                >Certify I-9</button>
            <button 
                id="btnCancelI9" 
                type="button"
                class="btn btn-cancel"
                (click)="clear()" >Cancel</button>
        </div>
    `
})
export class CertifyI9DialogComponent {
    user: UserInfo;
    documents: Array<II9DocumentData>;
    countries: Array<ICountryData>;
    states: Array<IStateData>;
    employee: IEmployeeOnboardingData;

    constructor(
        private ref:MatDialogRef<CertifyI9DialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:any,
    ) {
        this.user = data.user;
        this.documents = data.documents;
        this.countries = data.countries;
        this.states = data.states;
        this.employee = data.employee;
    }
    clear() {
        this.ref.close(false);
    }

    statusChange(evt){
        if(evt && evt.toLowerCase() == "certified"){
            let btn = document.getElementById('btnCertifiedNotification');
            if(btn) btn.click();
            this.ref.close(true);
        }
    }
}
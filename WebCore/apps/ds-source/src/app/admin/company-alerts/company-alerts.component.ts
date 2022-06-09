import { Component, OnInit, Inject } from '@angular/core';
import { AlertService } from './shared/alert.service';
import { AccountService } from '@ds/core/account.service';
import { IAlert, AlertType } from '../../../../../../lib/models/src/lib/alert.model';
import { DOCUMENT } from '@angular/common';
import { Observable, iif, of  } from 'rxjs';
import { tap, switchMap } from 'rxjs/operators';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { FormControl } from '@angular/forms';
import { AddAlertDialogComponent } from './add-alert-dialog/add-alert-dialog.component';
import { HttpErrorResponse } from '@angular/common/http';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';

@Component({
  selector: 'ds-alerts',
  templateUrl: './company-alerts.component.html',
  styleUrls: ['./company-alerts.component.scss']
})
export class CompanyAlertsComponent implements OnInit {

    isLoading: boolean = true;
    showFiles: boolean = false;
    allAlertList:Array<IAlert>;
    alertList:Array<IAlert>;
    selectedAlert:IAlert = null;
    displayExpired: boolean;
    userinfo: UserInfo;
    searchText: string;
    displayedCount: number = 0;
    totalCount: number = 0;

    sortByCtrl: FormControl = new FormControl("");
    expiredCtrl: FormControl = new FormControl("");

    constructor(private accountService: AccountService,
        private alertService: AlertService,
        private msg: DsMsgService,
        private dialog: MatDialog,
        private confirmDialog: MatDialog,
        @Inject(DOCUMENT) private document: Document) {

    }
    ngOnInit() {
        this.isLoading = true;
        this.showFiles = false;
        this.searchText = '';
        this.allAlertList = [];
        this.selectedAlert = <IAlert>{};

        this.checkCurrentUser().pipe(
            switchMap(userInfo =>
                this.alertService.getCompanyAlertsByClient(userInfo.lastClientId || userInfo.clientId,  false)),
            tap( x => {
                this.allAlertList = x;
                this.sortList();
                this.isLoading = false;
            })
        ).subscribe();

        this.sortByCtrl.setValue("type");
        this.expiredCtrl.setValue(false);

        this.expiredCtrl.valueChanges
        .pipe(tap( isExpired => {
            this.isLoading = true;
        }),switchMap(isExpired =>
            this.alertService.getCompanyAlertsByClient(this.userinfo.lastClientId || this.userinfo.clientId, isExpired)),
        tap( list => {
            this.allAlertList = list;
            this.sortList();
            this.isLoading = false;
        })).subscribe();

        this.sortByCtrl.valueChanges.pipe(tap( x => {
            this.sortList();
        })).subscribe();
    }

    checkCurrentUser(): Observable<UserInfo>{
        return iif(() => this.userinfo == null,
            this.accountService.getUserInfo().pipe(tap(u => {
                this.userinfo = u;
            })),
            of(this.userinfo));
    }

    applyFilter(srhTxt:string){
        this.searchText = srhTxt;
        this.sortList();
    }

    sortList():void
    {
        let k = [];
        this.totalCount = 0;
        if(this.allAlertList && this.allAlertList.length > 0)
        {
            this.totalCount = this.allAlertList.length;
            let srchTxt = this.searchText.toLowerCase();
            let sortBy = this.sortByCtrl.value.toString().toLowerCase();

            k = this.allAlertList.filter( x=> !srchTxt || x.title.toLowerCase().indexOf(srchTxt) > -1 );
            if(sortBy == 'type') k = k.sort( (a,b) => (a.alertCategoryId > b.alertCategoryId) ? 1 : -1 );
            else if(sortBy == 'date') k = k.sort( (a,b) => (a.dateEndDisplay > b.dateEndDisplay) ? 1 : -1 );
            else if(sortBy == 'title') k = k.sort( (a,b) => a.title.localeCompare(b.title) );
        }

        this.alertList = k;
        this.displayedCount = k.length;
    }

    popupAlertDialog(currAlert:IAlert) {
        var isNewReSource = false;
        var currentAlert:IAlert = <IAlert>{};

        if (!currAlert) {
            currentAlert = <IAlert>{
                alertId : 0,
                datePosted : new Date(),
                alertText: '',
                alertLink: '',
                dateStartDisplay : new Date(),
                dateEndDisplay : new Date(),
                securityLevel :<number>this.userinfo.userTypeId,
                alertType :<AlertType>(this.userinfo.userTypeId == 1? 1:2),
                clientId : this.userinfo.clientId,
                modified : new Date(),
                modifiedBy: this.userinfo.userId.toString(),
                alertCategoryId : 1,
                title: '',
                isExpired: false,
                isNew: true,
                hovered: false,
            };
            isNewReSource = true;
        }
        else {
            currentAlert = Object.assign({}, currAlert);
            currentAlert.clientId = this.userinfo.clientId;
        }
        let config = new MatDialogConfig<any>();
        config.width = "500px";
        config.data = currentAlert;

        return this.dialog.open<AddAlertDialogComponent, any, IAlert>(AddAlertDialogComponent, config)
        .afterClosed()
        .subscribe((alert: IAlert) => {
            if(alert){
                if(currentAlert.alertId == 0){
                    this.allAlertList.push(alert);
                    this.msg.setTemporarySuccessMessage("Alert added successfully.");
                }
                else{
                    var inx = this.allAlertList.findIndex(x => x.alertId == alert.alertId);
                    this.allAlertList.splice(inx, 1, alert);
                    this.msg.setTemporarySuccessMessage("Alert updated successfully.");
                }
                this.sortList();

            }
        });
    }
    download(item: IAlert) {
        this.alertService.getFileAlertToDownload(item).subscribe((res) => { },
        (resp: HttpErrorResponse) => {
            let msg = (resp.error.errors != null && resp.error.errors.length) ? resp.error.errors[0].msg : resp.message;
            this.msg.setTemporaryMessage(msg,MessageTypes.error);
        });
    }

    public fileName(url:string){
        var inx = url.lastIndexOf('/') > -1 ? url.lastIndexOf('/') : -1;
        if(inx < 0) inx = url.lastIndexOf('\\') > -1 ? url.lastIndexOf('\\') : -1;
        if(inx > -1) return decodeURIComponent(url.substring(inx+1));
        else         return "";
    }

    public test(){
        this.alertService.test().subscribe(result => {
            this.msg.setTemporarySuccessMessage("Tested successfully.");
        }, (resp: HttpErrorResponse) => {
            let msg = (resp.error.errors != null && resp.error.errors.length) ? resp.error.errors[0].msg : resp.message;
            this.msg.setMessage(msg,MessageTypes.error);
        })
    }
}

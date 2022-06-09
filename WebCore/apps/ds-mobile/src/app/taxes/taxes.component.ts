import { Component, OnInit } from '@angular/core';
import { TaxApiService } from '../shared/tax-api.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { IEmployeeTaxInfo } from '../../models/tax.model';
import { switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
    selector: 'ds-taxes',
    templateUrl: './taxes.component.html',
    styleUrls: ['./taxes.component.scss']
})
export class TaxesComponent implements OnInit {
    user: UserInfo;
    taxInfo: IEmployeeTaxInfo[];
    isLoading = true;
    constructor(
        private api: TaxApiService,
        private acctService: AccountService
    ) { }

    ngOnInit() {
        this.acctService.getUserInfo()
            .pipe(switchMap(user => {
                this.user = user;
                return this.api.getEmployeeTaxInfo(this.user.userEmployeeId) as unknown as Observable<IEmployeeTaxInfo[][]>;
            }))
            .subscribe((data: IEmployeeTaxInfo[][]) => {
               this.taxInfo = data[0];
               this.isLoading = false;
            });
    }

}

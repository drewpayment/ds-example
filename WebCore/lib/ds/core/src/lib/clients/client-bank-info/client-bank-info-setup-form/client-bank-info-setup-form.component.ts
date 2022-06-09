import { Component, OnInit } from '@angular/core';
import { IClientBankSetupInfo } from '../shared/client-bank-setup-info.model';
import { ClientBankInfoApiService } from '../shared/client-bank-info-api.service';

@Component({
    selector: 'ds-client-bank-info-setup-form',
    templateUrl: './client-bank-info-setup-form.component.html',
    styleUrls: ['./client-bank-info-setup-form.component.scss']
})
export class ClientBankInfoSetupFormComponent implements OnInit {

    bankInfo: IClientBankSetupInfo;

    constructor(private api: ClientBankInfoApiService) { }

    ngOnInit() {

        this.api.getClientBankSetupInfo(1182)
            .subscribe(data => {
                this.bankInfo = data;
            });

    }

}

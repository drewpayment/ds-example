import { Injectable }             from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DsPopupService } from '@ajs/ui/popup/ds-popup.service';
import { IClientWorkNumberPolicy } from '@ds/core/clients/shared';

@Injectable({
    providedIn: 'root'
})

export class WorkNumberService {
    constructor(private http: HttpClient,
        private DsPopup : DsPopupService) { }

    static readonly WORKNUMBER_API_BASE = "api/clients/equifax";

    getWorkNumberInfo(clientId: number) {
        const url = `${WorkNumberService.WORKNUMBER_API_BASE}/${clientId}`;
        return this.http.get<IClientWorkNumberPolicy>(url);
    }

    updateAllowEquifax(optIn: IClientWorkNumberPolicy) {
        const url  = `${WorkNumberService.WORKNUMBER_API_BASE}/update/optin`;
        return this.http.post(url, optIn);    
    }
}

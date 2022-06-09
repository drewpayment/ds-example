import { Component, OnInit } from '@angular/core';
import { AccessRuleApiService } from '../shared/access-rule-api.service';
import { ClaimSource, IClientAccessInfo, IUserActionTypeClaimType, IUserAccessInfo, IAccountFeatureClaimType, IOtherAccessClaimType, IUserTypeClaimType } from '@ds/core/users/shared';
import { IClientInfo } from '../shared/client.model';
import { IContact } from '@ds/core/contacts';
import { IClaimType } from '../shared/claim-type.model';

class ClaimVm {
    raw: IClaimType;
    sourceName: string;
    iconName: string;

    constructor(claim: IClaimType) {
        this.raw = claim;

        switch(claim.source) {
            case ClaimSource.AccountFeature: 
                this.sourceName = "Account Feature";
                this.iconName = "domain";
                break;
            case ClaimSource.Other:
                this.sourceName = "Other Rule";
                this.iconName = "settings";
                break;
            case ClaimSource.UserActionType:
                this.sourceName = "Action Type";
                this.iconName = "lock";
                break;
            case ClaimSource.UserType:
                this.sourceName = "User Type";
                this.iconName = "perm_identity";
                break;
        }
    }
}

@Component({
    selector: 'ds-access-rule-manager',
    templateUrl: './access-rule-manager.component.html',
    styleUrls: ['./access-rule-manager.component.scss']
})
export class AccessRuleManagerComponent implements OnInit {

    claims: ClaimVm[];
    filteredClaims: ClaimVm[];
    searchText: string;

    clients: IClientInfo[] = [];
    selectedClient: IClientInfo = null;
    clientAccessInfo: IClientAccessInfo = null;
    clientClaims: ClaimVm[] = [];

    users: IContact[] = [];
    selectedUser: IContact = null;
    userAccessInfo: IUserAccessInfo = null;
    userClaims: ClaimVm[] = [];

    constructor(
        private ruleApi : AccessRuleApiService
    ) { }

    ngOnInit() {
        this.ruleApi.getClaimTypes().subscribe(rules => {
            this.claims = rules.map(r => new ClaimVm(r));
            this.filterRules();
        });

        this.ruleApi.getClients(true).subscribe(clients => {
            this.clients = clients.sort((c1, c2) => {
                return c1.clientName > c2.clientName ? 1 : -1;
            })
        })
    }

    filterRules() {
        if (this.searchText) {
            let lower = this.searchText.toLowerCase();
            this.filteredClaims = this.claims.filter(r => {
                return r.raw.name.toLowerCase().includes(lower);
            })
        }
        else {
            this.filteredClaims = this.claims;
        }
    }

    clientSelected() {
        if (this.selectedClient) {
            this.ruleApi.getClientAccessInfo(this.selectedClient.clientId).subscribe(access => {
                this.clientAccessInfo = access;
                this.clientClaims = this.getClaimsFromRaw(access.claims);
            })

            this.ruleApi.getUserList(this.selectedClient.clientId).subscribe(users => {
                this.users = users.sort((u1, u2) => {
                    return u1.lastName > u2.lastName ? 1 : -1;
                });
            })
        }
        else {
            this.clientAccessInfo = null;
            this.clientClaims = [];
        }
        
        this.selectedUser = null;
        this.userAccessInfo = null;
        this.userClaims = [];
    }

    userSelected() {
        if (this.selectedUser) {
            this.ruleApi.getUserAccessInfo(this.selectedUser.userId).subscribe(user => {
                this.userAccessInfo = user;
                this.userClaims = this.getClaimsFromRaw(user.claims);
            })
        } 
        else {
            this.userAccessInfo = null;
            this.userClaims = [];
        }
    }

    getClaimsFromRaw(raw: IClaimType[]) : ClaimVm[] {
        return this.claims.filter(x => raw.some(y => {
            if (x.raw.source !== y.source)
                return false;
            
            switch (y.source) {
                case ClaimSource.AccountFeature:
                    return (<IAccountFeatureClaimType>x.raw).accountFeature === (<IAccountFeatureClaimType>y).accountFeature;
                case ClaimSource.Other:
                    return (<IOtherAccessClaimType>x.raw).otherAccessType === (<IOtherAccessClaimType>y).otherAccessType;
                case ClaimSource.UserActionType:
                    return (<IUserActionTypeClaimType>x.raw).designation === (<IUserActionTypeClaimType>y).designation;
                case ClaimSource.UserType:
                    return (<IUserTypeClaimType>x.raw).userType === (<IUserTypeClaimType>y).userType;
            }

            return false;
        }))
    }
}

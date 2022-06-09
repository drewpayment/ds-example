import { UserInfo } from '@ds/core/shared';

export interface IClientWorkNumberPolicy {
    clientWorkNumberPolicyId        : number | null,
    clientId                        : number | null,
    acceptedBy                      : number | null,
    modified                        : Date,
    isAccepted                      : boolean,
    user                            : UserInfo;
}
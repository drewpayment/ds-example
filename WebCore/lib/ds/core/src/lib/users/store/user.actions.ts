import { UserInfo } from '@ds/core/shared';
import { IEmployeeSearchResult } from '@ds/employees/search/shared/models/employee-search-result';
import { Action } from '@ngrx/store';


export enum UserActionTypes {
    LoadUser = '[User] Loads User into Store',
    UpdateUser = '[User] Update User',
    UpdateLastClient = '[User] Update selected Client',
    UpdateLastEmployee = '[User] Update selected employee',
    ClearUser = '[User] Clear user state',
}

export class LoadUser implements Action {
    readonly type = UserActionTypes.LoadUser;
}

export class UpdateUser implements Action {
    readonly type = UserActionTypes.UpdateUser;
    constructor(public user: UserInfo) {}
}

export class UpdateLastClient implements Action {
    readonly type = UserActionTypes.UpdateLastClient;
    constructor(public info: UserInfo) {}
}

export class UpdateLastEmployee implements Action {
  readonly type = UserActionTypes.UpdateLastEmployee;
  constructor(public info: IEmployeeSearchResult) {}
}

export class ClearUser implements Action {
  readonly type = UserActionTypes.ClearUser;
}

export type UserActions = UpdateUser | LoadUser | UpdateLastClient | ClearUser | UpdateLastEmployee;

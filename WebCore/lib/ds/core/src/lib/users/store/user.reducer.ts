import { InjectionToken } from '@angular/core';
import { NgrxStorageService } from '@ds/core/app-config/ngrx-store/ngrx-storage.service';
import { UserInfo } from '@ds/core/shared';
import {
  Action,
  ActionReducer,
  createSelector,
  on,
  StoreConfig,
} from '@ngrx/store';
import { merge } from 'lodash';
import { UserActions, UserActionTypes } from './user.actions';
import * as fromActions from './user.actions';

export const USER_STORAGE_KEYS = new InjectionToken<keyof UserState[]>(
  'UserStorageKeys'
);
export const USER_CONFIG_TOKEN = new InjectionToken<
  StoreConfig<UserState, fromActions.UserActions>
>('UserConfigToken');

export interface UserState {
  user: UserInfo;
}

export const userStoreFeatureKey = '__userStoreKey__';

export const initState: UserState = {} as UserState;

export function reducer(state = initState, action: UserActions) {
  switch (action.type) {
    case UserActionTypes.UpdateUser:
      return { ...state, user: action.user };
    case UserActionTypes.UpdateLastClient:
      return {
        ...state,
        user: {
          ...state.user,
          lastClientId: action.info.lastClientId,
          lastClientCode: action.info.lastClientCode,
          lastClientName: action.info.lastClientName,
          selectedClientId: () =>
            action.info.lastClientId || state.user.clientId,
        },
      };
    case UserActionTypes.UpdateLastEmployee:
      return {...state,
        user: {...state.user,
          lastEmployeeId: action.info.employeeId,
          lastEmployeeFirstName: action.info.firstName,
          lastEmployeeLastName: action.info.lastName,
          lastEmployeeMiddleInitial: action.info.middleInitial,
          selectedEmployeeId: () => state.user.lastEmployeeId || state.user.employeeId,
        } as UserInfo,
      };
    case UserActionTypes.ClearUser:
      return { user: null };
    default:
      return state;
  }
}

export const userState = (state): UserState => state[userStoreFeatureKey];
export const getUser = createSelector(
  userState,
  (state: UserState): UserInfo => state.user
);
export const getSelectedClientId = createSelector(
  userState,
  (state: UserState): number => state.user.selectedClientId()
);

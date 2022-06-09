import { InjectionToken } from '@angular/core';
import * as fromReducer from './reducers';
import * as fromActions from './actions/menu.actions';
import { StoreConfig } from '@ngrx/store';
import { State } from './reducers/menu.reducer';


export const MENU_STORAGE_KEYS = new InjectionToken<keyof State[]>('MenuStorageKeys');
export const MENU_LOCALSTORAGE_KEY = new InjectionToken<string[]>('MenuStorage');
export const MENU_CONFIG_TOKEN = new InjectionToken<StoreConfig<State, fromActions.MenuActions>>('MenuConfigToken');

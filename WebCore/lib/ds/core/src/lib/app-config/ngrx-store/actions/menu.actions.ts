import { Action } from '@ngrx/store';
import { IMenu, IMenuItem } from '../../shared';
import { NavHistoryItem } from '@ds/core/shared';

export enum MenuActionTypes {
    ClearState = '[Menu Wrapper] Clear All States',
    SetMenu = '[Menu Wrapper] Save',
    ResetMenu = '[Menu Wrapper] Reset',
    SetSelectedItem = '[Menu Wrapper] Set Selected Item',
    SetSidenavOpen = '[Menu Wrapper] Set Menu Open State',
    ToggleSidenavOpen = '[Menu Wrapper] Toggle Menu Open State',
    SetSidenavDrilledState = '[Menu Wrapper] Set Menu Drilled State',
    SetSidenavDrilledAnimationState = '[Menu Wrapper] Set Menu Drilled Animation State',
    SetResourceSelectedState = '[Menu Wrapper] Set Resource Selected State',
    SetMainMenuItems = '[Menu Wrapper] Set Current Menu Items State',
    AddActiveMenuIds = '[Menu Wrapper] Add Active Menu Item',
    RemoveActiveMenuIds = '[Menu Wrapper] Remove Active Menu Item',
    ClearActiveMenuIds = '[Menu Wrapper] Clear Active Menu Items',
    SetDisplayMenuItems = '[Menu Wrapper] Set Display Menu Items when Sidenav is drilled',
    ClearDisplayMenuItems = '[Menu Wrapper] Clear Display Menu Items',
    UpdateActiveProduct = '[Menu Wrapper] Update Active Product Info',
    ClearActiveProduct = '[Menu Wrapper] Clear Active Product Info',
    ClearNavigationHistory = '[Menu Wrapper] Clear Navigation History',
    AddNavigationHistoryItem = '[Menu Wrapper] Add Navigation Step',
    RemoveNavigationHistoryItem = '[Menu Wrapper] Remove Navigation Step',
    SetNavigationHistory = '[Menu Wrapper] Set Navigation History',
    SetMainMenuId = '[Menu Wrapper] Set Main Menu ID',
    InitiateLogout = '[Logout] User initiated logout',
    ClearedLocalStorage = '[Logout] LocalStorage cleared',
}

export class SetMainMenuId implements Action {
    readonly type = MenuActionTypes.SetMainMenuId;
    constructor(public id: number) {}
}

export class ClearState implements Action {
    readonly type = MenuActionTypes.ClearState;
}

/**
 * Menu configuration object actions
 */
export class SetMenu implements Action {
    readonly type = MenuActionTypes.SetMenu;
    constructor(public data: IMenu) {}
}

export class SetMainMenuItems implements Action {
    readonly type = MenuActionTypes.SetMainMenuItems;
    constructor(public data: IMenuItem[], public isInitialSetting: boolean = false) {}
}

export class ResetMenu implements Action {
    readonly type = MenuActionTypes.ResetMenu;
}

export class SetSelectedItem implements Action {
    readonly type = MenuActionTypes.SetSelectedItem;
    constructor(public item: IMenuItem) {}
}

export class SetSidenavOpenState implements Action {
    readonly type = MenuActionTypes.SetSidenavOpen;
    constructor(public payload: boolean) {}
}

export class ToggleSidenavOpen implements Action {
    readonly type = MenuActionTypes.ToggleSidenavOpen;
}

export class SetSidenavDrilledState implements Action {
    readonly type = MenuActionTypes.SetSidenavDrilledState;
    constructor(public payload: boolean) {}
}

export class SetSidenavDrilledAnimationState implements Action {
    readonly type = MenuActionTypes.SetSidenavDrilledAnimationState;
    constructor(public payload: boolean) {}
}

export class SetResourceSelectedState implements Action {
    readonly type = MenuActionTypes.SetResourceSelectedState;
    constructor(public payload: boolean) {}
}

export class SetDisplayMenuItemsState implements Action {
    readonly type = MenuActionTypes.SetDisplayMenuItems;
    constructor(public data: IMenuItem[]) {}
}

export class ClearDisplayMenuItemsState implements Action {
    readonly type = MenuActionTypes.ClearDisplayMenuItems;
}

export class UpdateActiveProduct implements Action {
    readonly type = MenuActionTypes.UpdateActiveProduct;
    constructor(public productInfo: { productTitle: string, productIcon: string }) {}
}

export class ClearActiveProduct implements Action {
    readonly type = MenuActionTypes.ClearActiveProduct;
}

export class ClearNavigationHistory implements Action {
    readonly type = MenuActionTypes.ClearNavigationHistory;
}

export class AddNavigationHistory implements Action {
    readonly type = MenuActionTypes.AddNavigationHistoryItem;
    constructor(public item: NavHistoryItem) {}
}

export class RemoveNavigationHistory implements Action {
    readonly type = MenuActionTypes.RemoveNavigationHistoryItem;
}

export class SetNavigationHistory implements Action {
    readonly type = MenuActionTypes.SetNavigationHistory;
    constructor(public history: NavHistoryItem[]) {}
}

export class InitiateLogout implements Action {
    readonly type = MenuActionTypes.InitiateLogout;
}

export type MenuActions = ClearState | SetMenu | ResetMenu | SetSelectedItem | SetSidenavOpenState
    | SetSidenavDrilledState | SetSidenavDrilledAnimationState | SetResourceSelectedState
    | ToggleSidenavOpen | SetMainMenuItems | SetDisplayMenuItemsState | ClearDisplayMenuItemsState
    | UpdateActiveProduct | ClearActiveProduct | ClearNavigationHistory | AddNavigationHistory | RemoveNavigationHistory
    | SetNavigationHistory | SetMainMenuId | InitiateLogout;

import { IMenu, IMenuItem } from '@ds/core/app-config/shared';
import { createReducer, on, Action } from '@ngrx/store';
import { MenuActions, MenuActionTypes, SetMainMenuItems, SetDisplayMenuItemsState, AddNavigationHistory, SetNavigationHistory, RemoveNavigationHistory } from '../actions/menu.actions';
import { NavHistoryItem } from '@ds/core/shared';

export interface State {
    menu: IMenu;
    isSidenavOpen: boolean;
    isSideNavDrilled: boolean;
    isSideNavDrilledNoAnimation: boolean;
    isResourceSelected: boolean;
    selectedUrlItem: IMenuItem;
    mainMenuItems: IMenuItem[];
    displayMenuItems: IMenuItem[];
    productTitle: string;
    productIcon: string;
    navigationHistory: NavHistoryItem[];
    mainMenuItemId: number;
}

// tslint:disable-next-line: no-inferrable-types
export const menuFeatureKey: string = 'nav';

export const initialState: State = {
    menu: null,
    isSidenavOpen: true,
    isSideNavDrilled: false,
    isSideNavDrilledNoAnimation: false,
    isResourceSelected: false,
    selectedUrlItem: null,
    mainMenuItems: [],
    displayMenuItems: [],
    productTitle: null,
    productIcon: null,
    navigationHistory: [],
    mainMenuItemId: null
} as State;

export function reducer(state = initialState, action: MenuActions): State {
    switch (action.type) {
        case MenuActionTypes.ClearState:
            return initialState;
        case MenuActionTypes.ResetMenu:
            return { ...state, menu: null };
        case MenuActionTypes.SetMenu:
            return { ...state, menu: action.data };
        case MenuActionTypes.SetSelectedItem:
            return { ...state, selectedUrlItem: action.item };
        case MenuActionTypes.SetSidenavOpen:
            return { ...state, isSidenavOpen: action.payload };
        case MenuActionTypes.SetResourceSelectedState:
            return { ...state, isResourceSelected: action.payload };
        case MenuActionTypes.SetSidenavDrilledState:
            return { ...state, isSideNavDrilled: action.payload };
        case MenuActionTypes.SetSidenavDrilledAnimationState:
            return { ...state, isSideNavDrilledNoAnimation: action.payload };
        case MenuActionTypes.ToggleSidenavOpen:
            return { ...state, isSidenavOpen: !state.isSidenavOpen };
        case MenuActionTypes.SetMainMenuItems:
            return setMainMenuItemsReducer(state, action);
        case MenuActionTypes.SetDisplayMenuItems:
            return setDisplayMenuItemsReducer(state, action);
        case MenuActionTypes.ClearDisplayMenuItems:
            return { ...state, displayMenuItems: [] };
        case MenuActionTypes.UpdateActiveProduct:
            return {
                ...state,
                productTitle: action.productInfo.productTitle,
                productIcon: action.productInfo.productIcon
            };
        case MenuActionTypes.ClearActiveProduct:
            return { ...state, productTitle: null, productIcon: null };
        case MenuActionTypes.ClearNavigationHistory:
            return { ...state, navigationHistory: [] };
        case MenuActionTypes.AddNavigationHistoryItem:
            return applyItemToHistoryReducer(state, action);
        case MenuActionTypes.RemoveNavigationHistoryItem:
            return removeLastNavigationHistoryItem(state, action);
        case MenuActionTypes.SetNavigationHistory:
            return setNavigationHistory(state, action);
        case MenuActionTypes.SetMainMenuId:
            return {...state, mainMenuItemId: action.id};
        case MenuActionTypes.InitiateLogout:
            return {...initialState};
        default:
            return state;
    }
}

export function applyItemToHistoryReducer(state: State, action: AddNavigationHistory): State {
    if (action.item == null) return state;

    const historyExists = state.navigationHistory.find(nh => nh.dest === action.item.dest) != null;
    if (historyExists) return state;

    if (state.navigationHistory.length) {
        const hasParent = state.navigationHistory.find(nh => nh.dest === action.item.source) != null;

        if (hasParent) {
            return { ...state, navigationHistory: mergeNavigationHistoryItem(state, action.item) };
        } else {
            return { ...state, navigationHistory: [action.item] };
        }
    }

    return { ...state, navigationHistory: [action.item] };
}

export function removeLastNavigationHistoryItem(state: State, action: RemoveNavigationHistory): State {
    const newHistory = [].concat(state.navigationHistory) as NavHistoryItem[];
    newHistory.shift();
    return { ...state, navigationHistory: newHistory };
}

export function setNavigationHistory(state: State, action: SetNavigationHistory): State {
    if (!action.history || !action.history.length) return state;

    const isEqual = isEqualArray(state.navigationHistory, action.history);

    if (isEqual) return state;

    const merged = [...state.navigationHistory, ...action.history];
    const set = new Set();
    const union = merged.filter(item => {
        if (!set.has(item.dest)) {
            set.add(item.dest);
            return true;
        }
        return false;
    }, set);

    return {...state,
        navigationHistory: union
    };
}

export function mergeNavigationHistoryItem(state: State, action: NavHistoryItem): NavHistoryItem[] {
    const sameParent = state.navigationHistory.findIndex(nh => nh.parent === action.parent);

    if (sameParent > -1) {
        const newHistory = [...state.navigationHistory];
        newHistory.splice(sameParent, 1);

        return [...newHistory, action].sort((a, b) => b.dest - a.dest);
    }

    return [...state.navigationHistory, action].sort((a, b) => b.dest - a.dest);
}

export function setMainMenuItemsReducer(state: State, action: SetMainMenuItems): State {
    const isInitialPageLoad = action.isInitialSetting;

    if (isInitialPageLoad && state.mainMenuItems.length) {
        return state;
    }

    return { ...state, mainMenuItems: action.data };
}

export function setDisplayMenuItemsReducer(state: State, action: SetDisplayMenuItemsState): State {
    if (action.data && action.data.length) {
        return { ...state, displayMenuItems: action.data };
    }
    return state;
}

export function eqSet(as, bs) {
    if (as.size !== bs.size) return false;
    for (const a of as) if (!bs.has(a)) return false;
    return true;
}

export function isEqualObject(a, b) {
    if (typeof a !== 'object' || a === null) return false;
    if (typeof b !== 'object' || b === null) return false;
    const aKeys = Object.keys(a);
    const bKeys = Object.keys(b);
    const aKeysSet = new Set(aKeys);
    const bKeysSet = new Set(bKeys);

    const eqSets = eqSet(aKeysSet, bKeysSet);

    if (!eqSets) return false;

    for (const p in a) {
        const av = a[p];
        const bv = b[p];

        if (av !== bv) return false;
    }

    return true;
}

export function isEqualArray(a, b): boolean {
    if (!Array.isArray(a) || !Array.isArray(b)) return false;
    if (a.length !== b.length) return false;

    for (let i = 0; i < a.length; i++) {
        const ai = a[i];
        const bi = b[i];

        if (ai === null || bi === null) return false;

        const atype = typeof ai;
        const btype = typeof bi;

        if (atype !== btype) return false;

        if (!isEqualObject(ai, bi))
            return false;
    }

    return true;
}

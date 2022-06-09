import { IMenu } from '../..';
import { IMenuItem } from '../../shared';
import { createSelector } from '@ngrx/store';
import * as fromMenu from './menu.reducer';
import { NavHistoryItem } from '@ds/core/shared';


export const nav = ((state): fromMenu.State => state.nav);
export const getActiveMenu = createSelector(nav, (state): IMenu => state.menu);
export const getSelectedItem = createSelector(nav, (state): IMenuItem => state.selectedUrlItem);
export const getIsResourceSelected = createSelector(nav, (state): boolean => state.isResourceSelected);
export const getIsSidenavDrilled = createSelector(nav, (state): boolean => state.isSideNavDrilled);
export const getIsSidenavDrilledNoAnimation = createSelector(nav, (state): boolean => state.isSideNavDrilledNoAnimation);
export const getIsSidenavOpen = createSelector(nav, (state): boolean => state.isSidenavOpen);
export const getMainMenuItems = createSelector(nav, (state): IMenuItem[] => state.mainMenuItems);
export const getDisplayMenuItems = createSelector(nav, (state): IMenuItem[] => state.displayMenuItems);
export const getProductTitle = createSelector(nav, (state): string => state.productTitle);
export const getProductIcon = createSelector(nav, (state): string => state.productIcon);
export const getNavigationHistory = createSelector(nav, (state): NavHistoryItem[] => state.navigationHistory);
export const getMainMenuId = createSelector(nav, (state): number => state.mainMenuItemId);

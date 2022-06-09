import { Action, ActionReducer } from "@ngrx/store";
import { NgrxStorageService } from "../ngrx-storage.service";
import { pick, merge } from "lodash";
import { UrlParts, NavHistoryItem } from "@ds/core/shared";
import { State, mergeNavigationHistoryItem } from "../reducers/menu.reducer";
import { IMenuItem } from "../..";
import {
  rehydrateActiveProductInfo,
  findMainMenuItemFromByAncestor,
  buildNavigationHistoryFromSelected,
} from "../../shared/menu-helpers";

export function storageMetaReducer<
  S extends State = State,
  A extends Action = Action
>(
  saveKeys: string[],
  localStorageKey: string,
  storageService: NgrxStorageService,
  document: Document
) {
  let onInit = true;

  return function (reducer: ActionReducer<S, A>) {
    return function (state: S, action: A): S {
      const nextState = reducer(state, action);

      if (onInit) {
        onInit = false;

        const savedState = storageService.getSavedState(localStorageKey) as S;
        const newState =
          savedState != null ? merge(nextState, savedState) : nextState;

        return rehydrateState(newState, action, document);
      }

      const stateToSave = pick(nextState, saveKeys);
      storageService.setSavedState(stateToSave, localStorageKey);
      return nextState;
    };
  };
}

export function rehydrateState<
  S extends State = State,
  A extends Action = Action
>(state: S, action: A, document: Document): S {
  const urlParts = UrlParts.ParseUrl(document.location.href);
  const navHistory = ((state as unknown) as State).navigationHistory;
  const menuItems =
    ((state as unknown) as State).menu != null
      ? ((state as unknown) as State).menu.items
      : [];
  let currItem = ((state as unknown) as State).selectedUrlItem;

  if (currItem && currItem.isActive)
    return state;

  // This is a list of pages that we want to keep the current menu state of. `CurrItem` is set menu click, and these pages are not a part of the menu and only accessible through redirect or a parent element.
  // It assumes the parent element is the element we want selected
  const hiddenPages = [
    { matchItem: '', url: "changeemployee.aspx" },
    { matchItem: 'payroll', url: "payrollchecks.aspx" },
    { matchItem: 'paycheckcalculator', url: "paycheckcalculator.aspx" },
    { matchItem: 'aca', url: "companyaca1095cinformation.aspx" },
    { matchItem: 'aca', url: "companyacacontact.aspx" },
    { matchItem: 'aca', url: "companyaca1094cinformation.aspx" },
    { matchItem: 'aca', url: "employee1095cconsent.aspx" },
    { matchItem: 'aca', url: "companyaca1095capproval.aspx" },
    { matchItem: 'jobposts', url: "applicantcompanyposting.aspx" },
    { matchItem: 'plans', url: "benefiteditplan.aspx" },
    { matchItem: 'enrollments', url: "benefiteditopenenrollment.aspx" },
  ];

  // Sets a boolean value to true if the URL is included in the list above
  for (let i = 0; i < hiddenPages.length; i++) {
    const p = hiddenPages[i];
    if (urlParts.href.toLowerCase().includes(p.url)) {
      const match = menuItems.find(m => m.title.trim().toLowerCase().replace(/\s/g, '') === p.matchItem && m.parentId == null);

      if (match) {
        return mergeRehydratedState(state, {...match, isActive: true});
      }

      return state;
    }
  }

  // Check to see if the currently selected item matches the URL
  // If does, we are going to return the current state so that we don't mess up anything in the
  // currently cached data.
  if (
    currItem &&
    currItem.resource &&
    currItem.resource.routeUrl != null &&
    urlParts.isEqualTo(currItem.resource.routeUrl)
  ) {
    const activeSiblings = navHistory.filter(
      (nh) => nh.parent === currItem.parentId && nh.dest !== currItem.menuItemId
    );
    let newHistory: NavHistoryItem[] = navHistory.slice();
    const isWrongHistory =
      newHistory.findIndex((nh) => nh.dest === currItem.menuItemId) < 0;
    let mainMenuId = ((state as unknown) as State).mainMenuItemId;

    if (isWrongHistory) {
      newHistory = [];
    }

    /**
     * If the user has clicked a sibling that exists in a different site (payroll -> company)
     * then the menu will get reloaded and we need to make sure we are removing siblings when we're
     * drilled into the menu so we don't get multiple highlighted links at the same level
     */
    if (activeSiblings) {
      newHistory = navHistory.filter((nh) => !activeSiblings.includes(nh));
    }

    // inject history if this is manufactured history
    if (
      newHistory.length < 2 &&
      ((state as unknown) as State).isSideNavDrilled
    ) {
      const mainMenuItem = findMainMenuItemFromByAncestor(currItem, menuItems);

      if (mainMenuItem != null) {
        const hasMenuMenuHistory = newHistory.findIndex(
          (nh) => nh.dest === mainMenuItem.menuItemId
        );

        if (!hasMenuMenuHistory) {
          newHistory.push({
            source: null,
            dest: mainMenuItem.menuItemId,
            parent: null,
          });
          mainMenuId = mainMenuItem.menuItemId;
        }

        newHistory = buildNavigationHistoryFromSelected(
          newHistory,
          menuItems,
          currItem
        );
      }
    }

    return {
      ...state,
      navigationHistory: newHistory,
      mainMenuItemId: mainMenuId,
    };
  } else if (
    urlParts.href.toLowerCase().includes("company/onboarding/addworkflow")
  ) {
    return {
      ...state,
      selectedUrlItem: menuItems.find(
        (m) => m.title.toLowerCase() == "onboarding"
      ),
    };
  }

  /**
   * Look for menu items that match our current URL
   */
  let hardSelect = false;
  if (
    urlParts.href.toLowerCase().includes("performance/manage/status") ||
    urlParts.href.toLowerCase().includes("performance/manage/approval") ||
    urlParts.href.toLowerCase().includes("performance/manage/analytics")
  )
    hardSelect = true; // Performance Manager

  let matches = menuItems.filter((mi) => {
    let match = false;
    if (
      mi.resource &&
      mi.resource.routeUrl != null &&
      (!mi.items || !mi.items.length)
    ) {
      match = urlParts.isEqualTo(mi.resource.routeUrl);
    }

    if (!match && mi.items && mi.items.length) {
      mi.items.forEach((item) => {
        if (!match && item.resource && item.resource.routeUrl)
          match = urlParts.isEqualTo(item.resource.routeUrl);
      });
    }

    return match;
  });

  if (!matches || (matches.length == 0 && hardSelect))
    matches.push(
      menuItems.find((m) => {
        if (m.resource && m.resource.routeUrl)
          return m.resource.routeUrl
            .toLowerCase()
            .includes("performancereviews.aspx");
        return false;
      })
    );

  if (matches && matches.length > 1) {
    const historyIds = navHistory.map((nh) => nh.dest);
    const ancestorHistoryMatches = matches.filter((m) =>
      historyIds.includes(m.parentId)
    );

    if (ancestorHistoryMatches.length === 1) {
      // found the one we were looking for...

      return mergeRehydratedState(state, ancestorHistoryMatches[0]);
    } else if (ancestorHistoryMatches.length > 1) {
      // somehow we have matched multiple things still..

      return mergeRehydratedState(state, ancestorHistoryMatches[0]);
    } else {
      // We didn't find any things that match in the history so we are going to generate
      // history for the user and build the menu in this case
      const match = matches[0];
      let childNodes;
      if (match.items && match.items.length) {
        childNodes = match.items;
      } else {
        const parent = menuItems.find((m) => m.menuItemId === match.parentId);
        childNodes = menuItems.filter((m) => m.parentId === parent.menuItemId);
      }

      const act = {
        source: null,
        dest: match.menuItemId,
        parent: match.parentId,
      };

      const mainMenuItem = ((state as unknown) as State).menu.items.find(
        (m) => m.menuItemId === act.parent
      );

      let productInfo = rehydrateActiveProductInfo(
        (state as unknown) as State,
        ((state as unknown) as State).mainMenuItems,
        match,
        ((state as unknown) as State).menu.items
      );

      return {
        ...state,
        selectedUrlItem: match,
        mainMenuItemId: mainMenuItem.menuItemId,
        navigationHistory: [act],
        displayMenuItems: childNodes,
        productTitle: productInfo.productTitle,
        productIcon: productInfo.productIcon,
        isSidenavOpen: true,
        isSideNavDrilled: true,
        isSideNavDrilledNoAnimation: true,
        isResourceSelected: true,
      };
    }
  } else if (matches && matches.length === 1) {
    const match = matches[0];
    return mergeRehydratedState(state, match);
  } else if (state && state.selectedUrlItem != null) {
    return state;
  }

  return ({
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
  } as unknown) as S;
}

/**
 * While rehydrating the navigational menu state, there is the possibility of finding multiple matches.
 * This method is used after the single match is determined and is used to merge into the existing state
 * that we have.
 */
export function mergeRehydratedState<S>(state: S, match: IMenuItem): S {
  const allItems = ((state as unknown) as State).menu.items;
  const selectedItem =
    ((state as unknown) as State).selectedUrlItem ||
    ({ menuItemId: null } as IMenuItem);
  const navHistory = ((state as unknown) as State).navigationHistory;

  const isInHistory = navHistory.find((nh) => nh.dest === match.menuItemId);
  const matchesSelectedItem = match.menuItemId === selectedItem.menuItemId;

  const productInfo = rehydrateActiveProductInfo(
    (state as unknown) as State,
    ((state as unknown) as State).mainMenuItems,
    match,
    ((state as unknown) as State).menu.items
  );

  let displayMenuItems = allItems.filter(
    (a) => a.parentId === match.menuItemId
  );
  if (!displayMenuItems || !displayMenuItems.length) {
    displayMenuItems = allItems.filter((a) => a.parentId === match.parentId);
  }

  const lastNav = navHistory[0] || ({ dest: null } as NavHistoryItem);

  if (isInHistory && matchesSelectedItem) {
    // this item is already in history and is already selected... move along
    return state;
  } else if (isInHistory && !matchesSelectedItem) {
    // this item is in history, but not selected, let's select it
    return {
      ...state,
      selectedUrlItem: match,
      productTitle: productInfo.productTitle,
      productIcon: productInfo.productIcon,
      isSidenavOpen: true,
      isSideNavDrilled: true,
      isSideNavDrilledNoAnimation: true,
      displayMenuItems,
    };
  } else if (!isInHistory && matchesSelectedItem) {
    const act = {
      source: lastNav.dest,
      dest: match.menuItemId,
      parent: match.parentId,
    };
    return {
      ...state,
      navigationHistory: mergeNavigationHistoryItem(
        (state as unknown) as State,
        act
      ),
      productTitle: productInfo.productTitle,
      productIcon: productInfo.productIcon,
      isSidenavOpen: true,
      isSideNavDrilled: true,
      isSideNavDrilledNoAnimation: true,
      displayMenuItems,
    };
  } else {
    const act = {
      source: lastNav.dest,
      dest: match.menuItemId,
      parent: match.parentId,
    };
    return {
      ...state,
      selectedUrlItem: match,
      navigationHistory: [act],
      productTitle: productInfo.productTitle,
      productIcon: productInfo.productIcon,
      isSidenavOpen: true,
      isSideNavDrilled: true,
      isSideNavDrilledNoAnimation: true,
      displayMenuItems,
    };
  }
}

import { defer, Observable, combineLatest, of } from 'rxjs'
import { IMenu } from '.';
import { map, switchMap } from 'rxjs/operators';
import { IMenuItem, IMenuProductInfo } from './menu-item.model';
import { UrlParts, NavHistoryItem } from '@ds/core/shared';
import { MenuService } from './menu.service';
import { ChangeDetectorRef } from '@angular/core';
import { State } from '../ngrx-store/reducers/menu.reducer';


const initNav = (id: number, items: IMenuItem[], parent?: IMenuItem) => {
    // Loop thru each menu item
    items.forEach(i => {
        i.menuItemId = id++;

        // Set the item's parent to the parent passed in
        // i.parent = parent;
        i.parentId = parent && parent.menuItemId > 0 ? parent.menuItemId : null;

        if (i.parentId != null && parent && parent.sectionTitle) {
            i.sectionTitle = parent.sectionTitle;
            i.menuIcon = parent.menuIcon;
        }

        // Only set product icons at the top level
        if (i.menuItemId < 1) {
            // Set the top level icon
            // we don't store it in the database
            switch (i.title.toLowerCase()) {
                case 'administrator':
                    i.menuIcon = 'build';
                    i.sectionTitle = 'Administrator';
                    break;
                case 'company':
                    i.menuIcon = 'business';
                    i.sectionTitle = 'Company';
                    break;
                case 'profile':
                    i.menuIcon = 'account_circle';
                    i.sectionTitle = 'Profile';
                    break;
                case 'employee':
                    i.menuIcon = 'supervisor_account';
                    i.sectionTitle = 'Employee';
                    break;
                case 'payroll':
                    i.menuIcon = 'attach_money';
                    i.sectionTitle = 'Payroll';
                    break;
                case 'reports':
                    i.menuIcon = 'description';
                    i.sectionTitle = 'Reports';
                    break;
                case 'time & attendance':
                    i.menuIcon = 'access_time';
                    i.sectionTitle = 'Time & Attendance';
                    break;
                case 'applicant tracking':
                    i.menuIcon = 'group_add';
                    i.sectionTitle = 'Applicant Tracking';
                    break;
                case 'benefits':
                    i.menuIcon = 'add_shopping_cart';
                    i.sectionTitle = 'Benefits';
                    break;
                case 'performance':
                    i.menuIcon = 'stars';
                    i.sectionTitle = 'Performance';
                    break;
                case 'notes':
                    i.menuIcon = 'notes';
                    i.sectionTitle = 'Notes';
                    break;
                default:
                    break;
            }
        }

        // If the menu item has sub menu items,
        // set the parent or the sub menu items
        // to the menu item we are on in the loop
        if (i.items)
            id = initNav(id, i.items, i);
    });
    return id;
};

export const initializeMenu = () => (source: Observable<IMenu>): Observable<IMenu> =>
    new Observable(observer => {
        return source.subscribe({
            next(x) {
                const menu = x;
                let id = 1;
                id = initNav(id, menu.items);
                observer.next(menu);
            },
            error(err) { observer.error(err); },
            complete() { observer.complete(); }
        });
    });

export const checkStorage = (menuService: MenuService, document: Document, cd: ChangeDetectorRef) =>
    (source: Observable<any>): Observable<IMenu> =>
    new Observable(observer => {
        return source
            .pipe(
                switchMap(_ => combineLatest(menuService.getMenu(),
                    menuService.getSelectedItem(),
                    menuService.getNavigationHistory(),
                    menuService.getMainMenuItems(),
                    menuService.getState()))
            )
            .subscribe({
                next([menu, selectedItem, navHistory, mainMenu, state]) {
                    if (selectedItem != null) {
                        observer.next(menu);
                    } else {
                        const url = UrlParts.ParseUrl(document.location.href);
                        const matched = menu.items.find(mi => mi.resource != null && url.isEqualTo(mi.resource.routeUrl));

                        if (matched) {
                            menuService.updateSidenavDrilledNoAnimation(true);

                            if (matched.items && matched.items.length) {
                                menuService.updateDisplayMenuItems(matched.items);
                            } else if ((!matched.items || !matched.items.length) && matched.parentId != null) {
                                const parent = menu.items.find(mi => mi.menuItemId === matched.parentId);

                                if (parent && parent.items) {
                                    menuService.updateDisplayMenuItems(parent.items);
                                }
                            }

                            navHistory.push({
                                source: matched.parentId,
                                dest: matched.menuItemId,
                                parent: matched.parentId,
                            });
                            const newHistory = buildNavigationHistoryFromSelected(navHistory, menu.items, matched);
                            const productInfo = rehydrateActiveProductInfo(state, mainMenu, matched, menu.items);

                            menuService.updateActiveProductInfo(productInfo.productTitle, productInfo.productIcon);
                            menuService.updateNavigationHistory(newHistory);
                            menuService.updateSelectedItem(matched);
                            menuService.updateSidenavDrilledNoAnimation(false);
                            cd.detectChanges();
                        }
                    }
                    observer.next(menu);
                },
                error(err) { observer.error(err); },
                complete() { observer.complete(); }
            });
    });

export function buildNavigationHistoryFromSelected(navHistory: NavHistoryItem[], dict: IMenuItem[], curr: IMenuItem): NavHistoryItem[] {
    if (curr.parentId != null) {
        const parent = dict.find(d => d.menuItemId === curr.parentId);

        if (parent) {
            if (navHistory[0] && !navHistory[0].source) {
                navHistory[0].source = parent.menuItemId;
            }

            navHistory.push({
                source: null,
                dest: parent.menuItemId,
                parent: parent.parentId,
            });

            navHistory = navHistory.sort((a, b) => b.dest - a.dest);

            if (parent.parentId != null) {
                navHistory = buildNavigationHistoryFromSelected(navHistory, dict, parent);
            }
        }
    } else if (curr.isMainMenuItem) {
        if (navHistory[0] && !navHistory[0].source) {
            navHistory[0].source = curr.menuItemId;
        }

        navHistory.push({
            source: null,
            dest: curr.menuItemId,
            parent: curr.parentId,
        });

        navHistory = navHistory.sort((a, b) => b.dest - a.dest);
    }

    return navHistory;
}

export function rehydrateActiveProductInfo(state: State, menuItems: IMenuItem[],
    match: IMenuItem, allItems: IMenuItem[]): IMenuProductInfo {

    if (match.parentId) {
        let lastMatch = match;
        let parentMatch = allItems.find(m => m.menuItemId == match.parentId);
        if (parentMatch && parentMatch.parentId == null) lastMatch = parentMatch;
        
        while(parentMatch && parentMatch.parentId) {
            parentMatch = allItems.find(m => m.menuItemId == parentMatch.parentId);
            if (parentMatch) lastMatch = parentMatch;
        }
        
        return {
            productTitle: lastMatch.title,
            productIcon: lastMatch.menuIcon
        };
    }

    return {
        productTitle: match.title,
        productIcon: match.menuIcon
    };
}

export function findMainMenuItemFromByAncestor(item: IMenuItem, allItems: IMenuItem[]): IMenuItem {
    if (item == null) return null;

    const parent = allItems.find(a => a.menuItemId === item.parentId);

    if (parent && parent.isMainMenuItem) {
        return parent;
    }

    return findMainMenuItemFromByAncestor(parent, allItems);
}

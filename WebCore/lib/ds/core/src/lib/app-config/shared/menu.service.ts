import { Injectable, Inject } from "@angular/core";
import { IMenuItem, IMenu, UIViewState } from ".";
import {
  SetMenu,
  SetSelectedItem,
  SetResourceSelectedState,
  SetSidenavDrilledState,
  SetSidenavOpenState,
  SetSidenavDrilledAnimationState,
  ToggleSidenavOpen,
  SetMainMenuItems,
  SetDisplayMenuItemsState,
  ClearDisplayMenuItemsState,
  UpdateActiveProduct,
  ClearActiveProduct,
  ClearNavigationHistory,
  AddNavigationHistory,
  RemoveNavigationHistory,
  SetNavigationHistory,
  SetMainMenuId,
} from "../ngrx-store/actions/menu.actions";
import { Store } from "@ngrx/store";
import * as fromReducer from "../ngrx-store/reducers";
import { Observable, combineLatest, Subject } from "rxjs";
import { map, takeUntil, mergeMap, tap } from "rxjs/operators";
import { NavHistoryItem, UrlParts } from "@ds/core/shared";
import { State } from "../ngrx-store/reducers/menu.reducer";
import { isObject } from "angular";
import { Router } from "@angular/router";
import { APP_CONFIG, AppConfig } from "../app-config";

@Injectable({
  providedIn: "root",
})
export class MenuService {
  static readonly AJS_SERVICE_NAME = "ngxMenuService";

  /**
   * This is a flat dictionary of the menu structure used for showing the proper menu items
   * to the user in any state that they end up in.
   */
  private cache: IMenuItem[] = [];
  private selectedItem: IMenuItem;

  selectedItem$: Observable<IMenuItem> = this.store.select(
    fromReducer.getSelectedItem
  );
  isResoureSelected$: Observable<boolean> = this.store.select(
    fromReducer.getIsResourceSelected
  );
  isSidenavOpen$: Observable<boolean> = this.store.select(
    fromReducer.getIsSidenavOpen
  );
  isSidenavDrilled$: Observable<boolean> = this.store.select(
    fromReducer.getIsSidenavDrilled
  );
  isSidenavDrilledNoAnimation$: Observable<boolean> = this.store.select(
    fromReducer.getIsSidenavDrilledNoAnimation
  );
  displayMenuItems$: Observable<IMenuItem[]> = this.store.select(
    fromReducer.getDisplayMenuItems
  );
  isMainMenuSet = false;

  constructor(
    private store: Store<State>,
    private router: Router,
    @Inject(APP_CONFIG) private config: AppConfig
  ) {}

  get(id: number): IMenuItem {
    return this.cache.find((c) => c.menuItemId === id);
  }

  getChildNodes(id: number): IMenuItem[] {
    return this.cache.filter((c) => c.parentId === id) || [];
  }

  has(id: number): boolean {
    return this.cache.findIndex((c) => c.menuItemId === id) > -1;
  }

  updateMenu(menu: IMenu): void {
    if (!this.isMainMenuSet) {
      menu.items = this.resolveProductIcons(menu.items);
      this.store.dispatch(new SetMainMenuItems(menu.items, true));
      this.isMainMenuSet = true;
    }

    this.cache = this.buildMenuDictionary(menu.items);
    const cacheMenu = {
      items: this.cache,
      menuId: menu.menuId,
      name: menu.name,
    } as IMenu;
    this.store.dispatch(new SetMenu(cacheMenu));
  }

  getState(): Observable<State> {
    return this.store.select(fromReducer.nav);
  }

  getMenu(): Observable<IMenu> {
    return this.store.select(fromReducer.getActiveMenu);
  }

  getMainMenuId(): Observable<number> {
    return this.store.select(fromReducer.getMainMenuId);
  }

  setMainMenuId(id: number) {
    this.store.dispatch(new SetMainMenuId(id));
  }

  getSelectedItem(): Observable<IMenuItem> {
    return this.store.select(fromReducer.getSelectedItem);
  }

  setSelectedItem(item: IMenuItem): void {
    this.store.dispatch(new SetSelectedItem(item));
  }

  /**
   * Used to update the selected item, but cannot be used to traverse back up
   * the menu structure.
   */
  updateSelectedItem(item: IMenuItem): void {
    this.selectedItem = item;

    if (item) {
      this.store.dispatch(new SetSelectedItem(item));
      this.store.dispatch(new SetResourceSelectedState(true));
      this.store.dispatch(new SetSidenavDrilledState(true));
      this.store.dispatch(new SetSidenavOpenState(true));
    }
  }

  updateSetResourceSelected(isResourceSelected = false): void {
    this.store.dispatch(new SetResourceSelectedState(isResourceSelected));
  }

  updateSidenavDrilled(isDrilled: boolean): void {
    this.store.dispatch(new SetSidenavDrilledState(isDrilled));
  }

  updateSidenavDrilledNoAnimation(state: boolean): void {
    this.store.dispatch(new SetSidenavDrilledAnimationState(state));
  }

  updateIsSidenavOpen(isOpen: boolean): void {
    this.store.dispatch(new SetSidenavOpenState(isOpen));
  }

  toggleSidenav(): void {
    this.store.dispatch(new ToggleSidenavOpen());
  }

  getActiveMainMenuItem(): IMenuItem {
    return this.cache.find((c) => c.isMainMenuItem && c.isActive);
  }

  matchesSelectedItem(item: IMenuItem): boolean {
    return item === this.selectedItem;
  }

  hasSelectedItem(): boolean {
    return this.selectedItem != null;
  }

  getMainMenuItems(): Observable<IMenuItem[]> {
    return this.store.select(fromReducer.getMainMenuItems);
  }

  /**
   * Gets a list of menu items intended to be shown on the sidenav menu when the menu is
   * drilled into by the user.
   */
  getDisplayMenuItems(): Observable<IMenuItem[]> {
    return this.store.select(fromReducer.getDisplayMenuItems);
  }

  updateDisplayMenuItems(items: IMenuItem[]) {
    if (items && items.length) {
      this.store.dispatch(new SetDisplayMenuItemsState(items));
    } else {
      this.store.dispatch(new ClearDisplayMenuItemsState());
    }
  }

  updateActiveProductInfo(productTitle: string, productIcon: string) {
    this.store.dispatch(new UpdateActiveProduct({ productTitle, productIcon }));
  }

  clearActiveProductInfo(): void {
    this.store.dispatch(new ClearActiveProduct());
  }

  getProductInfo(): Observable<{ productTitle: string; productIcon: string }> {
    return combineLatest(
      this.store.select(fromReducer.getProductTitle),
      this.store.select(fromReducer.getProductIcon)
    ).pipe(
      map(([title, icon]) => {
        return { productTitle: title, productIcon: icon };
      })
    );
  }

  getNavigationHistory(): Observable<NavHistoryItem[]> {
    return this.store.select(fromReducer.getNavigationHistory);
  }

  updateNavigationHistory(history: NavHistoryItem[]) {
    this.store.dispatch(new SetNavigationHistory(history));
  }

  clearNavigationHistory(): void {
    this.store.dispatch(new ClearNavigationHistory());
  }

  addNavigationHistory(item: NavHistoryItem) {
    this.store.dispatch(new AddNavigationHistory(item));
  }

  /**
   * Removes the last step in the navigation history.
   */
  removeNavigationHistory() {
    this.store.dispatch(new RemoveNavigationHistory());
  }

  selectMenuItemFromAngularJs(view: UIViewState) {
    let selectedItem: IMenuItem;
    const stop$ = new Subject();
    const target = view.data.title.trim().toLowerCase();
    const targetUrl = view.url.trim().toLowerCase().split("{")[0];
    const tree = this.router.parseUrl(targetUrl);
    const cleanedUrl = this.router.serializeUrl(tree);

    this.getMenu()
      .pipe(
        takeUntil(stop$),
        mergeMap((menu) => {
          const item = menu.items.find((i) => {
            if (i.resource != null) {
              const routeUrl = i.resource.routeUrl.replace(
                this.config.baseSite.url,
                ""
              );
              const resourceTree = this.router.parseUrl(routeUrl);
              const resourceUrl = this.router.serializeUrl(resourceTree);

              return cleanedUrl === resourceUrl;
            }
            return false;
          });

          if (item) {
            selectedItem = item;
            this.updateSelectedItem(item);

            if (item.items && item.items.length) {
              this.updateDisplayMenuItems(item.items);
            }
          }
          return this.getMainMenuItems();
        }),
        tap((mainMenu) => {
          const item = mainMenu.find(
            (m) => m.title.trim().toLowerCase() === target
          );
          if (item) {
            this.setMainMenuId(item.menuItemId);
            this.updateActiveProductInfo(item.sectionTitle, item.menuIcon);
          }
        }),
        tap((_) => stop$.next())
      )
      .subscribe();
  }

  /**
   * Our top-level menu items are our main menu product menu items. These are different
   * from the rest of the menu items, have associated titles and design-determined material
   * icons includes. This method loops over the array of menu times passed and transforms them
   * assuming that the entire array is product-level menu items.
   */
  private resolveProductIcons(items: IMenuItem[]): IMenuItem[] {
    items.forEach((i) => {
      i.isMainMenuItem = true;

      // Set the top level icon
      // we don't store it in the database
      switch (i.title.toLowerCase()) {
        case "administrator":
          i.menuIcon = "build";
          i.sectionTitle = "Administrator";
          break;
        case "company":
          i.menuIcon = "business";
          i.sectionTitle = "Company";
          break;
        case "profile":
          i.menuIcon = "account_circle";
          i.sectionTitle = "Profile";
          break;
        case "employee":
          i.menuIcon = "supervisor_account";
          i.sectionTitle = "Employee";
          break;
        case "payroll":
          i.menuIcon = "attach_money";
          i.sectionTitle = "Payroll";
          break;
        case "reports":
          i.menuIcon = "description";
          i.sectionTitle = "Reports";
          break;
        case "time & attendance":
          i.menuIcon = "access_time";
          i.sectionTitle = "Time & Attendance";
          break;
        case "applicant tracking":
          i.menuIcon = "group_add";
          i.sectionTitle = "Applicant Tracking";
          break;
        case "benefits":
          i.menuIcon = "add_shopping_cart";
          i.sectionTitle = "Benefits";
          break;
        case "performance":
          i.menuIcon = "stars";
          i.sectionTitle = "Performance";
          break;
        case "notes":
          i.menuIcon = "notes";
          i.sectionTitle = "Notes";
          break;
        case 'job board':
          i.menuIcon = 'view_agenda';
          i.sectionTitle = 'Job Board';
          break;
        case 'applicant info':
          i.menuIcon = 'person';
          i.sectionTitle = 'Applicant Info';
          break;  
        default:
          break;
      }
    });
    return items;
  }

  private buildMenuDictionary(arr: IMenuItem[]): IMenuItem[] {
    let result: IMenuItem[] = [];

    for (let i = 0; i < arr.length; i++) {
      const item = arr[i];

      if (item.items && item.items.length) {
        const childMenu = this.buildMenuDictionary(item.items);
        result = [...result, ...childMenu];
      }

      result.push(item);
    }

    return result;
  }
}

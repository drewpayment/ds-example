import {
  Component,
  OnInit,
  AfterViewInit,
  ViewChild,
  Inject,
  Input,
  ViewContainerRef,
  ChangeDetectorRef,
  OnDestroy,
  ViewEncapsulation,
} from "@angular/core";
import { BreakpointObserver, Breakpoints } from "@angular/cdk/layout";
import { Observable, combineLatest, Subject } from "rxjs";
import { map, tap, switchMap, takeUntil } from "rxjs/operators";
import {
  trigger,
  state,
  style,
  animate,
  transition,
} from "@angular/animations";
import {
  MenuApiService,
  SiteDesignationType,
} from "../../../app-config/shared/menu-api.service";
import {
  IMenu,
  IMenuItem,
  IApplicationResource,
  ApplicationSourceType,
} from "../../../app-config/shared";
import { DOCUMENT } from "@angular/common";
import { UrlParts } from "@ds/core/shared";
import { AccountService } from "@ds/core/account.service";
import { ConfigUrl, ConfigUrlType } from "@ds/core/shared/config-url.model";
import { MenuService } from "@ds/core/app-config/shared/menu.service";
import { NavHistoryItem, DsNavMenuItemOptions } from "@ds/core/shared";
import { checkStorage } from "@ds/core/app-config/shared/menu-helpers";

const SITES_DESIGNATION = {
  PAYROLL: "payroll",
  ESS: "ess",
  COMPANY: "company",
};

export enum EssHeaderType {
  default,
  benefits,
}

@Component({
  selector: "ds-nav-menu",
  templateUrl: "./ds-nav-menu.component.html",
  styleUrls: ["./ds-nav-menu.component.scss"],
  animations: [
    // Product Menu animation
    trigger("mainNavExpander", [
      state(
        "true",
        style({
          width: "330px",
        })
      ),
      state(
        "false",
        style({
          width: "70px",
        })
      ),
      transition("true => false", animate("0.3s ease")),
      transition("false => true", animate("0.4s ease")),
    ]),
    //Controls content animation when desktop nav moves in and out
    trigger("mainNavContentMargin", [
      state(
        "true",
        style({
          marginLeft: "330px",
        })
      ),
      state(
        "false",
        style({
          marginLeft: "70px",
        })
      ),
      transition("true => false", animate("0.3s ease")),
      transition("false => true", animate("0.4s ease")),
    ]),
    trigger("innerNavExpanderMain", [
      state(
        "false",
        style({
          width: "330px",
        })
      ),
      state(
        "true",
        style({
          width: "70px",
        })
      ),
      transition("true => false", animate("0.2s ease")),
      transition("false => true", animate("0.2s ease")),
    ]),
    // Fades inner nav content for a smooth transition when updating the ngFor sub nav
    trigger("navContentFadeIn", [
      transition(":enter", [
        style({ opacity: "0" }),
        animate(".5s ease-out", style({ opacity: "1" })),
      ]),
    ]),
  ],
  encapsulation: ViewEncapsulation.Emulated,
})
export class DsNavMenuComponent implements OnInit, OnDestroy, AfterViewInit {
  isLoaded = false;

  /**
   * Local instance of the overall menu configuration object.
   */
  menu$: Observable<IMenu> = this.menuService.getMenu();
  menu: IMenu;

  mainMenuId$: Observable<number> = this.menuService.getMainMenuId();

  /**
   * The menu item user has selected most recently. This is only
   * used for runtime determinations and should not be used as a state
   * persistent variable.
   *
   * @Internal
   */
  selectedItem$: Observable<IMenuItem> = this.menuService.getSelectedItem();
  selectedItem: IMenuItem;

  /**
   * Controls where the Sidenav Menu is expanded on the screen or just
   * shows an icon bar on the left.
   */
  isSidenavOpen$: Observable<boolean> = this.menuService.isSidenavOpen$;
  isSidenavOpen = true;

  /**
   * If user has drilled into a product submenu
   */
  isSideNavDrilled$: Observable<boolean> = this.menuService.isSidenavDrilled$;
  isSideNavDrilled = false;

  /**
   * Disables animation when used with isSideNavDrilled = true
   */
  isSideNavDrilledNoAnimation$: Observable<boolean> =
    this.menuService.isSidenavDrilledNoAnimation$;
  isSideNavDrilledNoAnimation = false;

  /**
   * Used for sub-menu items active states.
   */
  isResourceSelected$: Observable<boolean> =
    this.menuService.isResoureSelected$;
  isResourceSelected = false;

  applicationSource: { [key: string]: number } = {};
  urls: ConfigUrl[];
  siteUrls$ = this.account.getSiteUrls();

  mainMenuItems: Observable<IMenuItem[]> = this.menuService.getMainMenuItems();
  navigationHistory: Observable<NavHistoryItem[]> =
    this.menuService.getNavigationHistory();
  activeIds: Observable<number[]> = this.navigationHistory.pipe(
    map((items) => (items != null ? items.map((item) => item.dest) : []))
  );
  displayMenuItems: Observable<IMenuItem[]> =
    this.menuService.displayMenuItems$;

  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe([Breakpoints.XSmall, Breakpoints.Small])
    .pipe(map((result) => result.matches));

  @Input() siteDesignation: string;

  @ViewChild("main", { static: false, read: ViewContainerRef })
  mainRef: ViewContainerRef;

  @ViewChild("header", { static: false, read: ViewContainerRef })
  headerRef: ViewContainerRef;

  isPayroll = false;
  isEss = false;
  isCompany = false;
  /**
   * Async piped on template to make sure we're showing the correct site URL.
   */
  homeLink$ = this.menuApiService.homeUrl$;
  cancelSubscriptions = new Subject();
  productInfo$ = this.menuService.getProductInfo();

  hideMenuWrapperTooltip$ = this.account.hideMenuWrapperTooltip$;

  constructor(
    private menuApiService: MenuApiService,
    private breakpointObserver: BreakpointObserver,
    @Inject(DOCUMENT) private document: Document,
    private account: AccountService,
    private menuService: MenuService,
    private cd: ChangeDetectorRef,
  ) {
    this.applicationSource = {
      sourceWeb: 1,
      essWeb: 2,
      companyWeb: 3,
      adminWeb: 4,
    };
  }

  ngOnInit() {
    combineLatest(
      this.menu$,
      this.selectedItem$,
      this.isSidenavOpen$,
      this.isSideNavDrilled$,
      this.isSideNavDrilledNoAnimation$,
      this.isResourceSelected$
    )
      .pipe(takeUntil(this.cancelSubscriptions))
      .subscribe(
        ([
          menu,
          selectedItem,
          isSidenavOpen,
          isSideNavDrilled,
          isSideNavDrilledNoAnimation,
          isResourceSelected,
        ]) => {
          this.menu = menu;
          this.selectedItem = selectedItem;
          this.isSidenavOpen = isSidenavOpen;
          this.isSideNavDrilled = isSideNavDrilled;
          this.isSideNavDrilledNoAnimation = isSideNavDrilledNoAnimation;
          this.isResourceSelected = isResourceSelected;
          this.isLoaded = this.menu != null;
          this.cd.detectChanges();
        }
      );

    this.setSiteDesignation(this.siteDesignation);

    // todo: add company site designation type
    const siteDes: SiteDesignationType =
      this.siteDesignation == SITES_DESIGNATION.ESS
        ? SiteDesignationType.ess
        : this.siteDesignation == SITES_DESIGNATION.COMPANY
        ? SiteDesignationType.company
        : SiteDesignationType.source;

    this.account
      .getSiteUrls()
      .pipe(
        takeUntil(this.cancelSubscriptions),
        tap((urls) => {
          this.urls = urls;
        }),
        switchMap((_) =>
          this.menuApiService.getCurrentMenu({ siteDesignationType: siteDes })
        ),
        checkStorage(this.menuService, this.document, this.cd)
      )
      .subscribe();
  }

  ngAfterViewInit() {
    if (!window.toolbar.visible) {
      this.account.updateMenuWrapperTooltipVisibility('hide');
    }
    this.menuService.updateSidenavDrilledNoAnimation(false);
  }

  setSiteDesignation(des: string) {
    des = des || this.siteDesignation;
    switch (des) {
      case SITES_DESIGNATION.ESS:
        this.isPayroll = false;
        this.isEss = true;
        this.isCompany = false;
        break;
      case SITES_DESIGNATION.COMPANY:
        this.isPayroll = false;
        this.isEss = false;
        this.isCompany = true;
        break;
      case SITES_DESIGNATION.PAYROLL:
      default:
        this.isPayroll = true;
        this.isEss = false;
        this.isCompany = false;
        break;
    }
  }

  ngOnDestroy() {
    this.menuService.updateSidenavDrilledNoAnimation(true);
    this.cancelSubscriptions.next();
  }

  goHome() {
    this.menuService.setMainMenuId(null);
    this.menuService.clearNavigationHistory();
    this.menuService.setSelectedItem(null);
    this.menuService.updateSidenavDrilled(false);
    this.menuService.clearActiveProductInfo();
    this.menuService.updateDisplayMenuItems(null);
  }

  private findResourceName(
    item: IMenuItem,
    search: string
  ): IApplicationResource {
    if (
      item &&
      item.resource &&
      item.resource.name &&
      item.resource.name.trim().toLowerCase() == search.trim().toLowerCase()
    ) {
      return item.resource;
    } else if (item.items) {
      let result: IApplicationResource;
      item.items.forEach((i) => {
        if (result) return;
        result = this.findResourceName(i, search);
      });
      return result;
    }
  }

  getParentById(id: number): IMenuItem {
    return this.menuService.get(id);
  }

  goBack(
    navHistory: NavHistoryItem[],
    currentMenuItems: IMenuItem[],
    allItems: IMenuItem[]
  ): void {
    this.menuService.updateSidenavDrilledNoAnimation(false);
    const targetNavItem =
      navHistory.length > 1 ? navHistory.splice(1)[0] : navHistory[0];
    const isMainMenuItem =
      allItems.find((a) => {
        const destId = targetNavItem != null ? targetNavItem.dest : 0;
        return a.menuItemId === destId;
      }) == null;

    // GOING HOME
    if (isMainMenuItem) {
      this.menuService.setMainMenuId(null);
      this.menuService.clearNavigationHistory();
      // this.menuService.setSelectedItem(null);
      this.menuService.clearActiveProductInfo();
      this.menuService.updateSidenavDrilled(false);
      this.menuService.updateSetResourceSelected(false);
      this.menuService.updateDisplayMenuItems(null);
    } else {
      const destItem = allItems.find(
        (a) => a.menuItemId === targetNavItem.dest
      );

      let displayItems =
        destItem && destItem.items && destItem.items.length > 0
          ? destItem.items
          : [];
      const displayItemIds = displayItems.map((d) => d.menuItemId);
      const currentItemIds = currentMenuItems.map((c) => c.menuItemId);

      if (this.isEqualArrays(currentItemIds, displayItemIds)) {
        const ancestor = allItems.find(
          (a) => a.menuItemId === destItem.parentId
        );
        displayItems =
          ancestor && ancestor.items && ancestor.items.length
            ? ancestor.items
            : null;
      }

      // We are moving up a step in the menu, but not changing out destination, so we basically
      // only want to change the way that the menu looks without changing the selected menu item
      if (!displayItems || !displayItems.length) {
        this.menuService.removeNavigationHistory();
        this.menuService.updateSidenavDrilled(false);
        this.menuService.updateDisplayMenuItems(null);
        this.menuService.clearActiveProductInfo();
      } else {
        this.menuService.removeNavigationHistory();
        this.menuService.updateDisplayMenuItems(displayItems);

        if (destItem.resource && destItem.resource.routeUrl != null) {
          this.menuService.setSelectedItem(destItem);
        }
      }
    }

    this.menuService.updateSidenavDrilledNoAnimation(true);
  }

  private findMainMenuItemByAncestor(
    selected: IMenuItem,
    items: IMenuItem[]
  ): IMenuItem {
    if (!selected) return null;
    if (selected.isMainMenuItem || !selected.parentId) return selected;

    const parent = items.find((i) => i.menuItemId === selected.parentId);

    if (parent.isMainMenuItem) {
      return parent;
    }

    return this.findMainMenuItemByAncestor(parent, items);
  }

  selectItem(item: IMenuItem, oldItem: IMenuItem, allItems: IMenuItem[]) {
    this.menuService.updateSidenavDrilledNoAnimation(false);

    // The menu item we were on before selecting the current item
    const previousItem = oldItem;

    let destItem = item || previousItem;

    // Update the selected Item in the ngrx store
    if (destItem.resource && destItem.resource.routeUrl) {
      this.menuService.updateSelectedItem(destItem);
    }

    const childNodes = allItems.filter((ai) => ai.parentId === item.menuItemId);

    if (childNodes && childNodes.length) {
      this.menuService.updateDisplayMenuItems(childNodes);
    }

    this.menuService.addNavigationHistory({
      source: destItem.parentId,
      dest: destItem.menuItemId,
      parent: destItem.parentId,
    });

    const mainMenuItem = this.findMainMenuItemByAncestor(destItem, allItems);

    if (mainMenuItem) this.menuService.setMainMenuId(mainMenuItem.menuItemId);
    this.menuService.updateSidenavDrilledNoAnimation(true);
  }

  /**
   * When a user clicks the top-level main menu items associated with a Dominion
   * product. This updates the menu and records the navigation step into the data store.
   */
  selectMainMenuItem(item: IMenuItem) {
    this.menuService.setMainMenuId(item.menuItemId);
    this.menuService.removeNavigationHistory();
    this.menuService.updateIsSidenavOpen(true);
    this.menuService.setSelectedItem(item);
    const childNodes = this.menuService.getChildNodes(item.menuItemId);

    if (childNodes && childNodes.length) {
      // Only open the submenu if there are items to display
      this.menuService.updateSidenavDrilled(true);
      this.menuService.updateDisplayMenuItems(childNodes);
    }

    if (item.sectionTitle && item.menuIcon) {
      this.menuService.updateActiveProductInfo(
        item.sectionTitle,
        item.menuIcon
      );
    }

    const navItem = {
      source: this.selectedItem != null ? this.selectedItem.menuItemId : null,
      dest: item.menuItemId,
      parent: item.parentId,
    } as NavHistoryItem;

    this.menuService.addNavigationHistory(navItem);
  }

  getFullyQualifiedUrl(item: IMenuItem, urls: ConfigUrl[]): string {
    if (
      !item ||
      !item.resource ||
      !item.resource.routeUrl ||
      !urls ||
      !urls.length
    )
      return "";
    let url = "";

    if (
      item.resource.applicationSourceType === ApplicationSourceType.CompanyWeb
    ) {
      return item.resource.routeUrl;
    } else if (
      item.resource.applicationSourceType === ApplicationSourceType.SourceWeb
    ) {
      url = urls.find((u) => u.siteType === ConfigUrlType.Payroll).url;
      url = url.charAt(url.length - 1) === "/" ? url : url + "/";
      return url + item.resource.routeUrl;
    } else if (
      item.resource.applicationSourceType === ApplicationSourceType.EssWeb
    ) {
      url = urls.find((u) => u.siteType === ConfigUrlType.Ess).url;
      url = url.charAt(url.length - 1) === "/" ? url : url + "/";
      return url + item.resource.routeUrl;
    }
  }

  private isEqualArrays(orig: any[], array: any[]): boolean {
    // if the other array is a falsy value, return
    if (!array) return false;

    // compare lengths - can save a lot of time
    if (orig.length !== array.length) return false;

    for (let i = 0, l = orig.length; i < l; i++) {
      // Check if we have nested arrays
      if (orig[i] instanceof Array && array[i] instanceof Array) {
        // recurse into the nested arrays
        if (!orig[i].equals(array[i])) return false;
      } else if (orig[i] != array[i]) {
        // Warning - two different object instances will never be equal: {x:20} != {x:20}
        return false;
      }
    }
    return true;
  }

  private initNav(items: IMenuItem[]) {
    let hasProductItemBeenSet = false;
    // Loop thru each menu item
    items.forEach((i) => {
      if (hasProductItemBeenSet) return;

      // If the menu item has sub menu items,
      // set the parent or the sub menu items
      // to the menu item we are on in the loop
      if (i.items) this.initNav(i.items);

      // Open menu and add active classes on page load
      if (
        i.resource &&
        i.resource.routeUrl &&
        !this.isPerformanceRelatedItem(i.menuItemId)
      ) {
        const parsed = UrlParts.ParseUrl(this.document.location.href);

        // Compare URL to resource URL. If they match, set item as active, traversing back up the tree to set parent active states
        // if (i.resource && i.resource.routeUrl != null && resourceUrl == pathname) {
        if (parsed.isEqualTo(i.resource.routeUrl)) {
          // this.setProductItem(i);
          hasProductItemBeenSet = true;
        }
      }
    });
  }

  private isPerformanceRelatedItem(itemId: number): boolean {
    let isPerformanceRelatedItem = false;

    const item = this.menuService.get(itemId);

    if (!item) return isPerformanceRelatedItem;

    isPerformanceRelatedItem = item.title
      .trim()
      .toLowerCase()
      .includes("performance");

    if (isPerformanceRelatedItem) return isPerformanceRelatedItem;

    if (item.parentId != null) {
      isPerformanceRelatedItem = this.isPerformanceRelatedItem(item.parentId);
    }

    return isPerformanceRelatedItem;
  }

  /**
   * Takes a recently changes menu item and determines whether to make it
   * active or inactive in the menu structure and then persists to our management store.
   */
  private updateActiveState() {
    // if (item.isActive) {
    //     this.menuService.addActiveState(item.menuItemId);
    // } else {
    //     this.menuService.removeActiveState(item.menuItemId);
    // }
  }

  private checkForActiveLink(items: IMenuItem[]): boolean {
    let hasActiveLink = false;

    items.forEach((item) => {
      if (hasActiveLink) return;

      if (item.items) {
        hasActiveLink = this.checkForActiveLink(item.items);
      }

      if (hasActiveLink) return;

      hasActiveLink = item.isActive;
    });

    return hasActiveLink;
  }

  /**
   * Sets product level icon and opens the menu. This is used when setting a child
   * menu item, it iterates back through the menu, selecting appropriate parents and
   * opening/closing menu when necessary.
   */
  // private setProductItem(item: IMenuItem) {
  //     if (!this.menuService.matchesSelectedItem(item)) {
  //         this.menuService.updateSelectedItem(item);
  //         this.menuService.updateSetResourceSelected(true);
  //         this.menuService.updateSelectedItem(item);
  //         this.menuService.updateSetResourceSelected(true);
  //     }

  //     if (!item) return;

  //     // set isActive to true for every ancestor the current URL has
  //     this.setAncestorsActive(item.menuItemId);

  //     if (item.parentId != null) {
  //         const parent = this.menuService.get(item.parentId);
  //         this.selectItem(parent);
  //         this.menuService.updateSidenavDrilledNoAnimation(true);
  //         this.menuService.updateSidenavDrilled(true);
  //     }
  // }

  // private setAncestorsActive(id: number) {
  //     const item = this.menuService.get(id);
  //     if (!item) return;

  //     item.isActive = true;

  //     this.updateActiveState(item);

  //     if (item.parentId != null) {
  //         this.setAncestorsActive(item.parentId);
  //     }

  //     if (item.resource && item.resource.routeUrl)
  //         this.menuApiService.setViewPermissions(item.resource.routeUrl);
  // }

  // Toggles menu side from 300px to 70px
  desktopMenuCollapse() {
    this.menuService.toggleSidenav();
  }

  private clearAllActiveStates(item: IMenuItem) {
    if (!item) return;
    item.isActive = false;

    this.updateActiveState();

    if (item.items && item.items.length) {
      item.items.forEach((i) => {
        this.clearAllActiveStates(i);
      });
    }
  }

  mapToDsNavItemOptions(data: any, item: IMenuItem) {
    return {
      configUrls: data.urls,
      navigationHistory: data.navHistory,
      menu: data.menu,
      item: item,
      selected: data.selected,
    } as DsNavMenuItemOptions;
  }

  mainNavExpanderEnd(event: any) {
    if (!event.toState) {
      const currentlySelectedMainMenuItem =
        this.menuService.getActiveMainMenuItem();
      if (currentlySelectedMainMenuItem)
        this.clearAllActiveStates(currentlySelectedMainMenuItem);
      // this.setProductItem(this.selectedUrlItem);
    }
  }
}

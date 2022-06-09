import { Component, OnInit } from '@angular/core';
import { IMenu, IMenuItem, AppConfigApiService, IApplicationResource } from '../shared';
import { NestedTreeControl } from '@angular/cdk/tree';
import { MatTreeNestedDataSource } from '@angular/material/tree';

class MenuVm implements IMenu {
    menuId: number;    name: string;
    items?: MenuItemVm[];
    selected = false;
    isDemo = false;

    constructor(json: IMenu) {
        Object.assign(this, json);

        this.items = this.items || [];
        this.items = this.items.map(i => new MenuItemVm(i));
    }
}

class MenuItemVm implements IMenuItem {
    menuItemId: number;
    title: string;
    index: number;
    resource?: IApplicationResource;
    items?: MenuItemVm[];
    hovered: boolean = false;

    constructor(json: IMenuItem) {
        Object.assign(this, json);

        this.items = this.items || [];
        this.items = this.items.map(i => new MenuItemVm(i));
    }
}

@Component({
    selector: 'ds-menu-builder',
    templateUrl: './menu-builder.component.html',
    styleUrls: ['./menu-builder.component.scss']
})
export class MenuBuilderComponent implements OnInit {

    static DEMO_MENUS: IMenu[] = [
        {
            menuId: -1,
            name: "Demo Menu #1",
            items: [
                { menuItemId: -100, title: "Item 1", index: 0, items: [
                    { menuItemId: -110, title: "Item 1.1", index: 0 },
                    { menuItemId: -120, title: "Item 1.2", index: 1 },
                    { menuItemId: -130, title: "Item 1.3", index: 2, items: [
                        { menuItemId: -131, title: "Item 1.3.1", index: 0 },
                        { menuItemId: -132, title: "Item 1.3.2", index: 1 },
                        { menuItemId: -133, title: "Item 1.3.3", index: 2 },
                    ]},
                ]},
                { menuItemId: -200, title: "Item 2", index: 1, items: [
                    { menuItemId: -210, title: "Item 2.1", index: 0 },
                    { menuItemId: -220, title: "Item 2.2", index: 1 },
                    { menuItemId: -230, title: "Item 2.3", index: 2 },
                ] },
                { menuItemId: -300, title: "Item 3", index: 2, items: [
                    { menuItemId: -310, title: "Item 3.1", index: 0 },
                    { menuItemId: -320, title: "Item 3.2", index: 1 },
                    { menuItemId: -330, title: "Item 3.3", index: 2, items: [
                        { menuItemId: -331, title: "Item 3.3.1", index: 0 },
                        { menuItemId: -332, title: "Item 3.3.2", index: 1 },
                        { menuItemId: -333, title: "Item 3.3.3", index: 2 },
                    ]},
                ] },
                { menuItemId: -400, title: "Item 4", index: 3 },
            ]
        }
    ];

    treeControl = new NestedTreeControl<MenuItemVm>(n => n.items);
    dataSource = new MatTreeNestedDataSource<MenuItemVm>();
    hasChild = (_: number, node: MenuItemVm) => !!node.items && node.items.length > 0;

    menus: MenuVm[] = [];
    menu: MenuVm;

    constructor(private _appSvc: AppConfigApiService) { }

    ngOnInit() {

        this.menus = MenuBuilderComponent.DEMO_MENUS.map(m => { let vm = new MenuVm(m); vm.isDemo = true; return vm; });

        this._appSvc.getMenus().subscribe(menus => {
            menus.map(m => new MenuVm(m)).forEach(m => this.menus.push(m));
        });
    }

    selectMenu(menu: MenuVm) {
        this.menus.forEach(m => {
            m.selected = m === menu;
        });
        this.menu = menu;
        this.dataSource.data = this.menu.items;
    }

    addMenu() {
        let menu = new MenuVm(<IMenu>{});
        this.menus.push(menu);
        this.selectMenu(menu);
    }

    addMenuItem(parent: MenuItemVm) {

    }

    saveMenu(menu: MenuVm) {
        if (menu.isDemo)
            return;

        this._appSvc.saveMenu(menu).subscribe(result => {
            console.log(result);
        });
    }

    drop($event) {
        console.log($event);
    }
}

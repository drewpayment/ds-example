export interface INavItem {
    menuItemId: number;
    name: string;
    icon?: string;
    index: number;
    url?: string;
    parentId?: number;
    parent?: INavItem;
    items?: INavItem[]
}
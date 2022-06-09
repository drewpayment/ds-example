import { IMenuItem } from './menu-item.model';

export interface IMenu {
    menuId: number;
    name: string;
    items?: IMenuItem[];
}
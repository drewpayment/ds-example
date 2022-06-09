import { IApplicationResource } from './application-resource.model';

export interface IMenuItem {
    sectionTitle?: string;
    menuItemId: number;
    title: string;
    index: number;
    resource?: IApplicationResource;
    items?: IMenuItem[];
    parent?: IMenuItem;
    parentId?: number;
    menuIcon?: string;
    isActive?: boolean;
    isHidden?: boolean;
    isAngularRoute?: boolean;
    isProductActive?: boolean;
    isMainMenuItem?: boolean;
}

export interface IMenuProductInfo {
    productTitle: string; 
    productIcon: string;
}

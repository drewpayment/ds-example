import { ConfigUrl } from './config-url.model';
import { NavHistoryItem } from '.';
import { IMenu, IMenuItem } from '../app-config';


export interface DsNavMenuItemOptions {
    configUrls: ConfigUrl[];
    navigationHistory: NavHistoryItem[];
    menu: IMenu;
    
    /**
     * UI item that we are building for the menu.
     */
    item: IMenuItem;
    
    /**
     * Currently selected item in the menu strcuture.
     */
    selected: IMenuItem;
}

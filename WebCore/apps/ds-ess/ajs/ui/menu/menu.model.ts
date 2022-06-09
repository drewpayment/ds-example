import * as angular from "angular";

export interface IMenuItem {
    title:string;
    linkToState:string;
    linkToHref:string;
    activeState?:string;
    authorized?:boolean;
}

/**
 * Object representing a single menu item within a specified menu.
 *
 * @class
 */
export class MenuItem implements IMenuItem {
    title = '';
    linkToState = '';
    linkToHref = '';
    activeState = '';
    authorized = true;

    constructor(config?: IMenuItem) {        
        angular.extend(this, config);
    }
}


/**
 * Menu object constructor that can be 'new-ed' to create new menu instances.
 * 
 * @class
 */
export class Menu {
    items: IMenuItem[] = [];

    constructor(){
    }

    /**
     * Adds a menu-item to the current Menu object with the specified settings provided in the item configuration.
     *
     * @name #addItemFromOptions
     * @methodOf Menu
     */
    addItemFromOptions(itemConfig:IMenuItem) {
        this.items.push(new MenuItem(itemConfig));
    };


    /**
     * Adds a menu-item to the current Menu object with the specified settings provided in the item configuration.
     * Only if the property 'authorized' is TRUE
     *
     * @name #addItemFromOptions
     * @methodOf Menu
     */
   addItemFromOptionsOnlyIfAllowed(itemConfig:IMenuItem) {
        var item = new MenuItem(itemConfig);

        if (item.authorized)
            this.items.push(item);
    };

    /**
     * Clears out the items array. This is the array that holds all the menu item data.
     *
     * @name #clearMenuItems
     * @methodOf Menu
     */
    clearMenuItems () {
        this.items = [];
    };
}
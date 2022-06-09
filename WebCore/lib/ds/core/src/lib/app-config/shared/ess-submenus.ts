import { IMenuItem, ApplicationSourceType, IApplicationResource } from '.';

/**
 * This is the hardcoded menu items that we will use to build the left-hand menu structure on ESS. 
 * In order to use this, you must provide the parent properties at run-time.
 */
export const Ess_Profile_Submenu: IMenuItem[] = [
    {
        index: 0,
        menuItemId: 1,
        title: 'Profile',
        isActive: false,
        isAngularRoute: true,
        isHidden: false,
        isProductActive: true,
        parent: null,
        resource: {
            applicationSourceType: ApplicationSourceType.EssWeb,
            name: 'Profile',
            routeUrl: 'profile'
        } as IApplicationResource
    },
    {
        index: 1,
        menuItemId: 2,
        title: 'Paycheck Settings',
        isActive: false,
        isAngularRoute: true,
        isHidden: false,
        isProductActive: true,
        parent: null,
        resource: {
            applicationSourceType: ApplicationSourceType.EssWeb,
            name: 'Paycheck Settings',
            routeUrl: 'pay'
        } as IApplicationResource
    },
    {
        index: 2,
        menuItemId: 3,
        title: 'Time Off',
        isActive: false,
        isAngularRoute: true,
        isHidden: false,
        isProductActive: true,
        parent: null,
        resource: {
            applicationSourceType: ApplicationSourceType.EssWeb,
            name: 'Time Off',
            routeUrl: 'timeoff'
        } as IApplicationResource
    },
    {
        index: 3,
        menuItemId: 4,
        title: 'Account Settings',
        isActive: false,
        isAngularRoute: true,
        isHidden: false,
        isProductActive: true,
        parent: null,
        resource: {
            applicationSourceType: ApplicationSourceType.EssWeb,
            name: 'Account Settings',
            routeUrl: 'account-settings'
        } as IApplicationResource
    },
    {
        index: 4,
        menuItemId: 5,
        title: 'Resources',
        isActive: false,
        isAngularRoute: true,
        isHidden: false,
        isProductActive: true,
        parent: null,
        resource: {
            applicationSourceType: ApplicationSourceType.EssWeb,
            name: 'Resources',
            routeUrl: 'resources'
        } as IApplicationResource
    },
    {
        index: 5,
        menuItemId: 6,
        title: 'Notifications',
        isActive: false,
        isAngularRoute: false,
        isHidden: false,
        isProductActive: true,
        parent: null,
        resource: {
            applicationSourceType: ApplicationSourceType.SourceWeb,
            name: 'Notifications',
            routeUrl: ''
        } as IApplicationResource
    },
];

export const Ess_Performance_Submenu: IMenuItem[] = [
    {
        index: 0,
        menuItemId: 1,
        title: 'Overview',
        isActive: false,
        isAngularRoute: true,
        isHidden: false,
        isProductActive: true,
        parent: null,
        resource: {
            applicationSourceType: ApplicationSourceType.EssWeb,
            name: 'Overview',
            routeUrl: 'performance'
        } as IApplicationResource
    },
    {
        index: 1,
        menuItemId: 2,
        title: 'Reviews',
        isActive: false,
        isAngularRoute: true,
        isHidden: false,
        isProductActive: true,
        parent: null,
        resource: {
            applicationSourceType: ApplicationSourceType.EssWeb,
            name: 'Reviews',
            routeUrl: 'performance/reviews'
        } as IApplicationResource
    },
    {
        index: 2,
        menuItemId: 3,
        title: 'Goals',
        isActive: false,
        isAngularRoute: true,
        isHidden: false,
        isProductActive: true,
        parent: null,
        resource: {
            applicationSourceType: ApplicationSourceType.EssWeb,
            name: 'Goals',
            routeUrl: 'performance/goals'
        } as IApplicationResource
    },
    {
        index: 3,
        menuItemId: 4,
        title: 'Company Goals',
        isActive: false,
        isAngularRoute: true,
        isHidden: false,
        isProductActive: true,
        parent: null,
        resource: {
            applicationSourceType: ApplicationSourceType.EssWeb,
            name: 'Company Goals',
            routeUrl: 'performance/company-goals'
        } as IApplicationResource
    }
];

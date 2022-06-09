import { downgradeComponent } from '@angular/upgrade/static';
import { SettingsOutletComponent } from '../src/app/pay-settings/settings-outlet/settings-outlet.component';
import { AccountSettingsOutletComponent } from '../src/app/account-settings/account-settings-outlet/account-settings-outlet.component';
import { ResourcesListOutletComponent } from '../src/app/resources/resources-list-outlet/resources-list-outlet.component';
import { ProfileOutletComponent } from '../src/app/profile/profile-outlet/profile-outlet.component';
import { NgxLinkComponent } from '@ds/core/ngx-downgrades/ngx-link/ngx-link.component';
import { MenuWrapperToggleComponent } from '@ds/core/users/beta-features/menu-wrapper-toggle/menu-wrapper-toggle.component';
import { PerformanceComponent } from '../src/app/performance/performance.component';
import { SidebarComponent } from '../src/app/sidebar/sidebar.component';
import { LoadingMessageComponent } from '@ds/core/ui/loading-message/loading-message.component';
import { AvatarComponent } from '@ds/core/ui/avatar/avatar.component';

declare var angular: ng.IAngularStatic;


export module DowngradeNgxModule {
    export const MODULE_NAME = 'DowngradeNgxModule';
    export const MODULE_DEPENDENCIES = [];

    export const AjsModule = angular.module(MODULE_NAME, MODULE_DEPENDENCIES);

    AjsModule
        .directive(
            'dsSettingsOutlet',
            downgradeComponent({ component: SettingsOutletComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsAccountSettingsOutlet',
            downgradeComponent({ component: AccountSettingsOutletComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsResourcesListOutlet',
            downgradeComponent({ component: ResourcesListOutletComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsProfileOutlet',
            downgradeComponent({ component: ProfileOutletComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsPerformance',
            downgradeComponent({ component: PerformanceComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'essSidebar',
            downgradeComponent({ component: SidebarComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'ngxLink',
            downgradeComponent({ component: NgxLinkComponent }) as angular.IDirectiveFactory
        )
        .directive(
            'dsLoadingMessage',
            downgradeComponent({ component: LoadingMessageComponent }) as ng.IDirectiveFactory
        )
        .directive(
            'dsMenuWrapperToggle',
            downgradeComponent({ component: MenuWrapperToggleComponent }) as ng.IDirectiveFactory
        ).directive(
          'dsAvatar',
          downgradeComponent({ component: AvatarComponent }) as ng.IDirectiveFactory
        );
}

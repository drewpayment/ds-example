import * as angular from 'angular';
import { AccountService } from '@ajs/core/account/account.service';
import { DsStateService } from '@ajs/core/ds-state/ds-state.service';
import { Menu } from '../../ui/menu/menu.model';
import { STATES } from '../../shared/state-router-info';

/**
 * Controller used to generate the main sidebar used in the ESS application.
 *
 * @ngcontroller
 * @name ds.ess.common~MainSidebarController
 * @requires $scope Angular.scope
 * @requires Menu ds.ess.ui:Menu (see Menu.js)
 */
export class MainSidebarController {
    static readonly CONTROLLER_NAME = 'MainSidebarController';
    static readonly CONFIG = {
        template: require('./main-sidebar.html')
    };
    static readonly $inject = [
        '$rootScope',
        '$scope',
        AccountService.SERVICE_NAME,
        DsStateService.SERVICE_NAME
    ];

    constructor($rootScope: ng.IRootScopeService, $scope: any, accountSvc: AccountService, state: DsStateService) {
        $scope.menu = new Menu();
        $scope.hideMenu = true;

        function init() {
            updateMenu(state.router.current.data.menuName);
        }
        init();

        // ----------------------------------------------
        // This will setup the menu items based on rules
        // from the actions allowed for the user that
        // is currently logged in.
        //
        // See the Menu.addItemFromOptionsOnlyIfAllowed call
        // It used a property 'authorized' that should be bool
        // value. 
        //
        // If the menu item has no restriction there is no need
        // to set the 'authorized value. 
        //
        // If there are restrictions you must provide a function 
        // that checks the allowed actions against allowed actions
        // determined needed to show the menu item.
        //
        // example: add something like this in the promise area
        // below with all the other menu item definitions
        //
        // function AllowTaxMenuItem(data){
        //    //data is an array of allowed actions
        //    //check that the allowed actions exist
        //    //that you have determined are needed
        // }
        //
        // $scope.menu.addItemFromOptionsOnlyIfAllowed({ 
        //  url: 'Profile', 
        //  title: 'Profile', 
        //  activeState: 'ess.profile',
        //  authorized: AllowTaxMenuItem(data));
        // ----------------------------------------------
        function updateMenu(menuName) {

            accountSvc.getAllowedActions().then(function () {

                if (menuName == STATES.ds.ess.MENU_NAME) {
                    $scope.menu.addItemFromOptionsOnlyIfAllowed({ linkToState: 'ess.profile', title: 'Profile', activeState: 'ess.profile', authorized: true });
                    $scope.menu.addItemFromOptionsOnlyIfAllowed({ linkToState: 'ess.pay', title: 'Paycheck Settings', activeState: 'ess.pay', authorized: true });
                    $scope.menu.addItemFromOptionsOnlyIfAllowed({ linkToState: 'ess.leave.timeoff', title: 'Time Off', activeState: 'ess.leave.timeoff' });
                    // $scope.menu.addItemFromOptionsOnlyIfAllowed({ linkToState: 'ess.account', title: 'Account Settings', activeState: 'ess.account' });
                    $scope.menu.addItemFromOptionsOnlyIfAllowed({ linkToState: 'ess.account', title: 'Account Settings', activeState: 'ess.account' });
                    $scope.menu.addItemFromOptionsOnlyIfAllowed({ linkToState: 'ess.resources', title: 'Resources', activeState: 'ess.resources' });
                    $scope.menu.addItemFromOptionsOnlyIfAllowed({ linkToState: 'ess.consents', title: 'Electronic Consents', activeState: 'ess.consents' });
                    $scope.menu.addItemFromOptionsOnlyIfAllowed({ linkToHref: 'Legacy/ContactNotificationPreferences.aspx', title: 'Notifications', activeState: 'not.valid' });
                }

                updateActiveMenuItem();
            });
        }

        function updateActiveMenuItem() {
            angular.forEach($scope.menu.items, function (item) {
                item.isActive = state.router.includes(item.activeState);
            });
        }

        // ----------------------------------------------
        // fires when ever there is a state change.
        // ----------------------------------------------
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            let setMenu = fromState.data ||
                toState.data.menuName != fromState.data.menuName;

            if (setMenu) {
                (<Menu>$scope.menu).clearMenuItems();
                updateMenu(toState.data.menuName);
            }
        });

        $rootScope.$on('$stateChangeSuccess', updateActiveMenuItem);
    }
}

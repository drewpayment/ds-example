import * as angular from "angular";
import { STATES } from "../../shared/state-router-info";
import { AccountService } from "@ajs/core/account/account.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsStyleLoaderService } from "@ajs/ui/ds-styles/ds-styles.service";
import { IUserInfo } from "@ajs/user";
import { LastFirstMid } from "@util/nameUtilities";
import { MenuService } from "@ds/core/app-config/shared/menu.service";

export class EssHeaderController {
  static readonly CONTROLLER_NAME = "HeaderController";
  static readonly CONFIG = {
    template: require("./header.html"),
  };
  static readonly $inject = [
    "$rootScope",
    "$scope",
    "$state",
    AccountService.SERVICE_NAME,
    DsMsgService.SERVICE_NAME,
    DsStyleLoaderService.SERVICE_NAME,
    MenuService.AJS_SERVICE_NAME,
  ];

  constructor(
    $rootScope: ng.IRootScopeService,
    $scope: any,
    $state,
    accountSvc: AccountService,
    msg: DsMsgService,
    styles: DsStyleLoaderService,
    ngxMenuService: MenuService
  ) {
    $scope.user = {};
    $scope.legacyHomepageLink = { url: "/" };
    $scope.legacyHeaderLinks = [];
    $scope.layout = { useDefault: true };
    $scope.redirectToGoals = false;

    /**
     * Initialize the controller. Load user info and header links onto $scope to be rendered.
     * @private
     * @returns {}
     */
    function init(user: IUserInfo) {
      let lastClientName =
        user.lastClientName == null || user.lastClientId === undefined
          ? user.clientName
          : user.lastClientName;

      $scope.user.firstLastName = LastFirstMid(
        user.firstName,
        user.middleInitial,
        user.lastName
      );
      $scope.user.clientName = lastClientName;
      $scope.user.$isEmulating = user.$isEmulating;

      if (user.$isEmulating) {
        $scope.user.$emulationStyle = {
          "background-color": "teal",
          color: "white",
        };
      }

      // get action types for the user
      accountSvc.getAllowedActions().then((permissions) => {
        let isPerformanceEnabled = false;
        let isGoalTrackingEnabled = false;

        permissions.forEach((p) => {
          if (p.toLowerCase() == "performance.readreviews") {
            isPerformanceEnabled = true;
          } else if (p.toLowerCase() == "employeegoal.read") {
            isGoalTrackingEnabled = true;
          }
        });

        $scope.redirectToGoals = !isPerformanceEnabled && isGoalTrackingEnabled;
      });

      // get links to the legacy system to render in the header
      accountSvc
        .getAccessibleLegacyLinks()
        .then(linksLoaded)
        ["catch"](msg.showWebApiException);

      styles.registerStyleChangeHandler(function (src, isDefault) {
        $scope.layout.useDefault = isDefault;
      });
    }
    accountSvc.getUserInfo().then((user) => init(user));

    /**
     * Loads the legacy links onto the scope to be displayed in the header.
     *
     * @function
     * @private
     */
    function linksLoaded(links) {
      let curState = $state.current;

      // clear any previously loaded links
      $scope.legacyHeaderLinks = []; // Don't display header menus.
      let linkArr = [];

      angular.forEach(links, function (link) {
        // grab the homepage link
        if (link.isHomepage) $scope.legacyHomepageLink = link;

        // add header link
        if (link.isHeaderLink) linkArr.push(link);

        if (curState.data) {
          setLinksActiveState(link, curState);
        }
      });

      $scope.legacyHeaderLinks = linkArr;
    }

    function setLinksActiveState(link, curState) {
      if (curState.data.menuName == link.name) {
        link.isActive = true;
      } else {
        link.isActive = false;
      }
    }

    $scope.clearLocalStorage = function() {
      //console.log('LOG OUT');
      if (localStorage) localStorage.clear();
    }

    $scope.shouldUpdateNgxMenu = false;

    $scope.updateNgxMenu = function () {
      $scope.shouldUpdateNgxMenu = true;
    };

    // ----------------------------------------------
    // fires when ever there is a state change.
    // ----------------------------------------------
    $rootScope.$on(
      "$stateChangeStart",
      function (event, toState, toParams, fromState, fromParams) {
        let setMenu =
          fromState.data || toState.data.menuName != fromState.data.menuName;

        if ($scope.shouldUpdateNgxMenu) {
          ngxMenuService.selectMenuItemFromAngularJs(toState);
          $scope.shouldUpdateNgxMenu = false;
        }

        if (setMenu) {
          let linkArr = [];

          if ($state.current) {
            // set the active flag based on the current state(url)
            angular.forEach($scope.legacyHeaderLinks, function (link) {
              setLinksActiveState(link, toState);
              linkArr.push(link);
            });

            // update the links array so all the links are updated
            $scope.legacyHeaderLinks = linkArr;
          }
        }
      }
    );
  }
}

import * as angular from "angular";
import { AccountService } from "@ajs/core/account/account.service";
import { DsPopupService } from "@ajs/ui/popup/ds-popup.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { LastFirstMid } from "@util/nameUtilities";
import { isNotUndefinedOrNull, isUndefinedOrNull } from "@util/ds-common";

export class HeaderController {
  static readonly $inject = [
    "$rootScope",
    "$scope",
    "$state",
    AccountService.SERVICE_NAME,
    DsPopupService.SERVICE_NAME,
    DsMsgService.SERVICE_NAME,
  ];
  static readonly SELECTOR = "AjsCompanyHeader";

  constructor($rootScope, $scope, $state, AccountService, popup, msg) {
    $scope.user = {};
    $scope.legacyHomepageLink = { url: "/" };
    $scope.legacyHeaderLinks = [];
    $scope.showReportQueue = showReportQueue;

    // use service to get current user info
    AccountService.getUserInfo().then(
      getCurrentUserSuccessCallback,
      getCurrentUserErrorCallback
    );

    // get links to the legacy system to render in the header
    AccountService.getAccessibleLegacyLinks()
      .then(linksLoaded)
      ["catch"](msg.xhrMessageHandler);

    /**
     * Handler for the current user data returned from the service. Saves key user info to the
     * scope to be used in the view.
     *
     * @function
     * @private
     */
    function getCurrentUserSuccessCallback(user) {
      $scope.user.firstLastName = user.lastEmployeeId
        ? LastFirstMid(
            user.lastEmployeeFirstName,
            user.lastEmployeeMiddleInitial,
            user.lastEmployeeLastName
          )
        : " ";
      //LastFirstMid(user.firstName, user.middleInitial, user.lastName
      $scope.user.clientName = user.lastClientId
        ? user.lastClientName
        : user.clientName;
    }

    /**
     * Called when errors occur during the current user service call.  Clears user data and passes the errors
     * to the message service to be displayed.
     *
     * @function
     * @private
     */
    function getCurrentUserErrorCallback(data) {
      clearUser();
      msg.xhrMessageHandler(data);
    }

    /**
     * Clears the user info.
     *
     * @function
     * @private
     */
    function clearUser() {
      $scope.user = {};
    }

    /**
     * Loads the legacy links onto the scope to be displayed in the header.
     *
     * @function
     * @private
     */
    function linksLoaded(links) {
      var curState = $state.current;

      // clear any previously loaded links
      $scope.legacyHeaderLinks = [];
      var linkArr = [];

      angular.forEach(links, function (link) {
        // grab the homepage link
        if (link.isHomepage) $scope.legacyHomepageLink = link;

        // add header link
        if (link.isHeaderLink) linkArr.push(link);

        if (isNotUndefinedOrNull(curState.data)) {
          setLinksActiveState(link, curState);
        }
      });

      $scope.legacyHeaderLinks = linkArr;
    }

    function setLinksActiveState(link, curState) {
      if (curState.data && curState.data.menuName == link.name) {
        //console.log('SETTING ACTIVE: ' + link.name);
        link.isActive = true;
      } else {
        //console.log('SETTING NOT ACTIVE: ' + link.name);
        link.isActive = false;
      }
    }

    //----------------------------------------------
    //fires when ever there is a state change.
    //----------------------------------------------
    $rootScope.$on(
      "$stateChangeStart",
      function (event, toState, toParams, fromState, fromParams) {
        var setMenu =
          isUndefinedOrNull(fromState.data) ||
          toState.data.menuName != fromState.data.menuName;

        if (setMenu) {
          var linkArr = [];
          //console.log('$stateChangeStart FUNCTION');
          //console.log(toState);

          if (isNotUndefinedOrNull($state.current)) {
            //set the active flag based on the current state(url)
            angular.forEach($scope.legacyHeaderLinks, function (link) {
              setLinksActiveState(link, toState);
              linkArr.push(link);
            });

            //update the links array so all the links are updated
            //console.log('ACTIVATE SELECTED LINK FUNCTION: SETTING LEGACY LINKS ARRAY');
            //console.log(linkArr);
            $scope.legacyHeaderLinks = linkArr;
          }
        }
      }
    );

    /**
     *
     */
    function showReportQueue() {
      popup.open("Legacy/ReportQueuePopup.aspx", "reportQueue", {
        width: 670,
        height: 450,
      });
    }

    $scope.clearLocalStorage = function() {
      if (localStorage) localStorage.clear();
    }
  }
}

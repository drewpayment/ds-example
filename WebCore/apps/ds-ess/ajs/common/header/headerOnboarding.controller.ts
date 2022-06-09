import * as angular from "angular";
import { STATES } from "../../shared/state-router-info";
import { IUserInfo } from "@ajs/user";
import { LastFirstMid } from "@util/nameUtilities";
import { AccountService } from "@ajs/core/account/account.service";

export class EssOnboardingHeaderController {
  static readonly CONTROLLER_NAME = "HeaderOnboardingController";
  static readonly CONFIG = {
    template: require("./headerOnboarding.html"),
  };
  static readonly $inject = [
    "$rootScope",
    "$scope",
    "$state",
    AccountService.SERVICE_NAME,
  ];

  constructor(
    $rootScope: ng.IRootScopeService,
    $scope: any,
    $state,
    accountService: AccountService
  ) {
    $scope.user = {};
    $scope.legacyHomepageLink = { url: "/" };
    $scope.legacyHeaderLinks = [];
    $scope.isOnboarding = false;

    $scope.curState = $state.current.name;

    function init(user: IUserInfo) {
      $scope.user.firstLastName = LastFirstMid(
        user.firstName,
        user.middleInitial,
        user.lastName
      );
      $scope.user.clientName = user.clientName;
      $scope.user.$isEmulating = user.$isEmulating;
      if (user.$isEmulating) {
        $scope.user.$emulationStyle = {
          "background-color": "teal",
          color: "white",
        };
      }
    }

    accountService.getUserInfo().then((user) => init(user));

    function setLinksActiveState(link, curState) {
      if (curState.data.menuName == link.name) {
        //console.log('SETTING ACTIVE: ' + link.name);
        link.isActive = true;
      } else {
        //console.log('SETTING NOT ACTIVE: ' + link.name);
        link.isActive = false;
      }
    }

    $scope.clearLocalStorage = function() {
      if (localStorage) localStorage.clear();
    }

    //----------------------------------------------
    //fires when ever there is a state change.
    //----------------------------------------------
    $rootScope.$on(
      "$stateChangeStart",
      function (event, toState, toParams, fromState, fromParams) {
        var setMenu =
          fromState.data || toState.data.menuName != fromState.data.menuName;

        if (setMenu) {
          var linkArr = [];
          //console.log('$stateChangeStart FUNCTION');
          //console.log(toState);

          if ($state.current) {
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
  }
}

export class EssOnboardingHeaderComponent {
  static readonly COMPONENT_NAME = "ajsEssOnboardingHeader";
  static readonly COMPONENT_OPTIONS: ng.IComponentOptions = {
    controller: EssOnboardingHeaderController,
    template: require("./headerOnboarding.html"),
  };
}

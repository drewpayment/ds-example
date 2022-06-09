import { STATES } from "../../shared/state-router-info";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import { IUserInfo } from "@ajs/user";
import { AccountService } from '@ajs/core/account/account.service';

export class OnboardingEEOCAddController {
    static readonly $inject = [
        "$scope",
        AccountService.SERVICE_NAME,
        "$location",
        DsMsgService.SERVICE_NAME,
        DsNavigationService.SERVICE_NAME
    ];

    constructor ($scope, accountService: AccountService, $location, MessageService, DsNavigationService: DsNavigationService) {
        $scope.nav = DsNavigationService;

        $scope.hasError = false;
        $scope.isLoading = true;
        $scope.isLoaded = false;
        $scope.userData = {};

        function init(userAccount) {
            $scope.userData = userAccount;

            $scope.isLoading = false;
            $scope.isLoaded = true;
        }
        accountService.getUserInfo().then(user => init(user));
    }
}

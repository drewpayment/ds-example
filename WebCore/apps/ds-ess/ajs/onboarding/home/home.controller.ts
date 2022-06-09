import { STATES } from "../../shared/state-router-info";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { isUndefinedOrNullOrEmptyString } from "@util/ds-common";
import * as moment from "moment";
import { AccountService } from '@ajs/core/account/account.service';

export class OnboardingHomeController {
    static readonly $inject = [
        "$scope",
        AccountService.SERVICE_NAME,
        DsNavigationService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME
    ];

    constructor ($scope, accountService, DsNavigationService: DsNavigationService, DsOnboardingApi: DsOnboardingEmployeeApiService) {
        $scope.nav = DsNavigationService;

        $scope.hasError = false;
        $scope.isLoading = true;
        $scope.isLoaded = false;
        $scope.userData = {};
        $scope.welcomeMessage = "";

        function init(userAccount) {
            $scope.userData = userAccount;

            DsOnboardingApi.getEmployeeFinalizeStatus(userAccount.employeeId).then(function (finStatus) {
                if (finStatus.data.isFinalizeComplete === true) {
                    //Redirect to finalize page
                    $scope.nav.gotoRoute('onboarding/finalize', null);
                } else {
                    DsOnboardingApi.getEmployeeInfo(userAccount.employeeId).then(function (data) {
                        $scope.employeeContactInformationData = data;
                        $scope.isLoading = false;
                        $scope.isLoaded = true;
                        DsOnboardingApi.getClientEssOptions(userAccount.clientId).then(function (essOptions) {
                            if (essOptions.isCustomMessage)
                                $scope.welcomeMessage = replaceWildCards(essOptions.welcomeMessage);
                        });
                    });
                }
            });
        }
        accountService.getUserInfo().then(user => init(user));

        function replaceWildCards(message:string){
            var currDate = new Date();
            var startDate = 'date of hire';
            if($scope.employeeContactInformationData.hireDate)
                startDate = moment($scope.employeeContactInformationData.hireDate).format("M/D/YYYY");

            message = message.replace(/\{\*EmployeeName\}/g, $scope.employeeContactInformationData.firstName + ' ' +
                $scope.employeeContactInformationData.lastName);
            message = message.replace(/\{\*Date\}/g, moment(currDate).format("M/D/YYYY"));
            message = message.replace(/\{\*CompanyName\}/g, $scope.userData.clientName || $scope.userData.lastClientName );
            message = message.replace(/\{\*StartDate\}/g, startDate);
            return message;
        }

        $scope.StartOnboarding = function () {
            DsOnboardingApi.employeeStartsOnboarding($scope.userData.employeeId).then(function (finData) {
                // Redirect to Contactinfo page
                $scope.nav.gotoRoute('onboarding/contactinfo', null);
            });
        };
    }
}

import * as angular from "angular";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsOnboardingEmployeeApiService } from "@ajs/onboarding/shared/employee-api.service";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import { IUserInfo } from "@ajs/user";
import { STATES } from "../../shared/state-router-info";

export class EmployeePersonalInfoEditController {
    static $inject = [
        '$scope',
        DsMsgService.SERVICE_NAME,
        DsOnboardingEmployeeApiService.SERVICE_NAME,
        'pageData',
        DsNavigationService.SERVICE_NAME,
        'userAccount'
    ];

    constructor ($scope, MessageService: DsMsgService, dsOnboardingApi: DsOnboardingEmployeeApiService, pageData, DsNavigationService: DsNavigationService, userAccount: IUserInfo) {
        
        $scope.employeePersonalInformationData = angular.copy(pageData.employeePersonalInformationData);

        function init() {
            $scope.userData = userAccount;
        }
        init();
        
        $scope.updatePersonalInformation = function() {
            MessageService.sending(true);
            dsOnboardingApi.putEmployeePersonalInfo($scope.employeePersonalInformationData)
                .then(successfulSave)
                .catch(MessageService.showWebApiException);
        };

        function successfulSave() {
            DsNavigationService.gotoRouteThenShowMessage(STATES.ds.ess.MENU_NAME, "Your changes have been saved.", MessageService.messageTypes.success);
        }
        
        $scope.cancelEdit = function () {
            DsNavigationService.gotoProfile();
        };

    }
}

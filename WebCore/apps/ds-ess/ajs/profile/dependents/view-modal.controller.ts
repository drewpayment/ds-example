import * as angular from "angular";
import * as nameUtil from "@util/nameUtilities";
import * as permUtil from "@util/permissionsUtility";
import * as dateUtil from "@util/dateUtilities";
import { DsNavigationService } from "../../ui/nav/ds-navigation.service";

/**
 * Controller for modal dialog dependents view.
 *
 * @name ds.ess.profile:dependentViewModalController
 *
 * @param $scope              - angular scope used to manipulate objects from the view
 * @param $modalInstance      - ui-bootstrap modal object used to control the modal state
 * @param modalData           - dependent data that is resolved and passed into the controller before the modal is loaded
 * @param $filter             - used to fix date formatting before calculating age (for dependent birth dates)
 * @param DsNavigationService - used to redirect to edit view 
 */
export class DependentViewModalController {
    static CONTROLLER_NAME = 'DependentViewModalController';

    static readonly $inject = [
        '$scope',
        '$modalInstance',
        'modalData',
        '$filter',
        DsNavigationService.SERVICE_NAME
    ];

    constructor($scope, $modalInstance, modalData, $filter, dsNavigationService: DsNavigationService) {

        // ------------------------------------------------------------------------------------------
        // CONTROLLER & MODAL STATE INITIALIZATION
        // ------------------------------------------------------------------------------------------
        (function initModalState() {
            $scope.dependents = modalData.dependents;

            // mark all dependents as inactive in the carousel 
            angular.forEach($scope.dependents, function(dep) {
                dep.active = false;
            });

            // mark the selected dependent as active in the carousel
            modalData.selected.active = true;
        })();

        // ------------------------------------------------------------------------------------------
        // VIEW HELPERS
        // ------------------------------------------------------------------------------------------
        $scope.calculateAge = function (birthDate) {
            var date = $filter('date')(birthDate, 'MM/dd/yyyy');
            //console.log('CALCULATE AGE');
            return dateUtil.calculateBirthdateWithText(date, false);
        };

        $scope.hasEditPermission = function (dep) {
            return permUtil.userHasEditPermission(dep.editPermission);
        };

        $scope.getFirstMidLastName = function (contact) {
            return nameUtil.FirstMidLast(contact.firstName, contact.middleInitial, contact.lastName);
        };

        // ssn mask handlers
        $scope.enableMaskSelection = function (dep, enabled) {
            var canShow = dep.maskedSocialSecurityNumber !== dep.unmaskedSocialSecurityNumber;
            dep.showMaskSelection = canShow && enabled;
            dep.showUnmaskedSsn = false;
        };

        $scope.showUnmaskedSsn = function (dep, show) {
            dep.showUnmaskedSsn = show;
        };

        $scope.getSsn = function (dep) {
            if (dep.showUnmaskedSsn)
                return dep.unmaskedSocialSecurityNumber;

            return dep.maskedSocialSecurityNumber;
        };


        // ------------------------------------------------------------------------------------------
        // MODAL NAVIGATION
        // ------------------------------------------------------------------------------------------
        $scope.close = function () {
            $modalInstance.dismiss();
        };

        $scope.edit = function(dep) {
            $modalInstance.dismiss();
            dsNavigationService.gotoRoute("Dependents/Edit", dep);
        };
    }
}
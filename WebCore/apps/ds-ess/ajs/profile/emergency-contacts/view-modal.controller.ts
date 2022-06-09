import { DsNavigationService } from "../../ui/nav/ds-navigation.service";
import * as angular from "angular";
import { userHasEditPermission } from "@util/permissionsUtility";
import { FirstMidLast } from "@util/nameUtilities";

/**
 * Controller for modal dialog emergency contact view.
 *
 * @name Ess.Controllers:emergencyContactViewModalController
 *
 * @param $scope              - angular scope used to manipulate objects from the view
 * @param $modalInstance      - ui-bootstrap modal object used to control the modal state
 * @param modalData           - emergency contact data that is resolved and passed into the controller before the modal is loaded
 * @param DsNavigationService - used to redirect to edit view 
 */
export class EmergencyContactViewModalController {
    static CONTROLLER_NAME = 'EmergencyContactViewModalController';

    static readonly $inject = [
        '$scope',
        '$modalInstance',
        'modalData',
        DsNavigationService.SERVICE_NAME
    ];

    constructor ($scope, $modalInstance, modalData, dsNavigationService: DsNavigationService) {

        // ------------------------------------------------------------------------------------------
        // CONTROLLER & MODAL STATE INITIALIZATION
        // ------------------------------------------------------------------------------------------
        (function initModalState() {
            $scope.emergencyContacts = modalData.emergencyContacts;

            // mark all emergency contacts as inactive in the carousel 
            angular.forEach($scope.emergencyContacts, function (contact) {
                contact.active = false;
            });

            // mark the selected emergency contact as active in the carousel
            modalData.selected.active = true;
        })();

        // ------------------------------------------------------------------------------------------
        // VIEW HELPERS
        // ------------------------------------------------------------------------------------------
        $scope.hasEditPermission = function (contact) {
            return userHasEditPermission(contact.editPermission);
        };

        $scope.getFirstMidLastName = function (contact) {
            return FirstMidLast(contact.firstName, contact.middleInitial, contact.lastName);
        };
        //addressLine2 is not often filled in, should'nt determine visibility. 
        //Also field is nullable and throwing errors when "trim"ing
        $scope.hasAddress = function(contact) {
            return !(contact.addressLine1.trim() === ''
                //&& contact.addressLine2.trim() === ''  
                && contact.city.trim() === ''
                && contact.postalCode.trim() === '');
        };


        // ------------------------------------------------------------------------------------------
        // MODAL NAVIGATION
        // ------------------------------------------------------------------------------------------
        $scope.close = function () {
            $modalInstance.dismiss();
        };

        $scope.edit = function (contact) {
            $modalInstance.dismiss();
            dsNavigationService.gotoRoute("EmergencyContacts/Edit", contact);
        };
    }
}
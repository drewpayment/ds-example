import * as angular from 'angular';
import * as dateUtil from '../../../../../Scripts/util/dateUtilities';
import { AccountService } from '@ajs/core/account/account.service';

export class BenefitSummaryController {
    static readonly $inject = [
        '$scope',
        AccountService.SERVICE_NAME,
        'BenefitsService',
        '$location',
        'DsState',
        'DsMsg',
        'EmployeeDependentService',
        '$filter',
        'DsNavigationService'
    ];

    constructor(
        $scope,
        accountService: AccountService,
        BenefitsService,
        $location,
        state,
        MessageService,
        EmployeeDependentService,
        $filter,
        DsNavigationService
    ) {

        let context;

        $scope.hasError                     = false;
        $scope.selectedPlanCategory         = {};
        $scope.selectedOpenEnrollment       = {};
        $scope.userData                     = {};
        $scope.usedDependents               = [];
        $scope.employeeDependents           = [];
        $scope.employeeSelectionDependents  = [];

        // --------------------------------------------------------
        // initialize the controller with the info
        // --------------------------------------------------------
        function initCtrl(userAccount) {

            context = BenefitsService.getBenefitContext();

            // Make sure they have the data from the previous page.
            if (!context.hasSelectedEnrollment()) {
                //state.router.go('ess.benefits.home');
                BenefitsService.redirect('home');
                return;
            }

            $scope.selectedOpenEnrollment = context.selectedOpenEnrollment;
            $scope.hasDependentValidationErrors = false;

            // --------------------------------------------------------
            // Get the current user information and any dependents
            // --------------------------------------------------------
            clearErrors();
            $scope.userData = userAccount;
            $scope.employeeDependents = context.dependents;

            // angular.forEach($scope.employeeDependents, function (employeeDependent) {
            //    //Format the data in the same format as the database so there is no 'change' detected when submitted.
            //    employeeDependent.birthDate = $filter('date')(employeeDependent.birthDate, 'MM/dd/yyyy');
            // });

            // --------------------------------------------------------
            // This populates any dependents that were used when adding benefits.
            // Some employees have more dependents than were added to their plans.
            // We only want to list the dependents that were used during enrollment.
            // --------------------------------------------------------
            getUsedDependents();

        }

        accountService.getUserInfo().then(user => initCtrl(user));


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // DEPENDENTS
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // --------------------------------------------------------
        // This populates any dependents that were used when adding benefits.
        // Some employees have more benefits than were added to their plans.
        // We only want to list the dependents that were used during enrollment.
        // --------------------------------------------------------
        function getUsedDependents() {

            $scope.usedDependents.length = 0;

            if ($scope.selectedOpenEnrollment.employeeOpenEnrollment.employeeSelections) {

                let selections = $scope.selectedOpenEnrollment.employeeOpenEnrollment.employeeSelections;

                angular.forEach(selections, function (employeeSelection) {

                    let employeeDependents = employeeSelection.employeeDependents;

                    angular.forEach(employeeDependents, function (employeeDependent) {

                        if (!doesContainDependent(employeeDependent)) {

                            $scope.usedDependents.push(getFullDependent(employeeDependent));
                        }
                    });
                });
            }
        }

        // --------------------------------------------------------
        // This function is called to prevent duplicates from being entered in the usedDependent array.
        // --------------------------------------------------------
        function doesContainDependent(dep) {

            let keepGoing = true;
            let returnValue = false;

            angular.forEach($scope.usedDependents, function (usedDependent) {

                if (keepGoing && (dep.employeeDependentId === usedDependent.employeeDependentId)) {
                    keepGoing = false;
                    returnValue = true;
                }
            });

            return returnValue;
        }


        // --------------------------------------------------------
        // We already get the dependents, so find the match and return the full dependent with the edit permission attached.
        // --------------------------------------------------------
        function getFullDependent(dep) {

            let keepGoing = true;
            let returnValue = [];

            angular.forEach($scope.employeeDependents, function (fullDependent) {

                if (keepGoing && (dep.employeeDependentId === fullDependent.employeeDependentId)) {
                    keepGoing = false;
                    returnValue = fullDependent;
                }
            });

            return returnValue;
        }

        // --------------------------------------------------------
        // Get the dependents for each plan selection
        // --------------------------------------------------------
        $scope.getEmployeeSelectionDependents = function(employeeOpenEnrollmentSelectionId) {

            let keepGoing = true;
            $scope.employeeSelectionDependents  = [];
            $scope.employeeSelections           = context.selectedOpenEnrollment.employeeOpenEnrollment.employeeSelections;

            angular.forEach($scope.employeeSelections, function(employeeSelection) {
                if (keepGoing && employeeOpenEnrollmentSelectionId == employeeSelection.employeeOpenEnrollmentSelectionId) {

                    $scope.employeeSelectionDependents = employeeSelection.employeeDependents;
                    keepGoing = false;
                }
            });

            return $scope.employeeSelectionDependents;
        };

        // returns dependent name as F M L
        $scope.getFirstMidLastName = function (dependent) {
            return window.FirstMidLast(dependent.firstName , dependent.middleInitial, dependent.lastName);
        };

        // ssn mask handlers
        $scope.enableMaskSelection = function (dep, enabled) {
            let canShow = dep.maskedSocialSecurityNumber !== dep.unmaskedSocialSecurityNumber;
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


        function clearErrors() {
            $scope.hasError = false;
        }


        // ------------------------------------------------------------------------------------------
        // VIEW HELPERS
        // ------------------------------------------------------------------------------------------
        $scope.moveToPage = function (location) {
            MessageService.clearTemporaryMessage();
            if(location.indexOf('Enrollment') > -1)
                BenefitsService.redirect('enrollment');
            else
                BenefitsService.redirect('confirmation');
        };
    }
}

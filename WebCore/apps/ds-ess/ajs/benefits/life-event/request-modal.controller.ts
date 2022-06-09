import { STATES } from "../../shared/state-router-info";
import * as benefitSvc from "@ajs/benefits/benefits.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { DsInlineValidatedInputService } from "../../ui/form-validation/ds-inline-validated-input.service";
import { IUserInfo } from "@ajs/user";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";

export class BenefitLifeEventRequestModalController {
    static readonly $inject = [
        '$scope',
        '$modalInstance',
        STATES.ds.ess.RESOLVE.userAccount,
        benefitSvc.SERVICE_NAME,
        DsMsgService.SERVICE_NAME,
        DsInlineValidatedInputService.SERVICE_NAME
    ];

    constructor($scope: any, $modalInstance, userAccount: IUserInfo, BenefitsService, MessageService: DsMsgService, DsInlineValidatedInputService: DsInlineValidatedInputService) {
        $scope.lifeEventList = [];
        $scope.isEventSelected = false;
        $scope.currentUser = {};
        $scope.selectedEvent = [];

        (function initModalState() {

            MessageService.loading(true);

            //Grab the list of possible life events.
            BenefitsService.getLifeEventList().then(function (eventTypes) {
                $scope.lifeEventList = eventTypes;
            });

            $scope.currentUser = userAccount;

            MessageService.loading(false);
        })();

        // ------------------------------------------------------------------------------------------
        //Function when a event type is selected
        // ------------------------------------------------------------------------------------------
        $scope.selectEvent = function (event) {
            $scope.selectedEvent = event;
            $scope.isEventSelected = true;
        };

        // ------------------------------------------------------------------------------------------
        // Close Modal
        // ------------------------------------------------------------------------------------------
        $scope.close = function () {
           $modalInstance.dismiss();
        };

        function checkDate(dateIn) {
            var timestamp = Date.parse(dateIn);

            if (isNaN(timestamp) == false) {
                return true;

            }
            return false;
        }
        
        //--------------------------------------------------------
        // Create the life event if the form is valid.
        //--------------------------------------------------------
        $scope.validateThenSubmit = function (requestLifeEventForm) {
            
            if (checkDate($scope.selectedEvent.eventDate) && $scope.selectedEvent.eventDate.length > 0) {
                DsInlineValidatedInputService.validateAll("requestLifeEventForm")
                    .then(
                        function () {
                            
                            $scope.createLifeEvent(requestLifeEventForm);

                        },
                        function () {
                          
                            MessageService.setTemporaryMessage(window.MESSAGE_FAILURE_FORM_SUBMISSION_ESS,
                                MessageTypes.error,
                                2500);
                        }
                    );
            }
        };

        $scope.createLifeEvent = function () {

            var dto = {
                Description:          $scope.currentUser.lastName + ', ' + $scope.currentUser.firstName + ' - ' + $scope.selectedEvent.description,
                LifeEventReason:      $scope.selectedEvent.lifeEventReason,
                EventDate:            $scope.selectedEvent.eventDate
            };

            BenefitsService
                .putLifeEvent(dto)
                .then(function(data) {

                    var message = "";

                    if (data.planListCount > 0) {
                        message = "Your life event was created successfully.";
                    } else {
                        message = "Your life event was submitted successfully and is awaiting approval.";
                    }

                    MessageService.setTemporarySuccessMessage(message);
                    $modalInstance.close($scope.currentUser);
                    
                })
                .catch(MessageService.showWebApiException);
        };
    }
}
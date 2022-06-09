import * as angular from "angular";
import * as util from "../../../../../Scripts/util/ds-common";
import { CountryStateService } from "@ajs/location/country-state/country-state.svc";
import { DsInlineValidatedInputService } from "../../ui/form-validation/ds-inline-validated-input.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";

export class CountryAndStateControllerBase {
    static readonly CONTROLLER_NAME = "CountryAndStateController";
    static readonly $inject = [
        '$scope',
        DsMsgService.SERVICE_NAME,
        DsInlineValidatedInputService.SERVICE_NAME,
        CountryStateService.SERVICE_NAME
    ];

    constructor(
        private $scope: any, 
        private MessageService: DsMsgService, 
        private DsInlineValidatedInputService: DsInlineValidatedInputService, 
        private locations: CountryStateService) {

        //--------------------------------------------------------
        //initialize variables
        //--------------------------------------------------------
        $scope.countryAndState = {};
        var needSnapshot = true,
            lastKnownSelectedCountry,
            lastKnownSelectedState,
            originalData = {};

        $scope.countryAndState = {
            states: [],
            countries: [],
            counties: [],
            countryId: null,
            stateId: null,
            countyId: null,
            takeSnapshot:function() {
                return [this.countryId, this.stateId, this.countyId];
            },
            initialize: function (settings) {
                angular.extend(this, settings); //set country and state values from settings
            },
            setModifiedSnapshot: function() {
                needSnapshot = false;
                originalData = angular.copy(this.takeSnapshot()); //take snapshot of initialized data
            },
            isModified: function () {
                return !angular.equals(this.takeSnapshot(), originalData);
            }
        };

        //--------------------------------------------------------
        //The initial countries and states for the drop downs
        //--------------------------------------------------------
        $scope.countryAndState.loadCountries = function() {

            return locations
                .getCountryList()
                .then(function(countries) {
                    $scope.countryAndState.countries = countries;
                    $scope.countryAndState.countryId = locations.defaults.countries.usa;
                })
                .then($scope.countryAndState.loadStates)
                .catch(MessageService.showWebApiException);
        }

        //--------------------------------------------------------
        //load the states when the country is changed
        //--------------------------------------------------------
        $scope.countryAndState.loadStates = function () {

            var shouldLoadStates =
                $scope.countryAndState.countryId &&
                lastKnownSelectedCountry !== $scope.countryAndState.countryId;

            lastKnownSelectedCountry = $scope.countryAndState.countryId;

            if (shouldLoadStates) {

                //Get the states for the selected country.
                return locations
                    .getStatesByCountry($scope.countryAndState.countryId)
                    .then(statesLoaded)
                    .catch(MessageService.showWebApiException);
            }

            return null;
        };

        //--------------------------------------------------------
        //load the counties when the state is changed
        //--------------------------------------------------------
        $scope.countryAndState.loadCounties = function () {

            var shouldLoadCounties =
                $scope.countryAndState.stateId &&
                lastKnownSelectedState !== $scope.countryAndState.stateId;

            lastKnownSelectedState = $scope.countryAndState.stateId;

            if (shouldLoadCounties) {

                //Get the states for the selected country.
                return locations
                    .getCountiesByState($scope.countryAndState.stateId)
                    .then(countiesLoaded)
                    .catch(MessageService.showWebApiException);
            }

            return null;
        };

        //--------------------------------------------------------
        //This is a special case load states.
        //For the add we need to suppress inline validation during
        //loading. The load states is the moment we can clear
        //the suppression. If you don't need suppression just
        //call loadStates().
        //--------------------------------------------------------
        $scope.countryAndState.clearSuppressionLoadStates = function () {

            if ($scope.frmName) {
                DsInlineValidatedInputService.clearSuppressed($scope.frmName);
            }

            $scope.countryAndState.loadStates();
        };

        //--------------------------------------------------------
        //executed on a successful state load
        //--------------------------------------------------------
        function statesLoaded(states) {
            //console.log('%c LOAD STATES - STATES LOADED %S', 'color:Purple', lastKnownSelectedCountry);

            //the toggling of 0 and NULL below is a trick/hack to get the scope to trip a digest allowing watches on the value below to 
            //execute. Both of the following conditions results in a non-value situation that should be detected and reported invalid if validated
            if (!states || states.length < 1) {
                states = [{ stateId: ($scope.countryAndState.stateId === null) ? undefined : null, name: 'No States Defined.' }];
            } else {
                var item = { stateId: ($scope.countryAndState.stateId === null) ? undefined : null, name: window.DROP_DOWN_SELECT };
                // not using "select" in dropdown.  hiding all address information unless user wants to add it.
                //fix for adding multiple entries in DD list for "select"
                //if (states.findIndex(x => x.name === DROP_DOWN_SELECT) === -1 ) {
                //        //states.push(item);
                //    states.unshift(item);
                //    console.log("item already exists");
                //}
            }

            $scope.countryAndState.states = states;
            $scope.countryAndState.stateId = $scope.countryAndState.states[0].stateId;
            //check for country = USA and set to michigan
            if ($scope.countryAndState.countryId === locations.defaults.countries.usa) {
                //set state to Michigan
                $scope.countryAndState.stateId = locations.defaults.states.michigan;
            }

            $scope.countryAndState.loadCounties();
            

            if (needSnapshot) {
                $scope.countryAndState.setModifiedSnapshot();
            }

        }

        //--------------------------------------------------------
        //executed on a successful county load
        //--------------------------------------------------------
        function countiesLoaded(counties) {
            //console.log('%c LOAD COUNTIES - COUNTIES LOADED %S', 'color:Purple', lastKnownSelectedState);

            //the toggling of 0 and NULL below is a trick/hack to get the scope to trip a digest allowing watches on the value below to 
            //execute. Both of the following conditions results in a non-value situation that should be detected and reported invalid if validated
            if (!counties || counties.length < 1) {
                counties = [{ countyId: ($scope.countryAndState.countyId === null) ? undefined : null, name: 'No Counties Defined.' }];
            } else {
                var item = { countyId: ($scope.countryAndState.countyId === null) ? undefined : null, name: window.DROP_DOWN_SELECT };
                counties.unshift(item);
            }

            $scope.countryAndState.counties = counties;
            $scope.countryAndState.countyId = $scope.countryAndState.counties[0].countyId;

            if (needSnapshot) {
                $scope.countryAndState.setModifiedSnapshot();
            }
        }
    }
}

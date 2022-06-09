import * as angular from 'angular';
import * as dateUtil from '../../../../../Scripts/util/dateUtilities';
import { DsInlineValidatedInputService } from './ds-inline-validated-input.service';
import { isUndefinedOrNullOrEmptyString } from '@util/ds-common';

export class DsInlineValidatedInputDirective implements ng.IDirective {
    static readonly DIRECTIVE_NAME = 'dsInlineValidatedInput';

    restrict = 'E';
    transclude = true;
    scope =
    {
        // ----------------------------
        removeFormGroup: '@',                   // this was needed for proper html
        // ----------------------------
        inputName: '@',                         // the value you want for the INPUT 'name' and 'id' attribute.
        // Also it will added to the 'for' attribute of the LABEL.
        inputType: '@',                         // if you need to add a type to the input box. Used for 'number' type.
        inputDivClass: '@',                     // the class you supply will be added to the div.input (see template)
        inputPlaceHolderText: '@',              // if this has value placeholder text will be added to the text box
        // ----------------------------
        labelText: '@',                         // the label text for the field. No need to add ':' it's added automatically
        labelClass: '@',                        // the class you supply will be added to the LABEL (see template)
        removeLabel: '@',                       // do not include a label
        viewIconClass: '@',                     // the class to define what icon to display in view-mode
        // ----------------------------
        errorMsgDivClass: '@',                  // the class you supply will be added to the div.message-area
        // (see template) //todo: review: not using this ... remove it
        // ----------------------------
        dsRequired: '@',                        // true if field is required. For validation and adds '*' to label
        dsMinLength: '@',                       // min allwed chars. If a regex pattern is defined that prevents
        // this then you don't need it.
        dsMaxLength: '@',                       // max allowed chars. If a regex pattern is defined that prevents
        // this then you don't need it.
        // ----------------------------
        minNumber: '@',                         // min allwed chars. If a regex pattern is defined that prevents
        // this then you don't need it.
        maxNumber: '@',                         // max allowed chars. If a regex pattern is defined that prevents
        // this then you don't need it.
        // ----------------------------
        dsPattern: '@',                         // regex pattern for validation (javascript regex syntax)
        // todo: refactor: (jay) find a centralized solution for any pattern we use.
        dsInputHelpMsg: '@',                    // message to show on focus or when pattern error occurs
        dsMaskedPattern: '@',                   // A custom regex expression. There is some issues with adding
        // some via the HTML. This one breaks the page: "/^[^\s@]+@[^\s@]+\.[^\s@]+$/"
        dsMaskedPatternIsReversed: '@',         // an option for masked edit. no specific value has to be set,
        // just the attribute needs to exist
        dsMaskedPatternIsOptional: '@',         // an option for masked edit. no specific value has to be set,
        // just the attribute needs to exist
        dsMaskedPatternNoMaxLength: '@',        // an option for masked edit. no specific value has to be set,
        // just the attribute needs to exist
        // ----------------------------
        dsBuiltInMaskKey: '@',                  // This is an alternative for dsMaskedPattern. Added to reduce replicated masking patterns.
        dsBuiltInPatternKey: '@',               // This is an alternative for dsPattern. Added to reduce replicated
        // regex patterns. Also found out can't put all regex patterns in HTML. A key that will be used to set the
        // pattern value when this html is generated in compile. This will override dsMaskedPattern.
        dsBuiltInHelpMessageKey: '@',           // This will override and is an alternative for dsInputHelpMsg.
        // Added to reduce replicated regex patterns.  A key that will be used to set the help message value when
        // this html is generated in compile. If not required a note will be appended.
        // ----------------------------
        dsDisabled: '=',                        // truthy value to determine if this should be disabled or not
        // ----------------------------
        dsModel: '=',                           // the model variable (see as you normally would for ng-model)
        // ----------------------------
        dsViewModel: '=',                       // the model variable to be displayed in view-mode
        dsViewModeOverride: '=',                // used to override the default functionality of starting in view-mode
        // if a dsViewModel has been specified. When the attribute is present the directive will start in edit-mode instead.
        dsEditToViewPromise: '=',               // a function that returns a promise that once resolved successfully will
        // switch the directive back to view-mode
        // ----------------------------
        frmObj: '=',                            // the form object that this input box belongs to. I wasn't able to get a
        // reference any other way. //todo: refactor: (jay) probably a way to do this without having to pass the reference in.
        // ----------------------------
        dsCustomValidationRulePromise: '=',     // this is the rule used if using custom validation. it's must return an array:
        // empty or filled with string messages.
        // ----------------------------
        associatedInputModel: '=',              // the model value of another input that this value must match. You need to provide a message to show for non-matches. Compares using .toString(). ie. matching passwords
        associatedInputModelErrorMessage: '@',  // the message to display if this model value doesn't match the associated directive model. Compares using .toString()
        // ----------------------------
        isDate: '@',                            // this is a hack until masked edit is working for date. We're going to validate dates with code in this directive rather than a pattern or external rule.
        isNumber: '@',                          // since we can't use the number input type (browser support) we will handle converting input to number in the directive. It was found that the number type would tell angular to convert the 'string' value in the input box to number on the model. We can't be turning number values into string values; ain't nobody got time for that.
        // ----------------------------
        dsOnSuccess: '&',                       // this is a hack until masked edit is working for date. We're going to validate dates with code in this directive rather than a pattern or external rule.
        dsCompleted: '&',                       // fires when everything is done ... except the timeout for the LOOKING GOOD message
        dsBlur: '&',                            // hook used to attach an external callback on the inputs 'blur' event
        dsFocus: '&',                           // hook used to attach an external callback on the inputs 'focus' event
        dsSuppressMessages: '&',                // if set to true, prevents any messages from being displayed after the input element
        dsAutoFocusOn: '=',                     // bindable attribute used to give focus to the input element automatically

        // ----------------------------
        dsKeyupDelayedValidation: '@'           // waits the specified milliseconds (default is 500ms) after last keyup to do custom-validation. Every key stroke will reset the counter. if the attribute is not specified, custom-validation will only be performed on blur.
    };
    replace = true;

    constructor(
        private $rootScope: any,
        private $timeout: ng.ITimeoutService,
        private svc: DsInlineValidatedInputService,
        private $q: ng.IQService) {

        angular.extend(this, {
            template: require('./input.html')
        });
    }

    static $instance(): ng.IDirectiveFactory {
        const dir = (root, timeout, svc, q) => new DsInlineValidatedInputDirective(root, timeout, svc, q);
        dir.$inject = [
            '$rootScope',
            '$timeout',
            'DsInlineValidatedInputService',
            '$q'
        ];
        return dir;
    }


    compile(ele: ng.IAugmentedJQuery, attr: ng.IAttributes) {
        // console.log('%cINPUT DIRECTIVE COMPILE: %s', 'color:red;font-size:16pt', attr.inputName);

        // runs all the code in here immediately. done it this way primarily for collapsing; hopefully it's not a performance issue. FYI: this code only runs once for every directive created.
        // ----------------------------------------------modify the parent div element
        if (typeof attr.removeFormGroup != 'undefined')
            ele.removeClass('form-group');

        // ----------------------------------------------find the text and label elements
        let txt = ele.find('input');
        let lbl = ele.find('label');
        let viewIcon = ele.find('i');
        let hasValidationSetupError = false;

        // ----------------------------------------------basic html input attributes
        if (typeof attr.inputType != 'undefined' && attr.inputType != 'text') {
            // change the input type if something other than text
            // for IE7/8 compatibility we must completely swap out the input element
            // see: http://stackoverflow.com/questions/21303070/changing-input-type-password-to-input-type-text
            // and: http://api.jquery.com/attr/
            // todo: see if there is a way to copy all original attributes into the new <input/> so we don't
            // todo: have to hardcode them here
            let txtClone = null;
            txtClone = angular.element('<input type="' + attr.inputType + '" class="form-control" ng-hide="isViewMode" ng-disabled="dsDisabled" ng-model="dsModel" />');

            txt.after(txtClone);
            txt.remove();

            txt = txtClone;
        }

        txt.attr('id', attr.inputName);
        txt.attr('name', attr.inputName);

        // ----------------------------------------------place holder
        if (typeof attr.inputPlaceHolderText != 'undefined')
            txt.attr('placeholder', attr.inputPlaceHolderText);

        // ----------------------------------------------label attribtues
        if (typeof attr.removeLabel == 'undefined') {
            // DO ALL LABEL ATTRIBUTE MANIPULATION HERE
            lbl.attr('for', attr.inputName);

            if (typeof attr.labelClass != 'undefined') {
                lbl.addClass(attr.labelClass);
            }
        } else {
            lbl.remove();
        }

        // ----------------------------------------------add classes //todo: review: (jay) see if this can be put in the template
        if (typeof attr.inputDivClass != 'undefined')
            txt.parent().addClass(attr.inputDivClass);

        if (typeof attr.errorMsgDivClass != 'undefined')
            $(ele).children('.message-area').addClass(attr.errorMsgDivClass);

        if (typeof attr.viewIconClass != 'undefined')
            viewIcon.addClass(attr.viewIconClass);
        else
            viewIcon.addClass('icon-pencil');

        // ------------------------------------------set when a validation setup error is encountered

        function setValidationSetupErrror(msg) {
            hasValidationSetupError = true;
            attr.dsPattern = /error/;
            attr.dsInputHelpMsg = 'Validation error (' + msg + '). Please contact administrator.';
            // console.log('VALIDATION SETUP ERROR: ' + attr.inputName);
            // console.log(dsInputHelpMsg);
        }

        // only do this if there is no custome message provided
        if (typeof attr.dsMaskedPattern == 'undefined' && typeof attr.dsBuiltInMaskKey != 'undefined') {
            let mask = this.svc.resolveBuiltInMakedEdit(attr.dsBuiltInMaskKey);

            // console.log('ADDING THE BUILT IN MESSAGE: ' + attr.inputName);
            // console.log('BUILT IN PATTERN KEY: ' + attr.dsBuiltInHelpMessageKey);
            // console.log('BUILT IN PATTERN: ' + helpMessage);

            if (typeof mask == 'undefined')
                setValidationSetupErrror('Edit Mask');

            // set the value
            attr.dsMaskedPattern = mask;
        }

        // only do this if there is no custome message provided
        if (typeof attr.dsInputHelpMsg == 'undefined' && typeof attr.dsBuiltInHelpMessageKey != 'undefined') {
            let helpMessage = this.svc.resolveBuiltInHelpMessage(attr.dsBuiltInHelpMessageKey);

            // console.log('ADDING THE BUILT IN MESSAGE: ' + attr.inputName);
            // console.log('BUILT IN PATTERN KEY: ' + attr.dsBuiltInHelpMessageKey);
            // console.log('BUILT IN PATTERN: ' + helpMessage);

            if (typeof helpMessage == 'undefined')
                setValidationSetupErrror('Help Message');

            // set the help message.
            attr.dsInputHelpMsg = helpMessage;

            // if this isn't required let them know.
            if (typeof attr.dsRequired == 'undefined')
                attr.dsInputHelpMsg = helpMessage + ' Blank is acceptable.';

            // console.log('MESSAGE FINAL VALUE: ' + attr.dsInputHelpMsg);
        }

        if (!hasValidationSetupError && typeof attr.dsBuiltInPatternKey != 'undefined') {
            attr.dsPattern = null; // wipe out this if they tried to set it as well
            let patterStr = this.svc.resolveBuiltInRegexPattern(attr.dsBuiltInPatternKey);
            // console.log('ADDING THE BUILT IN PATTERN: ' + attr.inputName);
            // console.log('BUILT IN PATTERN KEY: ' + attr.dsBuiltInPatternKey);
            // console.log('BUILT IN PATTERN: ' + patterStr);

            if (typeof patterStr == 'undefined') {
                setValidationSetupErrror('Pattern');
            } else {
                attr.dsPattern = patterStr;
            }

        }

        return {
            post: (scope, ele, attr, controller) => this.post(scope, ele, attr, controller)
        };
    }

    post(scope: any, ele: ng.IAugmentedJQuery, attr: ng.IAttributes, controller?) { // fyi: this is the same as 'link: function(){...}
        // console.log('%cINPUT DIRECTIVE LINK: %s', 'color:red;font-size:16pt', attr.inputName);
        scope.dsMessages = [];
        scope.showingInputHelpText = false;
        scope.showMessages = false;
        scope.showInProgress = false;
        scope.hasError = false;

        // ----------------------------------------------
        // setup the mask edit for the input if a mask
        // edit was defined
        // ----------------------------------------------
        if (typeof attr.dsMaskedPattern != 'undefined') {

            // options with event handlers for the masked edit code.
            // http://blog.igorescobar.com (library being used for masked edit)
            let options = {
                reverse: (typeof attr.dsMaskedPatternIsReversed != 'undefined'),
                optional: (typeof attr.dsMaskedPatternIsOptional != 'undefined'),
                maxlength: !(typeof attr.dsMaskedPatternNoMaxLength != 'undefined')
            };

            $(ele).find('input').mask(attr.dsMaskedPattern, options);
        }

        // ----------------------------------------------
        // try to execute an apply. it won't happen if
        // apply/digest already in motion
        // ----------------------------------------------
        // todo: refactor: (jay) this should be put in the the DsValidatedInputService possibly?
        scope.tryApply = (func) => {
            func = func || function() {};

            if (!this.$rootScope.$root.$$phase) {
                scope.$apply(func);
            }
        };

        // ----------------------------------------------
        // an id used to keep track of validations
        // that have been spawned to keep messages from
        // going crazy like the grand finale on the 4th of july
        //
        // this could happen if a custom rule runs long
        // and the user provokes another validation via
        // the client interface
        // ----------------------------------------------
        scope.currentValidationId = 0;

        // ----------------------------------------------
        // will be true if this is a service validation
        // the service validation occurs when a user clicks
        // on a button a we exectute validate on all the
        // directives that are registered on the service
        // ----------------------------------------------
        scope.isServiceValidate = false;

        // ----------------------------------------------
        // gets the value from the input text box
        // this should be the same value as the model
        // but it's going to a be a string which is
        // helpful in some situations
        // ----------------------------------------------
        scope.inputBoxValue = function () {
            return ele.find('input').val();
        };

        // ----------------------------------------------
        // initialize variables
        // ----------------------------------------------
        scope.initValidationVariables = function () {
            scope.showingInputHelpText = false;
        };

        scope.isInViewMode = function () {
            let hasViewOverride = angular.isDefined(attr.dsViewModeOverride) &&
                (attr.dsViewModeOverride === '' || scope.dsViewModeOverride);
            // The control is in "View Mode" if the dsViewModel attribute is provided,
            // and (the override is not provided or isViewMode has been set to true and a view model has been provided)
            return (typeof attr.dsViewModel != 'undefined') &&
            (!hasViewOverride || (scope.isViewMode && scope.dsViewModel));
        };

        scope.$watch('dsViewModel',
            function() {
                // ----------------------------------------------
                // check view mode. will be set to true if the view mode is set
                // ----------------------------------------------
                scope.isViewMode = scope.isInViewMode();
            });

        // ----------------------------------------------
        // check if the input element should be given focus
        // concepts from: http://stackoverflow.com/questions/14833326/how-to-set-focus-in-angularjs
        // ----------------------------------------------
        // if (scope.dsAutoFocusOn) {
        //    $timeout(function() { ele.find('input').focus(); });
        // }

        // ----------------------------------------------
        // bound to the normal/error message area element's
        // ng-show attribute. used to show/hide the message area.
        // ----------------------------------------------
        scope.displayMessages = function() {
            return !messagesSuppressed() && scope.showMessages;
        };

        // ----------------------------------------------
        // bound to the in-progress message area element's
        // ng-show attribute. used to show/hide the inprogress area.
        // ----------------------------------------------
        scope.displayInProgress = function () {
            return !messagesSuppressed() && scope.showInProgress;
        };

        // ----------------------------------------------
        // checks if the directive is marked to prevent
        // all messages from being displayed.
        // ----------------------------------------------
        function messagesSuppressed() {
            return angular.isDefined(attr.dsSuppressMessages) && scope.dsSuppressMessages();
        }

        // ----------------------------------------------
        // show messages for information (help message)
        // ----------------------------------------------
        scope.setMessageAreaForInformation = () => {
            let showInformation =
                attr.dsInputHelpMsg &&
                !scope.hasError &&
                !scope.isServiceValidate &&
                !this.svc.isSuppressed(scope.frmObj.$name);

            scope.configureMessageArea(null);

            if (showInformation) {
                scope.showMessages = scope.addMessageIfDoesNotExist(attr.dsInputHelpMsg);
            }

            if (scope.hasError) {
                scope.configureMessageArea(window.HAS_ERROR_CLASS_NAME);
            } else {
                scope.showMessages = showInformation;
            }
        };

        // ----------------------------------------------
        // this method will setup the message area to
        // display an error
        // ----------------------------------------------
        scope.setMessageAreaForError = function () {
            // console.log('SHOWING ERROR');
            scope.configureMessageArea(window.HAS_ERROR_CLASS_NAME);
            scope.showMessages = true;
        };

        // ----------------------------------------------
        // show messages for success
        // ----------------------------------------------
        scope.setMessageAreaForSuccess = function () {
            // console.log('SHOWING SUCCESS');
            // console.trace();
            scope.configureMessageArea(window.HAS_SUCCESS_CLASS_NAME);
            scope.showMessages = scope.addMessageIfDoesNotExist('Looking Good!');
            scope.cancelCurrentSuccessTimeout();
            scope.timeOutRemoveSuccess();
        };

        // ----------------------------------------------
        // this will setup the timer that will
        // remove the success automatically
        // message after showing it
        // ----------------------------------------------
        scope.timeOutRemoveSuccess = () => {

            scope.succeessTimeoutReference = this.$timeout(
                    function () {
                        if ($(ele).hasClass(window.HAS_SUCCESS_CLASS_NAME)) {
                            // console.log('HIDING MESSAGE AREA BECAUSE OF SUCCESS');
                            scope.showMessages = false;
                            $(ele).removeClass(window.HAS_SUCCESS_CLASS_NAME);
                        } else {
                            // console.log('NOT NOT HIDING MESSAGE AREA BECAUSE OF SUCCESS');
                        }
                    }
                    , 3000
                );
        };

        // ----------------------------------------------
        // use this method to set the class on the
        // message area. If you don't pass a class
        // name in no class will be set on the
        // message area due to this method removing
        // all of the classes before adding the class
        // ----------------------------------------------
        scope.configureMessageArea = function (className) {
            // console.log('CONFIGURE MSG AREA BEFORE classes=[%s]', $(ele).attr('class'));
            $(ele).removeClass(window.HAS_ERROR_CLASS_NAME + ' ' + window.HAS_SUCCESS_CLASS_NAME + ' ' + window.IN_PROGRESS_CLASS_NAME);

            if (className) {
                $(ele).addClass(className);
            }
        };

        // ----------------------------------------------
        // cancel the timer for showing success
        // ----------------------------------------------
        scope.cancelCurrentSuccessTimeout = () => {
            if (scope.succeessTimeoutReference) {
                // console.log('CANCELLING CURRENT TIMEOUT');
                this.$timeout.cancel(scope.succeessTimeoutReference);
            }
        };

        // ---------------------------------------------
        // cancel the timer for showing in progress
        // ---------------------------------------------
        scope.cancelExistingInProgressReport = () => {
            // console.log('%cCANCELLING IN PROGRESS - start', 'COLOR:SLATEBLUE');
            if (scope.inProgressTimeoutReference) {
                this.$timeout.cancel(scope.inProgressTimeoutReference);
            }
        };


        // ---------------------------------------------
        // cancel the timer that executes validation after
        // a specified amount of time. This is used when
        // a user causes a keyup event or pastes and we
        // have to goto the server via a custom rule
        // ---------------------------------------------
        scope.cancelKeyupTimeoutReference = () => {
            if (scope.keyupTimeoutReference) {
                this.$timeout.cancel(scope.keyupTimeoutReference);
            }
        };

        // ----------------------------------------------
        // will add A RANGE of messages to the messages array
        // if it doesn't already exist
        // ----------------------------------------------
        scope.addRangeMessageIfDoesNotExist = function (messageToAddArray) {

            if (messageToAddArray) {
                for (let i = 0; i < messageToAddArray.length; i++) {
                    scope.addMessageIfDoesNotExist(messageToAddArray[i]);
                }
            }
        };

        // ----------------------------------------------
        // will add the the message to the messages array
        // if it doesn't already exist
        // ----------------------------------------------
        scope.addMessageIfDoesNotExist = function (messageToAdd) {

            if (messageToAdd) {

                let result = scope.dsMessages.some(
                    function (element, index, array) {
                        return element == messageToAdd;
                    }
                );

                if (!result) {
                    scope.dsMessages.push(messageToAdd);
                }
                return true;
            }

            return false;
        };

        // ----------------------------------------------
        // get a built in validation message by key
        // ----------------------------------------------
        scope.getBuiltInErrorMessageByKey = function (key) {
            switch (key) {
                case 'required':
                    return 'This field is required.';

                case 'minlength':
                    return 'Min Length Is: ' + scope.dsMinLength + '.';

                case 'maxlength':
                    return 'Max Length Is: ' + scope.dsMaxLength + '.';

                case 'nan':
                    return 'Input is non-numeric.';

                case 'special':
                    return attr.dsInputHelpMsg;
            }
        };

        // ----------------------------------------------
        // check the built in built in validation rules
        // ----------------------------------------------
        scope.validateBuiltIn = () => {
            let deferred = this.$q.defer();

            let messages = [],
                txtValue = scope.inputBoxValue(),
                skipMaxMinLength = false,
                isRequired = !!attr.dsRequired,
                isEmptyString = isUndefinedOrNullOrEmptyString(scope.dsModel);

            // console.log('--------------------------------------------------------');
            // console.log('VALIDATE STANDARD CALLED');
            // console.log('--------------------------------------------------------');
            // console.log('INPUT NAME: ---' + attr.inputName + '----');
            // console.log('TXT BOX: ---' + txtValue + '----');
            // console.log('MODEL:   ---' + scope.dsModel + '----');
            // console.log('IS EMPTY: ---' + isEmptyString + '----');

            if (!scope.isServiceValidate || isRequired || (!isEmptyString && scope.isServiceValidate)) {
                if (typeof attr.dsRequired != 'undefined') {
                    if (isEmptyString) {
                        // console.log('REQUIRED ERROR');
                        skipMaxMinLength = true;
                        messages.push(scope.getBuiltInErrorMessageByKey('required'));
                    }
                }

                if (!skipMaxMinLength && typeof attr.dsMinLength != 'undefined') {
                    if (txtValue.length < attr.dsMinLength) {
                        // console.log('MIN LENGTH ERROR');
                        messages.push(scope.getBuiltInErrorMessageByKey('minlength'));
                    }
                }

                if (!skipMaxMinLength && typeof attr.dsMaxLength != 'undefined') {
                    if (txtValue.length > attr.dsMaxLength) {
                        // console.log('MAX LENGTH ERROR');
                        messages.push(scope.getBuiltInErrorMessageByKey('maxlength'));
                    }
                }

                if (typeof attr.dsPattern != 'undefined') {
                    let checkPattern =
                        !isEmptyString || isRequired;

                    if (checkPattern) {
                        let re = attr.dsPattern;
                        // console.log('EVALUATING pattern: ' + re);
                        let matches = txtValue.match(re);

                        if (!matches) {
                            // console.log('PATTERN ERROR');
                            messages.push(scope.getBuiltInErrorMessageByKey('special'));
                        }
                    }
                }

                if (attr.isNumber) {
                    if (isEmptyString || isNaN(scope.dsModel)) {
                        // console.log('NAN ERROR');
                        messages.push(scope.getBuiltInErrorMessageByKey('special'));
                    } else {
                        let tmp = +scope.dsModel; // converts to number
                        scope.dsModel = tmp; // we convert model value (if marked as 'isNumber' so the model can be properly compared with original (within angular; mostly in controllers for isModified)
                        // console.log('CONVERTED TO NUMBER');
                    }
                }

                if (attr.isDate) {
                    if (!isEmptyString || isRequired) {
                    // var date = ele.find('input').val();
                    if (!window.validateDateString(txtValue)) {
                        // console.log('DATE ERROR');
                        messages.push('Invalid Date.');
                        // scope.tryDigest();
                    }
                    } else {

                        let enteredDate = new Date(ele.find('input').val() as any).getTime();

                        // date is good, so check if future dates are allowed.
                        if (attr.disallowFutureDate) {
                            if (attr.disallowFutureDate == true) {

                                if (enteredDate > Date.now()) {
                                    messages.push('Future date not allowed.');
                                }
                            }
                        }

                        // date is good, so check if past dates are allowed.
                        if (attr.disallowPastDate) {

                            if (attr.disallowPastDate == true) {

                                if (enteredDate < Date.now()) {
                                    messages.push('Past date not allowed.');
                                }
                            }
                        }
                    }
                }

                if (attr.associatedInputModel) {

                    // check to see if the 2 associated scopes are equal to each other
                    let result =
                        scope.dsModel.toString() === scope.associatedInputModel.toString();

                    if (!result) {
                        if (scope.associatedInputModelErrorMessage) {
                            messages.push(scope.associatedInputModelErrorMessage);
                        }
                    }
                }
            }
            // console.log('VALIDATION PERFORMED: %s', scope.dsModel);

            deferred.resolve(messages);
            return deferred.promise;
        };

        // ---------------------------------------------
        // executes the custom validation
        // ---------------------------------------------
        scope.executeCustomRule =  (builtInMessages, validationId) => {
            // console.log('DS INPUT  - EXECUTE CUST RULE: ' + scope.inputName);
            let deferred = this.$q.defer();

            if (attr.dsCustomValidationRulePromise && builtInMessages.length === 0) {
                // console.log('DS INPUT  - STARTING EXECUTE CUST RULE: ' + scope.inputName);
                // console.log(builtInMessages);
                // console.log('CUSTOM VALIDATION - VALIDATION ID: %s', validationId);

                scope.reportInProgress();

                return scope.dsCustomValidationRulePromise()
                    .then(
                        function (customRuleMessages) {
                            // console.log('DS INPUT CUSTOM RULE SUCCESS');
                            // console.log(customRuleMessages);
                            customRuleMessages = customRuleMessages.concat(builtInMessages);
                            return customRuleMessages;
                        }
                    );
            } else {
                // console.log('DID NOT HAVE CUSTOM RULE DEFINED');
                deferred.resolve(builtInMessages);
            }

            return deferred.promise;
        };

        // ----------------------------------------------
        // this is the last thing to be called for any interaction
        // ----------------------------------------------
        scope.complete = function () {
            // console.clear();
            // console.log(new Date().toLocaleTimeString());
            // console.log('DIRECTIVE COMPETE EXECUTING');
            // console.log(scope.hasError);
            // console.log(scope.dsMessages);

            scope.isServiceValidate = false;

            if (attr.dsCompleted) {
                scope.dsCompleted();
            }

        };

        // ----------------------------------------------
        // register this directive with the ds input service
        // ----------------------------------------------
        scope.registerWithDsInlineValidatedInputService = () => {
            let obj = {
                validationPromise: scope.serviceValidate,
                frmName: scope.frmObj.$name,
                inputName: attr.inputName,
                isCustom: (typeof scope.dsCustomValidationRulePromise != 'undefined')
            };

            this.svc.registerDirective(obj);
        };

        // ----------------------------------------------
        // determine if the keyup event should cause validation
        // ----------------------------------------------
        scope.shouldValidateAfterKeyupEvent = function (eventData) {
            // http://www.cambiaresearch.com/articles/15/javascript-char-codes-key-codes

            if (eventData.ctrlKey && eventData.keyCode <= 86) // paste ctrl+V
                return false;

            if (eventData.keyCode >= 9 && eventData.keyCode <= 46 && eventData.keyCode != 32) // 32 is space bar
                return false;

            if (eventData.keyCode >= 91 && eventData.keyCode <= 93)
                return false;

            if (eventData.keyCode >= 112 && eventData.keyCode <= 145)
                return false;

            // tab or enter
            if (eventData.keyCode === 9 || eventData.keyCode === 13)
                return false;

            // this causes (double) validation in a paste event
            if (eventData.keyCode === 17)
                return false;

            return true;
        };

        // ----------------------------------------------
        // add listener to the focus event
        // ----------------------------------------------
        ele.find('input').bind('focus', function (eventData) {
            // console.log('[FOCUS FIRED (%s)] Model Val(%s)', eventData.target.name, scope.dsModel);
            // console.log('[FOCUS (' + eventData.target.name + ')]');
            // console.trace();
            scope.tryApply(function () {
                // scope.setMessageAreaForInformation();
                if (!scope.showInProgress)
                    scope.reportHelpMessage();
            });

            // fire external focus eventhandler if specified
            if (angular.isDefined(attr.dsFocus))
                scope.dsFocus();
        });

        // ----------------------------------------------
        // add listener to the keyup event
        // ----------------------------------------------
        ele.find('input').bind('keyup', function (eventData) {
            // console.log('[KEYUP FIRED (%s)] Model Val(%s)', eventData.target.name, scope.dsModel);
            // console.log('[KEYUP FIRED: (' + eventData.target.name + ')]');

            if (scope.shouldValidateAfterKeyupEvent(eventData)) { // looking at the keycodes to determine if this is validatable input
                // console.log(eventData);
                // console.log('keyup char count: ' + eventData.target.value.length);
                // console.log('KEYUP MODEL BEFORE: ' + scope.dsModel);
                // console.log('KEYUP ELEMENT BEFORE: ' + ele.find('input').val());
                scope.inputTrigger(eventData);

            }
        });


        // ----------------------------------------------
        // add listener to the blur event
        // ----------------------------------------------
        ele.find('input').bind('blur', (eventData) => {
            // console.log('[BLUR FIRED (%s)] Model Val(%s)', eventData.target.name, scope.dsModel);
            // console.log('[BLUR (' + eventData.target.name + ')]');
            //// console.trace();

            if (!this.svc.isSuppressed(scope.frmObj.$name)) {

                let validationId = scope.initValidation();

                // console.log('%cBLUR VALIDATION %s', 'font-size: 16pt;', validationId);

                scope.validate(true, validationId).then(function () {
                    if (!scope.hasError) {

                        // check if a promise should be called to enable view mode on blur
                        // if so, call it and wait for it to resolve before switching to view-mode
                        if (typeof scope.dsEditToViewPromise != 'undefined') {
                            scope.dsEditToViewPromise().then(function () {
                                scope.isViewMode = true;
                                scope.tryApply();
                            });
                        }
                    }
                });

            }

            if (angular.isDefined(attr.dsBlur))
                scope.dsBlur();

            //// console.log('');
            //// console.log('BLUR END');
        });

        // ----------------------------------------------
        // add listener to the change event (no use yet)
        // ----------------------------------------------
        ele.find('input').bind('change', function (eventData) {
            // console.log('[CHANGE FIRED (%s)] Model Val(%s)', eventData.target.name, scope.dsModel);
            // console.log('[CHANGE (' + eventData.target.name + ')]');


        });

        // ----------------------------------------------
        // add listener to the paste event
        // ----------------------------------------------
        $(ele).find('input').bind('paste', (eventData) => {
            // console.log('[PASTE FIRED (%s)] Model Val(%s)', eventData.target.name, scope.dsModel);
            // console.log('TEXTBOX VALUE: %s', scope.inputBoxValue());

            let func = function() {
                scope.inputTrigger(eventData);
            };

            // allows the paste event to complete before validation, otherwise there would be no value in the text box. let's just call this magic; event magic.
            this.$timeout(func, 0);
        });

        // ----------------------------------------------
        // called by the paste or keyup event
        // ----------------------------------------------
        scope.initValidation = function() {
            scope.cancelKeyupTimeoutReference();
            scope.cancelExistingInProgressReport();
            scope.currentValidationId++;
            // console.log('INIT VALIDATION: %s', scope.currentValidationId);
            return scope.currentValidationId;
        };

        // ----------------------------------------------
        // called by the paste or keyup event
        // ----------------------------------------------
        scope.inputTrigger = (eventData) => {
            // console.log('INPUT TRIGGER: %s', scope.inputBoxValue());
            let validationId = scope.initValidation();
            let runCustomRule = !!attr.dsKeyupDelayedValidation;
            scope.dsModel = scope.inputBoxValue().trim(); // mask edit hack: this is because when you hold the key down and enter up to the max char with a mask it doesn't include the mask in the model value.
            scope.cancelKeyupTimeoutReference();

            if (runCustomRule) {
                let delay = isNumber(attr.dsKeyupDelayedValidation) ?
                    +attr.dsKeyupDelayedValidation :
                    500;

                scope.keyupTimeoutReference = this.$timeout(
                    function () {
                        scope.validate(runCustomRule, validationId);
                    },
                    delay
                );

            } else {
                // console.log('IMMEDIATE VALIDATION');
                // console.log('%cIMMEDIATE VALIDATION %s', 'font-size: 16pt;', validationId);
                scope.validate(runCustomRule, validationId);
            }
        };

        // ---------------------------------------------
        // Checks if the given value is a number.
        // from: http://stackoverflow.com/questions/1575390/javascript-parsing-an-integer-from-a-string
        // ---------------------------------------------
        function isNumber(value) {
            return !isNaN(parseFloat(value)) && isFinite(value);
        }

        // ---------------------------------------------
        // the function the service will call
        // ---------------------------------------------
        scope.serviceValidate = function () {
            // note: service validate has only one purpose: point out the errors
            // service validation shouldn't show success or JUST the help message
            // it can include the help message in the case of failure.
            let validationId = scope.initValidation();

            // console.log('DS INPUT SERVICE FUNCTION');
            return scope.validate(true, validationId);
        };

        // ---------------------------------------------
        // execute the validation (built in and custom)
        // ---------------------------------------------
        scope.validate = function (validateCustom, validationId) {
            // console.log('%cDS INPUT VALIDATE', 'font-size: 16pt');
            // console.log('DS INPUT VALIDATE');
            // console.log('STARTING VALIDATION - MODEL: %s', scope.dsModel);
            // console.log('STARTING VALIDATION - VALIDATION ID: %s', validationId);

            // console.log('A: %s', validationId, scope.inputName);
            return scope.validateBuiltIn()
                .then(
                    (validateCustom)
                        ? function(data) {
                            return scope.executeCustomRule(data, validationId);
                        }
                        : function (data) {
                            return data;
                        }
                )
                .then(function(data) {
                    return scope.evaluateResults(data, validationId);
                    // console.log('B: %s', validationId, scope.inputName);
                })
                ['finally'](function () {
                    // console.log('C: %s', validationId, scope.inputName);
                    scope.complete();
                    scope.tryApply();
                });
        };

        // ---------------------------------------------
        // runs after all validation occurs
        // ---------------------------------------------
        scope.evaluateResults = (validationErrorMessages, validationId) => {
            let successful = validationErrorMessages.length === 0;
            // console.log('DS INPUT - EVAL RESULTS - %s - %s - %s', successful, scope.hasError, scope.inputName);
            scope.cancelExistingInProgressReport();

            if (scope.currentValidationId == validationId) {
                // console.log('EVALUATING - EVALUATING - EVALUATING - %s - %s - %s', successful, scope.hasError, scope.inputName);

                scope.showInProgress = false; // if we're here we don't want in progress showing

                if (successful) {

                    if (!$(ele).hasClass(window.HAS_SUCCESS_CLASS_NAME)) {
                        scope.showMessages = false;
                    }

                    // note: scope.hasError holds the value for the previous check.
                    if (scope.hasError) {
                        scope.reportSuccess();
                    }

                    scope.hasError = false;
                } else {
                    scope.reportFailure(validationErrorMessages);
                    scope.hasError = true;
                }

                if (!successful)
                    return this.$q.reject();
            }
        };

        // ---------------------------------------------
        // show the help message ... prepare then show
        // at the time of writing only the focus event
        // will fire this
        // ---------------------------------------------
        scope.reportHelpMessage = function () {
            // console.log('%cREPORTING HELP MESSAGE', 'color:green');

            // note: since this is only called by focus, there will never be a need to add a success or failure message
            // even though they will no be added that doesn't mean the don't exist.
            // in the case of a current error we don't want to remove the error message, but we would like to show the help message.
            // in the case of current success we do want to remove the success status and show the help message staus.
            scope.cancelCurrentSuccessTimeout();

            // console.log('REPORT HELP MSG: %s --- %O', scope.hasError, scope.dsMessages);

            if (!scope.hasError)
                scope.dsMessages = [];

            // console.log('REPORT HELP MSG: %O', scope.dsMessages);

            scope.setMessageAreaForInformation();
            scope.tryApply();
        };

        // ---------------------------------------------
        // executes if it's a show success condition
        // ---------------------------------------------
        scope.reportSuccess = function () {
            // console.log('%cREPORTING SUCCESS', 'color:green');
            // scope.cancelExistingInProgressReport();
            scope.dsMessages = [];
            scope.hasError = false;
            scope.setMessageAreaForSuccess();
            scope.addMessageIfDoesNotExist('Looking Good!');
            scope.tryApply();
        };

        // ---------------------------------------------
        // execute if this is an error condition
        // ---------------------------------------------
        scope.reportFailure = function (newMessages) {
            // console.log('%cREPORTING FAILURE', 'color:green');
            scope.cancelExistingInProgressReport();
            scope.dsMessages = [];
            scope.hasError = true;
            scope.setMessageAreaForError();
            scope.showMessages = true;
            scope.addRangeMessageIfDoesNotExist(newMessages);
            scope.addMessageIfDoesNotExist(scope.dsHelpMessage);
            scope.tryApply();
        };

        scope.hideInProgressReport = function() {
            $(ele).removeClass(window.IN_PROGRESS_CLASS_NAME);
            scope.showInProgress = false;
        };


        // ---------------------------------------------
        // execute if this is an error condition
        // ---------------------------------------------
        scope.reportInProgress = () => {
            // console.log('REPORTING IN PROGRESS: %s - %s', scope.dsModel, scope.currentValidationId);
            scope.cancelExistingInProgressReport();

            scope.inProgressTimeoutReference = this.$timeout(
                function () {
                    // show the in progress indicator
                    // console.log('%cSHOWING IN PROGRESS', 'COLOR:BLUE');
                    scope.configureMessageArea(window.IN_PROGRESS_CLASS_NAME);
                    scope.showMessages = false;
                    scope.showInProgress = true;
                    scope.tryApply();
                },
                700
            );

        };

        scope.initValidationVariables();
        scope.registerWithDsInlineValidatedInputService();
    }
}

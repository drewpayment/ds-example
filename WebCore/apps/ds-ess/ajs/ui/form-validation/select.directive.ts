import * as angular from 'angular';
import { DsInlineValidatedInputService } from './ds-inline-validated-input.service';

export class DsInlineValidatedSelectDirective implements ng.IDirective {
    static DIRECTIVE_NAME = 'dsInlineValidatedSelect';

    restrict = 'E';
    scope = {
        // ----------------------------
        removeFormGroup: '@',   //
        // ----------------------------
        inputName: '@',         // the value you want for the INPUT 'name' and 'id' attribute.
                                // Also it will added to the 'for' attribute of the LABEL.
        inputDivClass: '@',     // the class you supply will be added to the div.input (see template)
        // ----------------------------
        labelText: '@',         // the label text for the field. No need to add ':' it's added automatically
        labelClass: '@',        // the class you supply will be added to the LABEL (see template)
        removeLabel: '@',         // do not include a label
        // ----------------------------
        errorMsgDivClass: '@',  // the class you supply will be added to the div.message-area (see template)
        // ----------------------------
        dsRequired: '@',        // true if field is required. For validation and adds '*' to label
        // ----------------------------
        dsInputHelpMsg: '@',    // message to show on focus or when pattern error occurs
        // ----------------------------
        isNumber: '@',                      // since we can't use the number input type (browser support) we will
                                            // handle converting input to number in the directive. It was found that
                                            // the number type would tell angular to convert the 'string' value in the
                                            // input box to number on the model. We can't be turning number values into
                                            // string values; ain't nobody got time for that.
        // ----------------------------
        dsSelectChangingFunc: '&',    // a function to call when there is a change
        dsSelectChangedFunc: '&',    // a function to call when there is a change
        dsCompleted: '&',           // fires when everything is done ... except the timeout for the LOOKING GOOD message
        dsSelectOptions: '@',       // the options for displaying the options supplied in the model
        // ----------------------------
        dsModel: '=',           // the model variable (see as you normally would for ng-model)
        frmObj: '=',            // the form object that this input box belongs to. I wasn't able to get a reference any
                                // other way.
                                // todo: refactor: (jay) probably a way to do this without having to pass the reference in.
        dsCustomValidationRulePromise: '=',    // this is the rule used if using custom validation
        dsAutoFocusOn: '=',     // bindable attribute which gives the select element focus when the bound expression is truthy
        // ----------------------------
        dsCanClearSuppression: '@',
        dsIncludeDefaultOption: '=',
        dsDefaultOptionText: '@'
    };

    replace = true;

    constructor(private $rootScope: any, private $timeout: ng.ITimeoutService,
        private $q: ng.IQService, private svc: DsInlineValidatedInputService) {
        angular.extend(this, {
            template: require('./select.html')
        });
    }

    static $instance(): ng.IDirectiveFactory {
        const dir = (root, timeout, q, svc) => new DsInlineValidatedSelectDirective(root, timeout, q, svc);
        dir.$inject = [
            '$rootScope',
            '$timeout',
            '$q',
            'DsInlineValidatedInputService'
        ];
        return dir;
    }

    compile(ele, attr) {
        // console.log('%cSELECT DIRECTIVE COMPILE: %s', 'color:red;font-size:16pt', attr.inputName);

        // ----------------------------------------------modify the parent div element
        if (typeof attr.removeFormGroup != 'undefined')
            ele.removeClass('form-group');

        // ----------------------------------------------find the text and label elements
        let dropDown = ele.find('select');
        let lbl = ele.find('label');


        // ----------------------------------------------name & id input element
        dropDown.attr('id', attr.inputName);
        dropDown.attr('name', attr.inputName);

        if (typeof attr.dsSelectOptions != 'undefined') {
            dropDown.attr('ng-options', attr.dsSelectOptions);
        }

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

        if (typeof attr.inputDivClass != 'undefined')
            dropDown.parent().addClass(attr.inputDivClass); // notice it's applying to the parent ... which is the div

        if (typeof attr.errorMsgDivClass != 'undefined')
            $(ele).children('.message-area').addClass(attr.errorMsgDivClass);

        return {
            post: (scope, ele, attr, controller) => this.post(scope, ele, attr, controller)
        };
    }

    post(scope, ele, attr, controller) { // fyi: this is the same as 'link: function(){...}
        const dir = this;
        // console.log('%cINPUT DIRECTIVE LINK: %s', 'color:red;font-size:16pt', attr.inputName);

        // console.log(uniqueName);
        scope.dsMessages = [];
        scope.showingInputHelpText = false;
        scope.showMessages = false;
        scope.hasError = false;

        // ----------------------------------------------
        // try to execute an apply. it won't happen if
        // apply/digest already in motion
        // ----------------------------------------------
        // todo: refactor: (jay) this should be put in the the DsValidatedInputService possibly?
        scope.tryApply = function (func) {
            if (arguments.length === 0) {
                func = function () {
                };
            }

            if (!dir.$rootScope.$root.$$phase) {
                scope.$apply(func);
            }
        };

        // ----------------------------------------------
        // will be true if this is a service validation
        // the service validation occurs when a user clicks
        // on a button a we exectute validate on all the
        // directives that are registered on the service
        // ----------------------------------------------
        scope.isServiceValidate = false;

        // ----------------------------------------------
        // will be set to true if the validation function
        // should execute the custom rule.
        // current true conditions:
        //  if the blur event is triggered
        // ----------------------------------------------
        scope.executeCustomRule = false;

        // ----------------------------------------------
        // gets the value from the input text box
        // this should be the same value as the model
        // but it's going to a be a string which is
        // helpful in some situations
        // ----------------------------------------------
        scope.inputBoxValue = function () {
            return ele.find('select').val();
        };

        // ----------------------------------------------
        // initialize variables
        // ----------------------------------------------
        scope.initValidationVariables = function () {
            scope.showingInputHelpText = false;
        };

        // ----------------------------------------------
        // will be set to true if the view mode is set
        // ----------------------------------------------
        scope.isViewMode = (typeof attr.dsViewModel != 'undefined') && (typeof attr.dsViewModeOverride == 'undefined');

        // ----------------------------------------------
        // check if the select element should be given focus
        // concepts from: http://stackoverflow.com/questions/14833326/how-to-set-focus-in-angularjs
        // ----------------------------------------------
        if (scope.dsAutoFocusOn) {
            this.$timeout(function () { ele.find('select').focus(); });
        }

        // ----------------------------------------------
        // show messages for information
        // ----------------------------------------------
        scope.setMessageAreaForInformation = () => {
            let showInformation =
                attr.dsInputHelpMsg &&
                !scope.hasError &&
                !scope.isServiceValidate &&
                !this.svc.isSuppressed(scope.frmObj.$name);

            if (showInformation) {
                // console.log('SHOWING INFORMATION');
                scope.configureMessageArea(null);
                scope.showMessages = scope.addMessageIfDoesNotExist(attr.dsInputHelpMsg);
            }
        };

        // ----------------------------------------------
        // show messages for in progress
        // ----------------------------------------------
        scope.setMessageAreaForInProgress = function () {

            // if no other special class is assigned
            if (!$(ele).hasClass(window.HAS_SUCCESS_CLASS_NAME)) {
                // console.log('SHOWING IN PROGRESS');
                scope.configureMessageArea(window.IN_PROGRESS_CLASS_NAME);
                // scope.showMessages = scope.addMessageIfDoesNotExist('Validating ...');
            }

        };

        // ----------------------------------------------
        // show messages for error
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
        // show messages for success
        // ----------------------------------------------
        scope.cancelCurrentSuccessTimeout = () => {
            if (scope.timeoutReference) {
                // console.log('CANCELLING CURRENT TIMEOUT');
                this.$timeout.cancel(scope.timeoutReference);
            }
        };

        // ----------------------------------------------
        // show messages for success
        // ----------------------------------------------
        scope.timeOutRemoveSuccess = () => {

            scope.timeoutReference = this.$timeout(
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
        // show messages for success
        // ----------------------------------------------
        scope.configureMessageArea = function (className) {
            // console.log('CONFIGURE MSG AREA BEFORE classes=[%s]', $(ele).attr('class'));
            $(ele).removeClass(window.HAS_ERROR_CLASS_NAME + ' ' + window.HAS_SUCCESS_CLASS_NAME + ' ' + window.IN_PROGRESS_CLASS_NAME);

            if (className) {
                $(ele).addClass(className);
            }

            // console.log('CONFIGURE MSG AREA AFTER classes=[%s]', $(ele).attr('class'));
        };

        // ----------------------------------------------
        // will add the the message to the messages array
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
                skipOthers = false,
                isRequired = !!attr.dsRequired,
                isEmptyString = !scope.dsModel;

            // console.log('--------------------------------------------------------');
            // console.log('VALIDATE STANDARD CALLED');
            // console.log('--------------------------------------------------------');
            // console.log('INPUT NAME: ---' + attr.inputName + '----');
            // console.log('TXT BOX: ---' + txtValue + '----');
            // console.log('MODEL:   ---' + scope.dsModel + '----');
            // console.log('IS EMPTY: ---' + isEmptyString + '----');

            if (!scope.isServiceValidate || isRequired || (!isEmptyString && scope.isServiceValidate)) {
                // if (typeof attr.dsRequired != "undefined") {
                // console.log('VALIDATE BUILT IN: VALIDATING REQUIRED');
                if (isEmptyString && isRequired) {
                    // console.log('REQUIRED ERROR');
                    skipOthers = true;
                    messages.push(scope.getBuiltInErrorMessageByKey('required'));
                }
                // }


                if (!skipOthers && attr.isNumber) {
                    if (isEmptyString || isNaN(scope.dsModel)) {
                        // console.log('NAN ERROR');
                        messages.push(scope.getBuiltInErrorMessageByKey('required')); // required is really the only issue we could have here. here were saying if it's not a number it's not a valid selection.
                    } else {
                        let tmp = +scope.dsModel; // converts to number
                        scope.dsModel = tmp; // we convert model value (if marked as 'isNumber' so the model can be properly compared with original (within angular; mostly in controllers for isModified)
                        // console.log('CONVERTED TO NUMBER');
                    }
                }

            }

            // console.log('VALIDATE BUILT IN - MESSAGES: %O', messages);
            deferred.resolve(messages);
            return deferred.promise;
        };

        // ---------------------------------------------
        // executes the custom validation
        // ---------------------------------------------
        scope.executeCustomRule = (builtInMessages) => {
            // console.log('DS INPUT  - EXECUTE CUST RULE: ' + scope.inputName);
            let deferred = this.$q.defer();

            if (attr.dsCustomValidationRulePromise && builtInMessages.length === 0) {
                // console.log('DS INPUT  - STARTING EXECUTE CUST RULE: ' + scope.inputName);
                // console.log(builtInMessages);

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
        // clear validation suppression if it exists for
        // the form object
        // ----------------------------------------------
        scope.tryClearingSuppression = () => {
            if (attr.dsCanClearSuppression) {
                this.svc.clearSuppressed(scope.frmObj.$name);
            }
        };

        // ----------------------------------------------
        // add listener to the focus event
        // ----------------------------------------------
        ele.find('select').bind('focus', function (eventData) {
            // console.log('[FOCUS (' + eventData.target.name + ')]');

            scope.tryClearingSuppression();

            scope.tryApply(function () {
                scope.setMessageAreaForInformation();
            });
        });

        // ----------------------------------------------
        // determine if the keyup event should cause validation
        // ----------------------------------------------
        scope.shouldValidateAfterKeyupEvent = function (eventData) {
            // http://www.cambiaresearch.com/articles/15/javascript-char-codes-key-codes

            if (eventData.ctrlKey && eventData.keyCode <= 86) // paste ctrl+V
                return false;

            if (eventData.keyCode >= 9 && eventData.keyCode <= 46)
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
        // add listener to the keyup event
        // ----------------------------------------------
        ele.find('select').bind('keyup', function (eventData) {
            // console.log('[KEYUP FIRED: (' + eventData.target.name + ')]');

            if (scope.shouldValidateAfterKeyupEvent(eventData)) {
                // console.log('[ALLOWING --- KEYUP FIRED: (' + eventData.target.name + ')]');
                scope.dsSelectChangedFunc();
            }
        });

        // ----------------------------------------------
        // add listener to the blur event
        // ----------------------------------------------
        ele.find('select').bind('blur', function (eventData) {
            // console.log('[BLUR (' + eventData.target.name + ')]');
        });

        // ----------------------------------------------
        // add listener to the change event (no use yet)
        // ----------------------------------------------
        ele.find('select').bind('change', function (eventData) {
            // console.log('[CHANGE (' + eventData.target.name + ')]');
            scope.dsSelectChangedFunc();
        });

        //////// ----------------------------------------------
        //////// add listener to the click event (no use yet)
        //////// ----------------------------------------------
        ////// ele.find('select').bind('click', function (eventData) {
        //////    //console.log('[SELECT-CLICK (' + eventData.target.name + ')]');
        ////// });

        // ----------------------------------------------
        // add listener to the paste event
        // ----------------------------------------------
        $(ele).find('select').bind('paste', function (eventData) {
            // console.log('[PASTE (' + eventData.target.name + ')]');
            //// console.log('TEXTBOX VALUE: %s', scope.inputBoxValue());
            //// ($ele).find('input').val();

            // var func = function () {
            //    scope.inputTrigger(eventData);
            // };

            //// allows the paste event to complete before validation, otherwise there would be no value in the text box. let's just call this magic; event magic.
            // $timeout(func, 0);
        });

        // ---------------------------------------------
        // the function the service will call
        // ---------------------------------------------
        scope.serviceValidate = function () {
            // console.log('DS INPUT SERVICE FUNCTION');
            return scope.validate(true);
        };

        // ---------------------------------------------
        // execute the validation (built in and custom)
        // ---------------------------------------------
        scope.validate = function (validateCustom) {
            // console.log('%cDS INPUT VALIDATE', 'font-size: 16pt');
            // scope.setMessageAreaForInProgress(); //or spinning icon

            // console.log('DS INPUT VALIDATE');
            // console.trace();
            return scope.validateBuiltIn()
                .then(
                    (validateCustom)
                        ? scope.executeCustomRule
                        : function (data) {
                            return data;
                        }
                )
                .then(scope.evaluateResults)
                ['finally'](function () {
                    scope.complete();
                });
        };

        // ---------------------------------------------
        // runs after all validation occurs
        // ---------------------------------------------
        scope.evaluateResults = (validationErrorMessages) => {
            // console.log('DS INPUT - EVAL RESULTS');
            let successful = validationErrorMessages.length === 0;

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
        };

        // ---------------------------------------------
        // executes if it's a show success condition
        // ---------------------------------------------
        scope.reportSuccess = function () {
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
            scope.dsMessages = [];
            scope.hasError = true;
            scope.setMessageAreaForError();
            scope.showMessages = true;
            scope.addRangeMessageIfDoesNotExist(newMessages);
            scope.addMessageIfDoesNotExist(scope.dsHelpMessage);
            scope.tryApply();
        };

        // --------------------------------------------------------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------------------------------------------------------
        // todo: refactor: (jay) the functions (not the statements) below this line are specific to the select (for now)
        // --------------------------------------------------------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------------------------------------------------------

        // ---------------------------------------------
        // executes when the ds model value changes
        // ---------------------------------------------
        scope.dsModelValueChanged = (newValue, oldValue) => {
            // scope.dsSelectChangingFunc();
            // console.log('DS-SELECT -- MODEL CHANGED: ' + attr.inputName);
            // console.log('DS-SELECT -- MODEL CHANGED: ' + scope.dsModel);
            // console.log('DS-SELECT -- old: ' + oldValue);
            // console.log('DS-SELECT -- new: ' + newValue);
            let result = this.svc.isSuppressed(scope.frmObj.$name);

            if (!result) {
                if (!(newValue === oldValue)) {
                    scope.validate(true);
                }
            }
            // scope.dsSelectChangedFunc();
        };

        // ---------------------------------------------
        // sets up a handler for watching
        // changes to the ds model
        // ---------------------------------------------
        scope.modelWatchHandler = scope.$watch('dsModel', scope.dsModelValueChanged);

        scope.initValidationVariables();
        scope.registerWithDsInlineValidatedInputService();
    }
}










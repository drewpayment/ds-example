import * as angular from 'angular';
import { DsInlineValidatedInputService } from './ds-inline-validated-input.service';

export class DsInlineValidatedRadioDirective implements ng.IDirective {
    static readonly DIRECTIVE_NAME = 'dsInlineValidatedRadio';
    restrict = 'E';
    scope = {
        // ----------------------------
        inputName: '@',         // the value you want for the INPUT 'name' and 'id' attribute. 
        // Also it will added to the 'for' attribute of the LABEL.
        inputDivClass: '@',     // the class you supply will be added to the div.input (see template)
        // ----------------------------
        labelText: '@',         // the label text for the field. No need to add ':' it's added automatically
        labelClass: '@',        // the class you supply will be added to the LABEL (see template)
        // ----------------------------
        errorMsgDivClass: '@',  // the class you supply will be added to the div.message-area (see template)
        // ----------------------------
        dsRequired: '@',        // true if field is required. For validation and adds '*' to label
        // ----------------------------
        dsInputHelpMsg: '@',    // message to show on focus or when pattern error occurs.
        // ----------------------------
        dsChangeFunc: '@',      // a function to call when there is a change //todo: (jay) this isn't tested at
        // this time. not sure if there will be a need for this
        dsRadioOptions: '=',    // the txt/val object array pair that should show
        // example: [{ txt: 'Male', val: 'M' }, { txt: 'Female', val: 'F' }]
        // ----------------------------
        dsModel: '=',           // the model variable (see as you normally would for ng-model)
        frmObj: '=',            // the form object that this input box belongs to. I wasn't able to get a reference
        // any other way. //todo: refactor: (jay) probably a way to do this without having to pass the reference in.
        dsCustomValidationRule: '=',    // this is the rule used if using custom validation
        dsCustomErrorMessages: '='      // this is used for executing a validation rule outside of the basic built
        // in stuff (required, max/min length, pattern). See one note for more information on use.
    };
    replace = true;

    constructor(private $rootScope: any, private $timeout: ng.ITimeoutService, private svc: DsInlineValidatedInputService) {
        angular.extend(this, {
            template: require('./radio.html')
        });
    }

    static $instance(): ng.IDirectiveFactory {
        const dir = (root, time, svc) => new DsInlineValidatedRadioDirective(root, time, svc);
        dir.$inject = [
            '$rootScope',
            '$timeout',
            'DsInlineValidatedInputService'
        ];
        return dir;
    }

    compile(ele, attr) {
        // ----------------------------------------------find the text and label elements
        let radio = ele.find('input');
        let lbl = ele.find('label')[0];

        // ----------------------------------------------name & id input element
        radio.attr('id', attr.inputName);
        radio.attr('name', attr.inputName);

        // ----------------------------------------------add ng select attributes
        if (typeof attr.dsSelectChangeFunc != 'undefined') {
            radio.attr('ng-change', attr.dsChangeFunc + '()');
        }

        // ----------------------------------------------add classes
        if (typeof attr.labelClass != 'undefined')
            $(lbl).addClass(attr.labelClass);

        if (typeof attr.inputDivClass != 'undefined')
            radio.parent().parent().addClass(attr.inputDivClass); // notice it's applying to the parent ... which is the div

        if (typeof attr.errorMsgDivClass != 'undefined')
            $(ele).children('.message-area').addClass(attr.errorMsgDivClass);

        // ----------------------------------------------setup anglar validation attributes
        if (typeof attr.dsRequired != 'undefined')
            radio.attr('required', '1');

        return {
            post: (scope, ele, attr, controller) => this.post(scope, ele, attr, controller)
        };
    }

    post(scope, ele, attr, controller) { // fyi: this is the same as 'link: function(){...}
        // -----------------------------------------------EXECUTE A DIGEST IF WE HAVE TO
        scope.tryDigest = () => {
            if (!this.$rootScope.$root.$$phase) {
                scope.$digest();
            }
        };

        // -----------------------------------------------INITIALIZE VARIABLES
        scope.initValidationVariables = function () {
            scope.showingInputHelpText = false;
            scope.showSuccess = false;
            scope.showMessages = false;
            scope.frmGroupClass = null;
            scope.dsMessages = [];
            scope.needToShowSuccess = scope.hasErrors;
        };

        // ----------------------------------------------------GET A BASIC ERROR MESSAGE (not custom)
        scope.getMessages = function (key) {
            switch (key) {
                case 'required':
                    return 'This field is required.';
            }
        };

        scope.setMessageArea = function () {
            scope.showSuccess = scope.needToShowSuccess && !scope.hasErrors;
            scope.frmGroupClass = (scope.hasErrors === true) ? 'has-error' : 'has-success';
            scope.showMessages = scope.dsMessages.length > 0;

            if (scope.showSuccess || scope.hasErrors === true) {
                $(ele).addClass(scope.frmGroupClass);
            } else {
                $(ele).removeClass('has-error has-success');
            }

            if (scope.showSuccess) {
                scope.dsMessages = ['Looking Good!'];
                scope.showMessages = true;
            }

            scope.tryDigest();
        };

        // -------------------------------------------CALLED AFTER ALL VALIDATION HAS TAKEN PLACE
        scope.grandFinale = () => {
            scope.setMessageArea();

            if (scope.showSuccess) {
                scope.timeoutReference = this.$timeout(
                    function () {
                        scope.dsMessages = [];
                        scope.showMessages = false;
                        scope.showSuccess = false;
                        scope.frmGroupClass = null;
                        $(ele).removeClass('has-error has-success');

                        if (scope.showingInputHelpText) {
                            scope.showMessages = true;
                            scope.addInputHelpMessage();
                        }

                    }
                    , 2000
                );

            }
        };

        // -------------------------------------------will add input help message if one isn't present in the array of messages
        scope.addInputHelpMessage = function () {
            let result = scope.dsMessages.some(function (element, index, array) {
                return element == attr.dsInputHelpMsg;
            });

            if (!result) {
                scope.dsMessages.push(attr.dsInputHelpMsg);
            }
        };

        // ----------------------------------------------LISTEN FOR CHANGES IN THE CUSTOM ERROR OBJECT
        if (typeof scope.dsCustomValidationRule != 'undefined') {
            scope.$watch(function () {
                return scope.dsCustomErrorMessages;
            },
            function (newObj, oldObj) {
                if (newObj != oldObj) {
                    scope.hasErrors = scope.dsCustomErrorMessages.length > 0;

                    if (scope.hasErrors)
                        scope.addInputHelpMessage();

                    for (let x = 0; x < scope.dsCustomErrorMessages.length; x++) {
                        scope.dsMessages.push(scope.dsCustomErrorMessages[x]);
                    }

                    scope.tryDigest();
                    scope.grandFinale();
                }

            });
        }

        // ----------------------------------------------------------------------------DETERMINE IF THE SCOPE VALUE IS VALID
        scope.isDsScopeValueValid = function () {

            let isValid =
                typeof scope.dsModel != 'undefined' &&
                scope.dsModel.toString().trim() != '';

            // console.log('RADIO IS VALID: ' + isValid);

            return isValid;
        };

        // ----------------------------------------------------------------------------BLUR BLUR BLUR
        scope.validate = function (callCustom) {
            scope.initValidationVariables();
            scope.hasErrors = false;

            if (!scope.isDsScopeValueValid()) {
                scope.dsMessages = [scope.getMessages('required')];
                scope.addInputHelpMessage();
                scope.hasErrors = true;
            }

            // execute the rule from the supplied custom error object
            if (callCustom && typeof scope.dsCustomValidationRule != 'undefined') {
                scope.dsCustomValidationRule();
            } else {
                scope.grandFinale();
            }

            return scope.hasErrors;
        };

        // ------------------------------------------------------------------REGISTERS THIS DIRECTIVE WITH THE INLINE SERIVCE
        scope.registerWithDsInlineValidatedInputService = () => {
            let obj = {
                validationPromise: scope.validate,
                frmName: scope.frmObj.$name,
                inputName: attr.inputName,
                isCustom: (typeof scope.dsCustomValidationRule != 'undefined')
            };

            this.svc.registerDirective(obj);
        };

        // ----------------------------------------------------EXECUTES WHEN A CHANGE HAPPENS ON THE MODEL OBJECT
        scope.dsModelValueChanged = function (newValue, oldValue) {

            // console.log('RADIO SCOPE VAL CHANGED');
            // console.log('NEW VAL' + newValue);
            // console.log('OLD VAL' + oldValue);

            // if (newValue != oldValue) {
                scope.validate(true);
            // }
        };

        // ----------------------------------------------------WATCHES FOR MODEL OBJECT CHANGES AND CALL FUNCTION THAT VALIDATES AFTE THE CHANGE
        // scope.$watch('dsModel', scope.dsModelValueChanged);

        // ----------------------------------------------------FINAL INTIALIZATION STUFF
        scope.initValidationVariables();
        scope.registerWithDsInlineValidatedInputService();
    }

}

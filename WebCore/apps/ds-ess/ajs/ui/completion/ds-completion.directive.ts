import * as angular from "angular";
import { CompletionController } from "./completion.controller";

/**
 * Directive used to construct a navigational menu within the ESS application.  The menu monitors state changes and
 * will update the menu's active menu-item based on the current state (see states.js).
 *
 * @ngdirective
 */
export class DsCompletionDirective implements ng.IDirective {
    static readonly DIRECTIVE_NAME = 'dsCompletion';

    restrict = 'E'; // restrict to element (eg: <nav-menu />)
    controller = CompletionController;
    replace: true; // replace the directive element w/ the template html
    scope = {
        completionId: '@',  // 'id' attribute to be applied to the underlying <ul> element of the menu html
        showMe: '=parm'
        //completionVisible: '='
        //menu: '='     // Ess.Services~Menu() object (see Menu.js) containing the menu-items & configuration to display
    };
    constructor(){
        angular.extend(this, {
            template: require('./ds-completion.html')
        })
    }

    static $instance(): ng.IDirectiveFactory {
        let dir = () => new DsCompletionDirective();
        dir.$inject = [];
        return dir;
    }
}
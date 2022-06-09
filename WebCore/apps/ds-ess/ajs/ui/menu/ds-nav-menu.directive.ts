import * as angular from "angular";

/**
 * Directive used to construct a navigational menu within the ESS application.  The menu monitors state changes and
 * will update the menu's active menu-item based on the current state (see states.js).
 *
 * @ngdirective
 */
export class DsNavMenuDirective implements ng.IDirective {
    static readonly DIRECTIVE_NAME = "dsNavMenu";

    restrict = 'E';         // restrict to element (eg: <nav-menu />)
    replace = true;         // replace the directive element w/ the template html
    scope = {
        menuId: '@',  // 'id' attribute to be applied to the underlying <ul> element of the menu html
        menu: '='     // Ess.Services~Menu() object (see Menu.js) containing the menu-items & configuration to display

    };
    constructor(){
        angular.extend(this, {            
            template: require('./ds-nav-menu.html')
        })
    }

    static $instance(): ng.IDirectiveFactory {
        let dir = () => new DsNavMenuDirective();
        return dir;
    }
}
import * as angular from "angular";

/**
 * Directive used to construct a navigational menu within the ESS application.  The menu monitors state changes and
 * will update the menu's active menu-item based on the current state (see states.js).
 *
 * @ngdirective
 */
export class DsNavWorkflowDirective implements ng.IDirective {
	static readonly DIRECTIVE_NAME = "dsNavWorkflow";
	restrict: 'E';         // restrict to element (eg: <nav-menu />)
	replace: true;         // replace the directive element w/ the template html

	constructor(){
		angular.extend(this, {
			template: require('./ds-nav-workflow.html')
		})
	}

	static $instance(): ng.IDirectiveFactory {
		let dir = () => new DsNavWorkflowDirective();
		return dir;
	}
}
/**
 * The run() method is called when the angular app is first initialized.  Therefore, we can setup any application-wide
 * logic here, such as app-wide route change management and authentication checks. 
 */
export class DsMobileAppModuleRun {
    static $inject = ['$window', '$rootScope', '$compile'];

    constructor(
        private $window: ng.IWindowService, 
        private $rootScope: ng.IRootScopeService, 
        private $compile: ng.ICompileService) {
        this.$onInit();
    }

    $onInit() {
        this.$window._$compile = this.$compile;
        this.$window._$rootScope = this.$rootScope;
    }

    static factory() {
        return [...DsMobileAppModuleRun.$inject, (win, root, comp) => new DsMobileAppModuleRun(win, root, comp)];        
    }
}

window.compileElement = (selector) => {
    var element = $(selector);
    var html = $("<div />").append(element.clone()).html().trim();
    if (html) {
        var built = window._$compile(html)(window._$rootScope);
        element.replaceWith(built);
    }
}

window.reloadAngular = () => {
    window.compileElement("#inputPunches");
}


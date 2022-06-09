import * as angular from 'angular';

export const AjsComponent = (options: ng.IComponentOptions) => {
    return controller => angular.extend(options, { controller });    
};

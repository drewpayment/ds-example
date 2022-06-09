import { DefaultUrlSerializer, UrlTree, Route } from '@angular/router';
import * as angular from 'angular';

export class LowerCaseUrlSerializer extends DefaultUrlSerializer {
    parse(url: string): UrlTree {
        let tmp = url;
        
        if (url.includes('?')) {
            let urlArray = url.split('?');
            tmp = urlArray[0];
            tmp = tmp.toLowerCase();
            tmp = tmp + '?' + urlArray.splice(1, urlArray.length).join("?");
        } else {
            tmp = tmp.toLowerCase();
        }
        
        return super.parse(tmp);
    }
}

export interface AjsComponentOptions extends ng.IComponentOptions {
    moduleOrName: string | ng.IModule;
    selector: string;
}

export function AjsComponent(opts: AjsComponentOptions) {
    return (controller: Function) => {
        const module = typeof opts.moduleOrName === 'string'
            ? angular.module(opts.moduleOrName) : opts.moduleOrName;
        module.component(opts.selector, opts);
    };
}

export function printAngularRouterPaths(parent: String, config: Route[]) {
    for (let i = 0; i < config.length; i++) {
        const route = config[i];
        console.log(parent + '/' + route.path);
        if (route.children) {
            const currentPath = route.path ? parent + '/' + route.path : parent;
            printAngularRouterPaths(currentPath, route.children);
        }
    }
}

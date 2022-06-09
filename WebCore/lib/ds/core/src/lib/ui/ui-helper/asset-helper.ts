import { Injectable, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';


@Injectable({
    providedIn: 'root'
})
export class AssetHelperService {
    
    constructor(@Inject('window') private window: any, @Inject(DOCUMENT) private document: Document) {}
    
    /**
     * This will try to get the current site's envirment with the appended "site environment" from the ASPX 
     * assuming it was set on the window object. This was specifically designed for the mobile site where we 
     * have a normal origin, but then have the appended mobile folder structure. In this state, we are unable to 
     * figure out what the api url and base url should be and cannot set the base url on the page because 
     * angular router will rewrite the URL in the browser causing issues if the user tries to refresh the page.
     * 
     * If you don't have a `window.apiUrl` variable available, you can still path the entire path after the f
     * location.origin that you would like to use and it will work... 
     */
    resolveAsset(path: string): string {
        /**
         * this.window.apiUrl must be a window variable set from the aspx page, so the sever communicates the site we're on
         */
        const baseUrlSegment = this.window ? this.window.apiUrl : '';
        const host = window && this.window.location ? this.window.location.origin : this.document.location.origin || '';
        
        if (path.charAt(0) == '/') {
            path = path.substring(1, path.length);
        }
        
        return `${host}${baseUrlSegment}${path}`;
    }
    
    resolveBaseUrl(path?: string): string {
        const hostSegment = this.window.apiUrl || '';
        const baseUrl = this.document.location.origin + hostSegment;
        return path ? `${baseUrl}${path}` : baseUrl; 
    }
    
}

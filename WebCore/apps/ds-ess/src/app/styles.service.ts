import { Injectable, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { Router, NavigationEnd } from '@angular/router';

/**
 * Until the final two modules are converted to rely on main.css, or the pages are converted to
 * Angular, we need a way to check and make sure we know what stylesheet to use for each page.
 */
export const ESS_STYLES = {
    profile: 'main',
    pay: 'main',
    timeoff: 'main',
    'account-settings': 'main',
    resources: 'main',
    benefits: 'main',
    performance: 'main'
};

@Injectable({
    providedIn: 'root'
})
export class EssStyleService {
    readonly domTag = '[ds-styles-service]';
    baseStyleLink: string;
    baseStylesheetName: string;

    constructor(
        @Inject(DOCUMENT) private document: Document,
        private router: Router
    ) {
        const linkElement = this.document.querySelector(this.domTag) as HTMLLinkElement;
        if (linkElement) {
            this.baseStyleLink = linkElement.href;
            this.setBaseStylesheetName();
        }

        // this.router.events.subscribe(event => {
        //     if (event instanceof NavigationEnd) {
        //         this.calculateDestinationStylesheet(event.url);
        //     }
        // });
    }

    calculateDestinationStylesheet(url: string) {
        url = url.trim().toLowerCase();
        const slashParts = url.split('/');

        for (const p in ESS_STYLES) {
            const index = slashParts.indexOf(p);

            if (index > -1) {
                // gets the 'term' that relates to the keys in ESS_STYLES
                const styleKeyName = slashParts[index].trim().toLowerCase();
                const newStyleType = ESS_STYLES[styleKeyName];

                if (!this.baseStylesheetName.includes(newStyleType)) {
                    this.changeStylesheet(newStyleType);
                }
            }
        }
    }

    changeStylesheet(dest: string) {
        let deleteStyles = '';
        switch (dest) {
            case 'ess':
                this.baseStylesheetName = this.baseStylesheetName.replace('main', dest);
                deleteStyles = 'main';
                break;
            case 'main':
                this.baseStylesheetName = this.baseStylesheetName.replace('ess', dest);
                deleteStyles = 'ess';
                break;
        }

        const parts = this.baseStyleLink.split('/');
        parts[parts.length - 1] = this.baseStylesheetName;
        this.baseStyleLink = parts.join('/');

        const lnkElem = this.document.querySelector(this.domTag) as HTMLLinkElement;
        lnkElem.href = this.baseStyleLink;
        this.clearOldStyles(deleteStyles);
    }

    // todo: NOT SURE I LIKE THIS
    clearOldStyles(styleName: string) {
        const lnkElems = this.document.querySelectorAll('link[rel="stylesheet"]') as NodeList;

        for (let i = 0; i < lnkElems.length; i++) {
            const lnk = lnkElems.item(i) as HTMLLinkElement;

            if (lnk.href.includes(styleName) && lnk.href.includes('css')
                && !lnk.href.trim().toLowerCase().includes(this.baseStylesheetName.trim().toLowerCase())) {
                lnk.remove();
            }
        }
    }

    private setBaseStylesheetName() {
        if (!this.baseStyleLink) return;
        const baseParts = this.baseStyleLink.split('/');
        this.baseStylesheetName = baseParts[baseParts.length - 1];
    }

}

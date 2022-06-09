import { ConfigUrlType, ConfigUrl } from '../shared/config-url.model';
import { UrlParts } from '../shared';
import { InjectionToken } from '@angular/core';

export const APP_CONFIG = new InjectionToken<AppConfig>('app.config');

/**
 * Joins two parts of a URL with a slash if needed.
 *
 * @param start  URL string
 * @param end    URL string
 *
 *
 * @returns The joined URL string.
 */
export function joinWithSlash(start: string, end: string): string {
    if (start.length == 0) {
        return end;
    }
    if (end.length == 0) {
        return start;
    }
    let slashes = 0;
    if (start.endsWith('/')) {
        slashes++;
    }
    if (end.startsWith('/')) {
        slashes++;
    }
    if (slashes == 2) {
        return start + end.substring(1);
    }
    if (slashes == 1) {
        return start + end;
    }
    return start + '/' + end;
}

export class AppConfig {
    baseSite: ConfigUrl;
    constructor(_baseSite: ConfigUrl) {
        this.baseSite = _baseSite;
    }
}

export function fetchSiteUrls(): Promise<AppConfig> {
    const urlParts = UrlParts.ParseUrl(document.location.href);

    return fetch(urlParts.joinApiPath('api/anonaccount/site-urls'))
        .then(resp => {
            if (200 >= resp.status && resp.status <=499)
                return resp.json();
            return null;
        })
        .then((resp: ConfigUrl[]) => {
            if (resp == null || !Array.isArray(resp))
                throw Error('Unable to bootstrap app. Not able to retrieve site configuration. Error at: fetchSiteUrls');
            const currSite = resp.find(c =>
                equalUrls(`${urlParts.protocol}//${urlParts.host}`, c.url, urlParts.toString()));
            return new AppConfig(currSite);
        });
}

export function equalUrls(baseUrl: string, first: string, second: string): boolean {
    const firstUrl = new URL(first, baseUrl);
    const secondUrl = new URL(second, baseUrl);
    return (
        first && second &&
        secondUrl.pathname.replace(/\/?$/, '/').toLowerCase().includes(firstUrl.pathname.replace(/\/?$/, '/').toLowerCase())
    );
}

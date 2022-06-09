import { coerceNumberProperty } from '@angular/cdk/coercion';
import { isObject } from '@util/coercion';

export interface IUrlParts {
    protocol: string;
    hostname: string;
    port: number;
    pathname: string;
    search: string;
    hash: string;
    host: string;
    parts: string[];
    href: string;
}

export class UrlParts implements IUrlParts {

    constructor(public href: string) {}
    protocol: string;
    hostname: string;
    private _pathname: string;
    set pathname(value: string) {
        this._pathname = value != null ? value.trim() : value;
    }
    get pathname(): string {
        return this._pathname;
    }
    port: number;
    search: string;
    hash: string;
    host: string;
    parts: string[];

    static ParseUrl(href: string): UrlParts {
        return new UrlParts(href).getParts();
    }

    getParts(): UrlParts {
        const parts = new UrlParts(this.href);
        if (!this.href) return parts;

        const a = document.createElement('a');
        a.href = this.href;

        parts.protocol = a.protocol;
        parts.hostname = a.hostname;
        parts.pathname = a.pathname;
        parts.port = coerceNumberProperty(a.port);
        parts.search = a.search;
        parts.hash = a.hash;
        parts.host = a.host;
        parts.parts = parts.pathname.split('/').filter(p => p);

        return parts;
    }

    toString() {
        const search = this.search && this.search.length > 1
            ? this.search : '';
        return `${this.protocol}//${this.hostname}${this.pathname}${search}`;
    }

    /**
     * Compares this URL to another URL.
     *
     * @param c Either an initialized UrlParts or URL string to be compared. String will be
     * initialized and compared if passed.
     * @param strict Determines whether we should normalized the URLs with `toString()` and
     * strictly compare the strings, or if `false` just compare parts of URLs disregarding
     * the `toString()` representation.
     */
    isEqualTo(c: UrlParts | string, strict?: boolean): boolean {
        let thisUrl = '';
        let cUrl = '';
        let result = false;
        const cSafe = isObject(c) ? c as UrlParts : UrlParts.ParseUrl(c as string);

        if (strict) {
            thisUrl = (this.toString() + this.hash);
            cUrl = (cSafe.toString() + cSafe.hash);
            result = thisUrl === cUrl;
        } else {
            thisUrl = this.pathname.trim().toLowerCase();
            cUrl = cSafe.pathname.trim().toLowerCase();
            result = thisUrl === cUrl;

            if (result) {
                let hash = this.hash;
                let cSafeHash = cSafe.hash;
                if (hash.includes( '?'))
                    hash = hash.split('?')[0];

                hash = hash.replace(/\//g,'');
                cSafeHash = cSafeHash.replace(/\//g,'');

                // if (hash.substring(hash.length-1, hash.length))
                //     hash = hash.slice(0,-1);
                result = hash.trim().toLowerCase() === cSafeHash.trim().toLowerCase();
            }
        }

        return result;
    }

    isAspxMatch(c: UrlParts | string): boolean {
        const cSafe = isObject(c) ? c as UrlParts : UrlParts.ParseUrl(c as string);

        if (this.pathname.toLowerCase().includes(cSafe.pathname.toLowerCase())) return true;
        if (cSafe.pathname.toLowerCase().includes(this.pathname.toLowerCase())) return true;

        return false;
    }

    addPath(path: string): UrlParts {
        return new UrlParts(this.joinWithSlash(this.href, path));
    }

    /**
     * Adds a path to the current URL and handles merging them neatly.
     */
    joinPath(path: string): string {
        return this.joinWithSlash(this.hostname, path);
    }

    /**
     * Attempts to guess the host API path that fits Dominion's API convention.
     * E.g. https://live.dominionsystems.com/payroll/,
     *  https://live.dominionsystems.com/ess/
     */
    joinApiPath(path: string): string {
        const hostAndPath = this.parts[0] + '/' + path;
        return this.joinPath(hostAndPath);
    }

    /**
     * Joins two parts of a URL with a slash if needed.
     *
     * @param start  URL string
     * @param end    URL string
     *
     *
     * @returns The joined URL string.
     */
    joinWithSlash(start: string, end: string): string {
        if (start.length == 0) {
            return end;
        }

        start = start.startsWith('http') ? start : `${this.protocol}//${start}`;

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


}

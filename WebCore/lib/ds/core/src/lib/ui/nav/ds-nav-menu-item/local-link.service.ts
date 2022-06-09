import { Injectable, Inject } from '@angular/core';
import { PathLocationStrategy, APP_BASE_HREF, DOCUMENT } from '@angular/common';
import { APP_CONFIG, AppConfig } from '@ds/core/app-config/app-config';


@Injectable()
export class LocalLinkService {
    private _baseHref: string;
    set baseHref(value: string) {
        this._baseHref = value;
    }
    get baseHref(): string {
        return this._baseHref;
    }

    constructor(@Inject(APP_CONFIG) private config: AppConfig, @Inject(DOCUMENT) private document: Document) {}

    isLocalLink(commands: string[]): boolean {
        let result = true;

        const isAspxDestination = commands.find(command => command.toLowerCase().includes('.aspx')) != null;

        if (isAspxDestination) return result = false;

        if (commands.length === 1 && commands[0].startsWith('http')) {
            return commands[0].includes(this.config.baseSite.url);
        }

        return result;
    }

}

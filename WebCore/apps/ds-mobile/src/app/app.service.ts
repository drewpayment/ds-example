import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { skipWhile } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class AppService {
    private _baseUrl$ = new BehaviorSubject<string>(null);
    baseUrl$ = this._baseUrl$.pipe(skipWhile(b => !b));
    hasTimePolicy$ = new BehaviorSubject<boolean>(true);

    constructor() {}

    updateBaseUrl(url: string) {
        if (!url) return;
        this._baseUrl$.next(url);
    }

}

import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { IPopupSettings } from 'lib/models/src/lib/popup-settings.model';
import { IPopup } from 'lib/models/src/lib/popup.model';
import {  Observable, Subject, timer } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class PopupService {

  private _closed$ = new Subject();

  constructor(
    @Inject('window') private window: Window,
    private router: Router
  ) {}

  open(url: string, windowName: string, settings) {
    url = this.getAbsoluteUrl(url);
    return new DsPopupInstance(url, windowName, settings);
  }

  private getAbsoluteUrl(url: string): string {
    let baseUrl: string = '';

    if (
      /^(https?|file|ftps?|mailto|javascript|data:image\/[^;]{2,9};):/i.test(
        url
      )
    ) {
      return url; // url is already absolute
    }

    baseUrl = this.router.parseUrl(url).root.toString();

    if (url.substring(0, 2) === '//') {
      return location.protocol + url;
    }
    if (url.charAt(0) === '/') {
      return location.protocol + '//' + location.host + url;
    }
    if (url.substring(0, 2) === './') {
      url = '.' + url;
    } else if (/^\s*$/.test(url)) {
      return ''; // empty = return nothing
    }

    url = '../' + url;

    while (/\/\.\.\//.test(url)) {
      url = url.replace(/[^\/]+\/+\.\.\//g, '');
    }

    // add the base URL segment to the URL
    url = baseUrl + url.substring(1, url.length);

    url = url
      .replace(/\.$/, '')
      .replace(/\/\./g, '')
      .replace(/"/g, '%22')
      .replace(/'/g, '%27')
      .replace(/</g, '%3C')
      .replace(/>/g, '%3E');

    return url;
  }
}

export class DsPopupInstance {
  popup: IPopup;
  constructor(
    private url: string,
    private windowName: string,
    private settings: IPopupSettings
  ) {
    this.popup = {
      url: this.url,
      windowName: this.windowName,
      settings: this.settings,
    };

    this.open();
  }

  private open() {
    const params = this.buildParams();

    this.popup.window = window.open(this.popup.url, this.popup.windowName, params);

    if (window.focus) {
      this.popup.window.focus();
    }
  }

  loaded(): Observable<IPopup> {
    return this.doUntil(timer(0, 50),
      () => this.popup.window.document != null)
      .pipe(map(() => this.popup));
  }

  closed(): Observable<void> {
    return this.waitUntil(timer(0, 50),
      () => !this.popup.window || this.popup.window.closed);
  }

  private buildParams() {
    let params;

    if (!this.popup.settings.left)
      this.popup.settings.left =
        (window.screen.width - this.popup.settings.width) / 2;
    if (!this.popup.settings.top)
      this.popup.settings.top =
        (window.screen.height - this.popup.settings.height) / 2;

    params = 'width=' + this.popup.settings.width;
    params += ',height=' + this.popup.settings.height;
    params += ',top=' + this.popup.settings.top;
    params += ',left=' + this.popup.settings.left;
    params += ',status=' + this.getTruthyVal(this.popup.settings.status);
    params += ',toolbar=' + this.getTruthyVal(this.popup.settings.toolbar);
    params += ',menubar=' + this.getTruthyVal(this.popup.settings.menubar);
    params += ',location=' + this.getTruthyVal(this.popup.settings.location);
    params +=
      ',scrollbars=' + this.getTruthyVal(this.popup.settings.scrollbars);
    params += ',resizable=' + this.getTruthyVal(this.popup.settings.resizable);

    return params;
  }

  private getTruthyVal(setting): number {
    if (!!setting && setting !== 'no') {
      return 1;
    }
    return 0;
  }

  private waitUntil<T>(source$: Observable<T>, predicate: () => boolean) {
    return new Observable<void>(ob => {
      const sub = source$.subscribe(item => {
        if (predicate()) {
          ob.next();
          ob.complete();
        }
      }, ob.error, ob.complete);
      return () => sub.unsubscribe();
    });
  }

  private doUntil<T>(source$: Observable<T>, predicate: () => boolean) {
    return new Observable<void>(ob => {
      const sub = source$.subscribe(item => {
        ob.next();

        if (predicate()) {
          ob.complete();
        }
      }, ob.error, ob.complete);

      return () => sub.unsubscribe();
    });
  }
}

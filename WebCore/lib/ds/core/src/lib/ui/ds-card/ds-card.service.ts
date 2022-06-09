import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable()
export class DsCardService {

  private _truncate:Subject<boolean> = new Subject<boolean>();

  truncate$:Observable<boolean> = this._truncate.asObservable();

  constructor() { }

  setTruncate(value:boolean) {
    this._truncate.next(value);
  }

}

import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChangeTrackerService {

  private _isDirty: boolean = false;

  constructor() { }

  setIsDirty(isDirty: boolean) {
    this._isDirty = isDirty;
  }

  clearIsDirty() {
    this._isDirty = false;
  }

  isDirty() : boolean {
    return this._isDirty;
  }

}

import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { ICheckDeactivator } from './check-deactivator.model';
import { DeactivatorService } from './deactivator.service';

@Injectable({
  providedIn: 'root'
})
export class CheckDeactivatorGuard implements CanDeactivate<ICheckDeactivator>  {

    constructor(private deactivator: DeactivatorService) {

    }

    canDeactivate(component: ICheckDeactivator) {
        if (this.deactivator.canDeactivate) {
            return true;
        } 
        else {
            return confirm('Leave page with unsaved changes?');
        }
    }
}

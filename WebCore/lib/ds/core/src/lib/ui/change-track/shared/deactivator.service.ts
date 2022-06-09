import { Injectable } from '@angular/core';
import { ComponentCanDeactivate } from './component-can-deactivate';

@Injectable({
  providedIn: 'root'
})
export class DeactivatorService {

    private _components: ComponentCanDeactivate[] = [];

    get canDeactivate() {
        return this._components.every(x => x.canDeactivate);
    }

    constructor() { }

    registerComponent(component: ComponentCanDeactivate) {
        this._components.push(component);
    }

    unregisterComponent(component: ComponentCanDeactivate) {
        let idx = this._components.indexOf(component);
        if (idx >= 0)
            this._components.splice(idx, 1);
    }
}

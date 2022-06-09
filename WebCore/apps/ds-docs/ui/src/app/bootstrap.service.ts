import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class BootstrapService {

    private _bootstrapped:boolean = false;
    
    get isBootstrapped():boolean {
        return this._bootstrapped;
    }
    set isBootstrapped(value:boolean) {
        this._bootstrapped = value;
    }

    constructor() { }
}

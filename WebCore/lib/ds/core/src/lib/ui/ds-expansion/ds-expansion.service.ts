import { Injectable } from '@angular/core';
import { Subject, Observable, ReplaySubject } from 'rxjs';
import { DsExpansionModule } from '@ds/core/ui/ds-expansion';
import { coerceBooleanProperty } from '@angular/cdk/coercion';

/**
 *
 * TPR-223: Expansion panel not working
 * The subjects behind these exposed observables need to be ReplaySubjects.  If ds-card-content
 * isn't initialized immediately when ds-card is, ds-card-content might subscribe after 
 * these subjects have already emitted which will cause ds-card-content to be in an incorrect state.
 */
@Injectable()
export class DsExpansionService {


    // maintains the current state of the expansion panel
    private _expanded:Subject<boolean> = new ReplaySubject<boolean>(1);

    // maintains state on whether the ds-card component has an expansion panel 
    private _collapse:Subject<boolean> = new ReplaySubject<boolean>(1);
    private _hasCardIcon:Subject<boolean> = new ReplaySubject<boolean>(1);

    /**
     * Observable that components subscribe to. They emit a stream of values to the subscriber
     * based with the current value. 
     * 
     */
    expanded$:Observable<boolean> = this._expanded.asObservable();
    collapse$:Observable<boolean> = this._collapse.asObservable();
    hasCardIcon$:Observable<boolean> = this._hasCardIcon.asObservable();


    constructor() { }

    toggle(state:boolean):void {
        this._expanded.next(state);
    }

    close():void {
        this._expanded.next(false);
    }

    open():void {
        this._expanded.next(true);
    }
    
    setCollapse(value:boolean) {
        value = coerceBooleanProperty(value);
        this._collapse.next(value);
    }

    setHasCardIcon(state:boolean) {
        this._hasCardIcon.next(state);
    }
}

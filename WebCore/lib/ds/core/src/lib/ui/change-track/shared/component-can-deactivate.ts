import { HostListener, Injectable } from '@angular/core';
import { NgForm, AbstractControl } from '@angular/forms';

/**
 * Adds standard support for notifying user that action
 * must be taken before navigating away from the current page
 * (via window beforeunload event) (e.g. change tracking).
 *
 * Derived from: https://medium.com/front-end-weekly/angular-how-keep-user-from-lost-his-data-by-accidentally-leaving-the-page-before-submit-4eeb74420f0d
 */
@Injectable()
export abstract class ComponentCanDeactivate {

    abstract get canDeactivate(): boolean;

    @HostListener('window:beforeunload', ['$event'])
    unloadNotification($event:any) {
        if (!this.canDeactivate) {
            $event.returnValue = true;
            return true;
        }
    }
}

/**
 * Adds "change-tracking" to a component containing a form.
 * Use for Reactive Forms.
 */
export abstract class FormCanDeactivate extends ComponentCanDeactivate {

    /**
     * Any FormControl, FormGroup or FormArray.
     * Used to track 'dirty' state.
     */
    abstract get form(): AbstractControl;

    /**
     * Indication if the form has been submitted.
     */
    abstract get submitted(): boolean;

    get canDeactivate() {
        return this.submitted || !this.form.dirty;
    }
}

/**
 * Adds "change-tracking" to a component containing a form.
 * Use for Template-Driven Forms.
 */
export abstract class NgFormCanDeactivate extends FormCanDeactivate {

    /**
     * NgForm to track for 'dirty' and 'submitted' states.
     */
    abstract get ngForm(): NgForm;

    get form() {
        return this.ngForm.form;
    }

    get submitted() {
        return this.ngForm.submitted;
    }
}

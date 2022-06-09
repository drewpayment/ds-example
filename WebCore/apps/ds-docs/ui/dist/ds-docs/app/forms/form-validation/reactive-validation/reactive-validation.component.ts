//reactive-validation.component.ts

import { Component, OnInit } from '@angular/core';
import { IPerson } from '../shared/models';
import { STATES } from '../shared/states';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'ds-reactive-validation',
    templateUrl: './reactive-validation.component.html',
    styleUrls: ['./reactive-validation.component.scss']
})
export class ReactiveValidationComponent implements OnInit {

    person: IPerson = {};
    states = STATES;

    form: FormGroup;
    get firstName() { return this.form.get('firstName'); }
    get lastName() { return this.form.get('lastName'); }
    get stateAbbreviation() { return this.form.get('stateAbbreviation'); }
    isSubmitted = false;

    constructor(private fb: FormBuilder) { }

    ngOnInit() {
        this.form = this.fb.group({
            firstName: [null, [Validators.required, Validators.pattern('[A-Za-z ]+')]],
            lastName: [null, [Validators.required, Validators.pattern('[A-Za-z ]+')]],
            stateAbbreviation: [null, Validators.required]
        });
    }

    reset() {
        this.person = {};
        this.form.reset(this.person);
        this.isSubmitted = false;
    }

    save() {
        this.isSubmitted = true;
        alert(`Form is ${ this.form.valid ? 'VALID' : 'INVALID'}`);
        if (this.form.valid) {
            Object.assign(this.person, this.form.value);
        }
    }
}


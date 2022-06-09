//template-validation.component.ts

import { Component, OnInit } from '@angular/core';
import { IPerson } from '../shared/models';
import { STATES } from '../shared/states';
import { NgForm } from '@angular/forms';

@Component({
    selector: 'ds-template-validation',
    templateUrl: './template-validation.component.html',
    styleUrls: ['./template-validation.component.scss']
})
export class TemplateValidationComponent implements OnInit {

    person: IPerson = {};
    states = STATES;

    constructor() { }

    ngOnInit() {

    }

    reset(form: NgForm) {
        this.person = {};
        form.resetForm(this.person);
    }

    save(form: NgForm) {
        alert(`Form is ${ form.valid ? 'VALID' : 'INVALID'}`);
    }
}

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IPerson } from '../template-form-change-track/template-form-change-track.component';

@Component({
  selector: 'ds-reactive-form-change-track',
  templateUrl: './reactive-form-change-track.component.html',
  styleUrls: ['./reactive-form-change-track.component.scss']
})
export class ReactiveFormChangeTrackComponent implements OnInit {
    
    form: FormGroup;
    submitted: boolean;

    person = <IPerson>{};

    constructor(private fb: FormBuilder) {
    }

    ngOnInit() {
        this.form = this.fb.group({
            firstName: [null],
            lastName: [null]
        });
    }

    save() {
        this.submitted = true;
        Object.assign(this.person, this.form.value);
        console.log(this.person);
        
        /** Mimic a save call to the server/api */
        setTimeout(() => {
            this.submitted = false;
            this.form.reset(this.person);
        }, 3000);
    }

    cancel() {
        this.form.reset(this.person); 
        this.submitted = false;
    }

}

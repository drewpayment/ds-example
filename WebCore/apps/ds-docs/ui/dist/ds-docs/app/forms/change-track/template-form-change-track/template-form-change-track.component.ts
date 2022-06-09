import { Component, OnInit, ViewChild } from '@angular/core';

export interface IPerson {
    firstName: string;
    lastName: string;
}

@Component({
  selector: 'ds-template-form-change-track',
  templateUrl: './template-form-change-track.component.html',
  styleUrls: ['./template-form-change-track.component.scss']
})
export class TemplateFormChangeTrackComponent implements OnInit {
    
    @ViewChild("f", { static: false })
    ngForm;

    person = <IPerson>{};
    orig = <IPerson>{};

    constructor() {
    }

    ngOnInit() {
    }

    save() {
        console.log(this.person);
        Object.assign(this.orig, this.person);
        
        /** Mimic a save call to the server/api */
        setTimeout(() => {
            this.ngForm.resetForm(this.orig);
        }, 3000);
    }

    cancel() {
        this.ngForm.resetForm(this.orig); 
    }

}

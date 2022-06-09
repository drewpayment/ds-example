import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';


@Component({
    selector: 'outlet', 
    template: '<router-outlet></router-outlet>'  
})
export class OutletComponent implements OnInit {
    
    constructor(private router: Router, private location: Location) {
        // console.dir([router, location]);
    }

    ngOnInit() {}
}

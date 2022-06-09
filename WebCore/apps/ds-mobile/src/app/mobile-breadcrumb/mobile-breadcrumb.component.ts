import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'ds-mobile-breadcrumb',
    templateUrl: './mobile-breadcrumb.component.html',
    styleUrls: ['./mobile-breadcrumb.component.scss']
})
export class MobileBreadcrumbComponent implements OnInit {

    @Input() navItems: any[];
    
    // FIXME: NEED TO FINISH THIS THING AND MAKE BREADCRUMBS PROGRAMMATIC
    
    constructor() { }

    ngOnInit() {
    }

}

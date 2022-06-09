import { Component, OnInit, ViewContainerRef, ElementRef } from '@angular/core';


@Component({
    selector: 'ds-app',
    templateUrl: './ds-app.component.html',
    styleUrls: ['./ds-app.component.scss']
})
export class DsAppComponent implements OnInit {
    
    constructor(private vcr: ViewContainerRef, private elem: ElementRef) {}
    
    ngOnInit() {}
    
}

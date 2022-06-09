import { Directive, ElementRef, ViewChild, OnInit, forwardRef } from '@angular/core';
import { DsNavMenuComponent } from './ds-nav-menu.component';
import { DsNavToolbarContentComponent } from '../ds-nav-toolbar-content/ds-nav-toolbar-content.component';
import { DsNavMainContentComponent } from '../ds-nav-main-content/ds-nav-main-content.component';


@Directive({
    selector: '[dsNavMenu]'
})
export class DsNavMenuDirective implements OnInit {
    @ViewChild(forwardRef(() => DsNavMenuComponent), { static: true }) navMenu: DsNavMenuComponent;
    @ViewChild(forwardRef(() => DsNavToolbarContentComponent), { static: false}) header: DsNavToolbarContentComponent;
    @ViewChild(forwardRef(() => DsNavMainContentComponent), { static: false }) content: DsNavMainContentComponent;
    
    constructor(private el: ElementRef) {}
    
    ngOnInit() {
        console.log('INIT');
        console.dir([this.navMenu, this.header, this.content]);
    }
    
}

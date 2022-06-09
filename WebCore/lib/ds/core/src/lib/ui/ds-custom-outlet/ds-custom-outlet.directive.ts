import { Directive, OnInit, OnDestroy, ViewContainerRef, ComponentFactoryResolver, ChangeDetectorRef, Input, ComponentRef } from '@angular/core';
import { ChildrenOutletContexts, RouterOutlet } from '@angular/router';
@Directive({
    selector: '[dsCustomOutlet], ds-custom-outlet',
    exportAs: 'outlet'
})
export class DsCustomOutletDirective implements OnInit, OnDestroy {
    public outlet: RouterOutlet;
    @Input() public name: string;
    constructor(
        private parentContexts: ChildrenOutletContexts,
        private vcr: ViewContainerRef,
        private resolver: ComponentFactoryResolver,
        private changeDetector: ChangeDetectorRef,
    ) {}    
    
    ngOnInit() {
        this.outlet = new RouterOutlet(this.parentContexts, this.vcr, this.resolver, this.name, this.changeDetector);
        this.outlet.ngOnInit();
    }
    
    ngOnDestroy() {
        if (this.outlet) this.outlet.ngOnDestroy();
    }
}

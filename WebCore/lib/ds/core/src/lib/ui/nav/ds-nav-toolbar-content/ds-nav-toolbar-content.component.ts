import { Component, OnInit, ComponentFactoryResolver, Injector, ApplicationRef, AfterViewInit, OnDestroy, ViewChild, ViewContainerRef, Inject } from '@angular/core';
import { PortalHost, DomPortalHost, CdkPortal, TemplatePortal } from '@angular/cdk/portal';
import { DOCUMENT } from '@angular/common';

@Component({
    selector: 'ds-nav-toolbar-content',
    templateUrl: './ds-nav-toolbar-content.component.html',
    styleUrls: ['./ds-nav-toolbar-content.component.scss']
})
export class DsNavToolbarContentComponent implements OnInit, AfterViewInit, OnDestroy {
    private portalHost: PortalHost;

    @ViewChild('content', { static: false }) contentRef;
    portal: TemplatePortal;

    constructor(
        private componentFactoryResolver: ComponentFactoryResolver,
        private injector: Injector,
        private appRef: ApplicationRef,
        private viewContainerRef: ViewContainerRef,
        @Inject(DOCUMENT) public document: Document
    ) { }

    ngOnInit() {
    }
    
    ngAfterViewInit(): void {
        // const elem = this.document.querySelector('#toolbar-content');
        
        // if (elem) {
        //     this.portalHost = new DomPortalHost(
        //         document.querySelector('#toolbar-content'),
        //         this.componentFactoryResolver,
        //         this.appRef,
        //         this.injector
        //     );
    
        //     this.portal = new TemplatePortal(
        //         this.contentRef,
        //         this.viewContainerRef
        //     )
    
        //     this.portalHost.attach(this.portal);
        // }
    }

    ngOnDestroy(): void {
        this.portalHost.detach();
    }
}

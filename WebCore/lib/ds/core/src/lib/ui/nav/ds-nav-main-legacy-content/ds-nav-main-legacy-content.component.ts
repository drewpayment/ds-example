import { Component, OnInit, ComponentFactoryResolver, Injector, ApplicationRef, AfterViewInit, OnDestroy, ViewChild, ViewContainerRef } from '@angular/core';
import { PortalHost, DomPortalHost, CdkPortal, TemplatePortal } from '@angular/cdk/portal';

@Component({
    selector: 'ds-nav-main-legacy-content',
    templateUrl: './ds-nav-main-legacy-content.component.html',
    styleUrls: ['./ds-nav-main-legacy-content.component.scss']
})
export class DsNavMainLegacyContentComponent implements OnInit, AfterViewInit, OnDestroy {
    private portalHost: PortalHost;

    @ViewChild('content', { static: false }) contentRef;
    portal: TemplatePortal;

    constructor(
        private componentFactoryResolver: ComponentFactoryResolver,
        private injector: Injector,
        private appRef: ApplicationRef,
        private viewContainerRef: ViewContainerRef
    ) { }

    ngOnInit() {
    }
    
    ngAfterViewInit(): void {
        this.portalHost = new DomPortalHost(
            document.querySelector('#main-content'),
            this.componentFactoryResolver,
            this.appRef,
            this.injector
        );

        this.portal = new TemplatePortal(
            this.contentRef,
            this.viewContainerRef
        )

        this.portalHost.attach(this.portal);
    }

    ngOnDestroy(): void {
        this.portalHost.detach();
    }
}

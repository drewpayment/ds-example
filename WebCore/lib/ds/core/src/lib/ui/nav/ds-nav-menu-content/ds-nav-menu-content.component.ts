import { Component, OnInit, ComponentFactoryResolver, Injector, ApplicationRef, AfterViewInit, OnDestroy, ViewChild, ViewContainerRef } from '@angular/core';
import { PortalHost, DomPortalHost, CdkPortal, TemplatePortal } from '@angular/cdk/portal';

@Component({
  selector: 'ds-nav-menu-content',
  templateUrl: './ds-nav-menu-content.component.html',
  styleUrls: ['./ds-nav-menu-content.component.scss']
})
export class DsNavMenuContentComponent implements OnInit, AfterViewInit, OnDestroy {
    private zone1PortalHost: PortalHost;
    private zone2PortalHost: PortalHost;

    @ViewChild('zone1', { static: false }) zone1ContentRef;
    @ViewChild('zone2', { static: false }) zone2ContentRef;

    zone1Portal: TemplatePortal;
    zone2Portal: TemplatePortal;

    constructor(
        private componentFactoryResolver: ComponentFactoryResolver,
        private injector: Injector,
        private appRef: ApplicationRef,
        private viewContainerRef: ViewContainerRef
    ) { }

    ngOnInit() {
    }
    
    ngAfterViewInit(): void {
        //zone1
        this.zone1PortalHost = new DomPortalHost(
            document.querySelector('#menu-zone-1-content'),
            this.componentFactoryResolver,
            this.appRef,
            this.injector
        );

        this.zone1Portal = new TemplatePortal(
            this.zone1ContentRef,
            this.viewContainerRef
        )

        //if (this.zone1PortalHost) this.zone1PortalHost.attach(this.zone1Portal);

        //zone2
        this.zone2PortalHost = new DomPortalHost(
            document.querySelector('#menu-zone-2-content'),
            this.componentFactoryResolver,
            this.appRef,
            this.injector
        );

        this.zone2Portal = new TemplatePortal(
            this.zone2ContentRef,
            this.viewContainerRef
        )

        //this.zone2PortalHost.attach(this.zone2Portal);
    }

    ngOnDestroy(): void {
        this.zone1PortalHost.detach();
        this.zone2PortalHost.detach();
    }
}


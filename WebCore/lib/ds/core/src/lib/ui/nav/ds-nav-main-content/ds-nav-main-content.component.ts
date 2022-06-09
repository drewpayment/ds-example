import {
  Component,
  OnInit,
  ComponentFactoryResolver,
  Injector,
  ApplicationRef,
  AfterViewInit,
  OnDestroy,
  ViewChild,
  ViewContainerRef,
  Inject,
  AfterViewChecked,
  Input,
  ElementRef,
  TemplateRef,
} from "@angular/core";
import {
  PortalHost,
  DomPortalHost,
  CdkPortal,
  TemplatePortal,
  DomPortalOutlet,
} from "@angular/cdk/portal";
import { DOCUMENT } from "@angular/common";
import { Router } from "@angular/router";

/**
 * @see https://juristr.com/blog/2018/05/dynamic-UI-with-cdk-portals/
 */
@Component({
  selector: "ds-nav-main-content",
  templateUrl: "./ds-nav-main-content.component.html",
  styleUrls: ["./ds-nav-main-content.component.scss"],
})
export class DsNavMainContentComponent {
  constructor(
    private componentFactoryResolver: ComponentFactoryResolver,
    private injector: Injector,
    private appRef: ApplicationRef,
    private vcr: ViewContainerRef,
    @Inject(DOCUMENT) public document: Document,
    private router: Router,
  ) {}
}

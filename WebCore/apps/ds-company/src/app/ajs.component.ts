import {
  Component,
  ElementRef,
  OnInit,
  OnDestroy,
  Input,
  ComponentFactoryResolver,
  Inject,
  Injector,
  AfterViewChecked,
  TemplateRef,
  ViewChild,
} from "@angular/core";
import { AjsCompanyLoaderService } from "./ajs-loader.service";
import { DOCUMENT, NgTemplateOutlet } from "@angular/common";
import { MenuWrapperToggleComponent } from "@ds/core/users/beta-features/menu-wrapper-toggle/menu-wrapper-toggle.component";
import { Subject } from "rxjs";

@Component({
  selector: "ds-ajs-company",
  template: ``,
})
export class AjsCompanyComponent implements OnInit, OnDestroy
{
  destroy$ = new Subject();
  @Input() isLegacyLayout = false;
  @ViewChild("menuToggle", { static: false, read: TemplateRef })
  menuToggle: TemplateRef<NgTemplateOutlet>;
  constructor(
    private loader: AjsCompanyLoaderService,
    private elRef: ElementRef,
    private cfr: ComponentFactoryResolver,
    @Inject(DOCUMENT) private document: Document,
    private injector: Injector
  ) {}

  ngOnInit() {
    // this.loader.load(this.elRef.nativeElement);
  }

  ngOnDestroy() {
    // this.loader.destroy();
    this.destroy$.next();
  }
}

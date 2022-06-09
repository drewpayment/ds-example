import { 
    AfterViewInit, 
    Directive, 
    ElementRef, 
    Input} from "@angular/core";
  
  @Directive({
    selector: '[autoFocus]'
  })
  export class AutoFocusDirective implements AfterViewInit {
  
    @Input() autoFocus: boolean = true;
    constructor( private elementRef: ElementRef ){}
    ngAfterViewInit() {
      if ( this.autoFocus ) {
        if ( this.elementRef ) this.elementRef.nativeElement.focus();
      }
    }
  }
import { Directive, ViewContainerRef } from '@angular/core';


@Directive({
  selector: '[dsNavMainContent]',
})
export class DsNavMainContentDirective {
  constructor(public viewContainerRef: ViewContainerRef) {}
}

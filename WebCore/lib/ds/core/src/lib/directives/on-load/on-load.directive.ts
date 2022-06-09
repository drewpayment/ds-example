import { 
  OnInit,
  Directive } from '@angular/core';
import { MatTooltip } from "@angular/material/tooltip";

@Directive({
  selector: "[showTooltipOnLoad]",
})
export class OnLoadDirective implements OnInit {

  constructor( private host: MatTooltip ) { }

  ngOnInit() {
    if (this.host) this.host.show();

  }
}

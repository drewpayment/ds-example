import { Component, OnInit, ContentChild, ElementRef, AfterViewInit, ContentChildren } from '@angular/core';

@Component({
  selector: 'ds-tooltip',
  templateUrl: './ds-tooltip.component.html',
  styleUrls: ['./ds-tooltip.component.scss']
})
export class DsTooltipComponent implements OnInit {
  constructor(private elt: ElementRef) { }

  ngOnInit() {
    let node = this.elt.nativeElement.childNodes[0];
    node.setAttribute('matTooltip', 'it works!');
  }

}

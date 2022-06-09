import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-tooltip-example',
  templateUrl: './tooltip.component.html',
  styleUrls: ['./tooltip.component.scss']
})
export class TooltipComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  contactInfo(): string {
    return 'Address : ' + '1234 Main Street'
        + ' \n  Tel : ' +  '(616) 867-5309'
        + ' \n  Favorite Food : ' +  'Doughnuts';
  }
}

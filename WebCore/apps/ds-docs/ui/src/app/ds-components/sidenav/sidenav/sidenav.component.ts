import { Component, OnInit } from '@angular/core';
import { changeDrawerHeightOnOpen } from '@ds/core/ui/animations/drawer-auto-height-animation';

@Component({
  selector: 'ds-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss'],
  animations: [
    changeDrawerHeightOnOpen
  ]
})
export class SidenavComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}

import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-nav-title',
  templateUrl: './nav-title.component.html',
  styleUrls: ['./nav-title.component.scss']
})
export class NavTitleComponent implements OnInit {

  selected = 'porkChop';

  constructor() { }

  ngOnInit() {
  }

  showTab(tab: string) {
    this.selected = tab;
  }
}

import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'ds-under-construction',
  templateUrl: './under-construction.component.html',
  styleUrls: ['./under-construction.component.scss']
})
export class UnderConstructionComponent implements OnInit {

  @Input() pageTitle: string;

  getDesktopHomeUrl(): string {
    const protocol = window.location.protocol;
    const host     = window.location.host;
    // This works because any page on mobile site will have path of: /something/Mobile/otherstuff
    const path     = window.location.pathname.match(/^\/[^\/]+\//);
    const result   = `${protocol}//${host}${path[0]}default.aspx`;
    // console.log(path);
    // console.log(result);
    return result;
  }

  constructor() { }

  ngOnInit() {
  }
}

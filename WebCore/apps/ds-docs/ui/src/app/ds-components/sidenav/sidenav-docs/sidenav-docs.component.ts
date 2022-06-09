import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidenav-docs',
  templateUrl: './sidenav-docs.component.html',
  styleUrls: ['./sidenav-docs.component.css']
})
export class SidenavDocsComponent implements OnInit {

  toggleSideNav = false;
  sidenavExample = 1;
  constructor() { }

  ngOnInit(): void {
  }

}

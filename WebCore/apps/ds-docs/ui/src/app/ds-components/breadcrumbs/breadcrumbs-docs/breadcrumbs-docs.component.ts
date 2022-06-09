import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-breadcrumbs-docs',
  templateUrl: './breadcrumbs-docs.component.html',
  styleUrls: ['./breadcrumbs-docs.component.scss']
})
export class BreadcrumbsDocsComponent implements OnInit {

  toggleBasicBreadcrumb: false;
  toggleBreadcrumbLayout: false;
  toggleBreadcrumbEmpComponent: false;

  constructor() { }

  ngOnInit() {
  }

}

import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-printable-form-docs',
  templateUrl: './printable-form-docs.component.html',
  styleUrls: ['./printable-form-docs.component.scss']
})
export class PrintableFormDocsComponent implements OnInit {

  togglePrint: boolean
  constructor() { }

  ngOnInit() {
  }

}

import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-switch-docs',
  templateUrl: './switch-docs.component.html',
  styleUrls: ['./switch-docs.component.scss']
})
export class SwitchDocsComponent implements OnInit {

  toggleSwitch = false;
  toggleTitleSwitch = false;
  constructor() { }

  ngOnInit() {
  }

}

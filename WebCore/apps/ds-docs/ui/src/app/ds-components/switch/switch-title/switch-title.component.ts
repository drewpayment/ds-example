import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-switch-title',
  templateUrl: './switch-title.component.html',
  styleUrls: ['./switch-title.component.scss']
})
export class SwitchTitleComponent implements OnInit {
    titleOn = false;
    constructor() { }

    ngOnInit() {
    }

}

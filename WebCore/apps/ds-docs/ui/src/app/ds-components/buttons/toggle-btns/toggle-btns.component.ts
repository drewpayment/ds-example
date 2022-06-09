import { Component, OnInit } from '@angular/core';
import { LENGTHOFTIME } from '../../shared/lengthOfTime';

@Component({
    selector: 'ds-toggle-btns',
    templateUrl: './toggle-btns.component.html',
    styleUrls: ['./toggle-btns.component.scss']
})
export class ToggleBtnsComponent implements OnInit {
    items = LENGTHOFTIME;
    constructor() { }

    ngOnInit() {
    }

}

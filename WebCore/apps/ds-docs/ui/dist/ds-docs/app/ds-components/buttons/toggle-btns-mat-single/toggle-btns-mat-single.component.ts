import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { LENGTHOFTIME } from '../../shared/lengthOfTime';

@Component({
    selector: 'ds-toggle-btns-mat-single',
    templateUrl: './toggle-btns-mat-single.component.html',
    styleUrls: ['./toggle-btns-mat-single.component.scss']
})
export class ToggleBtnsMatSingleComponent implements OnInit {

    items = LENGTHOFTIME;
    constructor() { }

    ngOnInit() {
    }

    checkForMatSingleClickReset(  ) {
    }
}

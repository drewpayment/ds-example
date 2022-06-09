import { Component, OnInit } from '@angular/core';
import { LENGTHOFTIME } from '../../shared/lengthOfTime';

@Component({
    selector: 'ds-input-button-group',
    templateUrl: './input-button-group.component.html',
    styleUrls: ['./input-button-group.component.scss']
})
export class InputButtonGroupComponent implements OnInit {
    items = LENGTHOFTIME;
    constructor() { }

    ngOnInit() {
    }

}

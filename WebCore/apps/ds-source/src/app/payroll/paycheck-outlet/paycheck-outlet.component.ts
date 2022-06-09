import { Component, OnInit } from '@angular/core';
import { Router }            from '@angular/router';
import { Location }          from "@angular/common";

@Component({
    selector: 'ds-paycheck-outlet',
    templateUrl: './paycheck-outlet.component.html',
    styleUrls: ['./paycheck-outlet.component.scss']
})
export class PaycheckOutletComponent implements OnInit {

    constructor(private router:Router, private location:Location) { }

    ngOnInit() { }
}

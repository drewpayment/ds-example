import { Component, OnInit } from '@angular/core';
import { Router }            from '@angular/router';
import { Location }          from "@angular/common";

@Component({
  selector: 'ds-employee-deduction-outlet',
  templateUrl: './employee-deduction-outlet.component.html',
  styleUrls: ['./employee-deduction-outlet.component.scss']
})
export class EmployeeDeductionOutletComponent implements OnInit {

  constructor(private router:Router, private location:Location) { }

  ngOnInit() {
  }

}

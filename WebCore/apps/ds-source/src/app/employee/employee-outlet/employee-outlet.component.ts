import { Component, OnInit } from '@angular/core';
import { Router }            from '@angular/router';
import { Location }          from "@angular/common";

@Component({
  selector: 'ds-employee-outlet',
  templateUrl: './employee-outlet.component.html',
  styleUrls: ['./employee-outlet.component.scss']
})
export class EmployeeOutletComponent implements OnInit {

  constructor(private router:Router, private location:Location) { }

  ngOnInit() {
  }

}

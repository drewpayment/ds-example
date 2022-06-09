import { Component, OnInit } from '@angular/core';
import { Router }            from '@angular/router';
import { Location }          from "@angular/common";

@Component({
  selector: 'ds-client-outlet',
  templateUrl: './client-outlet.component.html',
  styleUrls: ['./client-outlet.component.scss']
})
export class ClientOutletComponent implements OnInit {

  constructor(private router:Router, private location:Location) { }

  ngOnInit() {
  }

}

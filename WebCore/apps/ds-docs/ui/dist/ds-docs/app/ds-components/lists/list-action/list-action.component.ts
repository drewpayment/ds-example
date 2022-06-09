import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-list-action',
  templateUrl: './list-action.component.html',
  styleUrls: ['./list-action.component.scss']
})


export class ListActionComponent implements OnInit {

  editView = false
  constructor() { }

  ngOnInit() {
  }

}

import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-action-btns',
  templateUrl: './action-btns.component.html',
  styleUrls: ['./action-btns.component.scss']
})
export class ActionBtnsComponent implements OnInit {

  isDone = false;

  constructor() { }

  ngOnInit() {
    
  }

  done() {
    this.isDone = true;
  }
}

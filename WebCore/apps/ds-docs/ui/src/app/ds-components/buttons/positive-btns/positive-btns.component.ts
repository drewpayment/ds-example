import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-positive-btns',
  templateUrl: './positive-btns.component.html',
  styleUrls: ['./positive-btns.component.scss']
})
export class PositiveBtnsComponent implements OnInit {

  toggle1 = false;

  positivebtn = false;

  constructor() { }

  ngOnInit() {
  }

}

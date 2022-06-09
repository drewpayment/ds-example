import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-skeletons-docs',
  templateUrl: './skeletons-docs.component.html',
  styleUrls: ['./skeletons-docs.component.scss']
})
export class SkeletonsDocsComponent implements OnInit {

  constructor() { }

  toggleConfirm: false;
  toggleCommunication: false;
  ConfirmExample = 1;

  ngOnInit() {
  }

}

import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-progress-docs',
  templateUrl: './progress-docs.component.html',
  styleUrls: ['./progress-docs.component.scss']
})
export class ProgressDocsComponent implements OnInit {

  toggleLogo = false;
  toggleProgress = false;
  toggleProgressPosition = false;
  
constructor() { }

  ngOnInit() {
  }

}

import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-avatar-example',
  templateUrl: './avatar.component.html',
  styleUrls: ['./avatar.component.scss']
})
export class AvatarExampleComponent implements OnInit {

  constructor() { }

  isVendor: boolean;
  ngOnInit() {
    this.isVendor = true;
  }

}

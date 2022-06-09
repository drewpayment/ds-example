import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-avatar-docs',
  templateUrl: './avatar-docs.component.html',
  styleUrls: ['./avatar-docs.component.scss']
})
export class AvatarDocsComponent implements OnInit {

  toggleAvatar: false;
  toggleAvatarTable: false;
  toggleAvatarOnbaording: false;
  toggleAvatarWidget: false;
  toggleAvatarCallout: false;
  toggleAvatarAutoComplete: false;
  toggleAvatarColors: false;

  avatarTableExample = 1;
  avatarOnboardingExample = 1;
  avatarWidgetExample = 1;
  avatarCalloutExample = 1;
  constructor() { }

  ngOnInit() {
  }

}

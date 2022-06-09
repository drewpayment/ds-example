import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-button-docs',
  templateUrl: './button-docs.component.html',
  styleUrls: ['./button-docs.component.scss']
})
export class ButtonDocsComponent implements OnInit {

togglePositive = false;
toggleNegative = false;
toggleIcon = false;
toggleIconDropdown = false
toggleDropdown = false
toggleAction= false
toggleOutlineIcon = false
togglePills = false
toggleBadges= false
toggleStaticBadges = false
toggleToggleBtn = false
ToggleExample = 1
toggleTimeSpanSelector = false;
toggleToggleBtnMatMultiple = false;
toggleToggleBtnMatSingle = false;
toggleToggleBtnLegacyMultiple = false;
toggleToggleBtnLegacySingle = false;

constructor() { }

  ngOnInit() {
  }

}

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'ds-review-profile-form-outlet',
    templateUrl: './review-profile-form-outlet.component.html',
    styleUrls: ['./review-profile-form-outlet.component.scss']
})
export class ReviewProfileFormOutletComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

  profileSaved() {
    this.navigateToProfileList();
  }

  cancelled() {
    this.navigateToProfileList();
  }

  private navigateToProfileList() {
    this.router.navigate(["performance", "setup", "review-profiles"]);
  }

}

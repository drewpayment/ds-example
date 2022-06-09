import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { IReviewTemplate } from '@ds/core/groups/shared/review-template.model';

@Component({
    selector: 'ds-review-policy-setup',
    templateUrl: './review-policy-form-outlet.component.html',
    styleUrls: ['./review-policy-form-outlet.component.scss']
})
export class ReviewPolicyFormOutletComponent implements OnInit {
  reviewProfiles: any;
  constructor(private router: Router) { }

  @Input() 
  data: IReviewTemplate[];

  ngOnInit() {
  }

  recieveMessage($event) {
    this.reviewProfiles = $event;
  }

  cycleSaved() {
    this.navigateToProfileList();
  }

  cancelled() {
    this.navigateToProfileList();
  }

  edit() {
    this.navigateToProfileEdit();
  }

  private navigateToProfileEdit() {
    this.router.navigate(["performance", "setup", "review-policy", "edit"]);
  }

  private navigateToProfileList() {
    this.router.navigate(["performance", "setup", "review-policy"]);
  }
}
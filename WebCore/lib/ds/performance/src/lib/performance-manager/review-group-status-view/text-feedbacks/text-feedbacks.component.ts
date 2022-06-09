import { Component, OnInit, Input, ViewChild } from "@angular/core";
import {
  IFeedbackResponseData,
  IFeedbackSetup,
  IReviewIdWithFeedbackResponse,
} from "@ds/performance/feedback";
import { MatPaginator } from "@angular/material/paginator";

@Component({
  selector: "ds-text-feedbacks",
  templateUrl: "./text-feedbacks.component.html",
  styleUrls: ["./text-feedbacks.component.scss"],
})
export class TextFeedbacksComponent implements OnInit {
  pagingLength: number;
  title: string;

  @Input()
  feedback: IFeedbackSetup;

  @Input()
  responseList: Array<IReviewIdWithFeedbackResponse>;

  @Input()
  /**
   * reviewId => reviewedemployeename
   */
  reviewedEmps: { [id: number]: string } = {};

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor() {}

  ngOnInit() {
    let n = (f: IFeedbackResponseData) =>
      f.responseByContact.lastName + ", " + f.responseByContact.firstName;
    this.responseList.sort((a, b) => {
      return n(a.feedback).localeCompare(n(b.feedback));
    });

    this.pagingLength = this.responseList.length;
    if (this.pagingLength > 0) {
      this.title = this.feedback.body;
    }
  }

  GetResponse(item: IFeedbackResponseData): string {
    if (item.responseItems.length > 0) return item.responseItems[0].textValue;
    else return "";
  }
} // end of class..

import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { dsMessage } from "@ajs/core/msg/ds-msg-message.model";
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";
import { HttpErrorResponse } from "@angular/common/http";

@Injectable({
  providedIn: "root",
})
export class NgxMessageService {
  message$ = new Subject();

  message: dsMessage = {
    type: "",
    icon: "",
    text: "",
    textColor: null,
    details: [],
  };

  constructor() {}

  loading(isLoading: boolean = false, msg: string = "Loading...") {
    if (isLoading) {
      this.message.text = msg;
      this.message.type = MessageTypes.warning;
      this.message$.next(this.message);
    } else {
      this.clearMessage();
    }
  }

  setSuccessMessage(msg: string, duration: number = 5000) {
    this.message.text = msg;
    this.message.type = MessageTypes.success;
    this.message$.next(this.message);
    this.timeout(duration);
  }

  setErrorMessage(msg: string, duration: number = 10000) {
    this.message.text = msg;
    this.message.type = MessageTypes.error;
    this.message$.next(this.message);
    this.timeout(duration);
  }

  setWarningMessage(msg: string, duration: number = 10000) {
    this.message.text = msg;
    this.message.type = MessageTypes.warning;
    this.message$.next(this.message);
    this.timeout(duration);
  }

  setErrorResponse(
    httpErrResponse: HttpErrorResponse,
    duration: number = 10000
  ) {
    this.message.text = httpErrResponse.message;
    this.message.type = MessageTypes.error;

    if (httpErrResponse.error != null)
      this.message.details = httpErrResponse.error.errors;

    this.message$.next(this.message);
    this.timeout(duration);
  }

  timeout(duration: number) {
    setTimeout(() => {
      this.clearMessage();
    }, duration);
  }

  clearMessage() {
    this.message.text = "";
    this.message.type = "";
    this.message.details = [];
    this.message$.next(this.message);
  }
} // end of NgXMessageService

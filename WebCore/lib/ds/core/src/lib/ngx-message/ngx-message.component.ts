import { Component, OnInit } from '@angular/core';
import { NgxMessageService } from './ngx-message.service';
import { dsMessage } from '@ajs/core/msg/ds-msg-message.model';

@Component({
  selector: 'ds-ngx-message',
  templateUrl: './ngx-message.component.html',
  styleUrls: ['./ngx-message.component.scss']
})
export class NgxMessageComponent implements OnInit {

  showMessage = false;
  showDetail = false;
  message: dsMessage;

  constructor(private ngxMsgSvc: NgxMessageService) { }

  ngOnInit() {
    this.ngxMsgSvc.message$.subscribe((msgObj: dsMessage) => {
      this.message = msgObj;
      this.showMessage = (this.message != null && this.message.text != null && this.message.text.length > 0);
    });
  }

  toggleDetail(event: Event) {
    event.stopPropagation();
    event.preventDefault();
    this.showDetail = !this.showDetail;
  }

}

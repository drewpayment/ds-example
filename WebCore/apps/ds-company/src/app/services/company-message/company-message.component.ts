import { Component, OnInit } from '@angular/core';
import { MessageService } from '../message.service';
import { dsMessage } from '@ajs/core/msg/ds-msg-message.model';

@Component({
  selector: 'company-app-message',
  templateUrl: './company-message.component.html',
  styleUrls: ['./company-message.component.scss']
})
export class CompanyMessageComponent implements OnInit {

  showMessage = false;
  showDetail = false;
  message: dsMessage;

  constructor(private messageService: MessageService) { }

  ngOnInit() {
    this.messageService.message$.subscribe((msgObj: dsMessage) => {
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

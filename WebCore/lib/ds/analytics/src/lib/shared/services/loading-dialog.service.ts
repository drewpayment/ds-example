import { Component, Inject, OnInit, Injectable } from "@angular/core";
import { MatDialogRef, MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { LoadingMessageComponent } from '@ds/core/ui/loading-message/loading-message.component';

@Injectable({
  providedIn: 'root'
})
export class LoadingDialogService {

  instance: MatDialogRef<LoadingMessageComponent>;

  constructor(private dialog: MatDialog) {  }

  showDialog() {
    let config = new MatDialogConfig<any>();
    config.width = "20vw";
    config.disableClose = true;

    this.instance =  this.dialog.open<LoadingMessageComponent, any, null> (LoadingMessageComponent, config);
  }

  hideDialog() {
    if(this.instance)
      this.instance.close()
  }

}
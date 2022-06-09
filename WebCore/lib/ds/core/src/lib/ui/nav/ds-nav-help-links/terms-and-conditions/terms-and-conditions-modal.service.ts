import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TermsAndConditionsComponent } from './terms-and-conditions.component';

@Injectable({
  providedIn: 'root'
})
export class TermsAndConditionsModalService {

  constructor(private dialog: MatDialog) { }

  open() {
    var config: MatDialogConfig = {
      width: "800px"
    };


    const result = this.dialog.open<TermsAndConditionsComponent>(TermsAndConditionsComponent, config);

    return result;
  }
}

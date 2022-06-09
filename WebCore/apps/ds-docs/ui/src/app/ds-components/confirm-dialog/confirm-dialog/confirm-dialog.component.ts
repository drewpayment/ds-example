import { Component, OnInit } from '@angular/core';
import { ConfirmDialogService } from '@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service';

@Component({
  selector: 'ds-confirm-dialog-example',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent implements OnInit {
   status: string
  constructor( private confirmDialog: ConfirmDialogService ) { }
    
  ngOnInit() {
      this.status = '';
  }

  public delete() {
    const options = {
        title: 'Are you sure you want to delete this clock?',
        message: 'Really, really sure??',
        confirm: "Save"
    };
    this.confirmDialog.open(options);
    this.confirmDialog.confirmed().subscribe(confirmed => {
        if ( confirmed ) {
            this.status = 'Deleted';
        } else {
            this.status = 'Delete cancelled';
            return false;
        }
    })
  }

}

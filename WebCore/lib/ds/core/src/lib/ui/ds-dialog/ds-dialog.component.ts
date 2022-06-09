import { Component, OnInit, Directive } from '@angular/core';

@Directive({
    selector: 'mat-dialog-header, [mat-dialog-header], [matDialogHeader]',
    host: {
      'class': 'ds-dialog-header',
    }
})
export class DsDialogHeaderDirective {}

@Directive({
  selector: 'mat-dialog-actions, [mat-dialog-actions], [matDialogActions]',
  host: {
      'class': 'ds-dialog-footer',

  }
})
export class DsDialogFooterDirective {}

@Component({
  selector: 'ds-dialog',
  templateUrl: './ds-dialog.component.html',
  styleUrls: ['./ds-dialog.component.scss']
})
export class DsDialogComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}

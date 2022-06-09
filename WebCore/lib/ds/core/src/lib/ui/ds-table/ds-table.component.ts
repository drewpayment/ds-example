import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, Directive, HostBinding, Input, OnInit, } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { EssProfileDependentsEditState } from 'apps/ds-ess/ajs/profile/dependents/edit.state';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'ds-table-container',
  templateUrl: './ds-table.component.html',
  styleUrls: ['./ds-table.component.scss']
})
export class DsTableComponent implements OnInit {
  /**
   * Breaks the table apart on small screens
   */
   isHandset$: Observable<boolean> = this.breakpointObserver
   .observe([Breakpoints.XSmall, Breakpoints.Small])
   .pipe(map((result) => result.matches));

   @Input() formGroup: FormGroup;
   @HostBinding('class.ds-form') get isForm(){
    return this.formGroup;
  } 
   @HostBinding('class.view') get isEdit(){
     return !!this.formGroup && this.formGroup.disabled || this.formGroup == null;
   } 

  constructor(
    private breakpointObserver: BreakpointObserver
  ) { }
  ngOnInit() {}
}



@Directive({
  selector: 'add-button, [add-button], [addButton]',
  host: {'class': 'btn btn-outline-primary ds-table-button'}
})
export class addButton {} 

@Directive({
  selector: 'action, [action], [action]',
  host: {
    'class': 'action',
  }
})
export class action {} 
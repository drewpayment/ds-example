import { Component } from '@angular/core';

@Component({
    selector: 'layout',
    templateUrl: './layout.component.html'
})
export class LayoutComponent {
  toggleNav:boolean = false;
  toggleForms:boolean = false;
  togglePatterns:boolean = false;
}
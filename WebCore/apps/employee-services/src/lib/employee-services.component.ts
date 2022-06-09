import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'lib-employee-services',
  template: `
    <p>
      employee-services works! CHANGE MADE AFTER INITIAL BUILT, THIS IS A WATCH
    </p>
  `,
  styles: []
})
export class EmployeeServicesComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}

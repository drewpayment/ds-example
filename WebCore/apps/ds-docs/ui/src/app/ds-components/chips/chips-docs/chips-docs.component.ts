import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-chips-docs',
  templateUrl: './chips-docs.component.html',
  styleUrls: ['./chips-docs.component.css']
})
export class ChipsDocsComponent implements OnInit {

  toggleChip: false;
  toggleChipContact: false;
  toggleChipExample = 1;
  chipContactExample = 1;
  constructor() { }

  ngOnInit(): void {
  }

}

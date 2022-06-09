import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-bullet-lists',
  templateUrl: './bullet-lists.component.html',
  styleUrls: ['./bullet-lists.component.scss']
})
export class BulletListsComponent implements OnInit {
    view = true;
  constructor() { }

  ngOnInit() {
    // edit(i) {
    //     console.log(i);
    //   }
  }

  

    bulletList = [
        {id: 1, value: 'red', name: 'Red'},
        {id: 1, value: 'orange', name: 'Orange'},
        {id: 1, value: 'yellow', name: 'Yellow'},
        {id: 1, value: 'green', name: 'Green'},
        {id: 1, value: 'blue', name: 'Blue'},
        {id: 1, value: 'indigo', name: 'Indigo'},
        {id: 1, value: 'violet', name: 'Violet'},
    ]
}


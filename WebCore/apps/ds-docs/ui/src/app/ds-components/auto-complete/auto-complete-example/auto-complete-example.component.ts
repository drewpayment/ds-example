import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IContact } from '@ds/core/contacts';
import { of } from 'rxjs';
import { DEMO_CONTACTS } from '../../shared/contacts';

@Component({
  selector: 'ds-auto-complete-example',
  templateUrl: './auto-complete-example.component.html',
  styleUrls: ['./auto-complete-example.component.scss']
})
export class AutoCompleteExampleComponent implements OnInit {

  contacts: IContact[];
  contactForm: FormGroup;

  get singleCtrl() {
    return this.contactForm.get('single');
  }
  get multiCtrl() {
    return this.contactForm.get('multi');
  }
  constructor(
    private fb: FormBuilder
  ) {
    this.contactForm = fb.group({
      single: [null],
      multi: [null]
    });
   }

  ngOnInit() {
    of( DEMO_CONTACTS ).subscribe(contacts => {
      this.contacts = contacts;
    })
  }
  setPreset() {
    this.singleCtrl.setValue({
      "firstName": "Mary",
      "lastName": "Clark",
      "userId": 102,
      "employeeId": 102,
      "profileImage": {
        "employeeId": null,
        "clientGuid": null,
        "clientId": null,
        "employeeGuid": null,
        "sasToken": null,
        "profileImageInfo": [],
        "extraLarge": {
          "hasImage": true,
          "url": "https://randomuser.me/api/portraits/women/55.jpg"
        }
      }
    });
  }

  setPresets() {
    this.multiCtrl.setValue([
      {
        "firstName": "Jane",
        "lastName": "Smith",
        "userId": 101,
        "employeeId": 101,
        "profileImage": {
          "employeeId": null,
          "clientGuid": null,
          "clientId": null,
          "employeeGuid": null,
          "sasToken": null,
          "profileImageInfo": [],
          "extraLarge": {
            "hasImage": true,
            "url": "https://randomuser.me/api/portraits/women/90.jpg"
          }
        }
      },
      {
        "firstName": "George",  
        "lastName": "Weller",
        "userId": 103,
        "employeeId": 103,
        "profileImage": {
          "employeeId": null,
          "clientGuid": null,
          "clientId": null,
          "employeeGuid": null,
          "sasToken": null,
          "profileImageInfo": [],
          "extraLarge": {
            "hasImage": true,
            "url": "https://randomuser.me/api/portraits/men/18.jpg"
          }
        }
      }
    ]);
  }

}

import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ds-card-docs',
  templateUrl: './card-docs.component.html',
  styleUrls: ['./card-docs.component.scss']
})
export class CardDocsComponent implements OnInit {

  toggleCardHeader = false;
  toggleCardSecondaryHeader = false;
  toggleCardForm = false;
  toggleCardSubForm = false;
  toggleCardSmall = false;
  toggleCardObject = false;
  toggleCardCallout = false;
  toggleCardWidgetLarge = false;
  toggleCardCalloutIcon = false;
  toggleCardCalloutImage = false;
  toggleCardWidgetCollpase = false;
  toggleCardWidget = false;
  toggleCardHandle = false;
  toggleColorVariation= false;
  dsCardDemo = true;
  toggleColor= false;
  toggleCardSection = false;

  CardFullHeaderExample = 1;
  CardHeaderExample = 1;
  CardSecondaryHeaderExample = 1;
  CardFormExample = 1;
  CardSubFormExample = 1;
  CardSmallExample = 1;
  CardObjectExample = 1;
  CardCalloutExample = 1;
  CardWidgetLargeExample = 1;
  CardCalloutIconExample = 1;
  CardCalloutImageExample = 1;
  CardWidgetCollapseExample = 1;
  CardWidgetExample = 1;
  CardHandleExample = 1;
  constructor() { }

  ngOnInit() {
  }

}

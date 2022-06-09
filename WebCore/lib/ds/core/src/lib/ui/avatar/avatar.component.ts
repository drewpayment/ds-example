import {
  Component,
  HostBinding,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';

@Component({
  selector: 'ds-avatar',
  templateUrl: './avatar.component.html',
  styleUrls: ['./avatar.component.scss'],
})
export class AvatarComponent implements OnInit, OnChanges {
  _color: string;

  @Input()
  size: number;

  @Input()
  resource?: string;

  @Input()
  firstName?: string;

  @Input()
  lastName?: string;

  @Input()
  name?: string;

  // Vendor is a string to make it work on paycheck-table.component.ts
  // Not sure why boolean doesn't return a boolean
  @Input()
  vendor?: string;

  @Input('color')
  get color(): string {
    return this._color;
  }
  set color(value: string) {
    // // If no color input is set, set it to blue. Otherwise, use the color that was set
    // if ( value !== 'default' &&
    //      value !== 'purple'  ) {
    //   this._color = 'default';
    // } else {
    this._color = value;
    //}
  }

  nameSplitFirst: string;
  nameSplitLast: string;
  saveResource: string;
  constructor() {}

  ngOnInit() {
    if (this.resource) this.saveResource = this.resource;
    if (!this.vendor) this.vendor = 'false';

    // Handles cases where a dev uses only a name item (last, first) rather than lastName item and firstName item
    if (this.name && this.vendor == 'false') {
      let nameSplit = this.name.split(',');

      if (nameSplit) {
        this.nameSplitFirst = nameSplit[1].trim();
        this.nameSplitLast = nameSplit[0].trim();
      }
    }
  }

  ngOnChanges(simpleChanges: SimpleChanges) {
    if (!simpleChanges) return;

    if (simpleChanges.resource && !simpleChanges.resource.firstChange) {
      this.resource = simpleChanges.resource.currentValue;
    }

    if (simpleChanges.color && !simpleChanges.color.firstChange) {
      this.color = simpleChanges.color.currentValue;
    }
  }
}

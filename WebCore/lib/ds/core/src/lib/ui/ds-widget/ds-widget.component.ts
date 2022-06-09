import { Component, HostBinding, Input, OnChanges, OnInit } from '@angular/core';

type WidgetMode = 'card' | 'statistic' | 'statisticNobody';

@Component({
  selector: 'ds-widget',
  templateUrl: './ds-widget.component.html',
  styleUrls: ['./ds-widget.component.scss']
})
export class DsWidgetComponent implements OnInit {
    private _mode: WidgetMode;
    private _cardModeClass: string;
    private _color: string;

    //Loading
    @Input() loaded: boolean;
    @Input() skeleton = '';

    // Widget Mode
    @Input('mode')
    set mode(value: WidgetMode) {
        if (value !== 'statistic' && value !== 'statisticNobody')
            this._mode = 'card';
        else    
            this._mode = value;
    }
    get mode(): WidgetMode {
        return this._mode;
    }

    // Controls color of Icon
    @Input('color')
    set color(value: string) {
    if (value !== 'primary' && value !== 'secondary' && value !== 'danger' && value !== 'warning' &&
        value !== 'info' && value != 'yellow' && value !== 'light' && value !== 'dark' && value !== 'success' &&
        value !== 'form-danger' && value !== 'gray' && value !== 'medium' &&
        value !== 'medium-dark' && value !== 'super-dark-gray' && value !== 'archive' && 
        value !== 'disabled' && value !== 'navy') {
            this._color = 'primary';
    } else {
        this._color = value;
    }
    }
    get color(): string {
        return this._color;
    }

    private resolveCardMode(): boolean {
        this._cardModeClass =
          this._mode === 'statistic' ? 'text-center'
        : this._mode === 'statisticNobody' ? 'text-center'
        : '';
        return this._cardModeClass != null;
    }
    @Input('hover') hover: boolean;

    // Classes applied to component
    @HostBinding('class')
    get hostClasses(): string {
        return [
            '',
            this.resolveCardMode() ? `${this._cardModeClass}` : '',
            this._color ? `${this._color}` : '',
        ].join(' ');
    }
    
  constructor() { }

  ngOnInit() {
  }
}

// Header
@Component({
    selector: 'ds-widget-header, [ds-widget-header], [dsWidgetHeader]',
    templateUrl: './ds-widget-header.html',
    styleUrls: ['./ds-widget.component.scss'],
    host: {
        '[class.ds-widget-header]': 'true',
        '[class.flex-grow-1]': 'this.grow == "true"'
    }
})
export class DsWidgetHeader implements OnInit {
    @Input('grow') grow: boolean;
    ngOnInit() {
    }

}

@Component({
    selector: 'ds-widget-icon, [ds-widget-icon], [dsWidgetIcon]',
    template: `
        <div class="ds-widget-icon">
            <mat-icon><ng-content></ng-content></mat-icon>
        </div>
    `,
    styleUrls: ['./ds-widget.component.scss']
})
export class dsWidgetIcon implements OnInit {
    ngOnInit() {
    }
}

@Component({
    selector: 'ds-widget-title-value, [ds-widget-title-value], [dsWidgetTitleValue]',
    template: `
        <div class="ds-widget-title-value"><ng-content></ng-content></div>
    `,
    styleUrls: ['./ds-widget.component.scss']
})
export class dsCardTitleValue implements OnInit {
    ngOnInit() {
    }
}

@Component({
    selector: 'ds-widget-title, [ds-widget-title], [dsWidgetTitle]',
    template: `
        <div class="ds-widget-title" [class.text-truncate]="this.truncate"><ng-content></ng-content></div>
    `,
    styleUrls: ['./ds-widget.component.scss']
})
export class dsCardTitle implements OnInit {
    @Input('truncate') truncate: boolean;
    ngOnInit() {
    }
}

@Component({
    selector: 'ds-widget-content, [ds-widget-content], [dsWidgetContent]',
    templateUrl: './ds-widget-content.html',
    host: {
        '[class.flex-grow-1]': 'this.grow == "true"'
    },
    styleUrls: ['./ds-widget.component.scss']
})
export class dsWidgetContent implements OnInit {
    @Input('grow') grow: boolean;
    ngOnInit() {
    }
}

@Component({
    selector: 'ds-widget-detail, [ds-widget-detail], [dsWidgetDetail]',
    template: `
        <div [class.bold]="this.bold"><ng-content></ng-content></div>
    `,
    host: {
        '[class.d-block]': 'true',
        '[class.mt-1]': 'true' 
    },
    styleUrls: ['./ds-widget.component.scss']
})
export class dsWidgetDetail implements OnInit {
    @Input('bold') bold: boolean;
    ngOnInit() {
    }
}

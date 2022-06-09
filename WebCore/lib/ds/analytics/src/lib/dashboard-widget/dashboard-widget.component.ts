import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges, OnDestroy } from '@angular/core';
import { InfoData } from '../shared/models/InfoData.model';
import { HeaderDropDownOption } from '../shared/models/HeaderDropDownOption.model';
import { FormControl } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
    selector: 'ds-dashboard-widget',
    templateUrl: './dashboard-widget.component.html',
    styleUrls: ['./dashboard-widget.component.css']
})
export class DashboardWidgetComponent implements OnInit {

    // All Inputs that define the size and information on the card
    @Input() cardType = 'graph';
    @Input() loaded: boolean;
    @Input() hover = false;
    @Input() skeleton = '';

    constructor() { }

    ngOnInit() { }
}

// Content that displays on the card
@Component({
    selector: 'ds-dashboard-widget-content, [ds-dashboard-widget-content]',
    templateUrl: './widget-content.component.html'
})
export class WidgetContent {

    @Input() infoData: InfoData;
    @Input() oneLine = false;
    @Input() flexGrow = true;

}

// Content that displays on the card
@Component({
    selector: 'ds-dashboard-widget-bottom-content, [ds-dashboard-widget-bottom-content]',
    templateUrl: './widget-bottom-content.component.html'
})
export class WidgetBottomContent {

}

@Component({
    selector: 'ds-dashboard-widget-header, [ds-dashboard-widget-header]',
    templateUrl: './widget-header.component.html'
})
export class WidgetHeaderComponent implements OnInit, OnDestroy {

    firstLoad = true;
    @Input() title: string;
    @Input() tooltip: string;
    @Input() availableChartTypes: string[] = [];
    @Input() settingsItems: string[] = [];
    @Input() headerDropdownOptions: HeaderDropDownOption[];

    @Output() headerDropdownChange = new EventEmitter();
    @Output() chartChange = new EventEmitter();
    @Output() settingSelected = new EventEmitter();

    // Local variables to store current information
    selectedHeaderOption: string;
    selectedChartType: string;

    // ngModel for Header Drop Down
    options: HeaderDropDownOption[] = [];
    headerDropdownOption = new FormControl();
    destroy$ = new Subject();

    ngOnInit() {
        if (this.availableChartTypes.length > 0) {
            this.chartChanged(this.availableChartTypes[0]);
        }
        this.headerDropdownOption.valueChanges
            .pipe(takeUntil(this.destroy$))
            .subscribe(value => {
                const selected = this.options.find(o => o.id == value);
                if (!selected) return;

                this.headerDropdownChanged(selected);
            });

        this.loadHeaderOptions(this.headerDropdownOptions);
    }

    ngOnDestroy() {
        this.destroy$.next();
    }

    headerDropdownChanged(value: HeaderDropDownOption) {
        this.headerDropdownChange.emit({
            'event': 'HeaderDropdown',
            'value': value.id,
        });
    }

    chartChanged(event) {
        if (this.selectedChartType != event) {
            this.selectedChartType = event;
            this.chartChange.emit({
                'event': 'ChartType',
                'value': event
            });
        }
    }

    settingSelect(event) {
        this.settingSelected.emit({
            'event': 'SettingsDropdown',
            'value': event
        });
    }

    private loadHeaderOptions(options: HeaderDropDownOption[]) {
        if (options && options.length) {
            this.options = [...options];

            if (this.firstLoad) {
                this.firstLoad = false;

                this.headerDropdownOption.setValue(this.options[0].id, { emitEvent: false });
            }
        }
    }

}

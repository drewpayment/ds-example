import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import * as moment from "moment";
import { DateRange } from '../shared/models/DateRange.model';

@Component({
  selector: 'ds-filter-card',
  templateUrl: './filter-card.component.html',
  styleUrls: ['./filter-card.component.css']
})
export class FilterCardComponent implements OnInit {


  @Input() title: string = 'Company Analytics';
  @Input() dashboards: string[];
  @Input() filters: any[]; //TODO: Fix any typing
  @Input() dateRanges: string[];

  @Input() currentDashboard: string;
  @Input() currentFilters: any;
  @Input() currentDateRangeType: string  = 'Current Month';
  @Input() currentDateRangeCustom: DateRange;

  @Output() dashboardChanged = new EventEmitter();
  @Output() dateRangeChanged = new EventEmitter();
  @Output() submittedFilter = new EventEmitter();

  showFilters: boolean = false;
  showDateRangeFilters: boolean = false;
  activeFilters = [];

  constructor() { }

  ngOnInit() {
    this.filters = this.filters.filter(x => { return x.title != "Competency Model" })

    this.dashboardChange(this.currentDashboard)

    this.dateRangeChange();
  }

  GetDates(dateRange){
    var today = new Date(new Date())

    switch(dateRange){
      case 'Last 12 Months':
        return {
          StartDate: new Date(new Date().getFullYear() - 1, new Date().getMonth(), new Date().getDate() + 1, new Date().getHours(), new Date().getMinutes(), new Date().getSeconds()),
          EndDate: new Date()
        }
      case 'Current Week':
        return {
          StartDate: new Date(today.setDate(today.getDate() - today.getDay())),
          EndDate: new Date(),
          type: "current week"
        }
      case 'Last Week':
        return {
          StartDate: new Date(today.setDate(today.getDate() - 7 - today.getDay())),
          EndDate: new Date(new Date(today.setDate(today.getDate() + 6)).setHours(23, 59, 59)),
          type: "last week"
        }
      case 'Current Month':
        return {
          StartDate: new Date(new Date(new Date().setDate(1)).setHours(0, 0, 0, 0)),
          EndDate: new Date(today.getFullYear(), today.getMonth() + 1, 0),
          type: "current month"
        }
      case 'This Year':
        return {
          StartDate: new Date(new Date().getFullYear(), 0, 1),
          EndDate: new Date(new Date().getFullYear(), 11, 31)
        }
      case 'Last Year':
        return {
          StartDate: new Date(new Date().getFullYear() - 1, 0, 1),
          EndDate: new Date(new Date().getFullYear() - 1, 11, 31)
        }
      case 'Custom':
        return {
          StartDate: this.currentDateRangeCustom.StartDate,
          EndDate: this.currentDateRangeCustom.EndDate
        }
      default:
        return { StartDate: 'Unknown', EndDate: 'Unknown' }
    }
  }

  dashboardChange(event){
    this.dashboardChanged.emit({
      event: "Dashboard Changed",
      value: event
    });

    if (this.currentDashboard == 'Payroll' || this.currentDashboard == 'User Performance') {
        this.showDateRangeFilters = false;
        this.currentDateRangeType = "Last 12 Months";
        this.dateRangeChange();
    }
    else {
        this.showDateRangeFilters = true;
    }
    this.submitFilter()
  }

  dateRangeChange(){
    if(this.currentDateRangeType == 'Custom' && Object.keys(this.currentDateRangeCustom).length === 0){
      this.currentDateRangeCustom.StartDate = new Date(new Date().setFullYear(new Date().getFullYear() - 1 ));
      this.currentDateRangeCustom.EndDate = new Date()
    }
    if(Object.keys(this.currentDateRangeCustom).length == 0 || (new Date(this.currentDateRangeCustom.EndDate) >= new Date(this.currentDateRangeCustom.StartDate))){
      this.dateRangeChanged.emit({
        event: "Date Range Changed",
        value: this.GetDates(this.currentDateRangeType),
        type: this.currentDateRangeType
      })
    }
  }

  submitFilter(){
    this.activeFilters = this.readFilters(this.currentFilters);

    this.submittedFilter.emit({
      event: "Submit Filter",
      value: this.currentFilters
    });
  }

  clearFilters(){
    this.activeFilters = [];

    for(const c in this.currentFilters){
      if(c != 'dateRangeType' && c != 'dateRange') this.currentFilters[c] = "";
    }

    this.submittedFilter.emit({
      event: "Submit Filter",
      value: this.currentFilters,
      fillWidgets: true
    });

  }

  removeFilter(filter){
    this.activeFilters = this.activeFilters.filter(f => f !== filter);

    this.currentFilters[filter.type] = ""

    this.submittedFilter.emit({
      event: "Submit Filter",
      value: this.currentFilters,
      fillWidgets: true
    });
  }

  readFilters(form){
    var filters = [];

    for(const [key, val] of Object.entries(form)){
      if(key != 'dateRangeType' && key != 'dateRange' && val){
        var itemArray = this.filters.filter(v => v.title == key).map(x => x.items)[0];
        var items = itemArray.filter(v => v.id == val);

        if(items.length != 0){
          var text = items[0].name
          filters.push({
            type: key,
            value: text
          })
        }
      }
    }

    return filters;
  }

  toggleFilters(){
    this.showFilters = !this.showFilters;
  }

}

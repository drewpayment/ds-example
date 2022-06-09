import { Component, OnInit, Input} from '@angular/core';
import { InfoData } from '@ds/analytics/shared/models/InfoData.model';
import { ResourceApiService } from '@ds/core/resources/shared/resources-api.service';
import { AccountService } from '@ds/core/account.service';
import { IEmployeeImage } from '@ajs/core/ds-resource/models';
import { IExceptionData } from '@ds/analytics/shared/models/IExceptionData.model';
import { ExceptionCardDialogComponent } from './exception-card-dialog/exception-card-dialog.component';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { DateRange } from '@ds/analytics/shared/models/DateRange.model';
import { AnalyticsApiService } from '@ds/analytics/shared/services/analytics-api.service';
import * as moment from "moment";
import { MOMENT_FORMATS } from '@ds/core/shared/moment-formats';
import { switchMap, tap } from 'rxjs/operators';
import { UserInfo } from '@ds/core/shared';
import { LoadingDialogService } from '@ds/analytics/shared/services/loading-dialog.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'ds-exception-card',
  templateUrl: './exception-card.component.html',
  styleUrls: ['./exception-card.component.css']
})
export class ExceptionCardComponent implements OnInit {

  @Input() data: any;
  @Input() dateRange: DateRange;

  loaded: boolean = false;

  cardType = "info";

  infoData: InfoData;
  scheduleData: any[];
  scheduleEmployeeIds: number[] = [];

  //Profile Images
  employeeImage: any = [];
  employeeList: number[] = [];
  employeeCount: number;

  constructor(
    private resourceApiService: ResourceApiService,
    private accountService: AccountService,
    private dialog: MatDialog,
    private analyticsApi: AnalyticsApiService,
    private loadingSvc: LoadingDialogService,
    private messageService:DsMsgService,
    ) {}

  ngOnInit() {
    //Create List of employees associated
    this.employeeList = this.data.map((x) => x.employeeID);
    this.employeeList = Array.from(new Set(this.employeeList));
    this.employeeCount = this.employeeList.length;

    //Get the images
    this.accountService.getUserInfo().subscribe((user) => {
      for (var i = 0; i < 8; i++){
        if (this.employeeList[i] != undefined){
          this.resourceApiService.getEmployeeProfileImages(user.clientId, this.employeeList[i]).subscribe(employeeImageData => {
            let employee = this.data.find((x) => x.employeeID == employeeImageData.employeeId);
            this.employeeImage.push({
              image: employeeImageData,
              employee: employee
            });
          });
        }
      }
    });

    //Get Employee ID's for schedule data
    this.data.forEach(employee => {
      this.scheduleEmployeeIds.push(employee.employeeID);
    });

    this.infoData = {
      icon: '',
      color: 'success',
      value: `${this.data.length.toLocaleString("en")}`,
      title: `${this.data[0].clockException}`,
      showBottom: true
    };
    this.loaded = true;
  }

  openDialog() {
    this.loadingSvc.showDialog();
    this.accountService.getUserInfo().pipe(
      switchMap( user => this.analyticsApi.getEmployeeSchedule(user.clientId, this.scheduleEmployeeIds, 
        moment(this.dateRange.StartDate).format(MOMENT_FORMATS.DATE),
        moment(this.dateRange.EndDate).format(MOMENT_FORMATS.DATE)) ) ,
      tap((schedules: any) => {
        this.loadingSvc.hideDialog();
        this.scheduleData = schedules;
        this.data.forEach(element => {
          element.employeeSchedule = this.getSchedule(element.employeeID, element.eventDate);
        });
        let config = new MatDialogConfig<IExceptionData>();

        config.data = {
            employeeExceptions: this.data,
            featureName: this.data[0].clockException,
            dateRange: this.dateRange,
            scheduleData: schedules
        };

        config.width = "1000px";

        return this.dialog.open<ExceptionCardDialogComponent,IExceptionData,null>(ExceptionCardDialogComponent, config);
      })
    ).subscribe(resp => {}, (error: HttpErrorResponse) => {
      this.messageService.showWebApiException(error.error);
      this.loadingSvc.hideDialog();
    });
  }

  getTimeFromDate(dateTime){
    if (dateTime){
      let date = new Date(dateTime);
      let meridiem = date.getHours() >= 12 ? "pm" : "am";
      let time = this.hours12(date) + ":" + this.getMinutesWithLeadingZeros(date) + meridiem;
      return time;
    }
    else{
      return null
    }
  }

  getMinutesWithLeadingZeros(dt)
  {
    return (dt.getMinutes() < 10 ? '0' : '') + dt.getMinutes();
  }

  hours12(date) {
    return (date.getHours() + 24) % 12 || 12;
  }

  getSchedule(employeeId, eventDate){
    var eventDateDate = new Date(eventDate);
    var arr = this.scheduleData.filter(x => x.employeeId == employeeId && new Date(x.eventDate).getTime() == eventDateDate.getTime());
    if (arr.length > 0){
      var startTime = new Date(arr[0].startTime1);
      var endTime = new Date(arr[0].endTime1);
      return this.getTimeFromDate(startTime) + " - " + this.getTimeFromDate(endTime);
    }
    else{
      return ""
    }
  }

}

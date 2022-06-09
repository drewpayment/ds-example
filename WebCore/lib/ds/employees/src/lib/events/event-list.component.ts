import { UserType } from "@ajs/user";
import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
  OnInit,
  Input,
} from "@angular/core";
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { AccountService } from "@ds/core/account.service";
import {
  AppConfig,
  APP_CONFIG,
} from "@ds/core/app-config/app-config";
import { ClientService } from "@ds/core/clients/shared";
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import {
  BehaviorSubject,
  EMPTY,
  forkJoin,
  NEVER,
  Observable,
  of,
  Subject,
  throwError,
} from "rxjs";
import {
  catchError,
  filter,
  map,
  switchMap,
  takeUntil,
  tap,
} from "rxjs/operators";
import { ConfigUrl, ConfigUrlType } from "@ds/core/shared/config-url.model";
import { UserInfo } from "@ds/core/shared";
import * as moment from "moment";
import { MatDialog } from "@angular/material/dialog";
import { Store } from "@ngrx/store";
import {
  EmployeeState,
  getEmployeeState,
} from "@ds/employees/header/ngrx/reducer";
import { IEmployeeSearchResult } from "@ds/employees/search/shared/models/employee-search-result";
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeTrackerService } from '@ds/core/ui/forms/change-track/change-tracker.service';
import { Router } from '@angular/router';
import { SSL_OP_MSIE_SSLV2_RSA_PADDING } from 'constants';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { changeDrawerHeightOnOpen, matDrawerAfterHeightChange } from "@ds/core/ui/animations/drawer-auto-height-animation";
import * as angular from "angular";
import { IClientSubTopic, IClientTopic, IEvent } from "@ds/core/shared/event.model";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { EventService } from "lib/services/event.service";

@Component({
  selector: "ds-event-list",
  templateUrl: "./event-list.component.html",
  animations: [
    changeDrawerHeightOnOpen,
    matDrawerAfterHeightChange
  ]
})
export class EventListComponent implements OnInit, OnDestroy {
  private _isEssMode = false;
  @Input()
  get isEssMode(): boolean {
      return this._isEssMode;
  }
  set isEssMode(value: boolean) {
      this._isEssMode = coerceBooleanProperty(value);
  }
  
  destroy$ = new Subject();
  baseUrl = this.config.baseSite.url;
  formSubmitted: boolean;
  
  hasError: boolean;
  itemsViewable: boolean;
  clientId: number;
  
  isLoading: boolean = true;
  isAdding:boolean = false;
  eventList:Array<IEvent>;
  activeEvent:IEvent = null;
  activeId:number = 0;
  eventKlicked:IEvent = null;
  submitted:boolean;

  hrBlocked: boolean;
  allowAddEmployee: boolean;
  essViewOnly: boolean;
  essProfile: string;
  isSupervisorOnHimself: boolean;

  clientTopics:IClientTopic[];
  subTopicsTmp:IClientSubTopic[];
  clientSubTopics:IClientSubTopic[];

  form: FormGroup;

  @Input() user: UserInfo;
  @Input() employeeId: number;

  empEvents$ = (clientId:number, employeeId:number) => this.eventApi.getEmployeeEvents(employeeId).pipe(
    switchMap(eventList => {
      this.eventList = eventList || [];
      this.eventList.sort((x,y)=> moment(x.eventDate).toDate() < moment(y.eventDate).toDate() ? 1:-1 );
      this.itemsViewable =  (!this.isEssMode || eventList.filter(x=>x.isEmployeeViewable).length > 0);
      return this.eventApi.getClientSubTopicsByIds(clientId, this.eventList.map(x=>x.clientSubTopicId));
    }),
    tap(subTopics =>{
      this.subTopicsTmp = subTopics||[];
      this.clientSubTopics = angular.copy(this.subTopicsTmp) || [];
    })
  );

  selectedEmployee$ = this.store.select(
    getEmployeeState((x) => x.selectedEmployee)
  ) as any as Observable<IEmployeeSearchResult>;

  constructor(
    private router: Router,
    private accountService: AccountService,
    private msg: NgxMessageService,
    private confirmDialog: ConfirmDialogService,
    @Inject(APP_CONFIG) private config: AppConfig,
    private store: Store<EmployeeState>,
    private eventApi: EventService,
    private changeTrackerService: ChangeTrackerService,
    private fb: FormBuilder,
  ) {}

  ngOnInit() {
    this.isLoading = true;
    this.eventList = [];
    this.activeEvent = null;
    this.activeId = 0;
    
    this.clientId = this.user.selectedClientId();
    this.hrBlocked = this.user.isHrBlocked;
    this.essViewOnly = !this.isEssMode && this.user.isEmployeeSelfServiceViewOnly;
    this.allowAddEmployee = this.user.addEmployee;

    if(this.hrBlocked) 
	    this.router.navigate(['error'],  { queryParams:  {showButton: false, showHelpMessage: false, message:'You do not have access to this information.'},  
                                       queryParamsHandling: "merge" });
    this.createForm();

    this.eventApi.getClientTopics(this.clientId).subscribe(x=>{
      this.clientTopics = x;
    });

    

    this.empEvents$(this.clientId,this.employeeId).subscribe((x : Array<any>) => {
      this.isLoading = false;
    }, (error: HttpErrorResponse) => {
      this.msg.setErrorResponse(error);
      this.isLoading = false;
    }); 
    
    this.eventApi.topicsChanged$.subscribe((x : boolean) => {
      if (x) {
        let existingClientTopicId = Number(this.form.value.clientTopic);
        this.clientTopics = [];        
        this.eventApi.getClientTopics(this.clientId).subscribe(topics=>{
          this.clientTopics = topics;

          var topicIndex = this.clientTopics.findIndex((x) => x.clientTopicId == existingClientTopicId);
          if(topicIndex > -1) 
            this.form.controls['clientTopic'].patchValue(existingClientTopicId);
          else 
            this.form.controls['clientTopic'].patchValue(0);  

          let existingClientSubTopicId = Number(this.form.value.clientSubTopic);
          this.loadSubTopics(this.form.value.clientTopic);

          var subTopicIndex = this.clientSubTopics.findIndex((x) => x.clientSubTopicId == existingClientSubTopicId);
          if(subTopicIndex > -1) 
            this.form.controls['clientSubTopic'].patchValue(existingClientSubTopicId);
          else 
            this.form.controls['clientSubTopic'].patchValue(0);  

          this.form.controls['clientSubTopic'].patchValue(existingClientSubTopicId);

          this.applyFilters();
        });
      }
    }, (error: HttpErrorResponse) => {
      this.msg.setErrorResponse(error);
    });
  }

  private createForm() {
    this.form = this.fb.group({
      sortBy: this.fb.control('2', []),
      clientTopic: this.fb.control('0', []),
      clientSubTopic: this.fb.control('0', []),
    });
  }

  loadSubTopics(topicId:number) {
    this.clientSubTopics = [];
    if (topicId) {
        this.eventApi.getClientSubTopics(this.clientId, topicId).subscribe(subs => {
            this.clientSubTopics = subs;
        });
    }
    else 
      this.clientSubTopics = angular.copy(this.subTopicsTmp) || [];

    this.form.controls['clientSubTopic'].patchValue(0);
  }

  topicChange() {
    this.clientSubTopics = [];
    let clientTopicId = Number(this.form.value.clientTopic);
    if (clientTopicId && clientTopicId > 0) {
        this.loadSubTopics(clientTopicId);
    }
  }

  applyFilters() {
    this.isLoading = true;

    if (this.form.value.clientTopic != 0)
      this.eventList = this.eventList.filter((item) => item.clientTopicId === Number(this.form.value.clientTopic));
  
    if (this.form.value.clientSubTopic != 0)
      this.eventList = this.eventList.filter((item) => item.clientSubTopicId === Number(this.form.value.clientSubTopic));

    if (this.form.value.sortBy == 1)
      this.eventList.sort((x,y)=> moment(x.eventDate).toDate() > moment(y.eventDate).toDate() ? 1:-1 );
    else if (this.form.value.sortBy == 2)  
      this.eventList.sort((x,y)=> moment(x.eventDate).toDate() < moment(y.eventDate).toDate() ? 1:-1 );
    else if (this.form.value.sortBy == 3)
      this.eventList.sort((x,y)=> x.clientTopicDescription  > y.clientTopicDescription ? 1:-1 );
    else if (this.form.value.sortBy == 4)  
    this.eventList = this.eventList.sort((x,y)=> moment(x.expirationDate || new Date(-8640000000000000)).toDate() > moment(y.expirationDate || new Date(-8640000000000000)).toDate() ? 1:-1 );
    else if (this.form.value.sortBy == 5)
      this.eventList.sort((x,y)=> x.duration || 0 > y.duration || 0 ? 1:-1 );
    else if (this.form.value.sortBy == 6)  
      this.eventList.sort((x,y)=> x.duration || 0 < y.duration || 0 ? 1:-1 );

    this.isLoading = false;  
  }

  fetchDataAndApplyFilters() {
    this.isLoading = true;
    this.empEvents$(this.clientId,this.employeeId).subscribe((x : Array<any>) => {
      this.isLoading = false;
      this.applyFilters();
    }, (error: HttpErrorResponse) => {
      this.msg.setErrorResponse(error);
      this.isLoading = false;
    }); 
  }

  private getTopicName(topicId){
    let k = this.clientTopics.find(x => x.clientTopicId==topicId);
    return k ? k.description : "";
  }

  private getSubTopicName(subTopicId){
    let k = this.subTopicsTmp.find(x=>x.clientSubTopicId==subTopicId);
    return k ? k.description : "";
  }

  ngOnDestroy() {
    this.destroy$.next();
  }

  getNext(){
    let minId = Math.min(...this.eventList.map(x=>x.employeeEventId));
    if(!minId || minId>0) minId = 0;
    minId = (minId-1);
    var newName = "Noname " + (-minId);
    while(this.eventList.find(x=>x.event == newName) )
        newName = "Noname " + newName;
    return {id:minId,name:newName};
  }

  addEvent(){
      this.clearDrawer();
      var next = this.getNext();

      let newItem = <IEvent>{
        employeeEventId : next.id,  
        employeeId: this.employeeId,
        clientTopicId: null,
        clientSubTopicId: null,
        eventDate: new Date(),
        event: '',
        duration: 0,
        isEmployeeViewable: false,
        isEmployeeEditable: false,
        expirationDate: null,
        isDirty:true,
        clientId: this.user.lastClientId || this.user.clientId,
      };
      this.eventList.push(newItem);
      this.toggleActiveEvent(next.id);
      this.isAdding = true;
  }

  makeHovered(tl) {
    if (this.eventKlicked) {
      this.eventKlicked.hovered = false;
      this.eventKlicked = null;
    }
    tl.hovered = true;
  }
  cogClicked(tl:IEvent) {
      this.eventKlicked = tl;
      this.eventKlicked.hovered = false;
      setTimeout(() => {
        tl.hovered = true;
      }, 100);
  }
  deleteEventDialog(tl:IEvent){
      const options = {
          title: `Are you sure you want to delete "${ this.getTopicName(tl.clientTopicId) } | ${this.getSubTopicName(tl.clientSubTopicId)}" ?`,
          message: "",
          confirm: "Delete",
        };

      this.confirmDialog.open(options);
      this.confirmDialog.confirmed()
      .pipe(
      switchMap((confirmed) => {
          if (confirmed) {
              if(tl.employeeEventId < 0 ) 
              return of(true);
              else {
                this.msg.loading( true,'Sending...');
                return this.eventApi.deleteActiveEvent(tl);
              }
          } else {
          return EMPTY;
          }
      })
      )
      .subscribe(done => {
          if(done) {
              // remove from the list
              var inx = this.eventList.findIndex(
                  (x) => x.employeeEventId == tl.employeeEventId
              );
              if(inx>-1)this.eventList.splice(inx, 1);

              this.msg.setSuccessMessage('Event deleted successfully.');
              if(this.eventList.length){
                  this.clearDrawer();
              }
          }
      }, (error: HttpErrorResponse) => {
            this.msg.setErrorResponse(error);
      });
  }

  toggleActiveEvent(listId: number) {
      this.isAdding = false;
      this.changeTrackerService.clearIsDirty();
      if (this.activeId == listId) {
        this.clearDrawer();
      } else {
        this.activeId = listId;
        this.activeEvent = this.eventList.find(x => {
          return (x.employeeEventId == this.activeId);
        });
        this.eventApi.setActiveEvent(this.activeEvent);
      }
  }

  clearDrawer() {
    let addNewIndex = this.eventList.findIndex(x=>x.employeeEventId<0);
    if (addNewIndex > -1) {
      this.eventList.splice(addNewIndex, 1);
      this.isAdding = false;
      this.changeTrackerService.clearIsDirty();
    }

    this.activeId = 0;
    this.activeEvent = null;
  }

  refresh(id: number){
      if(id<0)
          this.clearDrawer();
      else{
        this.applyFilters();
        
        if(this.activeEvent){
          let k = this.subTopicsTmp.find(x=>x.clientSubTopicId==this.activeEvent.clientSubTopicId);
          if(!k){
            let load$ = this.eventApi.getClientSubTopic(this.activeEvent.clientSubTopicId).subscribe(x=>{
              this.subTopicsTmp.push(x);
              load$.unsubscribe();
            })
          }
        }
        this.clearDrawer();
      }
  }
}
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
  selector: "ds-group-event-form",
  templateUrl: "./group-event-form.component.html",
  animations: [
    changeDrawerHeightOnOpen,
    matDrawerAfterHeightChange
  ]
})
export class GroupEventFormComponent implements OnInit, OnDestroy {
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

  // @Input() user: UserInfo;
  // @Input() employeeId: number;

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
  }

  ngOnDestroy() {
    this.destroy$.next();
  }
}
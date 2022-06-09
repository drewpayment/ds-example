import { UserType } from "@ajs/user";
import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
  OnInit,
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
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { IEmergencyContact } from '../../models/contact/emergency-contact.model';
import { EmergencyContactService } from '../../Services/emergency-contact.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeTrackerService } from '@ds/core/ui/forms/change-track/change-tracker.service';
import { Router } from '@angular/router';
import { changeDrawerHeightOnOpen, matDrawerAfterHeightChange } from "@ds/core/ui/animations/drawer-auto-height-animation";

@Component({
  selector: "ds-emergency-contact",
  templateUrl: "./emergency-contact.component.html",
  styleUrls: ["./emergency-contact.component.scss"],
  animations: [
    changeDrawerHeightOnOpen,
    matDrawerAfterHeightChange
  ]
})
export class EmergencyContactComponent implements OnInit, OnDestroy {
  destroy$ = new Subject();
  baseUrl = this.config.baseSite.url;
  formSubmitted: boolean;
  
  hasError: boolean;
  clientId: number;
  employeeId: number;
  isLoading: boolean = true;
  isAdding:boolean = false;
  contactList:Array<IEmergencyContact>;
  activeContact:IEmergencyContact = null;
  activeId:number = 0;
  contactKlicked:IEmergencyContact = null;
  submitted:boolean;

  userinfo: UserInfo;
  totalCount: number = 0;
  hrBlocked: boolean;
  allowAddEmployee: boolean;
  breadcrumb: string;
  essProfile: string;
  isSupervisorOnHimself: boolean;

  payrollUrl: ConfigUrl;
  companyUrl: ConfigUrl;

  selectedEmployee$ = this.store.select(
    getEmployeeState((x) => x.selectedEmployee)
  ) as any as Observable<IEmployeeSearchResult>;

  get displayList() { return this.contactList.filter(x=>x.employeeEmergencyContactId>-1); }

  constructor(
    private router: Router,
    private accountService: AccountService,
    private clientService: ClientService,
    private msg: NgxMessageService,
    private dialog: MatDialog,
    private confirmDialog: ConfirmDialogService,
    @Inject(APP_CONFIG) private config: AppConfig,
    private store: Store<EmployeeState>,
    private contactApi: EmergencyContactService,
    private changeTrackerService: ChangeTrackerService,
  ) {}

  ngOnInit() {
    this.isLoading = true;
    this.contactList = [];
    this.activeContact = null;
    this.activeId = 0;

    forkJoin([this.accountService.getUserInfo(true), this.accountService.getSiteUrls()]).subscribe( ([user,sites]) => {
      this.userinfo = user;
      this.clientId = user.selectedClientId();
      this.hrBlocked = user.isHrBlocked;
      this.allowAddEmployee = user.addEmployee;
      this.employeeId = user.lastEmployeeId ;

      // redirect to no permisions page
      if(this.hrBlocked) 
        this.router.navigate(['error'],  { queryParams:  {showButton: false, showHelpMessage: false, message:'You do not have access to this information.'},  
                                           queryParamsHandling: "merge" });

      this.payrollUrl = sites.find((s) => s.siteType === ConfigUrlType.Payroll);
      let essUrl      = sites.find((s) => s.siteType === ConfigUrlType.Ess);
      this.companyUrl = sites.find((s) => s.siteType === ConfigUrlType.Company);
      this.essProfile = `${essUrl.url}profile`;

      this.breadcrumb = `${this.payrollUrl.url}ChangeEmployee.aspx?SubMenu=Employee&Force=True&URL=${this.companyUrl.url}manage/emergency-contacts`;
      // if the user doesn't have an employee selected, redirect them to the employee select list
      if (this.employeeId == null || this.employeeId < 1) {
        document.location.href = this.breadcrumb;
        return;
      }

      this.isSupervisorOnHimself = this.userinfo.userEmployeeId == this.employeeId;
      let api$ = of([]);
      if (!this.isSupervisorOnHimself)
        api$ = this.contactApi.getEmployeeEmergencyContacts(this.employeeId);

      api$.subscribe((x : IEmergencyContact[]) => {
        this.loadContacts(x);
        this.isLoading = false;
      }, (error: HttpErrorResponse) => {
        this.msg.setErrorResponse(error);
        this.isLoading = false;
      })
    })

    this.selectedEmployee$
      .pipe(
        takeUntil(this.destroy$),
        filter((x) => !!x && x.employeeId != this.employeeId),
        switchMap(( ee :  IEmployeeSearchResult) => {
          if(!this.userinfo) return EMPTY;          
          this.isLoading = true;
          this.employeeId = ee.employeeId ;
          this.isSupervisorOnHimself = this.userinfo.userEmployeeId == this.employeeId;
          let api$ = of([]);
          if (!this.isSupervisorOnHimself)
            api$ = this.contactApi.getEmployeeEmergencyContacts(this.employeeId);
          return api$;
        }),
      )
      .subscribe((x : IEmergencyContact[]) => {
        this.loadContacts(x);
        this.isLoading = false;
      }, (error: HttpErrorResponse) => {
        this.msg.setErrorResponse(error);
        this.isLoading = false;
      });
  }

  loadContacts(contactList: IEmergencyContact[]){
    this.contactList = contactList;
    this.isLoading = false;
    this.clearDrawer();
  }

  ngOnDestroy() {
    this.destroy$.next();
  }

  getNext(){
    let minId = Math.min(...this.contactList.map(x=>x.employeeEmergencyContactId));
    if(!minId || minId>0) minId = 0;
    minId = (minId-1);
    var newName = "Noname " + (-minId);
    while(this.contactList.find(x=>x.firstName == newName) )
        newName = "Noname " + newName;
    return {id:minId,name:newName};
  }

  addContact(){
      this.clearDrawer();
      var next = this.getNext();

      let newItem = <IEmergencyContact>{
        employeeEmergencyContactId : next.id,  
        employeeId: this.employeeId,
        addressLine1: '',
        addressLine2: '',
        city: '',
        postalCode: '',
        countryId: 1,
        countryName: 'US',
        stateId: 1,
        stateName: 'Michigan',
        homePhoneNumber: '',
        workPhoneNumber: '',
        cellPhoneNumber: '',
        relation: '',
        emailAddress: '',
        insertApproved: 1,
        firstName: '',//next.name,
        middleInitial: '',
        lastName: '',
        isDirty:true,
        clientId: this.userinfo.lastClientId || this.userinfo.clientId,
      };
      this.contactList.push(newItem);
      this.toggleActiveContact(next.id);
      this.isAdding = true;
  }

  makeHovered(tl) {
    if (this.contactKlicked) {
      this.contactKlicked.hovered = false;
      this.contactKlicked = null;
    }
    tl.hovered = true;
  }
  cogClicked(tl:IEmergencyContact) {
      this.contactKlicked = tl;
      this.contactKlicked.hovered = false;
      setTimeout(() => {
        tl.hovered = true;
      }, 100);
  }
  deleteContactDialog(tl:IEmergencyContact){
      const options = {
          title: `Are you sure you want to delete ${tl.firstName} contact?`,
          message: "",
          confirm: "Delete",
        };

      this.confirmDialog.open(options);
      this.confirmDialog.confirmed()
      .pipe(
      switchMap((confirmed) => {
          if (confirmed) {
              if(tl.employeeEmergencyContactId < 0 ) 
              return of(true);
              else {
                this.msg.loading(true, 'Deleting...');
                return this.contactApi.deleteActiveContact(tl);
              }
          } else {
          return EMPTY;
          }
      })
      )
      .subscribe(done => {
          if(done) {
              // remove from the list
              var inx = this.contactList.findIndex(
                  (x) => x.employeeEmergencyContactId == tl.employeeEmergencyContactId
              );
              if(inx>-1)this.contactList.splice(inx, 1);

              this.msg.setSuccessMessage('Contact deleted successfully.');
              if(this.contactList.length){
                  this.clearDrawer();
              }
          }}, (error: HttpErrorResponse) => {
              this.msg.setErrorResponse(error);
          });
  }

  toggleActiveContact(listId: number) {
      this.isAdding = false;
      this.changeTrackerService.clearIsDirty();
      if (this.activeId == listId) {
        this.clearDrawer();
      } else {
        this.activeId = listId;
        this.activeContact = this.contactList.find(x => {
          return (x.employeeEmergencyContactId == this.activeId);
        });
        this.contactApi.setActiveContact(this.activeContact);
      }
  }

  clearDrawer() {
    let addNewIndex = this.contactList.findIndex(x=>x.employeeEmergencyContactId<0);
    if (addNewIndex > -1) {
      this.contactList.splice(addNewIndex, 1);
      this.isAdding = false;
      this.changeTrackerService.clearIsDirty();
    }

      this.activeId = 0;
      this.activeContact = null;
  }

  refresh(contactId: number){
      if(contactId<0)
          this.clearDrawer();
      else
          this.toggleActiveContact(contactId);
  }


  formatPhone() {
    let phone = '616-566-5468'
  }

  switchEmployee () {
    this.isLoading = true;
  }
}

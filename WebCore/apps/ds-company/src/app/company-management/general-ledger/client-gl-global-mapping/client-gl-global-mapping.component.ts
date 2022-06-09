import { Component, OnInit, ViewChild, QueryList, ViewChildren } from '@angular/core';
import { UserInfo } from '@ds/core/shared';
import { forkJoin } from 'rxjs';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { HttpErrorResponse } from '@angular/common/http';
import { AccountService } from '@ds/core/account.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { GeneralLedgerService } from '../../../services/general-ledger.service';
import { ClientGLClassGroup, ClientGLMappingItem, GeneralLedgerAccount, MappingFilterOptions } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-client-gl-global-mapping',
  templateUrl: './client-gl-global-mapping.component.html',
  styleUrls: ['./client-gl-global-mapping.component.scss']
})
export class ClientGlGlobalMappingComponent implements OnInit {

  subFields = [
    16, // Expense Earnings
    21, // Expense SUTA
    37, // Expense Memo
    34, // Expense Match
    12, // Liability Local Tax
    11, // Liability State Tax
    13, // Liability SUTA tax
    15, // Liability Deductions
    28, // Payment Local Tax
    27, // Payment State Tax
    29, // Payment SUTA
    31, // Payment Client Vendor
    32  // Payment Dominion Vendor
  ];
  displayedColumns: string[] = ['Code','Description','Account'];
  canBeAccrued: boolean = true;
  canBeOffset: boolean = true;
  canBeDetail: boolean = true;
  isLoading: boolean = true;
  hasSubTransactions: boolean = false;
  hasTableData: boolean = false;
  isLoadingTable: boolean = false;
  isSaving: boolean = false;
  selectedDefaultAccountId: number = null;
  user: UserInfo;
  filterOptions: MappingFilterOptions;
  glAccounts: GeneralLedgerAccount[];
  glMappingItems: ClientGLMappingItem[];
  glClassGroups: ClientGLClassGroup[];
  matList: any;
  paginator: MatPaginator;
  inputString: string = "";
  isTimeOutStarted = false;
  @ViewChild('formControl', {static: false}) formControl;
  @ViewChild(MatPaginator, {static: false}) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.setDataSourceAttr();
  }
  @ViewChildren('accountSelect') accountSelectList : QueryList<CdkVirtualScrollViewport>;

  constructor(
    private api: GeneralLedgerService,
    private accountService: AccountService,
    private msg: NgxMessageService

  ) { }
  ngOnInit() {
    this.accountService.getUserInfo().subscribe((u: UserInfo) => {
      this.user = u;
      forkJoin(
        this.api.getMappingFilterOptions(),
        this.api.getClientGeneralLedgerAccounts(),
        this.api.getClientGeneralLedgerClassGroups(),
      ).subscribe(([filterOptions, accounts, classGroups]) => {
        this.filterOptions  = filterOptions,
        this.glAccounts     = accounts;
        this.glClassGroups  = classGroups;
        this.filterOptions.selectedClass             = 1;
        this.autoPickCategory();
        this.changeCategory();
        this.filterOptions.selectedGeneralLedgerType = (this.filterOptions.GLTypesFromCategory)
          ? this.filterOptions.GLTypesFromCategory[0].generalLedgerTypeId : null;

        this.searchForSubTransactions();
        this.isLoading = false;
      });
    });
  } // end of onInit()

  autoPickCategory() {
    this.filterOptions.selectedCategory =
        (this.filterOptions.cashGLTypes && this.filterOptions.cashGLTypes.length) ? 1
      : (this.filterOptions.liabilityGLTypes && this.filterOptions.liabilityGLTypes.length) ? 2
      : (this.filterOptions.expenseGLTypes && this.filterOptions.expenseGLTypes.length) ? 3
      : (this.filterOptions.paymentGLTypes && this.filterOptions.paymentGLTypes.length) ? 4 : null;
  }

  changeCategory() {
    switch (+this.filterOptions.selectedCategory) {
      case 1:
        this.filterOptions.GLTypesFromCategory = this.filterOptions.cashGLTypes;
        break;
      case 2:
        this.filterOptions.GLTypesFromCategory = this.filterOptions.liabilityGLTypes;
        break;
      case 3:
        this.filterOptions.GLTypesFromCategory = this.filterOptions.expenseGLTypes;
        break;
      case 4:
        this.filterOptions.GLTypesFromCategory = this.filterOptions.paymentGLTypes;
        break;
      default:
        this.filterOptions.GLTypesFromCategory = null;
        break;
    } // end of switch
    this.filterOptions.selectedGeneralLedgerType = this.filterOptions.GLTypesFromCategory[0].generalLedgerTypeId;
    this.searchForSubTransactions();
  } // end of category changed event

  searchForSubTransactions() {

    if (this.subFields.find((id) => id == this.filterOptions.selectedGeneralLedgerType)) {
      this.filterOptions.subPayrollTransactionsFromType = this.filterOptions.subPayrollTransactions.filter((pt) => (pt.generalLedgerTypeId == this.filterOptions.selectedGeneralLedgerType));
      this.filterOptions.selectedSubPayrollTransaction = (this.filterOptions.subPayrollTransactionsFromType && this.filterOptions.subPayrollTransactionsFromType.length)
                                                          ? this.filterOptions.subPayrollTransactionsFromType[0].foreignKeyId
                                                          : null;
      this.hasSubTransactions = true;
    } else {
      this.filterOptions.subPayrollTransactionsFromType = null;
      this.filterOptions.selectedSubPayrollTransaction = null;
      this.hasSubTransactions = false;
    }
  } // end of search for sub transactions

  getFilteredMappingItems() {
    if (this.filterOptions.selectedGeneralLedgerType != null && this.filterOptions.selectedGeneralLedgerType.toString() != "null") {
      this.isLoadingTable = true;
      this.hasTableData = true;

      const gl = this.filterOptions.GLTypesFromCategory.filter((gl) => gl.generalLedgerTypeId == this.filterOptions.selectedGeneralLedgerType)[0];

      if (gl != null) {
        this.canBeAccrued = gl.canBeAccrued;
        this.canBeOffset  = gl.canBeOffset;
        this.canBeDetail  = gl.canBeDetail;
      }

      this.api.getMappingItems(
        +this.filterOptions.selectedClass,
         this.filterOptions.selectedGeneralLedgerType,
        (this.filterOptions.selectedSubPayrollTransaction ? this.filterOptions.selectedSubPayrollTransaction : 0)
      ).subscribe((items) => {
        this.glMappingItems = items;
        this.createDisplayColumns();
        this.matList = new MatTableDataSource<ClientGLMappingItem>(items);
        //this.matList.paginator = this.paginator;
        this.isLoadingTable = false;
      });
    } else {
      this.hasTableData = false;
    } // end of if selected gl type
  } // end of get mapping items based on filtered data

  createDisplayColumns() {
    this.displayedColumns = ['Code','Description','Account'];
    // 'Class Group','Project','Accrued','Detail','Offset','Sequence Number'
    if (this.filterOptions.includeClassGroups) this.displayedColumns.push('Class Group');
    if (this.filterOptions.includeProject) this.displayedColumns.push('Project');
    if (this.filterOptions.includeAccrual && this.canBeAccrued) this.displayedColumns.push('Accrued');
    if (this.filterOptions.includeDetail && this.canBeDetail) this.displayedColumns.push('Detail');
    if (this.filterOptions.includeOffset && this.canBeOffset) this.displayedColumns.push('Offset');
    if (this.filterOptions.includeSequence) this.displayedColumns.push('Sequence Number');
  }

  checkForColumn(name: string) {
    return (this.displayedColumns.find((col) => col == name) ? true : false );
  }

  setDataSourceAttr() {
    if (this.matList) this.matList.paginator = this.paginator;
  }

  openChange($event: boolean, id: number, idx: number) {
    if ($event) {
      const index = this.glAccounts.findIndex((element : GeneralLedgerAccount) => { return element.accountId == id});

      if (this.accountSelectList && this.accountSelectList.length)
        this.accountSelectList.toArray()[idx].scrollToIndex(index);

    } // open event
  } // end of open Change function

  typeToObject($event, id: number, idx: number) {
    let input = String.fromCharCode($event.keyCode);
    if (/[a-zA-Z0-9-_ ]/.test(input)) {
      this.inputString = this.inputString + input;
      let index = this.glAccounts.findIndex((element : GeneralLedgerAccount) => {
        return element.description.toLowerCase().startsWith(this.inputString.toLowerCase());
      });
      if (index > -1) {
        index = index + 1;
        if (this.accountSelectList && this.accountSelectList.length)
          this.accountSelectList.toArray()[idx].scrollToIndex(index);
      }
    }

    if (!this.isTimeOutStarted) {
      this.isTimeOutStarted = true;
      setTimeout(() => { this.inputString = ""; this.isTimeOutStarted = false; }, 3000);
    }
  }

  getDescription(id: number) {
    const acc = this.glAccounts.find((element : GeneralLedgerAccount) => { return element.accountId == id});

    return acc != null ? acc.number + " (" + acc.description + ")"  : "";
  }

  resetAccount() {
    this.matList.data.forEach((item: ClientGLMappingItem) => {
      item.clientGeneralLedgerId = null;
    });
  }

  saveMappingItems() {
    this.api.saveClientGLMappingItems(this.matList.data, this.selectedDefaultAccountId).subscribe((results) => {
      this.msg.setSuccessMessage("General Journal Mapping saved successfully.");
      this.formControl.resetForm();
      this.matList.data = results;
      this.isSaving = false;
    }, (error : HttpErrorResponse) => {
      this.msg.setErrorMessage(error.message);
      this.isSaving = false;
  });
  }
} // end of class

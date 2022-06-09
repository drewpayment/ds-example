import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AccountService } from '@ds/core/account.service';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';
import { UserInfo } from '@ds/core/shared';
import { INPUT_DIRECTIVE_SUCCESS_MSG } from '@util/ds-common';
import { forEach } from 'angular';
import * as moment from 'moment';
import { BehaviorSubject, EMPTY, from, observable, Observable, of, throwError } from 'rxjs';
import { catchError, tap, map, switchMap } from 'rxjs/operators';
import { IClientW2ProcessingNotes } from '../../../models/w2/client-w2-processing-notes';
import { IW2Client } from '../../../models/w2/w2-client';
import { W2Service } from '../../../services/w2.service';
import { NotesDialogComponent } from './notes-dialog/notes-dialog.component';

@Component({
  selector: 'ds-w2-processing',
  templateUrl: './w2-processing.component.html',
  styleUrls: ['./w2-processing.component.css']
})

export class W2ProcessingComponent implements OnInit {

  constructor(private fb: FormBuilder, private w2Service: W2Service, private ngxMsg: NgxMessageService, private dialog: MatDialog, private accountService: AccountService) {
    this.createForm();
  }

  yearsList = this.GetListOfAvailableYears(6);

  types = [
    { id: 1, name: "All Active Clients" },
    { id: 2, name: "Delivery" },
    { id: 3, name: "Paperless" },
    { id: 4, name: "Dominion Prints" },
    { id: 5, name: "Terminated" },
    { id: 6, name: "1099s" },
    { id: 7, name: "Did not run last pay of year" },
    { id: 8, name: "Client Prints" }
  ];

  get year() { return this.form.controls.year as FormControl; }
  get type() { return this.form.controls.type as FormControl; }
  filteredType: string;
  clients: IW2Client[] = [] as IW2Client[];
  private _clients$ = new BehaviorSubject<IW2Client[]>(this.clients);
  clientsObservable$ = this._clients$.asObservable();
  displayedColumnsW2: string[] = ['clientCode', 'clientName', 'quantity', 'billed', 'toBill', 'employeesLastUpdatedOn', 'approvedForClient', 'create', 'creationDate', 'w2sReady', 'createManifest', 'deliveryDate', 'hasNotes', 'edit'];
  displayedColumns1099: string[] = ['clientCode', 'clientName', 'create', 'date1099'];
  displayedColumns = this.displayedColumnsW2;
  datasource = new MatTableDataSource<IW2Client>([]);
  @ViewChild('clientsPaginator', {static: false}) set paginator(pager: MatPaginator) {
    if (pager) this.datasource.paginator = pager;
  }
  @ViewChild(MatSort, { static: false }) set sort(sorter: MatSort) {
    if (sorter) this.datasource.sort = sorter;
  }
  pageSize: number = 10;
  pageIndex: number = 0;
  clientsToProcessLoaded = true;
  form: FormGroup = this.createForm();
  clientsLoaded: boolean = false;
  filterClicked: boolean = false;
  quantitySum: number = 0;
  billedSum: number = 0;
  toBillSum: number = 0;
  userInfo: UserInfo;

  private createForm() {
    return this.fb.group({
      year: this.fb.control("", [Validators.required]),
      type: this.fb.control("", [Validators.required])
    });
  }
  ngOnInit(): void {
    this.accountService.getUserInfo().subscribe(data => {
      this.userInfo = data;
    });

    this.year.patchValue(this.yearsList[0]);
    this.type.patchValue(this.types[0].id);


    this.datasource.filterPredicate = (data: IW2Client, filter: string) => {
      return data.clientCode.toLowerCase().indexOf(filter) !== -1
        || data.clientName.toLowerCase().indexOf(filter) !== -1;
     };
  }
  private GetListOfAvailableYears(numberOfYears) {
    var year = new Date().getFullYear();
    var yearList = [];

    for (let index = 0; index < numberOfYears; index++) {
      var newYear = (year - index).toString();
      yearList.push(newYear);
    }

    return yearList;
  }

  filterPressed() {
    this.clientsLoaded = false;
    this.filterClicked = true;
    this.getClientsToProcess();
    this.displayedColumns = this.type.value == 6 ? this.displayedColumns1099 : this.displayedColumnsW2;
    this.filteredType = this.type.value;

  }

  submit() {
    this.ngxMsg.loading(true, 'Submitting ...');

    if (this.type.value == 6) {
      this.ngxMsg.setWarningMessage("Creating 1099s...", 100000)
      this.w2Service.create1099s(this.year.value, this.clients)
      .pipe(
        catchError((error) => {
          this.ngxMsg.setErrorMessage('Sorry, this operation failed: \'Create 1099s\'', 60000);
          return throwError(error);
        }),
        tap((x) => {
          this.update1099Dates();
          this.datasource.data = this.clients;
          this.ngxMsg.setSuccessMessage("Successfully created 1099s.")
        })
      ).subscribe();
    }
    else {
      this.w2Service.submit(this.year.value, this.clients)
      .pipe(
        catchError((error) => {
          this.ngxMsg.setErrorMessage('Sorry, this operation failed: \'Submit\'', 60000);
          return throwError(error);
        }),
        switchMap((x) => {
          let createManifest = this.clients.some(x => x.createManifest);
          this.clients = x.clientList;
          this.datasource.data = this.clients;
          return createManifest ? this.w2Service.createManifest(this.year.value, x.uniqueId) : (of({}));
        })
      ).subscribe(() => {
        this.ngxMsg.setSuccessMessage("Successfully submitted W-2 Processing information for the clients selected.");
      });
    }
  }

  create() {
    let clientIds = this.clients.filter(x => x.create == true).map(x => x.clientId);

    if(clientIds.length == 0){
      this.ngxMsg.setSuccessMessage('No Clients Selected');
      return;
    }

    this.ngxMsg.setSuccessMessage('Creating Reports ...', 99999999);

    this.w2Service.createW2Reports(this.year.value, clientIds)
    .pipe(
      catchError((error) => {
        this.ngxMsg.setErrorMessage('Sorry, this operation failed: \'Create W-2 Reports\'', 60000);
        return throwError(error);
      }),
      tap((x) => {
        this.clients.forEach(function(client){
          x.forEach(function(resultClient){
            if(client.clientId == resultClient.clientId){
              client.creationDate = resultClient.creationDate
            }
          })
        });   
        this.clearCreateCheckboxes();
        this.datasource.data = this.clients;
      })
    ).subscribe(() => {
      this.ngxMsg.setSuccessMessage('Successfully created W-2 Reports.');
    });
  }

  private clearCreateCheckboxes() {
    for (let index = 0; index < this.clients.length; index++) {
      this.clients[index].create = null;
    }
  }

  private getClientsToProcess() {
    this.w2Service.getClientsToProcess(+this.year.value, +this.type.value).pipe(
      catchError((error) => {
        this.ngxMsg.setErrorMessage('Sorry, this operation failed: \'Get Clients To Process\'', 60000);
        return throwError(error);
      }),
      tap(x => {

        this.clients = x;

        this.datasource.data = this.clients;

        this.clientsLoaded = true;
        this.getW2Statistics();
      })
    ).subscribe();
  }

  lastSummarizedClicked(w2Client: IW2Client) {
    this.w2Service.summarize(+this.year.value, w2Client.clientId).pipe(
      catchError((error) => {
        this.ngxMsg.setErrorMessage('Sorry, this operation failed: \'Summarize\'', 60000);
        return throwError(error);
      }),
      tap(x => {
        this.ngxMsg.setSuccessMessage(`Sent Client: ${w2Client.clientCode} to be Summarized`);
      })
    ).subscribe(); 
  }

  createManualInvoice(w2Client: IW2Client){
    this.ngxMsg.setSuccessMessage('Creating Manual Invoice ...', 99999999);
    this.w2Service.createW2ProcessingManualInvoice(this.year.value, w2Client.clientId, w2Client.toBill).pipe(
      catchError((error) => {
        this.ngxMsg.setErrorMessage('Sorry, this operation failed: \'Create Manual Invoice\'. Please ensure you have the SystemAdmin role.', 60000);
        return throwError(error);
      }),
      tap(x => {
        this.ngxMsg.setSuccessMessage(`The invoice should appear on the screen.`)
      })
    ).subscribe();
  }


  onApprovedForClientCheckChange(clientCode: string, checked:boolean){
    this.clients.forEach(function(client){
      if (client.clientCode == clientCode){
        client.approvedForClient = checked;
      }
    });    
  }

  onW2sReadyCheckChange(clientCode: string, checked:boolean){
    this.clients.forEach(function(client){
      if (client.clientCode == clientCode){
        client.w2sReady = checked;
      }
    });
  }

  onCreateManifestCheckChange(clientCode: string, checked:boolean){
    this.clients.forEach(function(client){
      if (client.clientCode == clientCode){
        client.createManifest = checked;
      }
    });
  }

  onCreateCheckChange(clientCode: string, checked:boolean){
    this.clients.forEach(function(client){
      if (client.clientCode == clientCode){
        client.create = checked;
      }
    });
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.toLowerCase().trim();
    this.datasource.filter = filterValue;
  }

  getW2Statistics(){
    if(this.clients.length === 0){
      return;
    }

    this.billedSum = this.clients.map(item => item.billed).reduce((prev, next) => prev + next);
    this.toBillSum = this.clients.map(item => item.toBill).reduce((prev, next) => prev + next);
    this.quantitySum = this.clients.map(item => item.quantity).reduce((prev, next) => prev + next);
  }

  billWithNextPayroll(client: IW2Client){
    this.ngxMsg.setSuccessMessage('Sending ...', 99999999);
    this.w2Service.createW2ProcessingOneTimeBilling(+this.year.value, client.clientId, client.toBill)
    .pipe(
      catchError((error) => {
        this.ngxMsg.setErrorMessage('Sorry, this operation failed: \'Bill with Next Payroll\'', 60000);
        return throwError(error);
      }),
      tap(() => {
        this.ngxMsg.setSuccessMessage("Successfully created W-2 Processing one-time billing item.");
      })
    ).subscribe();
  }

  openNotesDialog(client: IW2Client) {
    const dialogRef = this.dialog.open(NotesDialogComponent, {
      width: "600px",
      data: {
        year: +this.year.value,
        clientInfo: client
      },
    });

    dialogRef.afterClosed().subscribe((result: IClientW2ProcessingNotes) => {
      if (result != null) {

        for (let index = 0; index < this.clients.length; index++) {
          const element = this.clients[index];

          if(element.clientId == result.clientId){
            this.clients[index].annualNotes = result.annualNotes;
            this.clients[index].miscNotes = result.miscNotes;
            this.clients[index].hasNotes = !this.isNullOrWhitespace(result.miscNotes) || !this.isNullOrWhitespace(result.annualNotes);
          } 
        }
      }
    });
  }

  updateW2CreationDates() {
    for (let index = 0; index < this.clients.length; index++) {
      if (this.clients[index].create) {
        this.clients[index].creationDate = moment().toDate();
        this.clients[index].create = null;
      }
    }
  }

  update1099Dates() {
    for (let index = 0; index < this.clients.length; index++) {
      if (this.clients[index].create) {
        this.clients[index].date1099 = moment().toDate();
        this.clients[index].create = null;
      }
    }
  }

  isNullOrWhitespace(str){
    return str === null || str.match(/^ *$/) !== null;
  }
}

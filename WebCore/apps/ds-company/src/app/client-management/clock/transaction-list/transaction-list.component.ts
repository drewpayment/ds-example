import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { tap } from 'rxjs/operators';
import { MachineService } from '../../../services/machine.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ITransaction } from '@models';

@Component({
  selector: 'ds-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.scss']
})
export class TransactionListComponent implements OnInit {

  @Input() filter: string;
  columns: string[] = ['clockName', 'devSn', 'ipAddress', 'pin', 'attTime', 'status', 'verify', 'workCode', 'reserved1', 'reserved2']
  transactionsLoaded: boolean = false;
  transactions: ITransaction[];
  filteredTransactions: ITransaction[];
  transactionsDataSource = new MatTableDataSource<ITransaction>([]);
  //@ViewChild('transactionsPaginator', { static: false }) transactionsPaginator: MatPaginator;
  @ViewChild('transactionsPaginator', {static: false}) set paginator(pager:MatPaginator) {
    if (pager) this.transactionsDataSource.paginator = pager;
  }
  @ViewChild(MatSort, { static: false }) set sort(sorter: MatSort) {
    if (sorter) this.transactionsDataSource.sort = sorter;
  }
  clientId?: number;
  constructor(private accountService: AccountService, private machineService: MachineService) { }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe(user => {
      this.clientId = user.lastClientId;
      this.machineService.getTransactionsByClientId(user.lastClientId).pipe(
        tap(x => {
          this.transactions = x;
          this.filterMachines(this.filter);
          this.transactionsLoaded = true;
        })
      ).subscribe();

      this.transactionsDataSource.sortingDataAccessor = (data, attribute) => {
        return data[attribute]
      };
    });
  }

  ngOnChanges() {
    if (this.transactionsLoaded) {
      this.filterMachines(this.filter);
    }
  }

  filterMachines(filterText: string) {
    if (this.transactions && this.transactions.length > 0) {
      if (filterText) {
        var filter = filterText.trim().toLowerCase();

        this.filteredTransactions = this.transactions.filter(
          transaction => (transaction.clockName && transaction.clockName.trim().toLowerCase().includes(filter)) ||
                         (transaction.devSn && transaction.devSn.trim().toLowerCase().includes(filter)) ||
                         (transaction.ipAddress && transaction.ipAddress.trim().toLowerCase().includes(filter))
        );
      } else {
        this.filteredTransactions = this.transactions;
      }

      this.filteredTransactions.push();
      this.transactionsDataSource.data = this.filteredTransactions;
    }
  }

}

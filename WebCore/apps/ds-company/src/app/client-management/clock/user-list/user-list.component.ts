import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { tap } from 'rxjs/operators';
import { MachineService } from '../../../services/machine.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { IUserInfo } from '@models';


@Component({
  selector: 'ds-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {

  @Input() filter: string;
  columns: string[] = ['clockName', 'devSn', 'ipAddress', 'pin', 'username', 'password', 'idCard', 'group', 'timezone', 'pri'];
  usersLoaded: boolean = false;
  users: IUserInfo[];
  filteredUsers: IUserInfo[];
  usersDatasource = new MatTableDataSource<IUserInfo>([]);
  @ViewChild('usersPaginator', {static: false}) set paginator(pager:MatPaginator) {
    if (pager) this.usersDatasource.paginator = pager;
  }
  @ViewChild(MatSort, { static: false }) set sort(sorter: MatSort) {
    if (sorter){
        this.usersDatasource.sort = sorter
    };
  }

  clientId?: number;
  constructor(private accountService: AccountService, private machineService: MachineService) { }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe(user => {
      this.clientId = user.lastClientId;
      this.machineService.getMachineUsersByClientId(user.lastClientId).pipe(
          // catchError((error) => {
          //     this.msg.setTemporaryMessage('Sorry, this operation failed: \'Get Devices By Client Id\'', MessageTypes.error, 60000);
          //     return throwError(error);
          // }),
          tap(x => {
              this.users = x;
              this.filterMachines(this.filter);
              this.usersLoaded = true;
          })
      ).subscribe();

      this.usersDatasource.sortingDataAccessor = (data, attribute) => {
        return data[attribute]
      };
  });
  }

  ngOnChanges() {
    if (this.usersLoaded) {
      this.filterMachines(this.filter);
    }
  }

  filterMachines(filterText: string) {
    if (this.users && this.users.length > 0) {
      if (filterText){
        var filter = filterText.trim().toLowerCase();

        this.filteredUsers = this.users.filter(
          user => (user.clockName && user.clockName.trim().toLowerCase().includes(filter)) ||
                  (user.devSn && user.devSn.trim().toLowerCase().includes(filter)) ||
                  (user.ipAddress && user.ipAddress.trim().toLowerCase().includes(filter))
        );
      } else {
        this.filteredUsers = this.users;
      }

      this.filteredUsers.push();
      this.usersDatasource.data = this.filteredUsers;
    }
  }

}

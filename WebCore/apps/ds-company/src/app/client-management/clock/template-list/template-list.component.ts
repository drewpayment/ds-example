import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { tap } from 'rxjs/operators';
import { MachineService } from '../../../services/machine.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ITemplate } from '@models';

@Component({
  selector: 'ds-template-list',
  templateUrl: './template-list.component.html',
  styleUrls: ['./template-list.component.scss']
})
export class TemplateListComponent implements OnInit {

  @Input() filter: string;
  columns: string[] = ['clockName', 'devSn', 'ipAddress', 'pin', 'fid', 'size', 'valid', 'tmpStr']
  templatesLoaded: boolean = false;
  templates: ITemplate[];
  filteredTemplates: ITemplate[];
  templatesDataSource = new MatTableDataSource<ITemplate>([]);
  //@ViewChild('templatesPaginator', { static: false }) templatesPaginator: MatPaginator;
  @ViewChild('templatesPaginator', {static: false}) set paginator(pager:MatPaginator) {
    if (pager) this.templatesDataSource.paginator = pager;
  }
  @ViewChild(MatSort, { static: false }) set sort(sorter: MatSort) {
    if (sorter) this.templatesDataSource.sort = sorter;
  }
  clientId?: number;

  constructor(private accountService: AccountService, private machineService: MachineService) { }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe(user => {
      this.clientId = user.lastClientId;
      this.machineService.getTemplatesByClientId(user.lastClientId).pipe(
          // catchError((error) => {
          //     this.msg.setTemporaryMessage('Sorry, this operation failed: \'Get Devices By Client Id\'', MessageTypes.error, 60000);
          //     return throwError(error);
          // }),
          tap(x => {
              this.templates = x;
              this.filterMachines(this.filter);
              this.templatesLoaded = true;
          })
      ).subscribe();

      this.templatesDataSource.sortingDataAccessor = (data, attribute) => data[attribute];
    });
  }

  ngOnChanges() {
    if (this.templatesLoaded) {
      this.filterMachines(this.filter);
    }
  }

  filterMachines(filterText: string) {
    if (this.templates && this.templates.length > 0) {
      if (filterText) {
        var filter = filterText.trim().toLowerCase();

        this.filteredTemplates = this.templates.filter(
          template => (template.clockName && template.clockName.trim().toLowerCase().includes(filter)) ||
                  (template.devSn && template.devSn.trim().toLowerCase().includes(filter)) ||
                  (template.ipAddress && template.ipAddress.trim().toLowerCase().includes(filter))
        );
      } else {
        this.filteredTemplates = this.templates;
      }

      this.filteredTemplates.push();
      this.templatesDataSource.data = this.filteredTemplates;
    }
  }

}

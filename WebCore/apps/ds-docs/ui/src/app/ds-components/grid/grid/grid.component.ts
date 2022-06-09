import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

export interface PeriodicElement {
  select: string;
  avatar: string;
  name: string;
  empno: string;
  award: string;
  earning: string;
  type: string;
  amount: string;
  effective: string;
  status: string;
  payout: string;
  rec: string;
  sup: string;
  jobTitle: string;
  action: string;
}

const ELEMENT_DATA: PeriodicElement[] = [
  {select: '', avatar: '', name: 'test a long nam a check stub to',  empno: '1234567945', award: '', earning: '', type: '', amount: '', effective: '12/24/2019', status: '', payout: '$27,300 to $34,080', rec: 'Score: 5.0 | 12.2%', sup: 'Brooks Von ',                     jobTitle: 'Crawler',               action: 'more_vert'},
  {select: '', avatar: '', name: 'Henry Abbott',                     empno: '3116',       award: '', earning: '', type: '', amount: '', effective: '01/02/2020', status: '', payout: '$57,260 to $59,265', rec: 'Score: 3.5 | 2.2%',  sup: 'Penny Adams',                     jobTitle: 'Software Developer II', action: 'more_vert'},
  {select: '', avatar: '', name: 'Blaise Anderson',                  empno: '1234567964', award: '', earning: '', type: '', amount: '', effective: '',           status: '', payout: '',                   rec: '',                   sup: 'test a long nam a check stub to', jobTitle: '',                      action: 'more_vert'}
]

@Component({
  selector: 'ds-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss']
})
export class GridComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  datasource = new MatTableDataSource (ELEMENT_DATA);
  displayedColumns: string[] = ['select', 'avatar', 'name', 'empno', 'award', 'earning', 'type', 'amount', 'effective', 'status', 'payout', 'rec', 'sup', 'jobTitle', 'action'];
}


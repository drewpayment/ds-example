import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ClientAccrualsStoreService } from '../../services/client-accruals-store.service';

@Component({
    selector: 'ds-client-accruals-setup',
    templateUrl: './client-accruals-setup.component.html',
    styleUrls: ['./client-accruals-setup.component.scss'],
})
export class ClientAccrualsSetupComponent implements OnInit {

  constructor(
    private store: ClientAccrualsStoreService,
    private cd: ChangeDetectorRef,
  ) {
    // Set tab state
    this.store.isSetupTab = true;
  }

  ngOnInit() {
    this.cd.detectChanges();
  }

}

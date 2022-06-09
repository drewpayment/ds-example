import { coerceNumberProperty } from '@angular/cdk/coercion';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { map, startWith, takeUntil } from 'rxjs/operators';
import { ClientAccrualConstants } from '../../../models/leave-management/client-accrual-constants';
import { ClientAccrualsStoreService } from '../../../client-management/services/client-accruals-store.service';

@Component({
  selector: 'ds-client-accruals-schedules-tab',
  templateUrl: './schedules-tab.component.html',
  styleUrls: ['./schedules-tab.component.scss'],
})
export class ClientAccrualsSchedulesTabComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject();

  get clientAccrualIdForm() {
    return this.store.form.get('accrual.clientAccrualId');
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private store: ClientAccrualsStoreService,
    private cd: ChangeDetectorRef,
  ) {
    // Set tab state
    this.store.isSchedulesTab = true;
  }

  ngOnInit() {
    this.cd.detectChanges();

    this.clientAccrualIdForm.valueChanges
    .pipe(
      startWith(this.clientAccrualIdForm.value),
      map(selectedAccrualId => coerceNumberProperty(selectedAccrualId)),
      takeUntil(this.destroy$)
    ).subscribe(selectedAccrualId => {
      // Don't let them on this component if the selected accrual isn't saved yet.
      if (selectedAccrualId <= ClientAccrualConstants.NEW_ENTITY_ID) {
        this.router.navigate(['../', 'setup'], {relativeTo: this.route});
        // .then(_ => this.cd.detectChanges());
      }
    });
  }

  ngOnDestroy() {
    this.destroy$.next();
  }

}

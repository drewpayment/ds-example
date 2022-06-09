import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { EmployeeEEOCApiService } from '@ds/core/employees/shared/employee-eeoc-api.service';
import { IClientData, IClientRelationData } from '@ajs/onboarding/shared/models';
import { EmpEeocService } from './emp-eeoc.service';
import { Subscription, Observable, of } from 'rxjs';
import { tap, switchMap, catchError, map } from 'rxjs/operators';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

@Component({
    selector: 'ds-employee-eeoc-export',
    templateUrl: './employee-eeoc-export.component.html',
    styleUrls: ['./employee-eeoc-export.component.scss']
})
export class EmployeeEEOCExportComponent implements OnInit, OnDestroy {
    clients: IClientData[];
    w2Years: number[];
    clientIds: number[] = [];
    selectedW2Year = this._formBuilder.control('');
    subs: Subscription[] = [];
    isLoading$: Observable<any>;
    isLoading = true;
    clientRelation$: Observable<IClientRelationData>;
    disableYearList$: Observable<boolean>;;

    eeocFormGroup: FormGroup;

    constructor(
        private _formBuilder: FormBuilder,
        private eeocApiService: EmployeeEEOCApiService,
        private service: EmpEeocService,
        private msg: DsMsgService,
    ) { }

    ngOnInit() {
        this.subs.push(this.selectedW2Year.valueChanges.subscribe(val => this.service.updateW2Year(val)));
        this.eeocFormGroup = this.service.eeocFormGroup;
        this.initializeComponentDataByYear();
    }

    ngOnDestroy() {
        this.subs.forEach(s => s.unsubscribe());
    }

    initializeComponentDataByYear(): void {
        this.clientRelation$ = this.eeocApiService.getClientRelatedClientIds(true);

        this.disableYearList$ = this.service.disableYearList.pipe(
            map((disableBool) => {
                return disableBool;
            })
        );

        this.isLoading$ = this.clientRelation$.pipe(
            tap((relation) => {
                this.clients = relation.clients;
                this.service.updateClients(this.clients);
                this.clients.forEach((client) => this.clientIds.push(client.clientId));
            }),
            switchMap(_ => {
                return this.eeocApiService.getEeocW2YearList(this.clientIds).pipe(
                    catchError(() => {
                        this.isLoading = false;
                        return of([]);
                    }),
                );
            }),
            tap((w2Years) => {
                this.w2Years = [];
                w2Years.forEach(year => {
                    if (year >= 2013) this.w2Years.push(year);
                });
                this.selectedW2Year.setValue(this.w2Years[0]);
                this.isLoading = false;
            }, (err) => {
                this.msg.setTemporaryMessage(err, this.msg.messageTypes.error);
                this.isLoading = false;
            }),
            catchError(() => {
                this.isLoading = false;
                return of({});
            }),
        );
    }
}

import { Component, OnInit, Input } from '@angular/core';
import { BanksApiService, IBankBasicInfo } from '@ds/core/banks';
import { ClientBankInfoApiService } from '../shared/client-bank-info-api.service';
import { forkJoin } from 'rxjs';
import { FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

enum SortType {
    BankName,
    RoutingNumber
}

@Component({
    selector: 'ds-client-bank-relate-form',
    templateUrl: './client-bank-relate-form.component.html',
    styleUrls: ['./client-bank-relate-form.component.scss']
})
export class ClientBankRelateFormComponent implements OnInit {

    @Input()
    clientId: number;

    banks: IBankBasicInfo[];
    clientBanks: IBankBasicInfo[];
    unselectedBanks: IBankBasicInfo[];

    loading = true;

    sortOptions: {name:string, value:SortType }[] = [
        { name: "Bank Name", value: SortType.BankName },
        { name: "Routing Number", value: SortType.RoutingNumber }
    ];

    sortCtrl = new FormControl(SortType.BankName);
    form: FormGroup;

    constructor(
        private bankSvc: BanksApiService,
        private clientBankSvc: ClientBankInfoApiService,
        private msg: DsMsgService,
        private fb: FormBuilder) { }

    ngOnInit() {
        let banks$       = this.bankSvc.getBankBasicList();
        let clientBanks$ = this.clientBankSvc.getClientBanks(this.clientId);

        forkJoin([banks$, clientBanks$]).subscribe(data => {
            let [banks, clientBanks] = data;

            this.banks       = banks || [];
            this.clientBanks = clientBanks || [];

            this.updateBankLists();
            this.sort(SortType.BankName);
            this.loading = false;
        });

        this.sortCtrl.valueChanges.subscribe(next => this.sort(next));

        this.form = this.fb.group({
            selected: [[]],
            available: [[]]
        });
    }

    addBanks() {
        let { available } = this.form.value;

        this.clientBanks = [...this.clientBanks, ...available];
        this.updateBankLists();
        this.sort(this.sortCtrl.value);
        this.form.patchValue({ available: []});
    }

    removeBanks() {
        let { selected: toRemove } : { selected: IBankBasicInfo[] } = this.form.value;

        this.clientBanks = this.clientBanks.filter(b => !toRemove.some(r => r.bankId === b.bankId));
        this.updateBankLists();
        this.sort(this.sortCtrl.value);
        this.form.patchValue({ selected: []});
    }

    save() {
        this.msg.loading(true);
        this.loading = true;
        this.clientBankSvc.saveClientBanks(this.clientId, this.clientBanks).subscribe(data => {
            this.msg.setTemporarySuccessMessage("Banks saved successfully.");
            this.form.reset({available:[], selected:[]});
            this.loading = false;
        });
    }

    private sort(sortBy: SortType) {
        let sortFn: (b1: IBankBasicInfo, b2: IBankBasicInfo) => number = null;

        switch(sortBy) {
            case SortType.BankName: {
                sortFn = (b1, b2) => b1.name.toLocaleLowerCase() > b2.name.toLocaleLowerCase() ? 1 : -1;
                break;
            }
            case SortType.RoutingNumber: {
                sortFn = (b1, b2) => b1.routingNumber > b2.routingNumber ? 1 : -1;
            }
        }

        this.banks.sort(sortFn);
        this.unselectedBanks.sort(sortFn);
        this.clientBanks.sort(sortFn);
    }

    private updateBankLists() {
        this.unselectedBanks = this.banks.filter(b => this.clientBanks.findIndex(cb => cb.bankId === b.bankId) < 0);
    }
}

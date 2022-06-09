import { Component, OnInit } from '@angular/core';
import { UserInfo } from '@ds/core/shared';
import { Store } from '@ngrx/store';
import { State } from '@ds/core/app-config/ngrx-store/reducers/menu.reducer';
import { InitiateLogout } from '@ds/core/app-config/ngrx-store/actions/menu.actions';

@Component({
    selector: 'ess-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

    user:UserInfo;


    constructor(private store: Store<State>) { }

    ngOnInit() {
    }

    clearMenu() {
        this.store.dispatch(new InitiateLogout());
    }

}

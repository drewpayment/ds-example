import { Injectable, Inject } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { DOCUMENT } from '@angular/common';
import { AccountService } from '@ds/core/account.service';
import { map } from 'rxjs/operators';


@Injectable()
export class VoidChecksGuard implements CanActivate {

    constructor(@Inject(DOCUMENT) private document: Document, private accountService: AccountService) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
        boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        // Should match PayrollAdjustmentsVoidChecks{,New}.aspx
        return this.document.location.pathname.toLowerCase().includes('payrolladjustmentsvoidchecks');
    }

}

import { Injectable, Inject } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { DOCUMENT } from '@angular/common';
import { map, switchMap } from 'rxjs/operators';
import { ArService } from '../shared/ar.service';
import { coerceNumberProperty } from '@angular/cdk/coercion';


@Injectable()
export class ArDominionCheckPaymentGuard implements CanActivate {

    constructor(private arService: ArService, private router: Router) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
        boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
            const depositId = coerceNumberProperty(route.params['id']);
            return this.arService.getArDepositById(depositId).pipe(
                map(deposit => {
                    if(!deposit || deposit.postedDate){
                        this.router.navigate(["ar/deposits"]);
                        return null;
                    }

                    return true;
                })
            )
    }

}

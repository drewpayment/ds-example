import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from "@ds/core/account.service";
import { PerformanceReviewRatingPermissions } from './review-ratings-permissions';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ReviewRatingGuard implements CanActivate {
  constructor(private accountService:AccountService) {}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this.accountService
    .canPerformActions([PerformanceReviewRatingPermissions.ReadRatings, PerformanceReviewRatingPermissions.WriteRatings])
    .pipe(map(x => typeof(x) === "boolean" ? x : false));
  }
}

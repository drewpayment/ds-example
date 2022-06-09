import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AccountService } from "@ds/core/account.service";
import { Observable } from 'rxjs';
import { GoalTrackingPermissions } from './goal-tracking-permissions';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class GoalTrackingGuard implements CanActivate {

constructor(private accountService:AccountService) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
      return this.accountService
      .canPerformActions([GoalTrackingPermissions.ReadGoals, GoalTrackingPermissions.WriteGoals])
      .pipe(map(x => typeof(x) === "boolean" ? x : false));
  }
}

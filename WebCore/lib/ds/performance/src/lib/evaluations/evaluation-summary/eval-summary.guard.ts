import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate } from '@angular/router';
import { Observable } from 'rxjs';
import { ActiveEvaluationService } from '../shared/active-evaluation.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EvalSummaryGuard implements CanActivate {

constructor(private evalStore:ActiveEvaluationService) { }

  canActivate = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => 
  this.evalStore
  .canShowSummary
  .pipe(map(x => x.isFormValidAndComplete === true && x.isPayrollRequestEnabled === true))
  
}

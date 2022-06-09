
import { Injectable } from "@angular/core";
import {
  ActivatedRouteSnapshot,
  CanDeactivate,
  RouterStateSnapshot,
  UrlTree,
} from "@angular/router";
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import { Observable } from "rxjs";
import { DepartmentsComponent } from "../company-management/labor/departments/departments.component";
import { ChangeTrackerService } from "@ds/core/ui/forms/change-track/change-tracker.service";

@Injectable({
  providedIn: "root",
})
export class DirtyCheckGuard implements CanDeactivate<any> {

  constructor(
    private confirmService: ConfirmDialogService,
    private changeTrackerService: ChangeTrackerService
  ) { }
  canDeactivate(
    component: any,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
      
      if (this.changeTrackerService.isDirty())
        return this.confirmLeave();
      
      return true;
  }

  confirmLeave(): Observable<boolean> {
    let wantsToLeave = false;

    // const options = {
    //   title: "Leave Page?",
    //   message: "Changes you made may not be saved.",
    //   confirm: "Leave",
    // };

    return new Observable(ob => { 
      wantsToLeave = confirm("Changes you made may not be saved.");
      if (wantsToLeave)
        this.changeTrackerService.clearIsDirty();
      ob.next(wantsToLeave);
      ob.complete();

      // this.confirmService.open(options);
      // this.confirmService.confirmed().subscribe((result) => {
      //   wantsToLeave = result;

      //   if (wantsToLeave)
      //     this.changeTrackerService.clearIsDirty();

      //   ob.next(wantsToLeave);
      //   ob.complete();
      // });
    });
  }

}

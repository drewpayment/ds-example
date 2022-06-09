import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { of } from "rxjs";
import { mergeMap, tap } from "rxjs/operators";
import { MenuActionTypes } from "../actions/menu.actions";

@Injectable()
export class MenuEffects {
  // initialLogout$ = createEffect(() =>
  //   this.actions$.pipe(
  //     ofType(MenuActionTypes.InitiateLogout),
  //     mergeMap(() => {
  //       return of({ type: MenuActionTypes.ClearState });
  //     })
  //   )
  // );

  constructor(private actions$: Actions) {}
}

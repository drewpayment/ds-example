import { Component, ChangeDetectionStrategy, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AccountService } from '@ds/core/account.service';
import { Observable, iif, of, Subject, merge } from 'rxjs';
import { UserType } from '@ds/core/shared';
import { map, share, exhaustMap, tap, withLatestFrom, take } from 'rxjs/operators';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import * as moment  from 'moment';
import { UserTermsAndConditionsDto } from '@ds/core/shared/user-terms-and-conditions.model';
import { HttpClient } from '@angular/common/http';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-terms-and-conditions',
  templateUrl: './terms-and-conditions.component.html',
  styleUrls: ['./terms-and-conditions.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TermsAndConditionsComponent implements OnDestroy {

  userCanAddressContract$: Observable<boolean>;
  termsAndConditions$: Observable<string>;
  acceptTermsHook: Subject<any> = new Subject();
  declineTermsHook: Subject<any> = new Subject();
  onDestroyHook: Subject<any> = new Subject();

  handlers$: Observable<any>;


  constructor(
    private dialogRef: MatDialogRef<TermsAndConditionsComponent>,
    private accountService: AccountService,
    private msg: NgxMessageService,
    private http:HttpClient
    ) {
      this.userCanAddressContract$ = TermsAndConditionsComponent.userCanAddressContract(accountService);
      this.termsAndConditions$ = accountService.getLatestTermsAndConditionsVersion(msg).pipe(
      map(x => {
        var urlCreator = window.URL || (window as any).webkitURL;
        return urlCreator.createObjectURL(x);
      }),
      share());

      this.CreateStreamToDeAllocateBlob(this.termsAndConditions$, this.onDestroyHook).subscribe();
      const handlers = [];
      handlers.push(this.createSavehandler(this.acceptTermsHook, true, msg));
      handlers.push(this.createSavehandler(this.declineTermsHook, false, msg));

      this.handlers$ = merge(... handlers);
     }

  ngOnDestroy(): void {
    this.onDestroyHook.next();
  }

  private CreateStreamToDeAllocateBlob(getBlob$: Observable<string>, componentDestroyed$: Observable<any>): Observable<any> {
    return componentDestroyed$.pipe(withLatestFrom(getBlob$)).pipe(
      tap(x => {
      var urlCreator = window.URL || (window as any).webkitURL;
        return urlCreator.revokeObjectURL(x[1]);
    }),
    take(1))
  }

  private createSavehandler(source: Observable<any>, isAccepted: boolean, msg: NgxMessageService): Observable<UserTermsAndConditionsDto> {
    return source.pipe(
      tap(() => msg.loading(true)),
      exhaustMap(() => this.accountService.userProcessedTermsAndConditions(isAccepted, msg)),
      tap(() => this.dialogRef.close()));
  }

  static userCanAddressContract(acctSvc: AccountService): Observable<boolean> {
    return acctSvc.PassUserInfoToRequest(userInfo => {

      const termsAndConditions$ = acctSvc.getUserTermsAndConditions(userInfo.userId).pipe(
        map(terms =>
          {
            const userAcceptedTermsInLastSixMonths = terms && terms.acceptDate && terms.userAccepted && moment().subtract('month', 6).isBefore(convertToMoment(terms.acceptDate));

            return !userAcceptedTermsInLastSixMonths;
          }))

      return iif(
      () => userInfo.userTypeId !== UserType.companyAdmin,
      of(false),
      termsAndConditions$);

    }).pipe(
      share())
  }

  close(){
    this.dialogRef.close();
  }

  acceptTerms(): void {
    this.acceptTermsHook.next();
}

declineTerms(): void {
  this.declineTermsHook.next();
}

}

@Pipe({name: 'safe'})
export class safeString implements PipeTransform {

  constructor(private sanitizer: DomSanitizer) { }

  transform(value: string): SafeResourceUrl {
    const result = (!value ? '../Alerts/PayrollServiceAgreement.pdf' : value) + '#zoom=65';
    return this.sanitizer.bypassSecurityTrustResourceUrl(result);
  }
}

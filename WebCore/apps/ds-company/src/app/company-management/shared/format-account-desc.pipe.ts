import { Pipe, PipeTransform } from '@angular/core';
import { GeneralLedgerAccount } from './models/general-ledger-account.model';

@Pipe({
  name: 'formatAccountDesc',
  pure: false
})
export class formatAccountDescPipe implements PipeTransform {
  transform = accountDesc;
}

export function accountDesc(gla: GeneralLedgerAccount){
  var br = String.fromCharCode(10);
  if(gla.number){
      return gla.number + " &ndash; " + gla.description;
  }
  return gla.description;
}
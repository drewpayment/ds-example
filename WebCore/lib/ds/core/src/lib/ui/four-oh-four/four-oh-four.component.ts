import { Component } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';


@Component({
  selector: 'ds-four-oh-four',
  templateUrl: './four-oh-four.component.html'
})
export class FourOhFourComponent {
  constructor(private account: AccountService,) {}

  goHome() {
    this.account.getSiteConfig(ConfigUrlType.Payroll).subscribe(site => {
      const url = site && site.url ? site.url : '/';
      window.location.href = url;
    });
  }
}

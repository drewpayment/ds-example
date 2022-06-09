import { Component, OnInit } from '@angular/core';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';
import { AccountService } from '@ds/core/account.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'ds-permission-error',
  templateUrl: './permission-error.component.html'
})
export class PermissionErrorComponent implements OnInit {
  showHomeButton: boolean = true;
  showHelpMessage:boolean = true;
  customMessage:string='';

  constructor(    
    private accountService: AccountService,
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.route.queryParams.subscribe(param => {
      if(param["showButton"]){
          this.showHomeButton = param["showButton"].toLowerCase() == 'true';
      }
      if(param["showHelpMessage"]){
        this.showHelpMessage = param["showHelpMessage"].toLowerCase() == 'true';
      }
      if(param["message"]){
        this.customMessage = param["message"];
      }
    });
  }

  goHome() {
    this.accountService.getSiteUrls().subscribe((urls) => {
      const source = urls.find((u) => u.siteType === ConfigUrlType.Payroll);
      let sourceUrl = source && source.url ? source.url : "";
      sourceUrl =
        sourceUrl.charAt(sourceUrl.length - 1) === "/"
          ? sourceUrl
          : sourceUrl + "/";
      window.location.href = sourceUrl;
    })  
  }
}
import * as angular from "angular";
import { Component, OnInit } from '@angular/core';
import { ClientService } from '@ds/core/clients/shared';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from "@ds/core/shared/user-info.model";
import { ClientEssOptions } from 'apps/ds-company/ajs/models/client-ess-options.model';
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PreviewWelcomeMessageDialogComponent } from './preview-welome-message/preview-welcome-message-dialog.component';
import { tap, switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs/internal/Observable';
import { HttpErrorResponse } from '@angular/common/http';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: 'ds-manage-welcome-message',
  templateUrl: './manage-welcome-message.component.html',
  styleUrls: ['./manage-welcome-message.component.scss']
})
export class ManageWelcomeMessageComponent implements OnInit {

    defaultWelcomeMessageText: string;
    essOptions: ClientEssOptions;
    showInstruction: boolean = true;
    isLoading: boolean = true;

    private user: UserInfo;

    constructor(
      private accountService: AccountService,
      private clientService: ClientService,
      private msgSvc: NgxMessageService,
      private dialog: MatDialog 
      ) {}

    ngOnInit() {

      this.defaultWelcomeMessageText = "This is where we'll gather information to help get you started in your position. You'll be able to submit your " + 
            "onboarding for review when you complete all required steps. If you have questions please reach out to your HR " +
            "representative or direct supervisor.";
        
        this.accountService.getUserInfo()
            .pipe(tap(user => this.user=user ),
              switchMap(user => this.getEssOptions(this.user.clientId))
            ).subscribe(x =>{}, (error: HttpErrorResponse) => this.msgSvc.setErrorResponse(error.error) ); 
      this.isLoading = false;
    }

    getEssOptions(clientId: number):Observable<any> {
      
      this.essOptions = {
        allowCheck: false,
        allowDirectDeposit: false,
        allowImageUpload: false,
        allowPaycard: false,
        allowPaystubEmails: false,
        isCustomMessage: false,
        clientId: clientId,
        directDepositLimit: 0,
        doInsert: false,
        welcomeMessage: this.defaultWelcomeMessageText,
        finalDisclaimerMessage: null,
        finalDisclaimerAgreementText: null,
        manageDirectDeposit: false,
        manageDirectDepositAmountAndAccountInfo: false,
        directDepositDisclaimer: null
        };

      return this.clientService.getClientEssOptions(clientId).pipe(tap(essOptions => { 
          this.isLoading = false;
          if(essOptions){
              this.essOptions = essOptions;
              
              if( this.essOptions.isCustomMessage == null)
                  this.essOptions.isCustomMessage = false;
              if( !this.essOptions.welcomeMessage )
                  this.essOptions.welcomeMessage = this.defaultWelcomeMessageText;
              else
                  this.essOptions.welcomeMessage = essOptions.welcomeMessage.replace(/<br\/>/g, '\n');
          }
      }));

    }

    insertTextAt(textToInsert : string) {
      Utilities.insertAtCaret(textToInsert, angular.element('textarea[name="WelcomeMessage"]').get(0));
      var contTA=angular.element('textarea[name="WelcomeMessage"]').get(0);
      this.essOptions.welcomeMessage= (<HTMLInputElement>contTA).value;
    }

    save() {
      
      if (this.essOptions.isCustomMessage && !this.essOptions.welcomeMessage) return;
      let options = angular.copy(this.essOptions);
      if(!this.essOptions.isCustomMessage) options.welcomeMessage = this.defaultWelcomeMessageText;
      else options.welcomeMessage = options.welcomeMessage.replace(/\n/g, '<br/>');

      this.clientService.updateClientEssOptions(options).subscribe(() => {
          this.msgSvc.setSuccessMessage("Welcome message updated successfully.", 5000);
      }); //.catch(this.msgSvc.showWebApiException);
    }

    preview() {
      
      if (this.essOptions.isCustomMessage && !this.essOptions.welcomeMessage) return;

      let previewText = this.essOptions.welcomeMessage;
      if(!this.essOptions.isCustomMessage) previewText = this.defaultWelcomeMessageText;

      let config = new MatDialogConfig<any>();
              config.width = "500px";
              config.data = this.replaceWildCards(previewText).replace(/\n/g, '<br/>');
              
              return this.dialog.open<PreviewWelcomeMessageDialogComponent, any, string>(PreviewWelcomeMessageDialogComponent, config)
    }

    replaceWildCards(message:string){
        var currDate = new Date();
        var startDate = new Date();
        startDate.setDate(startDate.getDate() + 7);
        message = message.replace(/\{\*Date\}/g, currDate.toLocaleDateString());
        message = message.replace(/\{\*CompanyName\}/g, this.user.lastClientName );
        message = message.replace(/\{\*StartDate\}/g, startDate.toLocaleDateString());
        return message;
    }
    
}

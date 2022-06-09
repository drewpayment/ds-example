import * as angular from "angular";
import { Component, OnInit } from '@angular/core';
import { ClientService } from '@ds/core/clients/shared';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from "@ds/core/shared/user-info.model";
import { ClientEssOptions } from 'apps/ds-company/ajs/models/client-ess-options.model';
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PreviewFinalDisclaimerDialogComponent } from './preview-final-disclaimer/preview-final-disclaimer-dialog.component';
import { tap, switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs/internal/Observable';
import { HttpErrorResponse } from '@angular/common/http';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: 'ds-manage-final-disclaimer',
  templateUrl: './manage-final-disclaimer.component.html',
  styleUrls: ['./manage-final-disclaimer.component.scss']
})
export class ManageFinalDisclaimerComponent implements OnInit {

    defaultFinalDisclaimerMessage: string;
    defaultFinalDisclaimerAgreementText: string;
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

        this.defaultFinalDisclaimerMessage = "Please review the documents and forms for accuracy. Once completed, sign this form to finalize onboarding.";

        this.defaultFinalDisclaimerAgreementText = "Under penalties of perjury, I declare that I have answered all previous questions " + 
            "to the best of my knowledge and belief, and they are true, correct and complete.";
        
        this.accountService.getUserInfo()
            .pipe(tap(user => this.user=user ),
              switchMap(user => this.getEssOptions(this.user.clientId))
            ).subscribe(x =>{}, (error: HttpErrorResponse) => this.msgSvc.setErrorResponse(error.error) ); 
        
        //this.isLoading = false;
    }

    getEssOptions(clientId: number):Observable<any> {
        
      return this.clientService.getClientEssOptions(clientId).pipe(tap(essOptions => { 
          this.isLoading = false;
          if(essOptions){
              this.essOptions = essOptions;
              
              if( this.essOptions.isCustomMessage == null)
                  this.essOptions.isCustomMessage = false;

              if( !this.essOptions.finalDisclaimerMessage)
                  this.essOptions.finalDisclaimerMessage = this.defaultFinalDisclaimerMessage;
              else
                  this.essOptions.finalDisclaimerMessage = essOptions.finalDisclaimerMessage.replace(/<br\/>/g, '\n');

              if( !this.essOptions.finalDisclaimerAgreementText)
                  this.essOptions.finalDisclaimerAgreementText = this.defaultFinalDisclaimerAgreementText;
              else
                  this.essOptions.finalDisclaimerAgreementText = essOptions.finalDisclaimerAgreementText.replace(/<br\/>/g, '\n');

          } else {
                // Set default value
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
                    welcomeMessage: null,
                    finalDisclaimerMessage: this.defaultFinalDisclaimerMessage,
                    finalDisclaimerAgreementText: this.defaultFinalDisclaimerAgreementText,
                    manageDirectDeposit: false,
                    manageDirectDepositAmountAndAccountInfo: false,
                    directDepositDisclaimer: null
                }
          }
      }));

    }

    insertTextAt(textToInsert : string) {
      Utilities.insertAtCaret(textToInsert, angular.element('textarea[name="FinalDisclaimerMessage"]').get(0));
      var contTA=angular.element('textarea[name="FinalDisclaimerMessage"]').get(0);
      this.essOptions.finalDisclaimerMessage= (<HTMLInputElement>contTA).value;
    }

    save() {
      
      if (this.essOptions.isCustomMessage && (!this.essOptions.finalDisclaimerMessage || !this.essOptions.finalDisclaimerAgreementText)) return;
      let options = angular.copy(this.essOptions);
      
      if(!this.essOptions.isCustomMessage) {
        options.finalDisclaimerMessage = this.defaultFinalDisclaimerMessage;
        options.finalDisclaimerAgreementText = this.defaultFinalDisclaimerAgreementText;
      }
      else {
          options.finalDisclaimerMessage = options.finalDisclaimerMessage.replace(/\n/g, '<br/>');
          options.finalDisclaimerAgreementText = options.finalDisclaimerAgreementText.replace(/\n/g, '<br/>');
      }

      this.clientService.updateClientEssOptions(options).subscribe(() => {
          this.essOptions.finalDisclaimerMessage = options.finalDisclaimerMessage.replace(/<br\/>/g, '\n');
          this.essOptions.finalDisclaimerAgreementText = options.finalDisclaimerAgreementText.replace(/<br\/>/g, '\n');
          this.msgSvc.setSuccessMessage("Final Disclaimer updated successfully.", 5000);
      }); //.catch(this.msgSvc.showWebApiException);
    }

    preview() {
      
      if (this.essOptions.isCustomMessage && (!this.essOptions.finalDisclaimerMessage || !this.essOptions.finalDisclaimerAgreementText)) return;

      let previewText = this.essOptions.finalDisclaimerMessage;
        if (!this.essOptions.isCustomMessage) 
            previewText = this.defaultFinalDisclaimerMessage;

      let config = new MatDialogConfig<any>();
              config.width = "700px";
              config.data = { main: this.replaceWildCards(previewText).replace(/\n/g, '<br/>'),
                              agreement: this.essOptions.finalDisclaimerAgreementText};
              
              return this.dialog.open<PreviewFinalDisclaimerDialogComponent, any, string>(PreviewFinalDisclaimerDialogComponent, config)
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

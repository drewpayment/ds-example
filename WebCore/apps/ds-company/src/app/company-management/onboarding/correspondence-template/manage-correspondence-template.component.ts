import * as angular from "angular";
import { Component, OnInit, Input } from '@angular/core';
import { ClientService } from '@ds/core/clients/shared';
import { AccountService } from '@ds/core/account.service';
import { ClientEssOptions } from 'apps/ds-company/ajs/models/client-ess-options.model';
import { Utilities } from 'apps/ds-company/src/app/shared/utils/utilities';
import { tap, switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs/internal/Observable';
import { HttpErrorResponse } from '@angular/common/http';
import { IUserInfo } from '@ajs/user';
import { UserInfo } from '@ds/core/shared';
import { IApplicantEmailTemplateData, ApplicantCorrespondenceTypeEnum, 
            IApplicantCorrespondenceTypeData, ICustomizeSenderData } from "apps/ds-company/src/app/models/correspodence-template-data";
import { CorrespondenceTemplateApiService } from "apps/ds-company/src/app/services/correspondence-template-api.service";
import { forkJoin } from "rxjs";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { CustomizeSenderDialogComponent } from "./customize-sender/customize-sender-dialog.component";
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ImageDto, ResourceType } from "@ajs/core/ds-resource/models";
import { IAzureViewDto } from "@ajs/core/ds-resource/models/azure-view-dto.model";
import { DsResourceApi } from "@ajs/core/ds-resource/ds-resource-api.service";
import {ImageUploaderComponent} from "@ds/core/resources/image-uploader/image-uploader.component";
import { ImageType } from '@ds/core/resources/shared/image-type.model';
import { ImageSizeType } from '@ds/core/resources/shared/image-size-type.model';
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import { ActivatedRoute, Params, Router, NavigationEnd } from '@angular/router';
import { PreviewCorrespondenceTemplateDialogComponent } from './preview-correspondence-template/preview-correspondence-template-dialog.component';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: 'ds-manage-correspondence-template',
  templateUrl: './manage-correspondence-template.component.html',
  styleUrls: ['./manage-correspondence-template.component.scss']
})
export class ManageCorrespondenceTemplateComponent implements OnInit {
    isLoading: boolean = true;
    pageTitle: string;
    clientId: number;
    clientName: string;
    isApplicantAdmin: boolean;
    userId: number;
    userTypeId: number;
    isApplicantHiringWorkflowEnabled: boolean;
    disclaimerTemplate: string;
    defaultTemplateHtml: string;
    isDisclaimerTemplate: boolean;
    isCustomDisclaimer: boolean;
    tmpIsCustomDisclaimer: boolean;    
    isEditMode = false;
    isCompanyLogoPresent: boolean;
    skipEditing: boolean;
    isLinkedToOnboarding: boolean;
    isMailTemplatesOnly: boolean;
    companyRootUrl: string;
    selected: string;
    frm: FormGroup;
    frmSubmitted: boolean = false;
   // useCustomMessage: boolean =false;
    pageType: number = 1;

    private user: UserInfo;
    applicantEmailTemplateData: IApplicantEmailTemplateData = { };
    applicantTextTemplateData: IApplicantEmailTemplateData = { };

    applicantEmailTemplates: IApplicantEmailTemplateData[] = [];
    applicantTextTemplates: IApplicantEmailTemplateData[] = [];

    applicantCorrespondenceTypeData: IApplicantCorrespondenceTypeData ={};
    applicantCorrespondenceTypes: IApplicantCorrespondenceTypeData[] = [];
    applicantCorrespondenceTypesFilter: (applicantCorrespondenceType) => boolean;

    public senderInfo: ICustomizeSenderData;
    // company image vars
    logo:ImageDto;

    readonly helpText = {
      disclaimer: 'The Company Disclaimer is included in all applications and displays before the applicant submits their application.',
    };

    wcButtons:Array<any> = [
      {name:"applicantName",            wildcard:"{*Applicant}",            lbl:"Applicant's Name",         order:10, isActive:true},
      {name:"applicantFirstName",       wildcard:"{*ApplicantFirstName}",   lbl:"Applicant's First Name",   order:20, isActive:true},
      {name:"applicantAddress",         wildcard:"{*ApplicantAddress}",     lbl:"Applicant's Address",      order:30, isActive:true},
      {name:"applicantPhoneNumber",     wildcard:"{*ApplicantPhoneNumber}", lbl:"Applicant's Phone Number", order:40, isActive:true},
      {name:"posting",                  wildcard:"{*Posting}",              lbl:"Post Title",               order:50, isActive:true},
      {name:"date",                     wildcard:"{*Date}",                 lbl:"Current Date",             order:60, isActive:true},
      {name:"logo",                     wildcard:"{*CompanyLogo}",          lbl:"Company's Logo",           order:70, isActive:true},
      {name:"companyName",              wildcard:"{*CompanyName}",          lbl:"Company's Name",           order:80, isActive:true},
      {name:"companyAddress",           wildcard:"{*CompanyAddress}",       lbl:"Company's Address",        order:90, isActive:true},
      {name:"onboardingInfo",           wildcard:"{*OnboardingUrl}\n\nUsername: {*UserName}\nPassword: {*Password}",lbl:"Onboarding Information", order:100, isActive:true}];



    constructor(
      private fb: FormBuilder,
      private accountService: AccountService,
      private clientService: ClientService,
      private apiService: CorrespondenceTemplateApiService,
      private dialog: MatDialog,
      private msg: NgxMessageService,
      private _confirmDialog: ConfirmDialogService,
      private route: ActivatedRoute,
      private router: Router,
      ) {}

    ngOnInit() {
        this.pageType = this.route.snapshot.params['pageType'];
        this.isLinkedToOnboarding = (this.pageType==1);
        this.isMailTemplatesOnly = (this.pageType==1);
        this.isDisclaimerTemplate = (this.pageType==3);

        this.accountService.getUserInfo().subscribe(x=> {
            this.user=x;
            this.clientId = x.lastClientId || x.clientId;
            this.userId = x.userId;
            this.userTypeId = x.userTypeId;
            
        });

        this.defaultTemplateHtml =
                `By accepting this form I hereby certify that the statements made by me on this application are true, complete, and correct to the best of my knowledge. I grant permission to verify such answers including a Criminal Record Check, Driving Record Check, Central Abuse Registry Check, Verification of Employment Eligibility, Health Status Verification, Education Verification, etc., and understand that any misstatement or omission of fact on this Application, or on related employment materials, may be considered as sufficient cause for rejection of this Application or for my dismissal if such information is discovered subsequent to my employment.
                 <br /><br />I authorize the references listed in this Application, and any prior employer, educational institution, or any other persons or organizations, to give {*CompanyName} any and all information concerning my previous employment/educational accomplishments, disciplinary information or any other pertinent information they may have, personal or otherwise.  I release all parties from all liability for any damage that may result in any way from their participation in such inquiries or information exchanges.
                 <br /><br />I understand this Application will remain active for a period of six(6) months and that I must notify {*CompanyName} in writing at the end of such period if I wish to reactivate or amend this Application, or if I wish to include any new/or changed information.
                 <br /><br />I further understand that if I am offered a position I will be asked to complete a Physical Examination and TB skin test, and may be required to complete a Pre-Employment Drug Screening Test.I authorize every medical doctor or other health care provider to provide {*CompanyName} with any and all information upon request, including but not limiting to medical reports, laboratory reports, X-rays, clinical abstracts, or other medical records relating to my previous health history or employment in connection with any examination, consultation, text or evaluation. I release every medical doctor, health care personnel, and every other person or entity providing my medical information to {*CompanyName} from any and all liability.
                 <br /><br />I also accept that employment with {*CompanyName} shall be at will, and subject to the wages, benefits, hours, conditions, and under other such employment policies outlined in the Employee Manual of Policy and Procedures. I understand that no one other than the Executive Director of this agency has the authority to enter into any agreement for a specified period of time or to make any agreement which is contrary to the foregoing and that any such agreement must be in writing and signed by the Executive Director in order for it to be binding.`;
        
        this.InitPage();

        this.router.events.subscribe((event) => {
            if (event instanceof NavigationEnd) {
                this.pageType = this.route.snapshot.params['pageType'];
                this.isLinkedToOnboarding = (this.pageType==1);
                this.isMailTemplatesOnly = (this.pageType==1);
                this.isDisclaimerTemplate = (this.pageType==3);
                this.InitPage();
            }
        })
    };

    private InitPage(){
        this.selected = "EMAIL";

        if (this.isDisclaimerTemplate) {
            this.isDisclaimerTemplate = true;
            this.pageTitle = "Company Disclaimer";
            this.wcButtons.find( x => x.name == "onboardingInfo" ).isActive = false;
        }
        else {
            this.isDisclaimerTemplate = false;
            this.pageTitle = this.selected == "EMAIL" ? "Email Templates" : "Text Templates";

            // Initialize wild card buttons, and customize order 
            if(this.isLinkedToOnboarding){
                this.wcButtons.filter( x => 
                    x.name == "applicantAddress" || x.name == "applicantPhoneNumber" || x.name == "date" )
                    .forEach(y => y.isActive = false);
                
                // move the logo next to company address
                let caOrder = this.wcButtons.find( x => x.name == "companyAddress" ).order;
                this.wcButtons.find( x => x.name == "logo" ).order = caOrder + 1;

                // change the post title description
                this.wcButtons.find( x => x.name == "posting" ).id = "Job Position";
            } else {
                this.wcButtons.find( x => x.name == "onboardingInfo" ).isActive = false;
            }
        }

        this.setApplicantCorrespondenceTypeDefaultValue();

        if (this.isDisclaimerTemplate) {
            this.applicantCorrespondenceTypes = [{
                applicantCorrespondenceTypeId: ApplicantCorrespondenceTypeEnum.applicationDisclaimer,
                description: "Application Disclaimer",
            }];

            this.apiService.getApplicantCompanyCorrespondences(this.clientId, ApplicantCorrespondenceTypeEnum.applicationDisclaimer).subscribe(data => {
                //Email templates
                this.applicantEmailTemplates = _.filter(data, item => item.isText === false);
                if (this.applicantEmailTemplates.length > 0) {
                    this.applicantEmailTemplateData = angular.copy(this.applicantEmailTemplates[0]);
                }
                else {
                    this.setApplicantEmailTemplateDefaultValue();
                }

                this.isCustomDisclaimer = (this.applicantEmailTemplates.length > 0);
                this.tmpIsCustomDisclaimer = this.isCustomDisclaimer;
                let emailBodyContent = angular.copy(this.applicantEmailTemplateData.body);
                emailBodyContent = emailBodyContent.replace(/<br\s{0,1}\/>/g, '\n');
                this.applicantEmailTemplateData.body = angular.copy(emailBodyContent);

                //Text templates
                this.setApplicantTextTemplateDefaultValue();

                this.isEditMode = true;
                this.isLoading = false;
            });
        }
        else {
            if(this.isLinkedToOnboarding){
                this.applicantCorrespondenceTypes = [
                {   applicantCorrespondenceTypeId: ApplicantCorrespondenceTypeEnum.onboardingInvitation,
                    description: "General",
                },{ applicantCorrespondenceTypeId: ApplicantCorrespondenceTypeEnum.applicationResponse,
                    description: "Applicant Tracking",
                }];
            } else {
                this.applicantCorrespondenceTypes = [
                { applicantCorrespondenceTypeId: ApplicantCorrespondenceTypeEnum.applicationResponse,
                    description: "Application Response",
                },{ applicantCorrespondenceTypeId: ApplicantCorrespondenceTypeEnum.applicationCompleted,
                    description: "Application Completed",
                }];
            }
            if(!this.isLinkedToOnboarding)
            {
                forkJoin([
                    this.apiService.getApplicantCompanyCorrespondences(this.clientId, null),
                    this.apiService.getApplicantCompanyCorrespondences(this.clientId, null, true)])
                    .subscribe(x => {
                        this.applicantEmailTemplates = _.filter(x[0], item=> 
                            item.applicantCorrespondenceTypeId==ApplicantCorrespondenceTypeEnum.applicationCompleted
                            || item.applicantCorrespondenceTypeId==ApplicantCorrespondenceTypeEnum.applicationResponse);
                        this.applicantEmailTemplates = _.sortBy(this.applicantEmailTemplates, function (i) { return i.description.toLowerCase(); });

                        this.applicantTextTemplates = _.filter(x[1], item=> 
                            item.applicantCorrespondenceTypeId==ApplicantCorrespondenceTypeEnum.applicationCompleted
                            || item.applicantCorrespondenceTypeId==ApplicantCorrespondenceTypeEnum.applicationResponse);;
                        this.applicantTextTemplates = _.sortBy(this.applicantTextTemplates, function (i) { return i.description.toLowerCase(); });
                            
                        this.setApplicantTextTemplateDefaultValue();
                        this.setApplicantEmailTemplateDefaultValue();
                        this.isLoading = false;
                    });
                
            }
            else{
                forkJoin([
                    this.apiService.getOnboardingCompanyCorrespondences(this.clientId, null),
                    this.apiService.getOnboardingCompanyCorrespondences(this.clientId, null, true)])
                    .subscribe(x => {
                        this.applicantEmailTemplates = _.filter(x[0], item=> 
                            item.applicantCorrespondenceTypeId==ApplicantCorrespondenceTypeEnum.onboardingInvitation
                            || item.applicantCorrespondenceTypeId==ApplicantCorrespondenceTypeEnum.applicationResponse);
                        this.applicantEmailTemplates = _.sortBy(this.applicantEmailTemplates, function (i) { return i.description.toLowerCase(); });

                        this.applicantTextTemplates = _.filter(x[1], item=> 
                            item.applicantCorrespondenceTypeId==ApplicantCorrespondenceTypeEnum.onboardingInvitation
                            || item.applicantCorrespondenceTypeId==ApplicantCorrespondenceTypeEnum.applicationResponse);;;
                        this.applicantTextTemplates = _.sortBy(this.applicantTextTemplates, function (i) { return i.description.toLowerCase(); });

                        this.setApplicantTextTemplateDefaultValue();
                        this.setApplicantEmailTemplateDefaultValue();
                        this.isLoading = false;
                    });
                    
            }
        }

        this.retriveCompanyLogo();

        this.apiService.getClientSMTPSetting(this.clientId).subscribe(data => {
            this.senderInfo = data;
        });

        this. initForm();
        this.isLoading = false;
    }

    public retriveCompanyLogo() {
        /**
         * Gets the client logo and flags if present
         */
         this.apiService.getClientResource(ResourceType.AzureClientImage, this.clientId, 'logo')
             .subscribe((image:IAzureViewDto) => {
                 this.isCompanyLogoPresent = (image != null && image.source != null);
                 if(image){
                     this.logo = {
                         resourceId: image.resourceId,
                         name: image.name,
                         clientId: image.clientId,
                         employeeId: image.employeeId,
                        hasImage: image.source != null,
                         imageSize: image.size,
                         imageType: image.type,
                         source: image.source,
                         token: image.token  
                     };
                 }
             }, 
             (err: HttpErrorResponse) => {
                 if (err.error && err.error.errors && err.error.errors.length) {
                   //this.msg.setErrorMessage(err.error.errors[0].msg);
                 } else {
                   //this.msg.setErrorMessage(err.message);
                 }   
                 this.isCompanyLogoPresent = false;         
             });
    }

    openImageUpload() {
        let config = new MatDialogConfig<any>();
        config.width = '450px';
        config.data = {
            user: null,
            employee: null,
            image: this.logo,
            imageType: ImageType.companyLogo,
            imageSize: ImageSizeType.companyLogo,
            cropitOptions: { minZoom: 'fit' },
            modalTitle: "Company Logo",
            firstName: '',
            lastName: '',
        };

        return this.dialog
        .open<ImageUploaderComponent, any, string>(
            ImageUploaderComponent,
            config
        )
        .afterClosed()
        .subscribe((result: any) => {
            if (result) {
                this.retriveCompanyLogo();
            }
        });
    }

    initForm() {
        if (this.isDisclaimerTemplate) {
            this.frm = this.fb.group({
                chkCustomMessage: this.fb.control(this.isCustomDisclaimer, Validators.required),
                templateMessageBody: this.fb.control(this.applicantEmailTemplateData.body, Validators.required),
            });
        }
        else{
            if(this.selected=="TEXT")
            {
                this.frm = this.fb.group({
                    templateName: this.fb.control(this.applicantTextTemplateData.description, [Validators.required, Validators.maxLength(50)]),
                    templateCategory: this.fb.control(this.applicantTextTemplateData.applicantCorrespondenceTypeId, Validators.required),
                    //chkCustomMessage: this.fb.control(this.isCustomDisclaimer, Validators.required),
                    templateMessageBody: this.fb.control(this.applicantTextTemplateData.body, Validators.required),
                });
            }
            else{
                this.frm = this.fb.group({
                    templateName: this.fb.control(this.applicantEmailTemplateData.description, [Validators.required, Validators.maxLength(50)]),
                    templateCategory: this.fb.control(this.applicantEmailTemplateData.applicantCorrespondenceTypeId, Validators.required),
                    templateSubject: this.fb.control(this.applicantEmailTemplateData.subject,  [Validators.required, Validators.maxLength(255)]),
                    //chkCustomMessage: this.fb.control(this.isCustomDisclaimer, Validators.required),
                    templateMessageBody: this.fb.control(this.applicantEmailTemplateData.body, Validators.required),
                });
            }
        }
      }

    private setApplicantEmailTemplateDefaultValue(resetData:boolean = true) {
      if(resetData)
      this.applicantEmailTemplateData = {
          applicantCompanyCorrespondenceId: null,
          isText: false,
          clientId: this.clientId,
          description: "",
          subject: "",
          applicantCorrespondenceTypeId: null,
          applicantCorrespondenceType: "",
          body: "",
          isActive: true,
          modified: null,
          modifiedBy: "",
          isApplicantAdmin: this.isApplicantAdmin
      };

      if (this.isDisclaimerTemplate) {
          this.applicantEmailTemplateData.applicantCorrespondenceTypeId = ApplicantCorrespondenceTypeEnum.applicationDisclaimer;
          this.applicantEmailTemplateData.description = "Default Disclaimer";
          this.applicantEmailTemplateData.subject = "Default Disclaimer";
          this.applicantEmailTemplateData.body = this.defaultTemplateHtml.replace(/\n+\s*/g, '')
                                                          .replace(/<br\s{0,1}\/>/g, '\n');
      }

  }

  private setApplicantTextTemplateDefaultValue() {
      this.applicantTextTemplateData = {
          applicantCompanyCorrespondenceId: null,
          isText: true,
          clientId: this.clientId,
          description: "",
          subject: "",
          applicantCorrespondenceTypeId: null,
          applicantCorrespondenceType: "",
          body: "",
          isActive: true,
          modified: null,
          modifiedBy: "",
          isApplicantAdmin: this.isApplicantAdmin
      };
  }

  private setApplicantCorrespondenceTypeDefaultValue() {
      this.applicantCorrespondenceTypeData = {
          applicantCorrespondenceTypeId: null,
          description: ""
      };
  }

  public openApplicantCorrespondenceTemplateDetails(isText: boolean, applicantCorrespondenceTemplate: IApplicantEmailTemplateData) {
        if(this.skipEditing){
            this.skipEditing = false;
            return;
        }
        this.isEditMode = true;

        if (!isText) {
            if (applicantCorrespondenceTemplate) {
                this.applicantEmailTemplateData = angular.copy(applicantCorrespondenceTemplate);
                let bodyContent = angular.copy(this.applicantEmailTemplateData.body);
                bodyContent = bodyContent.replace(/<br\s{0,1}\/>/g, '\n');
                this.applicantEmailTemplateData.body = angular.copy(bodyContent);
            }
            else {
                this.setApplicantEmailTemplateDefaultValue();
            }
            
        }
        else {
            if (applicantCorrespondenceTemplate) {
                this.applicantTextTemplateData = angular.copy(applicantCorrespondenceTemplate);
                let bodyContent = angular.copy(this.applicantTextTemplateData.body);
                bodyContent = bodyContent.replace(/<br\s{0,1}\/>/g, '\n');
                this.applicantTextTemplateData.body = angular.copy(bodyContent);
            }
            else {
                this.setApplicantTextTemplateDefaultValue();
            }
        }
        this.initForm();
    };

    public setDisclaimerType(isDefault: boolean){  
        const options = {
            title: `Are you sure you want to use the default disclaimer?`,
            message: 'Switching back to the default disclaimer will delete your custom one. ',
            confirm: "Use Default",
          };

        if(this.isCustomDisclaimer && isDefault){
            // User wants default disclaimer
            this._confirmDialog.open(options);
            this._confirmDialog.confirmed().subscribe((confirmed) => {
              if (confirmed) {
                this.isCustomDisclaimer = false;
                this.tmpIsCustomDisclaimer = false;
                this.setApplicantEmailTemplateDefaultValue(false);
                this.initForm();
              } else {
                this.tmpIsCustomDisclaimer = true;
              }
            });
        }
        else if(!this.isCustomDisclaimer && !isDefault){
            this.isCustomDisclaimer = true;
            this.applicantEmailTemplateData.body = this.defaultTemplateHtml.replace(/\n+\s*/g, '')
                                                          .replace(/<br\s{0,1}\/>/g, '\n');
            this.initForm();                                                          
        } 
    }

    public wcButtonsFilter(item: any){
        return (
            item.filter(x=>x.isActive==true)
          );
    }

    editSender() {
        let config = new MatDialogConfig<any>();
        config.width = "600px";
        config.data = { clientId: this.clientId, senderInfo: this.senderInfo };
        
        return this.dialog.open<CustomizeSenderDialogComponent, any, string>(CustomizeSenderDialogComponent, config)
            .afterClosed()
            .subscribe((returnData: any) => {
                if(returnData)
                    this.senderInfo=returnData;
            });
    }

    public cancel(isText: boolean) {
        
        if (!isText)
            this.setApplicantEmailTemplateDefaultValue();
        else 
            this.setApplicantTextTemplateDefaultValue();
        
        this.frmSubmitted = false;
        this.isEditMode = false;
    }

    public isOnboardingTemplate = ():boolean => {
        if(!this.applicantEmailTemplateData.applicantCorrespondenceTypeId) return false;
        else if( this.applicantEmailTemplateData.applicantCorrespondenceTypeId == ApplicantCorrespondenceTypeEnum.onboardingInvitation )return true;
        return false;
    };

    public categoryDesc = (typeId:ApplicantCorrespondenceTypeEnum):string => {
        var currCategory = this.applicantCorrespondenceTypes.find(x=>x.applicantCorrespondenceTypeId == typeId);
        if(currCategory){
            return currCategory.description
        }
        return "";
    };

    insertTextAt(textToInsert : string) {
        Utilities.insertAtCaret(textToInsert, angular.element('textarea[name="templateMessageBody"]').get(0));
        var contTA=angular.element('textarea[name="templateMessageBody"]').get(0);
        if(this.selected=="EMAIL"){
            this.applicantEmailTemplateData.body= (<HTMLInputElement>contTA).value;
        }
        else if(this.selected=="TEXT"){
            this.applicantTextTemplateData.body= (<HTMLInputElement>contTA).value;
        }
    }

    save(isText: boolean) {
        this.frmSubmitted = true;

        if (this.frm.valid) {

            let modified:IApplicantEmailTemplateData = null;
            let isNewTemplate: boolean = false;
            if (!isText) {
                let isNew = !this.applicantEmailTemplateData.applicantCompanyCorrespondenceId;
                if (isNew) {
                    this.applicantEmailTemplateData.applicantCompanyCorrespondenceId = 0;
                    this.applicantEmailTemplateData.clientId = this.clientId;
                    isNewTemplate = true;
                }
                modified = angular.copy(this.applicantEmailTemplateData);
                modified.isApplicantAdmin=this.isApplicantAdmin;
                modified.description= this.frm.value.templateName || modified.description;
                modified.subject= this.frm.value.templateSubject || modified.subject;
                modified.applicantCorrespondenceTypeId= this.frm.value.templateCategory || modified.applicantCorrespondenceTypeId;
                modified.body = modified.body.replace(/\n/g, '<br/>');
            }
            else {
                let isNew = !this.applicantTextTemplateData.applicantCompanyCorrespondenceId;
                if (isNew) {
                    this.applicantTextTemplateData.applicantCompanyCorrespondenceId = 0;
                    this.applicantTextTemplateData.clientId = this.clientId;
                    isNewTemplate = true;
                }
                modified = this.applicantTextTemplateData;
                modified.isApplicantAdmin=this.isApplicantAdmin;
                modified.description= this.frm.value.templateName;
                modified.applicantCorrespondenceTypeId= this.frm.value.templateCategory;
                modified.body = modified.body.replace(/\n/g, '<br/>');
            }

            if(this.isDisclaimerTemplate && !this.isCustomDisclaimer){
                if(!isNewTemplate ){
                    this.applicantEmailTemplateData.isActive = false;
                    modified.isActive = false;
                }else if(isNewTemplate){
                    // No need to save this one
                    this.msg.setSuccessMessage("Disclaimer saved successfully.");
                    return;
                }
            }

            this.apiService.saveApplicantCompanyCorrespondence(modified).subscribe(data => {
                this.frmSubmitted = false;
                let correspondenceTypeText = _.find(this.applicantCorrespondenceTypes, { 'applicantCorrespondenceTypeId': data.applicantCorrespondenceTypeId }).description;
                data.applicantCorrespondenceType = correspondenceTypeText;

                if (!isText) {
                    if (isNewTemplate) {
                        this.applicantEmailTemplates.push(data);
                    } 
                    else {
                        let i = _.findIndex(this.applicantEmailTemplates, x => x.applicantCompanyCorrespondenceId === data.applicantCompanyCorrespondenceId);
                        this.applicantEmailTemplates[i] = data;
                    }
                    this.applicantEmailTemplateData.applicantCompanyCorrespondenceId = data.applicantCompanyCorrespondenceId;
                    this.msg.setSuccessMessage(this.isDisclaimerTemplate ? "Disclaimer saved successfully." :
                        "Email template saved successfully.");
                }
                else {
                    if (isNewTemplate) {
                        this.applicantTextTemplates.push(data);
                    } 
                    else {
                        let i = _.findIndex(this.applicantTextTemplates, x => x.applicantCompanyCorrespondenceId === data.applicantCompanyCorrespondenceId);
                        this.applicantTextTemplates[i] = data;
                    }
                    this.applicantTextTemplateData.applicantCompanyCorrespondenceId = data.applicantCompanyCorrespondenceId;
                    this.msg.setSuccessMessage("Text template saved successfully.");
                }

                if(!this.isDisclaimerTemplate){
                   this.isEditMode = false;
                }
                

            }); //.catch(this.msgSvc.showWebApiException);
        }
    }

    copyCorrespondenceTemplate(isText: boolean, applicantCorrespondenceTemplate: IApplicantEmailTemplateData) {
        this.skipEditing = true;
        let baseTemplate = angular.copy(applicantCorrespondenceTemplate);
        baseTemplate.applicantCompanyCorrespondenceId = 0;
        baseTemplate.description = "Copy of : " + applicantCorrespondenceTemplate.description;
        if(!isText)
            baseTemplate.body = baseTemplate.body.replace(/\n/g, '<br/>');
        baseTemplate.isApplicantAdmin=this.isApplicantAdmin;

        this.apiService.saveApplicantCompanyCorrespondence(baseTemplate).subscribe(data => {
            if (!isText) {
                this.applicantEmailTemplateData = data;
                this.applicantEmailTemplates.push(data);
                this.setApplicantEmailTemplateDefaultValue();
                this.msg.setSuccessMessage("Email template copied successfully.");
            }
            else {
                this.applicantTextTemplateData = data;
                this.applicantTextTemplates.push(data);
                this.setApplicantTextTemplateDefaultValue();
                this.msg.setSuccessMessage("Text template copied successfully.");
            }
            this.isEditMode = false;
        });
        //.catch(this.msgSvc.showWebApiException);
    }

    deleteCorrespondenceTemplate(applicantCorrespondenceTemplate: IApplicantEmailTemplateData) {
        this.skipEditing = true;
        let type = (!applicantCorrespondenceTemplate.isText? "email":"text");
        const options = {
            title: `Are you sure you want to delete this ${type} template?`,
            message: "",
            confirm: "Delete",
          };
          this._confirmDialog.open(options);
          this._confirmDialog.confirmed().subscribe((confirmed) => {
            if (confirmed) {
                let idToDelete = applicantCorrespondenceTemplate.applicantCompanyCorrespondenceId;
                applicantCorrespondenceTemplate.isApplicantAdmin=true;
                this.apiService.deleteApplicantCompanyCorrespondence(applicantCorrespondenceTemplate).subscribe(() => {
                    if (!applicantCorrespondenceTemplate.isText) {
                        _.remove(this.applicantEmailTemplates, r => r.applicantCompanyCorrespondenceId === idToDelete);
                        this.msg.setSuccessMessage("Email Template deleted successfully.");
                        this.setApplicantEmailTemplateDefaultValue();
                    }
                    else {
                        _.remove(this.applicantTextTemplates, r => r.applicantCompanyCorrespondenceId === idToDelete);
                        this.msg.setSuccessMessage("Text Template deleted successfully.");
                        this.setApplicantTextTemplateDefaultValue();
                    }
                });
                //.catch(this.msgSvc.showWebApiException);
            }
          });
    }

    previewCorrespondenceTemplate(isText: boolean, applicantCorrespondenceTemplate: IApplicantEmailTemplateData) {
  
        let config = new MatDialogConfig<any>();
                config.width = "700px";
                config.data = { body: this.replaceWildCards(applicantCorrespondenceTemplate.body).replace(/\n/g, '<br/>'),
                                description: applicantCorrespondenceTemplate.description,
                                subject:applicantCorrespondenceTemplate.subject,
                                isText:isText};
                
                return this.dialog.open<PreviewCorrespondenceTemplateDialogComponent, any, string>(PreviewCorrespondenceTemplateDialogComponent, config)
      }
  
      replaceWildCards(message:string){
          message = message.replace(/\{\*CompanyName\}/g, this.user.lastClientName );
          return message;
      }

}

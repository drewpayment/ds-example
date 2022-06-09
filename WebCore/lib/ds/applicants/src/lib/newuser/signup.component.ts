import { Component, OnInit, Input, Output, Inject, ViewChild } from '@angular/core';
import { SignUpApiService } from './signup-api.service';
import { DOCUMENT } from '@angular/common';
import { FormControl, FormGroup, Validators, FormBuilder, AsyncValidatorFn } from '@angular/forms';
import { ValidatorFn, ValidationErrors } from '@angular/forms';
import { ViewState } from "./shared/step.model";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { ICountry, IState } from "@ajs/applicantTracking/shared/models";
import { DsConfirmService } from "@ajs/ui/confirm/ds-confirm.service";
import { ApplicantDto } from "@ajs/applicantTracking/newuser/applicant-newUser-dto.model";
import { CountryStateService } from "@ajs/location/country-state/country-state.svc";
import { Observable, from  } from 'rxjs';
import { map } from 'rxjs/operators';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { MatStepper } from '@angular/material/stepper';

@Component({
  selector: 'ds-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignUpComponent implements OnInit {

    @Input() clientCode: string;
    @Input() clientId: number;
    @Input() postingId: number;

    /** Array of ICouuntry  */
    public countries: Array<ICountry> = [];

    /** Array of IState */
    public states: Array<IState>   = [];

    /** RegExp pattern for zipcode  */
    public zipCodePattern: RegExp;

    /** Form data transfer object */
    public applicant: ApplicantDto;

    /** determines if passwords match */
    public notMatched: boolean;

    /** disallow the user to save */
    public noSave: boolean = false;

    /** tracks what posting the applicant was applying for at the time of new user**/
    public postUrl: string;

    @ViewChild(MatStepper, { static: false }) stepper: MatStepper;

    /** forms representing 3 steps **/
    form1: FormGroup;
    form2: FormGroup;
    form3: FormGroup;
    formSubmitted: boolean;
    get password() { return this.form1.controls.password as FormControl; }
    get userName() { return this.form1.controls.userName as FormControl; }
    get userNameUnique() { return this.form1.controls.userNameUnique as FormControl; }
    get passwordConfirm() { return this.form1.controls.passwordConfirm as FormControl; }
    get country() { return this.form3.controls.country as FormControl; }
    get state() { return this.form3.controls.state as FormControl; }
    get zip() { return this.form3.controls.zip as FormControl; }

    /** navigate through different cards **/
    readonly accountState: ViewState = {
        nextLabel: "Continue",
        prevLabel: "Cancel"
    };

    readonly contactState: ViewState = {
        nextLabel: "Continue",
        prevLabel: "Previous"
    }

    readonly addressState: ViewState = {
        nextLabel: "Create",
        prevLabel: "Previous"
    }
    public selected: ViewState;

    readonly safePasswordHelpTxt: string = "Password may be 8-50 characters, must contain one uppercase letter, one lowercase letter, and one numerical character, and are case sensitive.";
    readonly safePasswordErrTxt: string = "Password must contain an uppercase and lowercase letter, one number, and have 8-50 characters. Special characters are allowed.";
    readonly safePasswordFormat: RegExp = /^.*(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,50}$/;

    public passwordHelpTxt: string;
    public passwordErrTxt: string;
    public passwordFormat: RegExp;
    isHandset$: Observable<boolean> = this.breakpointObserver.observe([Breakpoints.XSmall])
        .pipe(
            map(result => result.matches)
        );

    constructor(private signupService: SignUpApiService,
        private countryStateService: CountryStateService,
        private msg: DsMsgService,
        private confirm: DsConfirmService,
        private fb: FormBuilder,
        private breakpointObserver: BreakpointObserver,
        @Inject(DOCUMENT) private document: Document) {

    }

    ngOnInit() {
        this.InitForms();

        if(+this.clientCode === 0 || this.clientId === 0) {
            this.msg.showErrorMsg("Invalid client.");
            this.noSave = true;
        } else {
            this.signupService.getCompanyJobBoardInfo(this.clientId).subscribe(data => {
                if (data != null) {
                    if (data.clientCode.toUpperCase() !== this.clientCode.toUpperCase()) {
                        this.noSave = true;
                        window.location.href = "ApplicantPostingListNL.aspx?code=" + this.clientCode;
                    }
                } else if (data == null) {
                    this.noSave = true;
                }

            },
            err => this.msg.showWebApiException(err));

            //As per Case SYS-1198, Safe Password is the new normal for all Users
            this.passwordHelpTxt = this.safePasswordHelpTxt;
            this.passwordErrTxt = this.safePasswordErrTxt;
            this.passwordFormat = this.safePasswordFormat;

            this.password.setValidators([Validators.required, Validators.pattern(this.passwordFormat)]);

            this.applicant = new ApplicantDto;
            this.loadCountries();
        }

        this.selected = this.accountState ;
    }

    public InitForms(){
        /// Build the forms here
        this.form1 = this.fb.group({
            userName:        this.fb.control(null, { validators: [Validators.required, Validators.pattern(/^(?=[a-zA-Z0-9._]{4,15}$)(?!.*[_.]{2})[^_.].*[^_.]$/)], updateOn: 'blur' }),
            userNameUnique: ['true', Validators.required],
            email:      [null, [Validators.required,Validators.pattern(/^[\w\.\%\+\-]+@[\w\.\-]+\.[a-zA-Z]{2,4}$/)]],
            password:   [null, Validators.required],
            passwordConfirm: this.fb.control(null, { validators: null }),
        });
        this.form2 = this.fb.group({
            firstName:  [null, Validators.required],
            mi:         [null, Validators.pattern(/^([a-zA-Z]){1}/)],
            lastName:   [null, Validators.required],
            primaryPhone:       this.fb.control(null, {validators: [Validators.required,Validators.pattern(/^\d{10}$/)] }),
            secondaryPhone:     this.fb.control(null, {validators: [Validators.pattern(/^\d{10}$/)]}),
            workPhone:          this.fb.control(null, {validators: [Validators.pattern(/^\d{10}$/)]}),
            workExtension:      [null, null],
            isTextEnabled:  [null, null],
        });
        this.form3 = this.fb.group({
            country:    [null, null],
            address1:   [null, null],
            address2:   [null, null],
            city:       [null, null],
            state:      [null, null],
            zip:        this.fb.control(null, { validators: null }),
        });

        // confirm password validation
        const confirmPasswordValidator = this.confirmPasswordValidatorFn(this.password);
        this.passwordConfirm.setValidators([Validators.required, confirmPasswordValidator]);
    }

    public confirmPasswordValidatorFn(password:FormControl): ValidatorFn{
        return (control: FormControl): ValidationErrors | null => {
            const confirmed = control.value;
            return (password.value != confirmed) ? {'notMatched': true} : null;
        };
    }

    /*****************************************
     * @method loadCountries()
     * @description loads a list of countries
     ****************************************/
    private loadCountries() {
        from(<PromiseLike<Array<ICountry>>> this.countryStateService.getCountryList(false)).subscribe(data => {
            this.countries = data;
            this.country.setValue( 1);
            this.loadStatesByCountryId();
        },
        err => this.msg.showWebApiException(err));
    }

    /***********************************************
     * @method zipCodePattern(countryCode:string)
     * @description sets the regexp based on country
     **********************************************/
    private setZipCodePattern(country:ICountry) {

        if (this.country && country.countryId != null) {
            if (country.countryId == 1)
                this.zipCodePattern = /^\d{5}(-\d{4})?$/;
            else if (country.countryId == 7)
                this.zipCodePattern = /^([ABCEGHJKLMNPRSTVXY]|[abceghjklmnprstvxy]){1}\d{1}([ABCEGHJKLMNPRSTVWXYZ]|[abceghjklmnprstvwxyz]){1}( |-){0,1}\d{1}([ABCEGHJKLMNPRSTVWXYZ]|[abceghjklmnprstvwxyz]){1}\d{1}$/;
            else
                this.zipCodePattern = /[\w -]+/;
        }
        else
            this.zipCodePattern = /[\w -]+/;

        this.zip.setValidators([Validators.pattern(this.zipCodePattern)]);
    }

    /****************************************************
     * @method loadStatesByCountryId
     * @description loads a list of states by couuntry id
     ****************************************************/
    private loadStatesByCountryId() {
        if (!!this.country.value) {
            // change the zip code pattern according to the country selection
            let countrySelected   = this.countries.find( obj => obj.countryId == this.country.value);
            this.setZipCodePattern(countrySelected);

            this.zip.updateValueAndValidity();

            from(<PromiseLike<Array<IState>>> this.countryStateService.getStatesByCountry(this.country.value, false)).subscribe(data => {
                this.states            = data;

                if(!!this.states && this.states.length >0 ){
                    let chosen             = this.states.find(x => x.stateId == 1);
                    if(!chosen) chosen     = this.states[0];
                    this.state.setValue( chosen.stateId);
                } else {
                    this.state.setValue( 1);
                }
            },
            err => this.msg.showWebApiException(err));
        }
    }

    public cancel() {
        if(this.stepper.selectedIndex == 0)
        {
            this.confirm.show(null, {
                bodyText: "Are you sure you want to cancel?",
                closeButtonText: "No",
                closeButtonClass: "btn-cancel",
                actionButtonText: "Yes"
            })
            .then(() => {
                window.location.href = "ApplicantPostingListNL.aspx?code=" + this.clientCode + '&posting=' + this.postingId;
            });
        }
        else
        {
            if(this.stepper.selectedIndex == 2)
                this.gotoStep(1);
            else
                this.gotoStep(0);
        }
    }

    public loadStep(evt:StepperSelectionEvent){
        this.formSubmitted = false;
        switch(evt.selectedIndex){
            case 0: this.selected = this.accountState; break;
            case 1: this.selected = this.contactState; break;
            case 2: this.selected = this.addressState; break;
        }
    }

    public gotoStep(stepIndex:number){
        this.formSubmitted = false;
        switch(stepIndex){
            case 0:
                this.selected = this.accountState;
                this.stepper.selectedIndex = 0;
                break;
            case 1:
                this.selected = this.contactState;
                this.stepper.selectedIndex = 1;
                break;
            case 2:
                this.selected = this.addressState;
                this.stepper.selectedIndex = 2;
        }
    }

    public save() {
        this.formSubmitted = true;
        if (!this.noSave) {
            // This check should happen in all steps as the username is validated by async webapi
            this.checkAccountFormValidity();

            if(this.stepper.selectedIndex == 2){
                this.form1.updateValueAndValidity();
                this.form2.updateValueAndValidity();
                this.form3.updateValueAndValidity();
                if (this.form1.invalid) {this.gotoStep(0); this.formSubmitted = true;return;}
                if (this.form2.invalid) {this.gotoStep(1); this.formSubmitted = true;return;}
                if (this.form3.invalid) return;

                // Save
                this.prepareModel();
                this.msg.sending(true);

                this.applicant.clientId     = this.clientId;
                this.applicant.postingId    = this.postingId;
                this.applicant.clientCode   = this.clientCode;
                this.applicant.address1     = this.applicant.address1 ? this.applicant.address1 : "";
                this.applicant.city         = this.applicant.city ?     this.applicant.city     : "";
                this.applicant.state        = this.applicant.state ?    this.applicant.state    : 1;
                this.applicant.countryId    = this.applicant.countryId ?    this.applicant.countryId    : 1;
                this.applicant.zipCode      = this.applicant.zipCode ?  this.applicant.zipCode  : "";

                /* generate the link we need to return the applicant to after creating the new user */

                /* start a new application and get link*/
                let link = this.document.URL;
                link = link.substr(0, link.lastIndexOf('/')) +
                    ('/applicantPostingListNL.aspx?code=' + this.clientCode + '&posting=' + this.postingId);
                this.applicant.postLink = link;

                this.signupService.saveNewUser(this.applicant).subscribe(x => {
                        this.msg.sending(false);
                        this.msg.setTemporarySuccessMessage("Account created. Redirecting you shortly.");
                        setTimeout(() => window.location.href = x.postLink, 1000);
                },
                err => this.msg.showWebApiException(err));

            } else {
                if(this.stepper.selectedIndex == 0){
                    this.form1.updateValueAndValidity();
                    if (this.form1.invalid) return;

                    this.gotoStep(1);
                }
                else{
                    this.form2.updateValueAndValidity();
                    if (this.form2.invalid) return;

                    this.gotoStep(2);
                }
            }
        }
    }

    checkAccountFormValidity(){
        this.form1.updateValueAndValidity();
        if (this.form1.invalid) {this.gotoStep(0); this.formSubmitted = true;return;}
    }

    getFormControlError(f:FormGroup, field:string, errorCodes:string[]):boolean {
        const control = f.get(field);
        let flag: boolean = false;
        _.forEach(errorCodes, (errorCode) => {
            flag = control.hasError(errorCode) && (control.touched || this.formSubmitted);
            if (flag === true)
                return false;
        });
        return flag;
    }
    getUserNameControlError(f:FormGroup):boolean {
        let errorCode:string = 'required';
        let errorCodePattern:string = 'pattern';
        const uniqueCntrl = f.get('userNameUnique');
        const unameCntrl = f.get('userName');
        let flag: boolean = false;

        if(  (uniqueCntrl.hasError(errorCode) || unameCntrl.hasError(errorCode) || unameCntrl.hasError(errorCodePattern)) && 
            (unameCntrl.touched || this.formSubmitted) )
            return true;

        return flag;
    }

    private prepareModel(): void {
        this.applicant.applicantId                 = 0;
        this.applicant.userName                    = this.form1.value.userName;
        this.applicant.password                    = this.form1.value.password;
        this.applicant.confirmPassword             = this.form1.value.passwordConfirm;
        this.applicant.email                       = this.form1.value.email;

        this.applicant.firstName                   = this.form2.value.firstName;
        this.applicant.middleInitial               = this.form2.value.mi;
        this.applicant.lastName                    = this.form2.value.lastName;
        this.applicant.phone                       = this.form2.value.primaryPhone;
        this.applicant.cellPhone                   = this.form2.value.secondaryPhone;
        this.applicant.isTextEnabled               = this.form2.value.isTextEnabled ? this.form2.value.isTextEnabled : false;
        this.applicant.workPhone                   = this.form2.value.workPhone;
        this.applicant.extension                   = this.form2.value.workExtension;

        this.applicant.address1                    = this.form3.value.address1;
        this.applicant.address2                    = this.form3.value.address2;
        this.applicant.city                        = this.form3.value.city;
        this.applicant.zipCode                     = this.form3.value.zip;
        this.applicant.countryId                   = this.form3.value.country;
        this.applicant.state                       = this.form3.value.state;
    }

    /**************
     * @method chkUsername
     * @description This method checks if the user name already exists
     * @returns isValidUsername boolean based on whether username exists
    */
    private chkUsername():void {
        this.userNameUnique.setValue('true');
        if(!this.form1.value.userName ) return;
        this.signupService.validateUsername(this.form1.value.userName)
        .subscribe(x => {
            if(!x.isValid) this.userNameUnique.setValue('');
        });
    }
}

import {
  Component,
  OnInit,
  Input,
  NgModule,
  ViewChild,
  ElementRef,
} from "@angular/core";
import { IContactPreferenceInfo } from "../shared/models/contact-preference-info.model";
import { NotificationPreferenceApiService } from "../shared/notification-preference-api.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { HttpErrorResponse } from "@angular/common/http";
import { IContactNotificationPreference } from "../shared/models/contact-notification-preference.model";
import { DsCoreFormsModule } from "@ds/core/ui/forms/forms.module";
import { NgForm } from "@angular/forms";
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from '@ds/core/shared';
import { Observable, Subject } from 'rxjs';
import { switchMap, takeUntil, tap } from 'rxjs/operators';

@Component({
  selector: "ds-contact-notification-preferences-form",
  templateUrl: "./contact-notification-preferences-form.component.html",
  styleUrls: ["./contact-notification-preferences-form.component.scss"],
})
export class ContactNotificationPreferencesFormComponent implements OnInit {

  destroy$ = new Subject();
  private _userInfo: UserInfo;
  userInfo$ = this.account.getUserInfo()
    .pipe(takeUntil(this.destroy$), tap(u => this._userInfo = u));

  contactPreferenceInfo: IContactPreferenceInfo;
  isLoading: boolean = true;
  isNewEmailSelected: boolean;
  isNewPhoneSelected: boolean = false;
  newEmailAddress: string;
  newPhoneNumber: string;
  selectAllEmailIsChecked: boolean;
  selectAllTextIsChecked: boolean;

  columnsToDisplay = ["notificationType", "preferenceEmail", "preferenceText"];

  constructor(
    private apiSvc: NotificationPreferenceApiService,
    private msg: DsMsgService,
    private account: AccountService,
  ) {}

  ngOnInit() {
    this.userInfo$.pipe(
      switchMap(u => this.apiSvc.getContactNotificationPreferences(
        u.selectedClientId(),
        u.userId,
        null
      )),
    )
      .subscribe(data => {
        this.isLoading = false;
        this.newEmailAddress = "";
        this.newPhoneNumber = "";
        this.contactPreferenceInfo = data;

        // Check if the email preference is selected for ALL notification types
        // If so, selects the "select all" email check box
        this.isAllEmailSelected();

        // Check if the text preference is selected for ALL notification types
        // If so, selects the "select all" text check box
        this.isAllTextSelected();
      });
  }

  // Functionality when an email radio button is selected
  emailSelectionChanged(event: any) {
    // Determine if "new email" is selected
    this.isNewEmailSelected = event.target.value == "addEmail";

    // If "new email" is not selected:
    //   - set the selected email as the contact info email
    //   - clear the new email value
    if (!this.isNewEmailSelected) {
      this.contactPreferenceInfo.contactInfo.email = event.target.value;
      this.newEmailAddress = "";
    }
  }

  // Functionality when a phone number radio button is selected
  phoneNumberSelectionChanged(event: any) {
    // Determine if "new phone #" is selected
    this.isNewPhoneSelected = event.target.value == "addPhone";

    // If "new phone #" is not selected:
    //   - set the selected phone # as the contact info phone #
    //   - clear the new phone number value
    if (!this.isNewPhoneSelected) {
      this.contactPreferenceInfo.contactInfo.phoneNumber = event.target.value;
      this.newPhoneNumber = "";
    }
  }

  save(form: NgForm) {
    if (form.invalid) return;

    // If "new email" is selected, set the new email as the contact info email
    if (this.isNewEmailSelected)
      this.contactPreferenceInfo.contactInfo.email = this.newEmailAddress;

    // if "new phone number" is selected, set the new phone number as the contact info phone number
    if (this.isNewPhoneSelected)
      this.contactPreferenceInfo.contactInfo.phoneNumber = this.newPhoneNumber;

    if (
      this.contactPreferenceInfo.contactInfo.email == null ||
      this.contactPreferenceInfo.contactInfo.email.length == 0
    )
      this.contactPreferenceInfo.contactInfo.email = this.contactPreferenceInfo.emailAddresses.find(
        (e) => e.isPreferred
      ).emailAddress;

    this.apiSvc
      .saveContactNotificationPreferences(this.contactPreferenceInfo)
      .subscribe(
        (data) => {
          this.contactPreferenceInfo = data;

          // If "new email" was selected, clear the info for "new email"
          if (this.isNewEmailSelected) {
            this.newEmailAddress = "";
            this.isNewEmailSelected = false;
          }

          // If "new phone number" was selected, clear the info for "new phone number"
          if (this.isNewPhoneSelected) {
            this.newPhoneNumber = "";
            this.isNewPhoneSelected = false;
          }
          this.msg.setTemporarySuccessMessage("Preferences successfully saved");
        },
        (error: HttpErrorResponse) => {
          this.msg.showWebApiException(error.error);
        }
      );
  }

  // Add focus to "new email" text box
  @ViewChild("email", { static: false }) emailField: ElementRef;
  focusEmailOnRadio(): void {
    this.emailField.nativeElement.focus();
  }

  // Add focus to "new Phone number" text box
  @ViewChild("text", { static: false }) textField: ElementRef;
  focusTextOnRadio(): void {
    this.textField.nativeElement.focus();
  }

  // Select the "new email" radio button when "new email" text box takes focus
  newEmailFocused() {
    this.isNewEmailSelected = true;
  }

  // Select the "new phone number" radio button when "new phone number" text box takes focus
  newPhoneFocused() {
    this.isNewPhoneSelected = true;
  }

  // Selects or deselects the email preference for ALL notification types
  selectAllEmail() {
    this.selectAllEmailIsChecked = !this.selectAllEmailIsChecked;

    for (let preference of this.contactPreferenceInfo.preferences) {
      preference.sendEmail = this.selectAllEmailIsChecked;
    }
  }

  // Selects or deselects the text preference for ALL notification types
  selectAllText() {
    this.selectAllTextIsChecked = !this.selectAllTextIsChecked;

    for (let preference of this.contactPreferenceInfo.preferences) {
      preference.sendSms = this.selectAllTextIsChecked;
    }
  }

  // Determines if the email preference is selected for ALL notification types
  isAllEmailSelected() {
    // Check if there are any preferences
    if (this.hasPreferences()) {
      this.selectAllEmailIsChecked = this.contactPreferenceInfo.preferences.every(
        function (preference: IContactNotificationPreference) {
          return preference.sendEmail == true;
        }
      );
    } else {
      this.selectAllEmailIsChecked = false;
    }
  }

  // Determines if the text preference is selected for ALL notification types
  isAllTextSelected() {
    if (this.hasPreferences()) {
      this.selectAllTextIsChecked = this.contactPreferenceInfo.preferences.every(
        function (preference: IContactNotificationPreference) {
          return preference.sendSms == true;
        }
      );
    } else {
      this.selectAllTextIsChecked = false;
    }
  }

  // Determines if the text preferences should be disabled
  isTextDisabled() {
    // Disable when "new phone number" is not selected and the contact info does not have a phone number set
    return (
      !this.isNewPhoneSelected &&
      (this.contactPreferenceInfo.contactInfo.phoneNumber == null ||
        this.contactPreferenceInfo.contactInfo.phoneNumber.length == 0)
    );
  }

  hasPreferences() {
    return (
      this.contactPreferenceInfo != null &&
      this.contactPreferenceInfo.preferences != null &&
      this.contactPreferenceInfo.preferences.length > 0
    );
  }
}

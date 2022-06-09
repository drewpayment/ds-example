import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserSettingsInput } from '@ds/core/employee-services/models';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
    selector: 'ds-user-settings',
    templateUrl: './user-settings.component.html',
    styleUrls: ['./user-settings.component.scss']
})
export class UserSettingsComponent implements OnInit {

    form: FormGroup = this.createForm();
    user: UserInfo;
    isLoading = true;
    changeRequestRequired = false;

    constructor(private fb: FormBuilder, private accountService: AccountService, private sb: MatSnackBar) { }

    ngOnInit() {
        this.accountService.getUserInfo().subscribe(u => {
            this.user = u;

            if (this.user) {
                this.accountService.getUserProfileSettingsInfo(this.user.lastClientId, this.user.userId)
                    .subscribe(user => {
                        const dto: UserSettingsInput = {
                            username: user.userName,
                            firstName: user.firstName,
                            lastName: user.lastName,
                            email: user.emailAddress,
                            changeRequestRequired: user.changeRequestRequired
                        };

                        this.changeRequestRequired = dto.changeRequestRequired;
                        this.patchForm(dto);
                        this.isLoading = false;
                    });
            }
        });
    }

    saveForm() {
        if (!this.form.valid) return;

        const model: UserSettingsInput = {
            username: this.form.controls.username.value,
            firstName: this.form.controls.firstName.value,
            lastName: this.form.controls.lastName.value,
            email: this.form.value.email,
            changeRequestRequired: this.changeRequestRequired
        };

        this.accountService.saveUserProfileSettingsInfo(this.user.selectedClientId(), this.user.userId, model)
            .subscribe((res: any) => {
                if(this.changeRequestRequired == true){
                    this.form.get('email').setValue(this.user.emailAddress);
                    this.sb.open('Change request submitted!', 'dismiss', {duration: 3000});
                } else{
                    this.form.get('email').setValue(res.email);
                    this.sb.open('Saved!', 'dismiss', {duration: 3000});
                }

                // if the update was successful, we are going to update the email address on the cached user info
                // for the logged in user's session so that the next api call makes sure to get the updated email address
                // on all pages throughout the angular app
                if (res && this.changeRequestRequired != true) {
                  this.accountService.getUserInfo(true).subscribe();
                    // const cachedUser = this.accountService.cachedUser$.getValue();
                    // cachedUser.emailAddress = res.email;
                    // this.accountService.cachedUser$.next(cachedUser);
                }
            });
    }

    private createForm(): FormGroup {
        return this.fb.group({
            username: this.fb.control({ value: '', disabled: true }),
            firstName: this.fb.control({ value: '', disabled: true }),
            lastName: this.fb.control({ value: '', disabled: true }),
            email: this.fb.control('', [Validators.required, Validators.email])
        });
    }

    private patchForm(v: UserSettingsInput) {
        this.form.patchValue({
            username: v.username,
            firstName: v.firstName,
            lastName: v.lastName,
            email: v.email,
            changeRequestRequired: v.changeRequestRequired
        });
    }

}

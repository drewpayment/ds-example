import { Routes } from '@angular/router';
import { ScheduleComponent } from './schedule/schedule.component';
import { ScheduleDetailsComponent } from './schedule-details/schedule-details.component';
import { PunchCardComponent } from './punch-card/punch-card.component';
import { ProfileComponent } from './profile/profile.component';
import { UserSettingsComponent } from './user-settings/user-settings.component';
import { EmergencyContactsComponent } from './emergency-contacts/emergency-contacts.component';
import { emergencyContactUpdatePageMatcher, dependentUpdatePageMatcher } from './shared/router-matchers';
import { UpdateEmergencyContactComponent } from './update-emergency-contact/update-emergency-contact.component';
import { DependentComponent } from './dependents/dependent/dependent.component';
import { DependentDetailComponent } from './dependents/dependent-detail/dependent-detail.component';
import { TaxesComponent } from './taxes/taxes.component';
import { TaxEditComponent } from './tax-edit/tax-edit.component';
import { ContactInformationComponent } from './contact-information/contact-information.component';
import { PaychecksSummaryComponent } from './paychecks/paychecks-summary/paychecks-summary.component';
import { PaychecksDetailsComponent } from './paychecks/paychecks-details/paychecks-details.component';
import { TimeOffComponent } from './time-off/time-off.component';
import { FederalW4FormComponent } from './federal-w4-form/federal-w4-form.component';


export const routes: Routes = [

    // { path: 'home', component: HomeComponent, pathMatch: 'full' },
    { 
        path: 'schedule', 
        children: [
            { path: '', component: ScheduleComponent, pathMatch: 'full' },
            { path: 'details/:startDate', component: ScheduleDetailsComponent },
            { path: 'details/:startDate/:endDate', component: ScheduleDetailsComponent },
        ]
    },
    { path: 'punch-card', component: PunchCardComponent },
    {
        path: 'profile',
        children: [
            { path: '', component: ProfileComponent, pathMatch: 'full' },
            { path: 'settings', component: UserSettingsComponent },
            { 
                path: 'emergency-contacts', 
                children: [
                    { path: '', component: EmergencyContactsComponent, pathMatch: 'full' },
                    { matcher: emergencyContactUpdatePageMatcher, component: UpdateEmergencyContactComponent }
                ]
            },
            {
                path: 'dependents',
                children: [
                    { path: '', component: DependentComponent, pathMatch: 'full' },
                    { matcher: dependentUpdatePageMatcher, component: DependentDetailComponent }
                ]
            },
            {
                path: 'taxes',
                children: [
                    { path: '', component: TaxesComponent, pathMatch: 'full' },
                    { path: 'edit/:id', component: TaxEditComponent },
                    { path: 'w4/:id', component: FederalW4FormComponent }
                ]
            },
            {
                path: 'contact-information',
                component: ContactInformationComponent
            }
        ]
    },
    {
        path: 'paycheck',
        children: [
            { path: '', component: PaychecksSummaryComponent, pathMatch: 'full' },
            { path: 'details/:id', component: PaychecksDetailsComponent }
        ]
    },
    { path: 'time-off', component: TimeOffComponent },
    
    // BASE ROUTES
    // { path: 'demo', component: HomeComponent },
    // { path: '**', redirectTo: 'punch-card' },
];

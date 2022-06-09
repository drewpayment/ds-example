import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProfileOutletComponent } from './profile-outlet/profile-outlet.component';
import { EmployeesModule } from '@ds/employees';
import { HttpClientModule } from '@angular/common/http';
import { ProfileAppRoutingModule } from './profile-app-routing.module';
import { MenuWrapperHeaderModule } from '@ds/core/ui/menu-wrapper-header/menu-wrapper-header.module';
import { OnboardingGuard } from '../onboarding.guard';

@NgModule({
  declarations: [ProfileOutletComponent],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,

    EmployeesModule,
    MenuWrapperHeaderModule,

    ProfileAppRoutingModule,
  ],
})
export class ProfileAppModule {}

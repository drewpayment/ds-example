import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SignUpComponent } from '@ds/applicants/newuser/signup.component';
import { ApplicantsModule } from '@ds/applicants/applicants.module';

@NgModule({
  imports: [
    CommonModule,
    ApplicantsModule,
  ],
  declarations: [],
  entryComponents:[
    SignUpComponent,
  ]
})
export class ApplicantsAppModule { }

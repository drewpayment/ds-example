import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsExpansionModule } from '@ds/core/ui/ds-expansion';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { EventsPageComponent } from './../events/events-page.component';
import { EventListComponent } from './../events/event-list.component';
import { EventFormComponent } from './../events/event-form/event-form.component';
import { ManageTopicsDialogComponent } from './../events/manage-topics/manage-topics-dialog.component';
import { EmployeesModule } from '../employees.module';
import { GroupEventFormComponent } from './group-event-form.component';

export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};

@NgModule({
    imports: [
        BrowserModule,
        MaterialModule,
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        DsExpansionModule,
        DsCardModule,
        LoadingMessageModule,
        DsCoreFormsModule,
        EmployeesModule,
        
        NgxMaskModule.forRoot(options)
  ],
  declarations: [EventListComponent, EventFormComponent, EventsPageComponent, ManageTopicsDialogComponent, GroupEventFormComponent],
    entryComponents: [ManageTopicsDialogComponent,],
    exports: [EventListComponent, EventFormComponent, GroupEventFormComponent]
})
export class DsEventsModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule, UrlSerializer } from '@angular/router';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NotesListOutletComponent } from './notes-list-outlet/notes-list-outlet.component';
import { EmployeesModule } from '@ds/employees';
import { EmployeeNotesModule } from '@ds/core/employees/employee-notes/employee-notes.module';
import { HeaderComponent } from '@ds/core/ui/menu-wrapper-header/header/header.component';
import { MenuWrapperHeaderModule } from '@ds/core/ui/menu-wrapper-header/menu-wrapper-header.module';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { LowerCaseUrlSerializer } from '@ds/core/utilities';

export const NotesRoutes: Routes = [
  {
    path: 'notes',
    children: [
      { path: '', component: NotesListOutletComponent },
      { path: '', component: HeaderComponent, outlet: 'header' },
    ],
  },
];

@NgModule({
  declarations: [NotesListOutletComponent],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    EmployeesModule,
    EmployeeNotesModule,
    MenuWrapperHeaderModule,

    RouterModule.forChild(NotesRoutes),
  ],
  exports: [RouterModule],
  providers: [
    {
      provide: UrlSerializer,
      useClass: LowerCaseUrlSerializer,
    },
  ],
})
export class NotesAppModule {}

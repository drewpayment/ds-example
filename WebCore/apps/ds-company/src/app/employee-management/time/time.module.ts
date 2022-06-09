import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material';
import { TimeCardComponent } from './time-card/time-card.component';
import { RouterModule, Route } from '@angular/router';

const routes: Route[] = [
  {
    path: '',
    component: TimeCardComponent,
  }
];

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,

    RouterModule.forChild(routes),
  ],
  declarations: [
    TimeCardComponent,
  ],
  exports: [
    TimeCardComponent,
  ]
})
export class TimeModule {}

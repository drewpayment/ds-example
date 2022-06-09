import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { Route } from '@angular/router';
import { MaterialModule } from '../material';
import { FourOhFourComponent } from './four-oh-four.component';

export const FourOhFourRoutes: Route[] = [
  {
    path: 'four-oh-four',
    component: FourOhFourComponent,
  },
  {
    path: '**',
    redirectTo: '/four-oh-four',
  },
];

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
  ],
  declarations: [
    FourOhFourComponent,
  ],
})
export class FourOhFourModule {}
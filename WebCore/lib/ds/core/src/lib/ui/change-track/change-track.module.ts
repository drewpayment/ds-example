import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TrackChangesDirective } from '@ds/core/ui/change-track/track-changes.directive';

@NgModule({
  declarations: [
      TrackChangesDirective
  ],
  imports: [
    CommonModule
  ],
  exports: [
      TrackChangesDirective
  ]
})
export class ChangeTrackModule { }

import { AfterViewInit } from "@angular/core";
import { Directive, Self, Optional, OnInit, OnDestroy } from "@angular/core";
import { ControlContainer, NgForm } from "@angular/forms";
import { Observable, Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { ChangeTrackerService } from "./change-tracker.service";
import { dirtyCheck } from "./dirty-check";

@Directive({
  selector: "[dsTrackFormChanges]",
})
export class TrackFormChangesDirective
  implements OnInit, OnDestroy, AfterViewInit
{
  isDirty: boolean;
  destroy$ = new Subject();

  constructor(
    @Optional() @Self() private container: ControlContainer,
    @Optional() @Self() private ngForm: NgForm,
    private changeTrackerService: ChangeTrackerService
  ) {}

  ngOnInit(): void {}

  ngOnDestroy(): void {
    this.destroy$.next();
  }

  ngAfterViewInit() {
    if (this.ngForm != null)
      dirtyCheck(this.ngForm.form, { withDisabled: true, debounce: 300 })
        .pipe(takeUntil(this.destroy$))
        .subscribe((dirty) => (this.changeTrackerService.setIsDirty(dirty)));
    if (this.container != null)
      dirtyCheck(this.container.control, { withDisabled: true, debounce: 300 })
        .pipe(takeUntil(this.destroy$))
        .subscribe((dirty) => (this.changeTrackerService.setIsDirty(dirty)));
  }
}

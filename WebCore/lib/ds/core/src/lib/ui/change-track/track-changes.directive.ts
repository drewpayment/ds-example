import { Directive, Input, OnInit, OnDestroy } from '@angular/core';
import { ComponentCanDeactivate } from './shared/component-can-deactivate';
import { DeactivatorService } from '@ds/core/ui/change-track/shared/deactivator.service';

@Directive({
  selector: '[dsTrackChanges]'
})
export class TrackChangesDirective extends ComponentCanDeactivate implements OnInit, OnDestroy {
    
    @Input()
    hasChanges = false;
    
    get canDeactivate() {
        return !this.hasChanges;
    };

    constructor(private deactivator: DeactivatorService) {
        super();
    }

    ngOnInit(): void {
        this.deactivator.registerComponent(this);
    }
    ngOnDestroy(): void {
        this.deactivator.unregisterComponent(this);
    }

}

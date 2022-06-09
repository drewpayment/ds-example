import { Directive, ElementRef, Injector, Input } from "@angular/core";
import { UpgradeComponent } from "@angular/upgrade/static";

@Directive({
    selector: 'employee-list-nav'
})
export class EmployeeListNavBreadcrumbDirective extends UpgradeComponent {
    @Input() pageTitle:string;
    @Input() forceReturnUrl:boolean;
    constructor(elementRef:ElementRef, injector:Injector) {
        super('employeeListNav', elementRef, injector);
    }
}
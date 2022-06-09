import { Component, OnInit, Input, OnChanges, SimpleChanges, HostListener, Inject } from '@angular/core';
import { APP_CONFIG, AppConfig } from '@ds/core/app-config/app-config';
import { Router } from '@angular/router';
import { UrlParts } from '@ds/core/shared';

@Component({
    selector: 'ds-ngx-link',
    templateUrl: './ngx-link.component.html',
    styleUrls: ['./ngx-link.component.scss']
})
export class NgxLinkComponent implements OnChanges {
    
    

    private _link:string;
    @Input()
    get link():string {
        return this._link;
    }
    set link(value:string) {
        // console.log(value);
        // this._link = "/"+UrlParts.ParseUrl(value).parts.slice(1).join("/");
        // console.log(this._link);
        this._link = value;
    }
    
    @Input('active') isActive:boolean = false;
    
    constructor() { }

    ngOnChanges(changes:SimpleChanges) {
        if(changes.isActive && !changes.isActive.isFirstChange) {
            this.isActive = changes.isActive.currentValue;
        }
    }

    

}

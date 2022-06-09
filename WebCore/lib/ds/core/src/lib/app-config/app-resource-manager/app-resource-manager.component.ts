import { Component, OnInit } from '@angular/core';
import { AppConfigApiService, IApplicationResource } from '../shared';
import { AppResourceDialogService } from '../app-resource-dialog/app-resource-dialog.service';


class ResourceVm {
    resourceTypeName: string;
    applicationSourceName: string;
    constructor(
        public raw: IApplicationResource,
        private apiSvc: AppConfigApiService,
        private resourceDialogSvc: AppResourceDialogService) {

        this.updateDisplayNames();
    }

    edit() {
        let dialog = this.resourceDialogSvc.open(this.raw);

        dialog.afterClosed().subscribe(result => {
            if (result && result.resource) {
                this.raw = result.resource;
                this.updateDisplayNames();
            }
        });
    }

    private updateDisplayNames() {
        this.resourceTypeName = this.apiSvc.getApplicationResourceTypeName(this.raw.resourceType);
        this.applicationSourceName = this.apiSvc.getApplicationSourceTypeName(this.raw.applicationSourceType);
    }
}

@Component({
  selector: 'ds-app-resource-manager',
  templateUrl: './app-resource-manager.component.html',
  styleUrls: ['./app-resource-manager.component.scss']
})
export class AppResourceManagerComponent implements OnInit {

    resources: ResourceVm[] = [];
    filteredResources: ResourceVm[] = [];
    searchText: string;
    constructor(
        private apiSvc: AppConfigApiService,
        private resourceDialogSvc: AppResourceDialogService
    ) { }

    ngOnInit() {
        this.refreshResourceList();
    }

    addResource() {
        let dialog = this.resourceDialogSvc.open(null);
        dialog.afterClosed().subscribe(result => {
            if (result && result.resource)
                this.refreshResourceList();
        })
    }

    refreshResourceList() {
        this.apiSvc.getApplicationResources().subscribe(resources => {
            this.resources = resources.map(r => new ResourceVm(r, this.apiSvc, this.resourceDialogSvc));
            this.filterResources();
        });
    }

    filterResources() {
        if (this.searchText) {
            this.filteredResources = this.resources.filter(r => {
                let lower = this.searchText.toLowerCase();
                return r.raw.name.toLowerCase().includes(lower) || r.raw.routeUrl.toLowerCase().includes(lower);
            });
        } 
        else {
            this.filteredResources = this.resources;
        }
    }
}

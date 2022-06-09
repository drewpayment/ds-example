import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IApplicationResource } from './application-resource.model';
import { ApplicationResourceType } from './application-resource-type.enum';
import { ApplicationSourceType } from './application-source-type.enum';
import { of } from 'rxjs';
import { IMenu } from './menu.model';
import { INavItem } from './nav-item.model';

@Injectable({
    providedIn: 'root'
})
export class AppConfigApiService {

    private resourceTypes: { resourceType: ApplicationResourceType, name: string }[] = [
        { resourceType: ApplicationResourceType.WebPage, name: "Page" }
    ];

    private applicationSources: { sourceType: ApplicationSourceType, name: string }[] = [
        { sourceType: ApplicationSourceType.SourceWeb, name: "Source" },
        { sourceType: ApplicationSourceType.EssWeb, name: "ESS" },
        { sourceType: ApplicationSourceType.CompanyWeb, name: "Company" },
        { sourceType: ApplicationSourceType.AdminWeb, name: "Admin" }
    ];

    API_BASE = 'api/app-config';

    constructor(private http: HttpClient) { }

    getApplicationResources() {
        return this.http.get<IApplicationResource[]>(`${this.API_BASE}/resources`);
    }

    saveApplicationResource(resource: IApplicationResource) {
        let url = `${this.API_BASE}/resources`;
        if (resource.resourceId)
            url += `/${resource.resourceId}`;

        return this.http.post<IApplicationResource>(url, resource);
    }

    getApplicationSourceTypes() {
        return of(this.applicationSources);
    }

    getApplicationResourceTypes() {
        return of(this.resourceTypes);
    }

    getApplicationSourceTypeName(type: ApplicationSourceType) {
        let info = this.applicationSources.find(s => s.sourceType === type);
        return info.name;
    }

    getApplicationResourceTypeName(type: ApplicationResourceType) {
        let info = this.resourceTypes.find(s => s.resourceType === type);
        return info.name;
    }

    getMenus() {
        return this.http.get<IMenu[]>(`${this.API_BASE}/menus`);
    }

    getNavMenu(menuId : number) {
        let url = `${this.API_BASE}/nav/menus`;

        if (menuId) {
            url += `/${menuId}`;
        }

        return this.http.get<INavItem[]>(url);
    }

    saveMenu(menu: IMenu) {
        let url = `${this.API_BASE}/menus`;
        if (menu.menuId)
            url += `/${menu.menuId}`;

        return this.http.post<IMenu>(url, menu); 
    }

    getSystemAdminMenu() {
        return this.http.get<IMenu[]>(`${this.API_BASE}/menus/systemAdmin`);
    }
}

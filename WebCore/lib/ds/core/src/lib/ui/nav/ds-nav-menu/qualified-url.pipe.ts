import { PipeTransform, Pipe } from '@angular/core';
import { ConfigUrl, ConfigUrlType } from '@ds/core/shared/config-url.model';
import { IMenuItem, ApplicationSourceType } from '@ds/core/app-config';
import { UrlParts } from '@ds/core/shared';


@Pipe({
    name: 'qualifiedUrl',
})
export class QualifiedUrlPipe implements PipeTransform {

    transform(urls: ConfigUrl[], item: IMenuItem) {
        if (!item || !item.resource || !item.resource.routeUrl || !urls || !urls.length) return '';
        let url = '';

        if (item.isAngularRoute) {

            if (item.resource.routeUrl.includes('http')) {
                const currUrl = urls.find(u => u.siteType === (item.resource.applicationSourceType - 1));
                return item.resource.routeUrl.replace(currUrl.url, '').toLowerCase();
            } else {
                return item.resource.routeUrl.toLowerCase();
            }
        }

        if (item.resource.routeUrl.includes('http')) return item.resource.routeUrl;

        if (item.resource.applicationSourceType === ApplicationSourceType.CompanyWeb) {
            return item.resource.routeUrl;
        } else if (item.resource.applicationSourceType === ApplicationSourceType.SourceWeb) {
            url = urls.find(u => u.siteType === ConfigUrlType.Payroll).url;
            const urlParts = UrlParts.ParseUrl(url);
            return urlParts.href + '/' +  item.resource.routeUrl;
        } else if (item.resource.applicationSourceType === ApplicationSourceType.EssWeb) {
            url = urls.find(u => u.siteType === ConfigUrlType.Ess).url;
            url = url.charAt(url.length - 1) === '/' ? url : url + '/';
            return url + item.resource.routeUrl;
        }
    }

}

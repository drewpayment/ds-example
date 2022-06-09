import {
  Route,
  UrlHandlingStrategy,
  UrlSegment,
  UrlTree,
} from "@angular/router";
import { AjsCompanyComponent } from "./ajs.component";
import { HeaderComponent } from "@ds/core/ui/menu-wrapper-header/header/header.component";
import { PermissionErrorComponent } from "@ds/core/ui/permission-error/permission-error.component";
import { FourOhFourRoutes } from "@ds/core/ui/four-oh-four/four-oh-four.module";
import { UserType } from '@ajs/user';
import { DefaultComponent } from './employee-self-service/landing-page/default.component';
import { LandingPageGuard } from './guards/landing-page.guard';
import { Injectable } from '@angular/core';

@Injectable()
export class Ng1Ng2UrlHandlingStrategy implements UrlHandlingStrategy {
  shouldProcessUrl(url): boolean {
    console.log('Should process called for: ' + url);
    let segments: UrlSegment[] = [];

    if (typeof url === 'string') {
      const paths = url.split('/');
      segments = paths.map(
        (p) =>
          ({
            path: p,
          } as UrlSegment)
      );
    } else {
      segments = (url as UrlTree).root.segments;
    }

    return !isAngularJsUrl(segments);
  }

  extract(url: UrlTree) {
    return url;
  }

  merge(url: UrlTree, whole: UrlTree) {
    return url;
  }
}

export const routes: Route[] = [
  {
    matcher: isAngularJsUrl,
    component: AjsCompanyComponent,
  },
  {
    path: '',
    canActivate: [LandingPageGuard],
    data: {
      userTypes: [
        UserType.systemAdmin,
        UserType.companyAdmin,
        UserType.supervisor,
        UserType.employee,
        UserType.applicant,
      ],
    },
    component: DefaultComponent,
  },
  {
    path: '',
    component: HeaderComponent,
    outlet: 'header',
  },
  {
    path: 'error',
    component: PermissionErrorComponent
  },
  ...FourOhFourRoutes
];

/**
 * This function is used by the route matcher.
 * Angular JS routes are hardcoded in this function and should be removed as those
 * legacy components are converted into Angular components. Until then, this matcher will
 * check for the start of a URL that goes into a UI-Router trunk of our route tree and
 * then will trigger our Angular JS lazy loading to serve the Angular JS components.
 */
export function isAngularJsUrl(url: UrlSegment[]) {
  const angularJsUrls = [
    'onboarding',
    'organization',
    'leave-management',
    'labor',
    'jobprofiles',
    'jobprofiledetails',
  ];

  if (!url || !url.length) return null;

  let startingUrlSegmentIndex = 0;

  /**
   * If Angular decides to include your site's base href in the angular route, this logic will remove
   * it from the URL segments so that it doesn't break the logic or trying to match routes to our AJS routes.
   */
  const baseHrefTags = document.getElementsByTagName('base');
  if (baseHrefTags && baseHrefTags.length) {
    const baseHref = baseHrefTags[0].getAttribute('href').replace(/\//g, '');
    const baseUrlIndex = url.findIndex(
      (u) => u.path.trim().toLowerCase() == baseHref.trim().toLowerCase()
    );

    if (baseUrlIndex > -1) {
      startingUrlSegmentIndex++;
    }
  }

  for (let i = 0; i < angularJsUrls.length; i++) {
    const ajsUrl = angularJsUrls[i];
    if (url[startingUrlSegmentIndex].path.startsWith(ajsUrl)) {
      addTrailingForwardSlashIfNotExist();
      return { consumed: url };
    }
  }

  return null;

  function addTrailingForwardSlashIfNotExist() {
    if (url[url.length - 1].path && !url[url.length - 1].path.endsWith('/')) {
      url[url.length - 1].path = url[url.length - 1].path + '/';
    }
  }
}

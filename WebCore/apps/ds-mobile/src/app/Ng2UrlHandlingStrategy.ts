import { routes } from './routes';
import { UrlHandlingStrategy } from '@angular/router';

// combine all NGX routes here... 
const checkRoutes = routes;

export class Ng2UrlHandlingStrategy implements UrlHandlingStrategy {

    /**
     * Check to make sure the URL is a qualified route for the NGX router to activate with. If it is not, 
     * we are going to simply return false and the Angular Router will not process the request and 
     * no NGX components will render. 
     * 
     */
    shouldProcessUrl(url) {
        let shouldProcess = false;

        for (const route of checkRoutes) {
            if (shouldProcess) break;
            if (!route.path.length) continue;

            // check for child routes 
            if (route.children) {
                for (const childRoute of route.children) {
                    if (shouldProcess) break;
                    if (!childRoute.path.length) continue;

                    shouldProcess = url.toString().indexOf(childRoute.path) > -1;
                }
            }

            shouldProcess = url.toString().indexOf(route.path) > -1;
        }

        return shouldProcess;
    }

    extract(url) {
        return url;
    }

    merge(url, whole) {
        return url;
    }
}

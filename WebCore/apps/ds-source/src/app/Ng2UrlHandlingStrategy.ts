import { UrlHandlingStrategy } from "@angular/router";
import { routes } from "./performance/performance.module";
import { EmployeePerformanceRoutes } from "./performance/employees/employees-routing.module";
import { payrollRoutes } from "./payroll/payroll.module";
import { clientRoutes } from './client/client.module';
import { employeeRoutes } from './employee/employee.module';
import { laborRoutes } from './labor/labor-routing.module';
import { ArRoutes } from './admin/ar/ar.module';
import { AdminRoutes } from './admin/admin.module';
import { onboardingRoutes } from './onboarding/onboarding.module';


declare var angular: ng.IAngularStatic;

// combine all NGX routes here... 
const checkRoutes = routes.concat(EmployeePerformanceRoutes).concat(payrollRoutes)
    .concat(clientRoutes).concat(employeeRoutes).concat(laborRoutes).concat(ArRoutes).concat(AdminRoutes).concat(onboardingRoutes);

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
                    if (shouldProcess) break;
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
import { ReportScheduleService } from '@ajs/reportscheduling/reportschedule.service';

export function ReportScheduleServiceProviderFactory($injector: any) {
    return $injector.get(ReportScheduleService.SERVICE_NAME);
}
export const ReportScheduleServiceProvider = {
    provide: ReportScheduleService,
    useFactory: ReportScheduleServiceProviderFactory,
    deps: ['$injector']
};

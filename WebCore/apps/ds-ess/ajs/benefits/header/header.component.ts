import * as angular from 'angular';
export class BenefitsWorkflowHeaderComponent {

    static SELECTOR = 'benefitsHeader';
    static CONFIG: ng.IComponentOptions = {
        controller: BenefitsWorkflowHeaderComponent,
        template: require('./header.html'),
        bindings: {
            activeStep: '<',
            isSigned: '=',
            dateSigned: '='
        }
    };

    activeStep: number;
    isSigned: boolean;
    dateSigned: string;

    constructor() {
    }

    $onInit() {}
}

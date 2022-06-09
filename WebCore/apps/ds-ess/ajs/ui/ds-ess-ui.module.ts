import * as angular from "angular";
import { DsCoreModule } from "@ajs/core/ds-core.module";
import { DsCompletionDirective } from "./completion/ds-completion.directive";
import { DsInlineValidatedInputService } from "./form-validation/ds-inline-validated-input.service";
import { DsInlineValidatedInputDirective } from "./form-validation/input.directive";
import { DsInlineValidatedRadioDirective } from "./form-validation/radio.directive";
import { DsInlineValidatedSelectDirective } from "./form-validation/select.directive";
import { DsNavMenuDirective } from "./menu/ds-nav-menu.directive";
import { DsNavigationService } from "./nav/ds-navigation.service";
import { DsNavWorkflowFooterComponent } from "./workflow/ds-nav-workflow-footer.directive";
import { DsNavWorkflowDirective } from "./workflow/ds-nav-workflow.directive";
import { WorkflowService } from "./workflow/workflow.service";
import { EssMsgController } from './msg/msg.controller';

/**
 * @ngdoc module
 * @module ds.ess.ui
 * @requires ds.core
 *
 * @description
 * Contains definitions of core UI services & directives used in the ESS app.
 */
export module DsEssUiModule {
    export const AjsModule = angular.module('ds.ess.ui', [
        DsCoreModule.AjsModule.name
    ]);

    AjsModule.directive(DsCompletionDirective.DIRECTIVE_NAME, DsCompletionDirective.$instance());
    AjsModule.service(DsInlineValidatedInputService.SERVICE_NAME, DsInlineValidatedInputService);
    AjsModule.directive(DsInlineValidatedInputDirective.DIRECTIVE_NAME, DsInlineValidatedInputDirective.$instance());
    AjsModule.directive(DsInlineValidatedRadioDirective.DIRECTIVE_NAME, DsInlineValidatedRadioDirective.$instance());
    AjsModule.directive(DsInlineValidatedSelectDirective.DIRECTIVE_NAME, DsInlineValidatedSelectDirective.$instance());
    AjsModule.directive(DsNavMenuDirective.DIRECTIVE_NAME, DsNavMenuDirective.$instance());
    AjsModule.service(DsNavigationService.SERVICE_NAME, DsNavigationService);
    AjsModule.component(DsNavWorkflowFooterComponent.COMPONENT_NAME, DsNavWorkflowFooterComponent.COMPONENT_OPTIONS);
    AjsModule.component(EssMsgController.CONTROLLER_NAME, EssMsgController.CONFIG);
    AjsModule.directive(DsNavWorkflowDirective.DIRECTIVE_NAME, DsNavWorkflowDirective.$instance());
    AjsModule.service(WorkflowService.SERVICE_NAME, WorkflowService);
}

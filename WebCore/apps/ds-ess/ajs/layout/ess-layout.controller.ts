import { UserInfo } from '@ds/core/shared';
import {AccountService} from '@ds/core/account.service';
declare const document: Document;

export class EssLayoutController {
    static readonly controllerName = 'essLayoutComponent';
    static readonly $inject = ['$rootScope', AccountService.NGX_SERVICE_NAME];
    user: UserInfo;
    isLegacyWrapper = false;
    isOnboarding = false;
    isOnboardingPages = false;
    isLoading = true;

    constructor(
        private $rootScope: ng.IRootScopeService,
        private ngacct: AccountService,
    ) {
        this.$onInit();
    }

    $onInit() {
        this.checkMenuWrapperState();
        this.$rootScope.$on('$stateChangeSuccess', (event, toState, toParams, fromState, fromParams) => {
            this.checkMenuWrapperState();
        });
    }

    checkMenuWrapperState() {
        this.ngacct.hasMenuWrapperFeature().subscribe(hasFeature => {
            const url = document.location.href.trim().toLowerCase();
            this.isOnboarding = url.includes('onboarding');
            this.isOnboardingPages = url.includes('onboarding');
            this.isLegacyWrapper = !hasFeature || this.isOnboardingPages;
            this.isLoading = false;
        });
    }

}

import { DsConfigurationServiceProvider } from "@ajs/core/ds-configuration/ds-configuration.service";

/**
 * @description
 * Configuration settings for the Source application module.
 */
export function DsCompanyAppAjsConfig(

    $urlRouterProvider,
    $locationProvider,
    configProvider: DsConfigurationServiceProvider,
    $urlMatcherFactory: any
) {
    $urlMatcherFactory.caseInsensitive(true);
    $urlMatcherFactory.strictMode(false);

    // set base API URL used for all service calls
    configProvider.setApiBaseUrl('api');

    // set the application's name (used in each state's page title)
    configProvider.setApplicationName('DominionSource');

    //--------------------------------------------------------------------
    // DEFAULT URL/STATE
    //--------------------------------------------------------------------
    $urlRouterProvider.otherwise(($injector, $location) => {
      const $state = $injector.get('$state');
      const ng2UrlHandlingStrategy = $injector.get('ng2UrlHandlingStrategy');

      const url = $location.url();
      if (ng2UrlHandlingStrategy.shouldProcessUrl(url)) {
        $state.go('ngx');
      } else {
        $state.go('/Labor/Schedule/Group');
      }
    });

    //--------------------------------------------------------------------
    // setup HTML5 url mode ('site.com/view1') for browsers which support it
    // otherwise, fallback to hash-bang url mode ('site.com/#!/view1')
    //--------------------------------------------------------------------
    $locationProvider.html5Mode(true).hashPrefix('!');
}

DsCompanyAppAjsConfig.$inject = [
    '$urlRouterProvider',
    '$locationProvider',
    DsConfigurationServiceProvider.PROVIDER_NAME,
    '$urlMatcherFactoryProvider'
];

import { DsConfigurationServiceProvider } from "@ajs/core/ds-configuration/ds-configuration.service";

/**
 * @ngdoc config
 * @name ds.mobile.app 
 *
 * @description
 * Main configuration file for the angular application. 
 */
export class DsMobileAppModuleConfig {
    static $inject = [DsConfigurationServiceProvider.PROVIDER_NAME];
    static $instance = [...DsMobileAppModuleConfig.$inject, (configProv) => new DsMobileAppModuleConfig(configProv)]

    constructor(private configProv: DsConfigurationServiceProvider) {
        configProv.setApiBaseUrl('/../../api');
    }
}

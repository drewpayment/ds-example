import { DsConfigurationServiceProvider } from "../../../Scripts/ds/core/ds-configuration/ds-configuration.service";

/**
 * @ngdoc config
 * @name ds.source.app 
 *
 * @description
 * Main configuration file for the angular application. 
 */
export class DsSourceAppModuleConfig {
    static $inject = [DsConfigurationServiceProvider.PROVIDER_NAME];
    static $instance = [...DsSourceAppModuleConfig.$inject, (configProv) => new DsSourceAppModuleConfig(configProv)]

    constructor(private configProv: DsConfigurationServiceProvider) {
        configProv.setApiBaseUrl('api');
    }
}

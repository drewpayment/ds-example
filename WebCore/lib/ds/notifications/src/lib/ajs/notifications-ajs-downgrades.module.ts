import { downgradeComponent } from '@angular/upgrade/static';
import * as angular from 'angular';
import { ClientNotificationPreferencesFormComponent } from '../client-notification-preferences-form/client-notification-preferences-form.component';
import { ContactNotificationPreferencesFormComponent } from '../contact-notification-preferences-form/contact-notification-preferences-form.component';

export module DsNotificationsAjsDowngradeModule {
    export const AjsModule = angular
        .module('ds.notifications.downgrade', [])
        .directive(
            'dsClientNotificationPreferencesForm',
            downgradeComponent({ component: ClientNotificationPreferencesFormComponent }) as angular.IDirectiveFactory
        )
        .directive(
            'dsContactNotificationPreferencesForm',
            downgradeComponent({ component: ContactNotificationPreferencesFormComponent }) as angular.IDirectiveFactory
        );

}
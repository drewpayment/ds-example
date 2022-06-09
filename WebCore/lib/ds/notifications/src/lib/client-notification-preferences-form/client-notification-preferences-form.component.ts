import { Component, OnInit, Input } from '@angular/core';
import { INotificationPreferencesProductGroups } from '../shared/models/notification-preferences-product-groups.model';
import { IClientNotificationPreference } from '../shared/models/client-notification-preference.model';
import { NotificationPreferenceApiService } from '../shared/notification-preference-api.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { HttpErrorResponse } from '@angular/common/http';
import { checkboxComponent } from '@ajs/applicantTracking/application/inputComponents';

@Component({
    selector: 'ds-client-notification-preferences-form',
    templateUrl: './client-notification-preferences-form.component.html',
    styleUrls: ['./client-notification-preferences-form.component.scss']
})
export class ClientNotificationPreferencesFormComponent implements OnInit {

  @Input()
  clientId:number;
  preferencesProductGroups: INotificationPreferencesProductGroups[];
  preferences: IClientNotificationPreference[];

  constructor(private apiSvc:NotificationPreferenceApiService, private msg:DsMsgService) {        
  }

  ngOnInit() {
      this.apiSvc.getClientNotificationPreferences(this.clientId).subscribe((data) => {
          this.preferencesProductGroups = data;
      });
  }

  save() {
      this.apiSvc
          .saveClientNotificationPreferences(this.clientId, this.preferencesProductGroups)
          .subscribe((data) => {
              data.forEach(d => {
                  let current = _.find(this.preferencesProductGroups, g => g.name === d.name);
                  d.$accordionToggle = current && current.$accordionToggle;
              });
              this.preferencesProductGroups = data;
              this.msg.setTemporarySuccessMessage('Preferences saved successfully');
          },
            (error:HttpErrorResponse) => {
                this.msg.showWebApiException(error.error);
            });
  }

  disableProductGroupCard(productGroup:INotificationPreferencesProductGroups) {
      var color: string = 'info';
      var enabledCount: number;

      enabledCount = this.getProductGroupEnabledCount(productGroup, true) + this.getProductGroupEnabledCount(productGroup, false);

      // if the product is not enabled or no preferences are enabled within the product then disable
      if(enabledCount === 0) {
          color = 'info-special'
        }
      if(!productGroup.isEnabled) {
          color = 'archive'
      }

      return color;
  }

  getProductGroupEnabledCount(productGroup: INotificationPreferencesProductGroups, isRequiredCount: boolean) {
      var count: number;
      if(isRequiredCount)
      {
          count = productGroup.clientNotificationPreferences ? 
                  productGroup.clientNotificationPreferences.requiredPreferences.filter(pref => pref.isEnabled).length : 
                  0;

      }
      else 
      {
        count = productGroup.clientNotificationPreferences ? 
        productGroup.clientNotificationPreferences.optionalPreferences.filter(pref => pref.isEnabled).length : 
        0;
      }

      return count;
  }

  toggleProductGroupContent(productGroup:INotificationPreferencesProductGroups) {
      var toggle:boolean;
      
      if(!productGroup.isEnabled)
      {
          // if product is not enabled, don't allow the accordion to be toggled (shown/hidden)
          toggle = productGroup.isEnabled;
      }
      else
      {
          // toggle the product accordion
          toggle = !productGroup.$accordionToggle;
      }

      productGroup.$accordionToggle = toggle;
  }

  toggleNotificationTypePreference(productGroup: INotificationPreferencesProductGroups, preference:IClientNotificationPreference) {
      var toggle:boolean;

      if(!productGroup.isEnabled || !preference.canClientControl)
      {
          // if the product is not enabled or the client cannot control the notification type,
          // don't allow the enabled state to be toggled.
          toggle = preference.isEnabled;
      }
      else
      {
          // allow the enabled state to be toggled.
          toggle = !preference.isEnabled;
      }

      return toggle;
  }

}

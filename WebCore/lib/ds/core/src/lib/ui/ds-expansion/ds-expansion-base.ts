import {InjectionToken, Inject, Directive} from '@angular/core';
import {CdkAccordion} from '@angular/cdk/accordion';

export type DsExpansionDisplayMode = 'default' | 'flat';

@Directive({
    selector: 'ds-expansion-base, [dsExpansionBase]',
    exportAs: 'dsExpansionBase',
  })
export class DsExpansionBase extends CdkAccordion {
    hideToggle:boolean;
    displayMode:DsExpansionDisplayMode;
    _handleHeaderKeydown:(event:KeyboardEvent) => void;
    _handleHeaderFocus:(header:any) => void;
}

/**
 * Token used to provide a `DsExpansion` to `DsExpansionPanel`.
 * Used primarily to avoid circular imports between `DsExpansion` and `DsExpansionPanel`.
 */
export const DS_EXPANSION_BASE = new InjectionToken<DsExpansionBase>('DS_EXPANSION_BASE');

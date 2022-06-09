import { Component, OnInit } from '@angular/core';
import { IAccessRuleSet } from '../shared/access-rule-set.model';

@Component({
    selector: 'ds-access-rule-set-editor',
    templateUrl: './access-rule-set-editor.component.html',
    styleUrls: ['./access-rule-set-editor.component.scss']
})
export class AccessRuleSetEditorComponent implements OnInit {

    ruleSet: IAccessRuleSet

    constructor() { }

    ngOnInit() {
    }

}

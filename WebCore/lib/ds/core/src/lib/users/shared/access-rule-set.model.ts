import { IAccessRule } from './access-rule.model';

export enum AccessRuleSetConditionType {
    And = 0,
    Or = 1
}

export interface IAccessRuleSet extends IAccessRule {
    condition: AccessRuleSetConditionType;
    rules: IAccessRule[];
}
import { KeyValue } from "@models/key-value.model";

export interface IEmployeeTaxCostCenterConfiguration {
    availableCostCenters: KeyValue[];
    selectedCostCenters: KeyValue[];
}
import { InitData, MappedInitData } from './InitData.model';
import { FilterOption } from './filter-option.model';
import { GetTimeApprovalTableDto } from './GetTimeApprovalTableDto.model';

export interface ComponentData {
    initData: InitData;
    tableData: GetTimeApprovalTableDto;
    filterValues: {[categoryId: number]: FilterOption[]};
  }

export interface MappedComponentData extends ComponentData {
  initData: MappedInitData;
  angularRouteParams: any;
}
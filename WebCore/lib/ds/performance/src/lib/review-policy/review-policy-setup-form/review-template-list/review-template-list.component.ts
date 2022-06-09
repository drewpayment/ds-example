import { Component, OnInit, ChangeDetectionStrategy, Input, Output, EventEmitter } from '@angular/core';
import { IReviewTemplate } from '../..';
import { FormControl } from '@angular/forms';
import { ReferenceDate } from '../../../../../../core/src/lib/groups/shared/schedule-type.enum';
import { Maybe } from '@ds/core/shared/Maybe';
import { Pipe, PipeTransform } from '@angular/core';
import { ReviewPolicyService } from '../review-policy.service';
import { GroupService } from '@ds/core/groups/group.service';
import { treatHardCodedRangeAsCalendarYear } from '@ds/core/groups/shared/treat-hardcoded-date-range-as-calendar-year.fn';

const getAllReferenceDateTypes = () => {
  return Object.keys(ReferenceDate)
  .map(x => +x)
  .filter(x => !isNaN(x))
}

/**
 * This function treats `ReferenceDate.HardCodedRange` and `ReferenceDate.CalendarYear` as the same type
 */
const removeDuplicates = (x: number[]) => {
  const filter = {};
  x.forEach(y => {
      var key = y;
      key = treatHardCodedRangeAsCalendarYear(key);
      filter[key] = null;
  });
  return filter;
};
const convertToArrayOfNumbers = (x: {}) => {
  return Object.keys(x).map(y => +y);
};

@Component({
  selector: 'ds-review-template-list',
  templateUrl: './review-template-list.component.html',
  styleUrls: ['./review-template-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ReviewTemplateListComponent implements OnInit {
  /**
   * Changing this value causes our pure filter pipe to run
   */
  public ListChanged = false;
  private _sourceTemplates: IReviewTemplate[];
  @Input()
  public get SourceTemplates(): IReviewTemplate[] {
    return this._sourceTemplates;
  }
  public set SourceTemplates(value: IReviewTemplate[]) {
    this._sourceTemplates =  value;
    this.ListChanged = !this.ListChanged;
  }

  @Input() IsArchiveView: boolean;

  @Output() RequestToToggleList = new EventEmitter<any>();

  selectedReviewType = new FormControl("2");
  readonly typeList: ReferenceDate[] = new Maybe(getAllReferenceDateTypes())
  .map(removeDuplicates)
  .map(convertToArrayOfNumbers)
  .valueOr(<number[]>[]);

  public addRecurring: () => void;
  public addOneTime: () => void;

  constructor(
    policySvc: ReviewPolicyService,
    groupSvc: GroupService) {
    this.addRecurring = () => policySvc.AddRecurringTemplate(groupSvc);
    this.addOneTime = () => policySvc.AddNewHireTemplate(groupSvc);
   }

   get TypeList(): ReferenceDate[] {
    return this.typeList;
}

get ReferenceDate() {
  return ReferenceDate;
}

  ngOnInit() {
  }

  swapDisplayList(): void {
    this.RequestToToggleList.next();
  }
}

@Pipe({name: 'filterByType'})
export class FilterByTypePipe implements PipeTransform {
  /**
   * Returns a list of templates that the user wants to see.
   * @param templates The list to filter
   * @param type The selected type
   * @param listChanged Only to make Angular run the function when we want it to
   */
  transform(templates: IReviewTemplate[], type:  string, listChanged: boolean): any {
    const typeAsNumber = +type;

    const shouldDisplayAll = (selectedType: number) => selectedType === 2;

    return new Maybe(templates).map(x => x.filter(template => shouldDisplayAll(typeAsNumber) || (typeAsNumber === 0 && template.isRecurring === false) || (typeAsNumber === 1 && template.isRecurring === true))).valueOr([]);
  }
}

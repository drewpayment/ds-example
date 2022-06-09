import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';
import { ReferenceDate } from '../../../../../../core/src/lib/groups/shared/schedule-type.enum';
import { Pipe, PipeTransform } from '@angular/core';
import { ReviewPolicyService } from '../review-policy.service';
import { GroupService } from '@ds/core/groups/group.service';
import { InvalidReferenceDateException } from '../review-policy-setup-form.component';

type MenuItemAction = {
  action: (id) => void,
  text: string
}

@Component({
  selector: 'ds-icon-widget',
  templateUrl: './icon-widget.component.html',
  styleUrls: ['./icon-widget.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class IconWidgetComponent implements OnInit {

  @Input() TemplateId: number;
  private _name: string;
  @Input()
  public get Name(): string {
    return this._name;
  }
  public set Name(value: string) {
    this._name = value;
  }
  @Input() refDateType: ReferenceDate;
  private _isArchived: boolean;
  @Input()
  public get isArchived(): boolean {
    return this._isArchived;
  }
  public set isArchived(value: boolean) {
    this._isArchived = value;
    this.MenuItemsToDisplay = this.isArchived ? this.ArchiveMenuItemList : this.ActiveMenuItemList;
  }
  private _isRecurring: boolean;
  @Input()
  public get isRecurring(): boolean {
    return this._isRecurring;
  }
  public set isRecurring(value: boolean) {
    this._isRecurring = value;
  }
  private readonly ActiveMenuItemList: MenuItemAction[] = [];
  private readonly ArchiveMenuItemList: MenuItemAction[] = [];
  MenuItemsToDisplay: MenuItemAction[] = [];

  constructor(
    policySvc: ReviewPolicyService,
    groupSvc: GroupService) {

    this.ActiveMenuItemList.push({action: this.callAction(groupSvc, policySvc.EditTemplate), text: 'Edit'});
    this.ActiveMenuItemList.push({action: this.callAction(groupSvc, policySvc.CopyTemplate), text: 'Copy'});
    this.ActiveMenuItemList.push({action: this.callAction(groupSvc, policySvc.ArchiveTemplate), text: 'Archive'});
    this.ArchiveMenuItemList.push({action: this.callAction(groupSvc, policySvc.EditTemplate), text: 'Edit'});
    this.ArchiveMenuItemList.push({action: this.callAction(groupSvc, policySvc.ReActivateTemplate), text: 'Restore'});
   }

   private callAction(svc: GroupService, action: (id: number, groupSvc: GroupService) => void): (id: any) => void {
    return id => action(id, svc);
   }

  ngOnInit() {

  }


}

@Pipe({name: 'refDateToMatIcon'})
export class RefDateToMatIconPipe implements PipeTransform {
  transform(isRecurring: boolean): any {
    return isRecurring ? 'repeat' : 'today';
  }
}

@Pipe({name: 'refDateToColor'})
export class RefDateToColorPipe implements PipeTransform {
  transform(value: ReferenceDate): any {
    switch(value){
      case ReferenceDate.CalendarYear:
        return 'success';
      case ReferenceDate.DateOfHire:
        return 'primary';
      case ReferenceDate.HardCodedRange:
        return 'success';
      default:
          throw new InvalidReferenceDateException();
    }
  }
}

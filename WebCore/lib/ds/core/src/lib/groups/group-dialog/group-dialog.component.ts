import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Group, GroupJsonDto } from '../shared/group.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AutocompleteItem, AutocompleteItemWithRT } from '../shared/autocomplete-item.model';
import { IGroupDialogData } from './group-dialog-data.model';
import { MapToAutocomplete } from './to-autocomplete-item.pipe';
import { ClientDivisionDto } from '@ajs/ds-external-api/models/client-division-dto.model';
import { ClientDepartmentDto } from '@ajs/ds-external-api/models/client-department-dto.model';
import { JobProfileDto } from '@ajs/ds-external-api/models/job-profile-dto.model';
import { IEmployeePayType, EmployeePayTypeEnum } from '@ds/reports/shared/employee-pay-type.model';
import { Maybe } from '@ds/core/shared/Maybe';
import { LengthOfServiceBoundType } from '../shared/length-of-service-bound-type.model';
import { EmployeeStatusTypeEnum } from '@ajs/employee/hiring/shared/models/employee-hire-data.interface';
import { DataManagementServiceFactory, DefaultAutoCompleteDataManagementStrategy } from '@ds/core/ui/forms/generic-autocomplete/generic-autocomplete.component';
import { sortTemplates, baseSortTemplates, SomethingWithReviewTemplate } from '../shared/sort-templates.pipe';
import { ICompetencyModel } from '@ds/performance/competencies/shared/competency-model.model';
import { IReviewTemplate } from '../shared/review-template.model';
import { IClientCostCenterDto } from '@ajs/labor/punch/api';


@Component({
  selector: 'ds-group-dialog',
  templateUrl: './group-dialog.component.html',
  styleUrls: ['./group-dialog.component.scss']
})
export class GroupDialogComponent implements OnInit {
  form: FormGroup;
  submitted = false;

  statuses: AutocompleteItem[] = [
    {display: "Unknown", value: EmployeeStatusTypeEnum.Unknown},
    {display: "Full Time", value: EmployeeStatusTypeEnum.FullTime},
    {display: "Part Time", value: EmployeeStatusTypeEnum.PartTime},
    {display: "Call In", value: EmployeeStatusTypeEnum.CallIn},
    {display: "Special", value: EmployeeStatusTypeEnum.Special},
    {display: "Manager", value: EmployeeStatusTypeEnum.Manager},
    {display: "Last Pay", value: EmployeeStatusTypeEnum.LastPay},
    {display: "Full Time Temp", value: EmployeeStatusTypeEnum.FullTimeTemp},
    {display: "Military Leave", value: EmployeeStatusTypeEnum.MilitaryLeave},
    {display: "Student Intern", value: EmployeeStatusTypeEnum.StudentIntern},
    {display: "Seasonal", value: EmployeeStatusTypeEnum.Seasonal},
    {display: "Part Time Temp", value: EmployeeStatusTypeEnum.PartTimeTemp},
    {display: "Severence", value: EmployeeStatusTypeEnum.Severance}
  ]

  lengthOfServiceSelectOptions: { description: string, id: number }[] = [
    { description: 'Greater Than', id: LengthOfServiceBoundType.GreaterThan },
    { description: 'Greater Than or Equal To', id: LengthOfServiceBoundType.GreaterThanOrEqualTo },
    { description: 'Less Than', id: LengthOfServiceBoundType.LessThan },
    { description: 'Less Than or Equal To', id: LengthOfServiceBoundType.LessThanOrEqualTo },
    { description: 'Equal To', id: LengthOfServiceBoundType.EqualTo }
  ]

  divisionNames: {[id: number]: string} = {};

  constructor(
    private dialogRef: MatDialogRef<GroupDialogComponent, Group>,
    @Inject(MAT_DIALOG_DATA)
        public dialogData: IGroupDialogData,
    fb: FormBuilder) {
    this.divisionNames = new Maybe(this.dialogData.divisions).map(x => x.map(y => ({ id: y.clientDivisionId, name: y.name }))).map(x => {

      const result = {};
      x.forEach(item => {
        result[item.id] = item.name;
      });

      return result;
    }).valueOr({});
      this.dialogData.departments = this.dialogData.departments.filter(x => x.isActive);
      this.dialogData.jobProfiles = this.dialogData.jobProfiles.filter(x => x.isActive);
      this.dialogData.costCenters = this.dialogData.costCenters.filter(x => x.isActive);
      this.dialogData.payTypes = this.dialogData.payTypes.filter(payType => payType.payTypeId === EmployeePayTypeEnum.hourly || payType.payTypeId === EmployeePayTypeEnum.salary)
      const safeGroup = new Maybe(this.dialogData.group);
      const safelyGetProperty = this.setMaybe(new Maybe(this.dialogData.group));

      this.form = fb.group({
        groupId: fb.control(safeGroup.map(x => x.groupId).value()),
        name: fb.control(safeGroup.map(x => x.name).value(), [Validators.required]),
        reviews: fb.control(safeGroup.map(x => x.reviewTemplates).valueOr([])),
        status: fb.control(safelyGetProperty(x => x.statuses, [])),
        compModel: fb.control(safeGroup.map(x => x.competencyModels).valueOr([])),
        department: fb.control(safelyGetProperty(x => x.departments, [])),
        jobTitle: fb.control(safelyGetProperty(x => x.jobTitles, [])),
        costCenter: fb.control(safelyGetProperty(x => x.costCenters, [])),
        payType: fb.control(safelyGetProperty(x => x.payTypes, [])),
        duration: fb.control(safelyGetProperty(x => x.duration, null), {updateOn: 'blur'}),
        unitType: fb.control(safelyGetProperty(x => x.dateUnit, null)),
        inputControl: fb.control(null, {updateOn: 'blur'}),
        lengthOfService: fb.control(safelyGetProperty(x => x.boundType, null))
      });
     }

     /**
      * Returns a function that allows you to safely get the data you want without specifying the the whole property chain and manually checking for null.
      * @param raw The unmapped Group we want to get our data from
      */
     private setMaybe(raw: Maybe<Group>): <T>(propToGet: (raw: GroupJsonDto) => T, whenNull: T) => T {
       const json = raw.map(x => x.jsonData);
       return (propToGet, whenNull) => {
         return json.map(propToGet).valueOr(whenNull);
       }
     }

  ngOnInit() {
  }

  sorttemplates(templates: AutocompleteItemWithRT[]): AutocompleteItemWithRT[]{
    return baseSortTemplates(templates.map(x => ({model: x, getTemplate: () => x.template} as SomethingWithReviewTemplate<AutocompleteItemWithRT>))).map(x => x.model);
  }

  compModelMapper: MapToAutocomplete<ICompetencyModel> = (val) => ({display: val.name, value: val.competencyModelId} as AutocompleteItem)
  divisionsMapper: MapToAutocomplete<ClientDivisionDto> = (val) => ({display: val.name, value: val.clientDivisionId} as AutocompleteItem)
  departmentsMapper: MapToAutocomplete<ClientDepartmentDto> = (val) => ({display: val.name + (this.divisionNames[val.clientDivisionId] ? ' - ' + this.divisionNames[val.clientDivisionId] : ''), value: val.clientDepartmentId} as AutocompleteItem)
  jobProfilesMapper: MapToAutocomplete<JobProfileDto> = (val) => ({display: val.description, value: val.jobProfileId} as AutocompleteItem)
  costCentersMapper: MapToAutocomplete<IClientCostCenterDto> = (val) => ({display: val.description, value: val.clientCostCenterId} as AutocompleteItem)
  payTypesMapper: MapToAutocomplete<IEmployeePayType> = (val) => ({display: val.description, value: val.payTypeId} as AutocompleteItem)
  templatesMapper: MapToAutocomplete<IReviewTemplate> = (val) => ({display: val.name, value: val.reviewTemplateId, template: val} as AutocompleteItemWithRT)

  cancel(): void {
    this.dialogRef.close();
  }

  save(): void {
    this.submitted = true;
    if(this.form.valid){
      this.dialogRef.close(this.convertFormToGroup(this.form.value));
    }
  }

  convertFormToGroup(formValue: any): Group {
    return {
      clientId: null,
      competencyModels: formValue.compModel,
      groupId: formValue.groupId,
      name: formValue.name,
      reviewTemplates: formValue.reviews,
      jsonData: {
        costCenters: formValue.costCenter,
        dateUnit: formValue.unitType,
        departments: formValue.department,
        duration: formValue.duration,
        payTypes: formValue.payType,
        jobTitles: formValue.jobTitle,
        statuses: formValue.status,
        boundType: formValue.lengthOfService
      }
    };
  }

}

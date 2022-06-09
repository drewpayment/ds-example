import { Component, OnInit } from "@angular/core";
import { CompanyManagementService } from "../../../services/company-management.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { HttpErrorResponse } from "@angular/common/http";
import { IClientDepartment } from "@ajs/client/models/client-department.model";
import { AccountService } from "@ds/core/account.service";
import { UserInfo, UserType } from "@ds/core/shared";
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import { map, switchMap, tap } from "rxjs/operators";
import { Employee } from "@ajs/ds-external-api/models/employee-dto.model";
import { ClientDivision } from "@ds/core/employee-services/models";
import { QuickViewInformation } from '@models';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
  selector: "ds-departments",
  templateUrl: "./departments.component.html",
  styleUrls: ["./departments.component.scss"]
})
export class DepartmentsComponent implements OnInit {
  isSaving = false;
  loaded: Boolean = false;
  isSubmitted: Boolean = false;
  user: UserInfo;
  form: FormGroup;
  employees: Employee[]; //client employees to assign to Head Of Department
  divisions: ClientDivision[];
  departments: IClientDepartment[];
  allDepartments: IClientDepartment[];
  selectedDepartment: IClientDepartment;
  selectedDivisionId: number = 0;
  modifyAllDivisions: Boolean; // hide/show modify all divisions checkbox
  showIsActive: Boolean = true; // hide/show active checkbox
  deleteConfirmed: Boolean;
  quickViewInformation: QuickViewInformation;
  blockedDepartments: number[];
  showDelete: boolean = false;

  constructor(
    private fb: FormBuilder,
    private CompanyManagementService: CompanyManagementService,
    private confirmDialog: ConfirmDialogService,
    private ngxMsgSvc: NgxMessageService,
    private accountService: AccountService
  ) {}

  ngOnInit() {
    this.initForm();
    this.accountService
      .getUserInfo()
      .pipe(
        switchMap((user) => {
          this.user = user;
          if (user.userTypeId == UserType.companyAdmin)
            this.showIsActive = false;
          return this.CompanyManagementService.getCompanyDepartmentInformation(
            this.user.clientId
          );
        }),
        tap((data: any) => {
          this.divisions = data.divisions;
          this.departments = data.departments;
          this.blockedDepartments = data.blockedDepartments;
          this.employees = data.headOfDepartments;
          this.allDepartments = data.departments;
          this.modifyAllDivisions = data.hasDepartmentsAcrossDivisionsOption; //if Departments Across Divisions Company Option is on, hide checkbox
          this.form.controls["department"].disable();
          let quickViewChange: QuickViewInformation = {
            headOfDepartments: data.headOfDepartments.length,
            divisions: data.divisions.length,
            departments: this.allDepartments.length,
          } as QuickViewInformation;
          this.setQuickViewInformation(quickViewChange);
          this.showDelete = false;
          this.loaded = true;
        })
      )
      .subscribe();
  }

  initForm() {
    this.form = this.fb.group({
      division: 0,
      department: 0,
      name: ["", Validators.required],
      code: ["", Validators.required],
      hod: null, //head of department
      modifyAll: false,
      active: true,
    });
    this.showDelete = false;
  }

  changeDepartment() {
    //-- Add Department --
    if (this.form.controls["department"].value == 0) {
      this.form.reset({
        ... this.form.value,
        department: 0,
        name: "",
        code: "",
        hod: null,
        modifyAll: false,
        active: true,
      });
      this.form.markAsUntouched();
      this.showDelete = false;
    }
    //Existing Department
    else {
      this.showDelete = true;

      this.selectedDepartment = this.departments.find((x) => x.clientDepartmentId == this.form.controls['department'].value);

      //Hide/Show Delete
      if (this.blockedDepartments.some(x => x == this.selectedDepartment.clientDepartmentId)) this.showDelete = false;

      if ( this.selectedDepartment ) {
        this.form.reset({
          division: this.selectedDepartment.clientDivisionId,
          department: this.selectedDepartment.clientDepartmentId,
          name: this.selectedDepartment.name,
          code: this.selectedDepartment.code,
          hod: this.selectedDepartment.departmentHeadEmployeeId,
          active: this.selectedDepartment.isActive,
        });
      }
    }
  }

  changeDivision() {
    //-- Select Division --
    if (this.form.controls["division"].value == 0) {
      this.selectedDivisionId = 0; //hide bottom half of form
      this.form.controls["department"].disable(); //disable department select
      this.initForm(); //clear form
      let quickViewChange: QuickViewInformation = {
        headOfDepartments: this.employees.length,
        divisions: this.divisions.length,
        departments: this.allDepartments.length,
      } as QuickViewInformation;
      this.setQuickViewInformation(quickViewChange);
    }
    //Existing Division
    else
    {
      this.form.controls['department'].enable(); //enable department select
      this.selectedDivisionId = this.form.controls['division'].value; //show bottom half of form
      this.departments = this.allDepartments.filter((d) => d.clientDivisionId == this.form.controls['division'].value);
      var divisionEmployees = this.employees.filter((e) => e.clientDivisionId == this.form.controls['division'].value);
      var currentDivision = this.divisions.find((x) => x.clientDivisionId == this.form.controls['division'].value);
      let quickViewChange: QuickViewInformation = {
        headOfDepartments: divisionEmployees.length,
        divisions: this.divisions.length,
        departments: this.departments.length,
      } as QuickViewInformation;
      this.setQuickViewInformation(quickViewChange, currentDivision.name);
      this.form.patchValue({
        department: 0,
        name: "",
        code: "",
        hod: null,
        active: true,
      });
    }
  }

  save() {
    this.isSubmitted = true;
    this.form.markAllAsTouched();
    //Check for errors
    if ( this.form.invalid ) return;

    //Check if Division is selected
    if ( this.checkDivisionSelected() ) return;

    //Check if name exists
    if ( this.checkDuplicateName() ) return;

    //Check if code exists
    if ( this.checkDuplicateCode() ) return;

    this.isSaving = true;

    //Set model for save
    this.selectedDepartment = {
      clientDepartmentId: this.form.value.department,
      clientDivisionId: this.form.value.division,
      departmentHeadEmployeeId: this.form.value.hod,
      name: this.form.value.name,
      code: this.form.value.code,
      isActive: this.form.value.active,
      clientId: this.user.clientId,
      modifyAllDivisions: this.form.value.modifyAll,
    };

    //Save
    this.CompanyManagementService.saveCompanyDepartment(
      this.selectedDepartment,
      this.user.clientId
    ).subscribe(
      (response: IClientDepartment) => {
        if (this.form.value.modifyAll)
          this.ngxMsgSvc.setSuccessMessage(
            "Department updated successfully to all divisions"
          );
        else this.ngxMsgSvc.setSuccessMessage("Department saved successfully");
        this.isSubmitted = false;
        this.isSaving = false;

        if (response) {
          if (this.form.value.modifyAll) {
            this.CompanyManagementService.getCompanyDepartmentInformation(
              this.user.clientId
            ).subscribe((data: any) => {
              this.departments = data.departments.filter(
                (d) => d.clientDivisionId == this.form.value.division
              );
              this.allDepartments = data.departments;
              let currentDivision = this.divisions.find(
                (x) => x.clientDivisionId == this.form.value.division
              );
              let quickViewChange: QuickViewInformation = {
                headOfDepartments: data.headOfDepartments.filter(
                  (e) => e.clientDivisionId == this.form.value.division
                ).length,
                divisions: data.divisions.length,
                departments: this.departments.length,
              } as QuickViewInformation;
              this.setQuickViewInformation(
                quickViewChange,
                currentDivision.name
              );
              this.form.controls["department"].patchValue(
                response.clientDepartmentId
              );
              this.changeDepartment();
            });
          } else {
            let index = this.departments
              .map((d) => d.clientDepartmentId)
              .indexOf(response.clientDepartmentId);
            if (index == -1) {
              this.departments.push(response); //insert
              this.allDepartments.push(response); //insert
            } else {
              this.departments[index] = response; //update
            }
            let currentDivision = this.divisions.find(
              (x) => x.clientDivisionId == this.form.value.division
            );
            let quickViewChange: QuickViewInformation = {
              headOfDepartments: this.employees.filter(
                (x) => x.clientDivisionId == this.form.value.division
              ).length,
              divisions: this.divisions.length,
              departments: this.departments.length,
            } as QuickViewInformation;
            this.setQuickViewInformation(quickViewChange, currentDivision.name);
            this.form.controls["department"].patchValue(
              response.clientDepartmentId
            );
            this.changeDepartment();
          }
        }
      },
      (error: HttpErrorResponse) => {
        this.isSaving = false;
        this.ngxMsgSvc.setErrorResponse(error);
      }
    );
  }

  deleteClicked() {
    //Check for Active Employees assigned to department
    if (
      this.employees.some(
        (e) =>
          e.clientDepartmentId == this.form.value.department &&
          e.isActive == true
      )
    ) {
      this.ngxMsgSvc.setErrorMessage(
        "This department cannot be deleted because it is assigned to employees."
      );
      return;
    }

    const options = {
      title: "Are you sure you want to delete this department?",
      confirm: "Delete",
    };
    this.confirmDialog.open(options);
    this.confirmDialog
      .confirmed()
      .pipe(map((confirmation) => (this.deleteConfirmed = confirmation)))
      .subscribe(() => {
        this.deleteClientDepartment(this.deleteConfirmed);
      });
  }

  deleteClientDepartment(deleteConfirmed) {
    if (deleteConfirmed) {
      this.CompanyManagementService.deleteCompanyDepartment(
        this.selectedDepartment,
        this.user.clientId
      ).subscribe(
        (res) => {
          this.ngxMsgSvc.setSuccessMessage("Department Deleted Successfully");
          this.departments = this.departments.filter(
            (d) =>
              d.clientDepartmentId != this.selectedDepartment.clientDepartmentId
          ); //remove from departments array
          this.allDepartments = this.allDepartments.filter(
            (d) =>
              d.clientDepartmentId != this.selectedDepartment.clientDepartmentId
          ); //remove from allDepartments array
          this.form.controls["division"].patchValue(0);
          this.changeDivision();
        },
        (error: HttpErrorResponse) => {
          this.ngxMsgSvc.setErrorResponse(error);
        }
      );
    }
  }

  checkDuplicateName(): Boolean {
    if (!this.form.value.modifyAll) {
      let duplicate = this.departments.find(
        (d) => d.name === this.form.value.name
      );
      if (
        duplicate &&
        duplicate.clientDepartmentId != this.form.value.department
      ) {
        this.ngxMsgSvc.setErrorMessage(
          "The specified department name " +
            duplicate.name +
            " already exists for this division. Changes were not saved."
        );
        return true;
      } else {
        return false;
      }
    } else {
      let duplicates = this.allDepartments.filter(
        (ad) => ad.name === this.form.value.name
      ); //if no duplicates its OK
      let current = duplicates.find(
        (x) => (x.clientDepartmentId = this.form.value.department)
      ); //if currently selected is in duplicate list, its OK
      if (duplicates.length && !current) {
        this.ngxMsgSvc.setErrorMessage(
          "The specified department name " +
            duplicates[0].name +
            " already exists across another division. Changes were not saved."
        );
        return true;
      } else {
        return false;
      }
    }
  }

  checkDuplicateCode(): Boolean {
    if (!this.form.value.modifyAll) {
      let duplicateCode = this.departments.find(
        (d) => d.code === this.form.value.code
      );
      if (
        duplicateCode &&
        duplicateCode.clientDepartmentId != this.form.value.department
      ) {
        this.ngxMsgSvc.setErrorMessage(
          "The specified department code " +
            duplicateCode.code +
            " already exists for this division. Changes were not saved."
        );
        return true;
      } else {
        return false;
      }
    } else {
      let duplicates = this.allDepartments.filter(
        (ad) => ad.code === this.form.value.code
      ); //if no duplicates its OK
      let current = duplicates.find(
        (x) => (x.clientDepartmentId = this.form.value.department)
      ); //if currently selected is in duplicate list, its OK
      if (duplicates.length && !current) {
        this.ngxMsgSvc.setErrorMessage(
          "The specified department code " +
            duplicates[0].code +
            " already exists across another division. Changes were not saved."
        );
        return true;
      } else {
        return false;
      }
    }
  }

  checkDivisionSelected(): Boolean {
    if(this.form.value.division === 0 || this.form.value.division === null || this.form.value.division === undefined ) {
      this.ngxMsgSvc.setErrorMessage("No Division selected. Changes were not saved.");
      return true;
    }
    else {
      return false;
    }
  }

  setQuickViewInformation(
    quickViewChange: QuickViewInformation,
    divisionName: string = "All Divisions"
  ) {
    this.quickViewInformation = {
      title: divisionName,
      headOfDepartments: quickViewChange.headOfDepartments,
      divisions: quickViewChange.divisions,
      departments: quickViewChange.departments,
    } as QuickViewInformation;
  }
}

import {
  Component,
  OnInit,
  AfterViewChecked,
} from "@angular/core";
import {
  DsStyleLoaderService,
  IStyleAsset,
} from "@ajs/ui/ds-styles/ds-styles.service";
import { MatDialog } from "@angular/material/dialog";
import { IEmployeeContactInfo } from "../../shared/employee-contact-info.model";
import { EmployeeProfileService } from "../../shared/employee-profile-api.service";
import { ResourceApiService } from "@ds/core/resources/shared/resources-api.service";
import { IEmployeeImage } from "@ds/core/resources/shared/employee-image.model";
import { UserInfo } from "@ds/core/shared";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import * as _ from "lodash";
import { EmployeeContactInfoFormComponent } from "../employee-contact-info-form/employee-contact-info-form.component";
import { JobProfileModalComponent } from "./../../job-profile/job-profile-modal/job-profile-modal.component";
import { IJobProfileBasicInfo } from "../../shared/job-profile-basic-info.model";
import { JobProfileService } from "../../shared/job-profile-api.service";
import { Observable, combineLatest, of } from "rxjs";
import { switchMap, map } from "rxjs/operators";
import { AccountService } from "@ds/core/account.service";
import { IEmployeeAvatars } from "@ds/core/employees/shared/employee-avatars.model";
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';

@Component({
  selector: "ds-employee-contact-info",
  templateUrl: "./employee-contact-info.component.html",
  styleUrls: ["./employee-contact-info.component.scss"],
})
export class EmployeeContactInfoComponent implements OnInit, AfterViewChecked {
  mainStyle: IStyleAsset;
  user: UserInfo;
  employeeContactInfo: IEmployeeContactInfo;
  jobProfileData: IJobProfileBasicInfo;
  employeeImage: IEmployeeImage;
  hasViewPermissions: boolean;
  hasEditPermissions: boolean;
  showJobTitleLink: boolean;
  isPageLoaded$: Observable<boolean>;

  constructor(
    private styles: DsStyleLoaderService,
    private service: EmployeeProfileService,
    private jobProfileService: JobProfileService,
    private resourceApiService: ResourceApiService,
    private accountService: AccountService,
    private msgSvc: DsMsgService,
    private dialog: MatDialog,
  ) {}

  ngOnInit() {
    this.hasViewPermissions = false;
    this.hasEditPermissions = false;
    this.showJobTitleLink = false;
    this.employeeContactInfo = this.createEmptyEmployeeContactInfo();

    this.isPageLoaded$ = this.accountService.getUserInfo().pipe(
      switchMap((u) => {
        this.user = u;
        return combineLatest(
          this.accountService.canPerformActions("Employee.EmployeeView"),
          this.accountService.canPerformActions("Employee.EmployeeUpdate"),
          this.accountService.hasFeature(u.clientId, Features.EmployeeChangeRequests),
        );
      }),
      switchMap(([view, edit, crEnabled]) => {
        this.hasViewPermissions = view === true;
        this.hasEditPermissions = (edit === true) && (!crEnabled || !crEnabled.isEnabled);
        return this.service.getEmployeeContactInfo(this.user.employeeId);
      }),
      switchMap((info) => {
        this.employeeContactInfo = info;
        if (
          this.employeeContactInfo &&
          this.employeeContactInfo.jobProfileId &&
          this.employeeContactInfo.jobProfileId !== 0 &&
          this.employeeContactInfo.jobProfileId !== -2147483648
        ) {
          return this.jobProfileService.getJobProfileBasicInfo(
            this.employeeContactInfo.jobProfileId
          );
        }
        return of(null);
      }),
      switchMap((jobProfile) => {
        if (jobProfile) {
          this.jobProfileData = jobProfile;
          if (
            this.jobProfileData.requirements.trim().length > 0 ||
            this.jobProfileData.workingConditions ||
            this.jobProfileData.benefits ||
            this.jobProfileData.jobProfileResponsibilities.length > 0 ||
            this.jobProfileData.jobProfileSkills.length > 0
          ) {
            this.showJobTitleLink = true;
          }
        }
        return this.resourceApiService.getEmployeeProfileImages(
          this.user.clientId,
          this.user.employeeId
        );
      }),
      switchMap((imageData: IEmployeeImage) => {
        this.employeeImage = imageData;

        return this.resourceApiService.getEmployeeAvatar(this.user.employeeId);
      }),
      map((avatar: IEmployeeAvatars) => {
        this.employeeImage._employeeAvatar = avatar;
        return true;
      })
    );
  }

  private createEmptyEmployeeContactInfo(): IEmployeeContactInfo {
    return {
      employeeId: null,
      employeeNumber: null,
      firstName: null,
      middleInitial: null,
      lastName: null,
      addressLine1: null,
      addressLine2: null,
      city: null,
      postalCode: null,
      countryId: null,
      countryName: null,
      stateId: null,
      stateName: null,
      countyId: null,
      countyName: null,
      homePhoneNumber: null,
      cellPhoneNumber: null,
      relation: null,
      emailAddress: null,
      gender: null,
      birthDate: null,
      jobProfileId: null,
      jobTitleInfoDescription: null,
      divisionId: null,
      divisionName: null,
      departmentId: null,
      departmentName: null,
      driversLicenseExpirationDate: null,
      driversLicenseNumber: null,
      driversLicenseIssuingStateId: null,
      driversLicenseIssuingStateName: null,
      noDriversLicense: null,
    };
  }

  showEditContactInfoDialog(employeeContactInfo: IEmployeeContactInfo): void {
    this.dialog
      .open(EmployeeContactInfoFormComponent, {
        width: "700px",
        data: {
          user: this.user,
          employeeContactInfo: employeeContactInfo,
          hasEditPermissions: this.hasEditPermissions,
        },
      })
      .afterClosed()
      .subscribe((result) => {
        if (result == null) return;
        this.msgSvc.sending(true);
        this.service
          .updateEmployeeContactInfo(result, this.hasEditPermissions)
          .subscribe((data) => {
            if (!_.isEmpty(data)) {
              this.employeeContactInfo = data;

              const successMessage = this.hasEditPermissions
                ? "Successfully updated changes."
                : "Successfully submitted change request.";
              this.msgSvc.setTemporarySuccessMessage(successMessage, 5000);
            }
          });
      });
  }

  showJobProfileDialog(jobProfileData: IJobProfileBasicInfo): void {
    this.dialog.open(JobProfileModalComponent, {
      width: "700px",
      data: {
        user: this.user,
        jobProfileData: jobProfileData,
      },
    });
  }

  /**
   * We tell DsStyleLoaderService that this component should use main stylesheet AFTER OnInit and AfterViewInit
   * because we need to make sure that everything is resolved above this component. The DsStyleLoaderService is not
   * instantiated until after OutletComponent is finished loading.
   */
  ngAfterViewChecked() {
    this.styles.useMainStyleSheet();
  }
}

import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { NPSSurveyService } from "@ds/core/resources/shared/nps-survey.service";
import { UserInfo, UserType } from "@ds/core/shared";
import { AccountService } from "@ds/core/account.service";
import { ClientService } from "@ds/core/clients/shared";
import { forkJoin, Observable } from "rxjs";
import { switchMap, tap, map } from "rxjs/operators";
import { IClientData } from "@ajs/onboarding/shared/models";
import { AssetHelperService } from "@ds/core/ui/ui-helper";
import { INpsResponseDto } from "@ds/core/resources/shared/nps-response-dto.model";
import { INpsQuestionDto } from "@ds/core/resources/shared/nps-question-dto.model";
import {
  FormGroup,
  FormControl,
  FormBuilder,
  Validators,
} from "@angular/forms";
import {
  BreakpointObserver,
  Breakpoints,
  BreakpointState,
} from "@angular/cdk/layout";
import { DeviceDetectorService } from "ngx-device-detector";
import { ActivatedRoute } from "@angular/router";
import { DOCUMENT } from "@angular/common";

@Component({
  selector: "nps-survey-dialog",
  templateUrl: "./nps-survey-dialog.component.html",
  styleUrls: ["./nps-survey-dialog.component.scss"],
})
export class NPSSurveyDialogComponent implements OnInit {
  user: UserInfo;
  clientData: IClientData;
  isAccepted: number;
  isYes: boolean;
  npsQuestionData: INpsQuestionDto;
  form: FormGroup = this.createForm();
  isSuccess = true;
  isMobileStyle = false;
  id: number;
  isRatingSelected = false;
  isSubmitted = false;
  deviceInfo = null;
  checkDevice$: Observable<any>;
  isMobile = false;

  constructor(
    private dialogRef: MatDialogRef<NPSSurveyDialogComponent>,
    private npsSurveyService: NPSSurveyService,
    private accountService: AccountService,
    private assets: AssetHelperService,
    private breakpointObserver: BreakpointObserver,
    private detector: DeviceDetectorService,
    private fb: FormBuilder,
    @Inject(DOCUMENT) public document: Document
  ) {}

  ngOnInit() {
    this.checkDevice$ = this.accountService.getUserInfo().pipe(
      tap((user) => (this.user = user)),
      map((_) => this.detector.getDeviceInfo()),
      tap((info) => {
        this.deviceInfo = info;

        // Show mobile styles only if device type is unknown (equivalent to desktop),
        // the device is not an iPad (We default to desktop site for those),
        // and the user type is an employee or supervisor
        this.isMobileStyle =
          this.isDeviceDetectorMobileDevice() &&
          this.normalize(this.deviceInfo.device) !== "ipad" &&
          this.user.userTypeId > UserType.companyAdmin;

        // EXCEPTION EXPLICITLY REQUESTING DESKTOP

        const href = this.document.location.href;
        const isRequstDesktop = this.normalize(href).includes("requestdesktop");
        if (isRequstDesktop) {
          this.isMobileStyle = false;
        }
      }),
      switchMap((_) =>
        this.npsSurveyService.getNPSActiveQuestionByUserType(
          this.user.userTypeId
        )
      ),
      tap((result) => (this.npsQuestionData = result))
    );
    this.breakpointObserver
      .observe([Breakpoints.HandsetPortrait])
      .subscribe((state: BreakpointState) => {
        if (state.matches) {
          this.isMobile = true;
        } else {
          this.isMobile = false;
        }
      });
  }

  private normalize(value: string): string {
    return value.trim().toLowerCase().replace(/\s+/g, "");
  }

  private isDeviceDetectorMobileDevice(): boolean {
    return this.deviceInfo != null && this.deviceInfo.device != "Unknown";
  }

  private createForm(): FormGroup {
    return new FormGroup({
      feedback: new FormControl(null),
      score: new FormControl(false, Validators.required),
    });
  }
  GetSurveyInterface(): INpsResponseDto {
    const npsResponse: INpsResponseDto = {
      questionId: this.npsQuestionData.questionId,
      userId: this.user.userId,
      userTypeId: this.user.userTypeId,
      clientId: this.user.selectedClientId(),
      responseDate: new Date(),
      score: this.form.value.score,
      feedback: this.form.value.feedback,
    };
    return npsResponse;
  }
  userSave() {
    this.npsSurveyService
      .saveNPSResponse(this.GetSurveyInterface())
      .subscribe((x) => {
        this.isSuccess = true;
      }, this.setSuccess);
  }
  private setSuccess() {
    this.isSuccess = false;
  }
  getAssetsPath(path: string): string {
    return this.assets.resolveAsset(path);
  }
  saveClose() {
    this.isSubmitted = true;
    if (!this.isRatingSelected) return;
    this.userSave();
    this.dialogRef.close(this.isSuccess);
  }
  close() {
    this.dialogRef.close(null);
  }
  formatScore(idx: number) {
    this.isRatingSelected = true;
    this.id = idx;
    for (let i = 0; i <= 10; i++) {
      this.document
        .getElementById("score_" + i)
        .classList.remove("nps-checked");
    }
    for (let j = 0; j <= idx; j++) {
      this.document.getElementById("score_" + j).classList.add("nps-checked");
    }
  }
}

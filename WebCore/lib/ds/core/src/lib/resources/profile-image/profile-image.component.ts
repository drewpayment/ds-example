import {
  Component,
  OnInit,
  AfterViewInit,
  AfterViewChecked,
  Input,
  Injector,
  OnChanges,
  SimpleChanges,
  ChangeDetectorRef,
  Output,
  EventEmitter,
} from '@angular/core';
import {
  DsStyleLoaderService,
  IStyleAsset,
} from '@ajs/ui/ds-styles/ds-styles.service';
import { MatDialog } from '@angular/material/dialog';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { ProfileImageService } from '@ds/core/resources/shared/profile-image.service';
import { map, withLatestFrom } from 'rxjs/operators';
import { DomSanitizer } from '@angular/platform-browser';
import {
  ImageDto,
  IEmployeeImage,
  EmployeeImageUploadResult,
} from '@ds/core/resources/shared/employee-image.model';
import { ImageType } from '@ds/core/resources/shared/image-type.model';
import { ImageSizeType } from '@ds/core/resources/shared/image-size-type.model';
import { ICropitOptions } from '@ds/core/resources/shared/cropit-options.model';
import { ImageUploaderComponent } from '@ds/core/resources/image-uploader/image-uploader.component';
import * as _ from 'lodash';
import { FormBuilder } from '@angular/forms';
import { IEmployeeSearchResult } from 'lib/ds/employees/src/lib/search/shared/models/employee-search-result';

@Component({
  selector: 'ds-profile-image',
  templateUrl: './profile-image.component.html',
  styleUrls: ['./profile-image.component.scss'],
})
export class ProfileImageComponent
  implements OnInit, AfterViewChecked, OnChanges
{
  @Input() employeeImage: IEmployeeImage;
  @Input('image-type') imageType: ImageType;
  @Input('image-size') imageSize: ImageSizeType;
  @Input('cropit-options') cropitOptions: ICropitOptions;
  @Input() firstName: string;
  @Input() lastName: string;
  @Input() image: string;
  private employeeId: number;
  private clientId: number;
  mainStyle: IStyleAsset;
  user: UserInfo;
  WRITE_SELF = 'EmployeeProfiles.WriteSelfImage';
  WRITE_OTHER = 'EmployeeProfiles.WriteOtherImage';
  canEditSelf = false;
  canEditOther = false;
  editable = false;
  hasImage: boolean;
  source: string;
  employeeAvatarColor: string;
  @Output() imageChange = new EventEmitter<IEmployeeImage>();

  constructor(
    private styles: DsStyleLoaderService,
    private profileImageService: ProfileImageService,
    private accountService: AccountService,
    private dialog: MatDialog,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.setup();

    this.accountService.getUserInfo().subscribe((user) => {
      this.user = user;
      this.clientId = user.selectedClientId();
      this.setPermissions();
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!changes) return;
    if (changes.employeeImage && !changes.employeeImage.firstChange) {
      const ee = changes.employeeImage.currentValue as IEmployeeImage;
      this.employeeId = ee.employeeId;
      this.source = null;
      if (this.image) this.source = this.image;

      if (ee.extraLarge && !this.source) {
        this.source = ee.extraLarge.url;
        this.hasImage = this.source != null && this.source.length > 0;
      }

      this.employeeAvatarColor = ee._employeeAvatar
        ? ee._employeeAvatar.avatarColor
        : 'teal';
      // this.cd.detectChanges();
    }
  }

  private setPermissions(): void {
    this.accountService.getAllowedActions(false).subscribe((data) => {
      let result: string[] = data;
      _.find(result, (action) => {
        if (action === this.WRITE_SELF) this.canEditSelf = true;
        if (action === this.WRITE_OTHER) this.canEditOther = true;
      });

      this.editable =
        this.user.employeeId === this.employeeId
          ? this.canEditSelf
          : this.canEditOther;
    });
  }

  private getImageKeyFromDto(
    employee: IEmployeeSearchResult = null,
    image: ImageDto = null
  ): string {
    let imageKey: string;
    if (employee != null) {
      const profileImage = employee.profileImage;
      if (profileImage.extraLarge && profileImage.extraLarge.hasImage) {
        imageKey = 'extraLarge';
      } else if (profileImage.large && profileImage.large.hasImage) {
        imageKey = 'large';
      } else if (profileImage.medium && profileImage.medium.hasImage) {
        imageKey = 'medium';
      } else if (profileImage.small && profileImage.small.hasImage) {
        imageKey = 'small';
      }
    } else if (image != null) {
      imageKey = this.getImageKeyDescription(image.imageSize);
    } else {
      return '';
    }
    return imageKey;
  }

  private getImageKeyDescription(imageSize: ImageSizeType): string {
    return imageSize == ImageSizeType.XL
      ? 'extraLarge'
      : imageSize == ImageSizeType.LG
      ? 'large'
      : imageSize == ImageSizeType.MD
      ? 'medium'
      : imageSize == ImageSizeType.SM
      ? 'small'
      : null;
  }

  openModal(evt): void {
    evt.preventDefault();
    // if (!this.editable) return;
    const nullImage = null;
    this.dialog
      .open(ImageUploaderComponent, {
        disableClose: true,
        width: '350px',
        data: {
          user: this.user,
          employee: this.employeeImage,
          image: nullImage,
          imageSize: this.imageSize,
          imageType: this.imageType,
          cropitOptions: this.cropitOptions,
          firstName: this.firstName,
          lastName: this.lastName,
        },
      })
      .afterClosed()
      .subscribe((res: EmployeeImageUploadResult) => {
        if (res.imageHasChanges) {
          this.employeeImage = res;
          this.source =
            this.employeeImage &&
            this.employeeImage.extraLarge &&
            this.employeeImage.extraLarge.hasImage
              ? this.employeeImage.extraLarge.url
              : null;
          this.hasImage = this.source != null;

          if (!this.hasImage) this._setAvatarColor();

          this.imageChange.emit(res);
        }
      });
  }
  setup() {
    this.employeeId = this.employeeId || this.employeeImage.employeeId;
    this.clientId = this.clientId || this.employeeImage.clientId;

    if (this.image) this.source = this.image;
    if (this.employeeImage.extraLarge)
      this.source = this.employeeImage.extraLarge.url;

    this.hasImage = this.source != null && this.source.length > 0;
    this._setAvatarColor();
  }

  private _setAvatarColor() {
    this.employeeAvatarColor = this.employeeImage._employeeAvatar
      ? this.employeeImage._employeeAvatar.avatarColor
      : 'teal';
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

import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import * as _ from 'lodash';
import { UserInfo } from '@ds/core/shared';
import { IAzureViewDto } from '@ds/core/resources/shared/azure-view-dto.model';
import {
  ImageDto,
  IEmployeeImage,
  IImage,
  EmployeeImageUploadResult,
} from '@ds/core/resources/shared/employee-image.model';
import { ImageType } from '@ds/core/resources/shared/image-type.model';
import { ImageSizeType } from '@ds/core/resources/shared/image-size-type.model';
import { ICropitOptions } from '@ds/core/resources/shared/cropit-options.model';
import { AccountService } from '@ds/core/account.service';
import { ProfileImageService } from '@ds/core/resources/shared/profile-image.service';
import { ResourceApiService } from '@ds/core/resources/shared/resources-api.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { IProfileImageDto } from '@ds/core/resources/shared/profile-image.model';
import { ConfirmModalComponent } from '../confirm-modal/confirm-modal.component';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialog,
} from '@angular/material/dialog';
import { EmployeeApiService } from '@ds/core/employees/shared/employee-api.service';
import { IEmployeeAvatars } from '@ds/core/employees/shared/employee-avatars.model';
import { profile } from 'console';
import { withLatestFrom } from 'rxjs/operators';

interface DialogData {
  user: UserInfo;
  employee: IEmployeeImage;
  //employee:IEmployeeSearchResult | IEmployee,
  image: ImageDto;
  imageType: ImageType;
  imageSize: ImageSizeType;
  cropitOptions: ICropitOptions;
  modalTitle: string;
  firstName: string;
  lastName: string;
}

const CHOOSE_TEXT = 'Browse';
const CHANGE_TEXT = 'Change';
const CANCEL_TEXT = 'Cancel';
const CLOSE_TEXT = 'Close';
const MODAL_TITLE = 'Employee Avatar';

interface ICropitSource {
  url: string;
}

@Component({
  selector: 'ds-image-uploader',
  templateUrl: './image-uploader.component.html',
  styleUrls: ['./image-uploader.component.scss'],
})
export class ImageUploaderComponent implements OnInit, AfterViewInit {
  //#region vars
  user: UserInfo;
  employeeId: number;
  clientId: number;
  employeeImage: IEmployeeImage;
  image: ImageDto;
  imageType: ImageType;
  imageSize: ImageSizeType;
  cropitOptions: ICropitOptions;
  modalTitle: string;
  f: FormGroup;
  formSubmitted: boolean;
  pageTitle: string;
  private _editMode: boolean;

  private oldImage: IEmployeeImage;
  private oldImageKey: string;

  showEditControls: boolean;
  hideZoom: boolean;
  hasImageChanged: boolean;
  isExistingImage: boolean;
  isWrongFileType: boolean;

  outlinePrimaryClass: boolean;
  dragAndDrop: boolean;

  wrongFileErrorText: string = '';

  defaultOptions: ICropitOptions;
  options: ICropitOptions;
  methods: { rotate: (isClockwise: boolean) => void };
  imagePickerButtonText: string = CHOOSE_TEXT;
  closeCancelButtonText: string = CLOSE_TEXT;
  swapOkClose: boolean = false;

  imageSource: ICropitSource;
  source: string;
  token: string;
  cropper: JQuery;
  preview: JQuery;
  modalCloseData: ImageDto;

  useEmployeeAvatar: boolean;
  employeeAvatarColor: string = 'teal';

  avatarFirstName: string;
  avatarLastName: string;
  saved: boolean = false;

  backgroundColors = [
    'dark-red',
    'red',
    'orange',
    'yellow-orange',
    'yellow',
    'lime-green',
    'green',
    'dark-green',
    'teal',
    'aqua',
    'sky-blue',
    'blue',
    'navy',
    'black',
    'gray',
    'brown',
    'indigo',
    'purple',
    'pink',
    'magenta',
  ];

  //#endregion

  constructor(
    public dialogRef: MatDialogRef<
      ImageUploaderComponent,
      EmployeeImageUploadResult
    >,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private accountService: AccountService,
    private msgSvc: DsMsgService,
    private profileImageService: ProfileImageService,
    private resourceApiService: ResourceApiService,
    private employeeApiService: EmployeeApiService //private timeout: ng.ITimeoutService
  ) {
    this.setCropitMethodsAndOptions();
  }

  ngOnInit(): void {
    if(this.data && this.data.imageType == ImageType.companyLogo){
      // if the imagetype and imagesize are not defined, we assume the modal is editing a profile image.
      this.imageType = this.data.imageType || ImageType.companyLogo;
      this.imageSize = this.data.imageSize || ImageSizeType.companyLogo;
      this.modalTitle = this.data.modalTitle || "Company Logo";
      this.image = this.data.image;
      this.imageSource = null;

      // starts the cropit plugin and injects options, etc
      this.initializeCropit();
      this.useEmployeeAvatar = false;
    } else {
      this.modalTitle = this.modalTitle || MODAL_TITLE;
      this.employeeImage = this.data.employee;
      this.employeeAvatarColor = this.employeeImage._employeeAvatar
        ? this.employeeImage._employeeAvatar.avatarColor
        : 'teal';
      this.useEmployeeAvatar = true; //initialize 'use avatar' to true. If there is an image, this will be set to false before onInit is complete

      this.accountService.getUserInfo().subscribe((u) => {
        this.user = u;
        this.avatarFirstName = this.data.firstName || this.user.firstName;
        this.avatarLastName = this.data.lastName || this.user.lastName;
      });

      // if the imagetype and imagesize are not defined, we assume the modal is editing a profile image.
      this.imageType = this.imageType || ImageType.profile;
      this.imageSize = this.imageSize || ImageSizeType.XL;
    }
    // call to make sure that we are checking about an existing image being passed into the component
    this.checkForExistingImage();

    /**
     * --- Chrome & FF styling for zoom slider ---
     * Chrome/FF browsers do not recognize :before & :after pseudo-classes and the only way to show a before/after
     * color on the input range is to watch the document for changes and then use JQuery to change the colors with
     * a linear gradient style.
     */
    $(document).on('input', '#cropit-image-zoom-input', function () {
      var $this = $(this);
      var percent = $this.val() * 100;
      $this.css(
        'background',
        'linear-gradient(to right, #FAE47D 0%, #FAE47D ' +
          percent +
          '%, #dbdbdb ' +
          percent +
          '%, #dbdbdb 100%)'
      );
    });

    this.imageSource = this.oldImage ? this.oldImage[this.oldImageKey] : null;

    // starts the cropit plugin and injects options, etc
    this.initializeCropit();

    /**
     * These listeners handle two very important events for us. First, they
     * are listening to any drag event on the window and making sure that if the user
     * drops a file onto the window while we are showing them the image upload diretive
     * that the browser will not try to load the file. This is useful so that if the user accidentally
     * doesn't drop the file on the image uploader div, they won't find themselves looking at a browser page
     * of just their image.
     *
     * Secondly, it is also keeping track of our current states and telling our directive if the user
     * has dropped an image and setting a bool that controls the correct buttons showing up on the modal.
     */
    window.addEventListener(
      'dragover',
      (e: Event) => {
        e.preventDefault();
        let body = $(e.target).closest('.modal-body');
        if (body.length) this.dragAndDrop = true;
      },
      false
    );
    window.addEventListener(
      'drop',
      (e: Event) => {
        e.preventDefault();
      },
      false
    );
  }

  /**
   * Closes the currently shown modal.
   *
   */
  cancel(): void {
    this.dialogRef.close({ ...this.employeeImage, imageHasChanges: false });
  }

  /**
   * Upload the image that is currently in the cropit plugin.
   *
   */
  upload(): void {
    this.msgSvc.sending(true);
    let data: string = this.cropper.cropit('export', {
      type: 'image/jpeg',
      quality: 0.9,
    });

    if (data == null) {
      this.msgSvc.showErrorMsg(
        'Sorry, it looks like something went wrong. Please try again.'
      );
      this.dialogRef.close(null);
      //this.modalInstance.dismiss('cancel');
    }

    // Checks the imageType... right now, the profile image open modal stuff is not setting an image type...
    // So, the default is going to be profile image settings if the imagetype and imagesize are not defined
    // when the modal is opened.
    if (
      this.imageType == ImageType.companyLogo ||
      this.imageType == ImageType.companyHero
    ) {
      this.saveClientImageResource(data);
    } else {
      this.saveProfileImage(data);
    }
  }

  /**
   * Closes the modal and returns a successful result to the trigger's promise. Values of
   * the image should be updated prior to calling this if source anything needs to change.
   *
   */
  closeModal(employeeAvatar: IEmployeeAvatars = null): void {
    if (employeeAvatar) this.dialogRef.close(employeeAvatar as any);
    else this.dialogRef.close({ ...this.employeeImage, imageHasChanges: true });
  }

  change(): void {
    // have to use timeout because this jquery click creates a new angular digest
    // cycle that will conflict with the current one if we try to make it happen
    // immediately.
    setTimeout(() => {
      $('#cropit-image-input').click();
    }, 100);
  }

  /**
   * Ask to make sure the user wants to change and replace any currently saved photo.
   */
  confirmSave(): void {
    this.saved = true;
    //Saving Employee Avatar
    if (this.useEmployeeAvatar) {
      this.saveEmployeeAvatar();
    }
    //Saving Photo
    else {
      if (this.isExistingImage && this.hasImageChanged) {
        const confirmDialogRef = this.dialog.open(ConfirmModalComponent, {
          width: '350px',
          data: {
            displayText: 'Are you sure you want to change this photo?',
            cautionText:
              'The current photo will be replaced. This change cannot be undone.',
            confirmButtonText: 'Change',
            cancelButtonText: 'Cancel',
            swapOkClose: false,
          },
        });

        confirmDialogRef.afterClosed().subscribe((confirmed: boolean) => {
          if (confirmed) {
            this.msgSvc.sending(true);
            this.upload();
          }
        });
      } else {
        this.msgSvc.sending(true);
        this.upload();
      }
    }
  }
  confirmDelete(): void {
    const confirmDialogRef = this.dialog.open(ConfirmModalComponent, {
      width: '350px',
      data: {
        displayText: 'Are you sure you want to delete this photo?',
        cautionText: 'This action cannot be undone.',
        confirmButtonText: 'Delete',
        cancelButtonText: 'Cancel',
        swapOkClose: true,
      },
    });

    confirmDialogRef.afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        this.msgSvc.sending(true);
        this.delete();
      }
    });
  }

  toggleEmployeeAvatar(showInitials: boolean): void {
    this.useEmployeeAvatar = showInitials;
    if (!showInitials) {
      this.imageSource = this.oldImage ? this.oldImage[this.oldImageKey] : null;
      // starts the cropit plugin and injects options, etc
      this.initializeCropit();
    }
  }

  changeEmployeeAvatarColor(backgroundColor) {
    this.employeeAvatarColor = backgroundColor;
  }

  saveEmployeeAvatar() {
    //Create Save object
    var dto: IEmployeeAvatars = {
      employeeAvatarId:
        this.employeeImage._employeeAvatar != null || undefined
          ? this.employeeImage._employeeAvatar.employeeAvatarId
          : 0,
      employeeId: this.employeeImage.employeeId,
      clientId: this.employeeImage.clientId,
      avatarColor: this.employeeAvatarColor, //setting new color
    };
    //If there is an existing image, and we are saving Avatar, delete existing image
    if (this.isExistingImage || this.hasImageChanged) {
      const confirmDialogRef = this.dialog.open(ConfirmModalComponent, {
        width: '350px',
        data: {
          displayText: 'Saving this will delete your photo',
          cautionText: 'This action cannot be undone.',
          confirmButtonText: 'Save',
          cancelButtonText: 'Cancel',
          swapOkClose: true,
        },
      });
      confirmDialogRef.afterClosed().subscribe((confirmed: boolean) => {
        if (confirmed) {
          this.msgSvc.sending(true);
          //Save
          this.employeeApiService.saveEmployeeAvatar(dto).subscribe((res) => {
            this.employeeAvatarColor = res.avatarColor;
            this.employeeImage._employeeAvatar = res;
            this.clearOutImage();
            this.delete(res);
          });
        }
      });
    } else {
      //Save
      this.employeeApiService.saveEmployeeAvatar(dto).subscribe((res) => {
        this.employeeAvatarColor = res.avatarColor;
        this.employeeImage._employeeAvatar = res;
        this.clearOutImage();
        this.dialogRef.close({ ...this.employeeImage, imageHasChanges: true });
      });
    }
  }

  // PRIVATE FUNCTIONS

  private setCropitMethodsAndOptions() {
    //#region cropit options & methods
    this.methods = {
      rotate: (isClockwise: boolean) => {
        let direction = isClockwise ? 'rotateCW' : 'rotateCCW';
        $('.image-editor').cropit(direction);
        this.hasImageChanged = true;
        this.outlinePrimaryClass =
          this.showEditControls && this.hasImageChanged;
        this.closeCancelButtonText = this.outlinePrimaryClass
          ? CANCEL_TEXT
          : CLOSE_TEXT;
      },
    };

    this.defaultOptions = {
      smallImage: 'allow',
      imageBackground: true,
      imageBackgroundBorderWith: 15,
      initialZoom: 'min',
      onImageLoading: () => {
        this.hideZoom = false;
        if (this.dragAndDrop) {
          this.imagePickerButtonText = CHANGE_TEXT;
          this.showEditControls = true;
          this.hasImageChanged = true;
          this.outlinePrimaryClass = true;
          this.closeCancelButtonText = this.outlinePrimaryClass
            ? CANCEL_TEXT
            : CLOSE_TEXT;
        }

        this.destroyClickEventListener();

        this.isWrongFileType = false;
      },
      onFileChange: (event: any) => {
        if (!event.currentTarget.files.length) {
          this.isWrongFileType = false;
          return;
        }

        if (
          event.currentTarget.files[0].type.match('image/jpeg') ||
          event.currentTarget.files[0].type.match('image/png')
        ) {
          this.isWrongFileType = false;
          this.dragAndDrop = false;

          this.showEditControls = true;
          this.hasImageChanged = true;
          this.outlinePrimaryClass = true;
          this.imagePickerButtonText = CHANGE_TEXT;
          this.closeCancelButtonText = this.outlinePrimaryClass
            ? CANCEL_TEXT
            : CLOSE_TEXT;

          this.destroyClickEventListener();
          this.resetZoomState();
        } else {
          this.isWrongFileType = !this.isWrongFileType;
          if (!this.isWrongFileType) {
            this.removeCropitImage();
            this.showWrongFileMessage();
            this.restartCropit();
          }
        }
      },
      onZoomDisabled: () => {
        this.hideZoom = !this.hideZoom ? true : this.hideZoom;
      },
      onFileReaderError: () => {
        if (this.isWrongFileType) {
          this.removeCropitImage();
          this.showWrongFileMessage();
          this.restartCropit();
        }
      },
    };
    //#endregion
  }

  /**
   * Checks for existing image being passed in via the ImageDto or IEmployeeSearchResult DTO.
   * If there is one, sets the key information for the cropper and some of the controls necessary.
   *
   */
  /**
   * Takes an Blob class image and sends to backend for processing and saving to Azure.
   *
   * @param data
   */
  private saveClientImageResource(data: string): void {
    this.resourceApiService
      .saveClientImageResource(
        this.image.resourceId,
        this.image.clientId,
        this.image.name,
        data
      )
      .subscribe((image: IAzureViewDto) => {
        this.msgSvc.setTemporarySuccessMessage(
          'Your image has been saved successfully.',
          6000
        );
        this.hasImageChanged = true;
        this.isExistingImage = true;
        this.source = image.source;
        this.token = image.token;
        this.dialogRef.close(this.source as any);

        this.image = <ImageDto>{
          resourceId: image.resourceId,
          clientId: image.clientId,
          hasImage: image.source != null,
          imageSize: image.size,
          imageType: image.type,
          source: image.source + image.token,
          token: image.token,
          name: image.name,
        };
      });
  }

  /**
   * Takes an Blob class image and sends to backend for processing and saving to Azure.
   *
   * @param data
   */
  private saveProfileImage(data: string): void {
    this.clientId = this.employeeImage.clientId || this.clientId;
    this.employeeId = this.employeeImage.employeeId || this.employeeId;

    this.resourceApiService
      .saveProfileImage(this.clientId, this.employeeId, data)
      .pipe(
        withLatestFrom(
          this.resourceApiService.getProfileImageSas(
            this.clientId,
            this.employeeId
          )
        )
      )
      .subscribe(([image, token]: [IProfileImageDto, string]) => {
        this.msgSvc.setTemporarySuccessMessage(
          'Your image has been uploaded successfully.',
          6000
        );
        this.token = token;
        this.source = image.source + this.token;

        this.employeeImage = this.setImageDetails(
          image,
          token,
          this.employeeImage
        );
        this.hasImageChanged = this.isExistingImage = true;

        this.dialogRef.close({ ...this.employeeImage, imageHasChanges: true });
      });
  }

  private checkForExistingImage(): void {
    this.options = { ...this.defaultOptions, ...this.cropitOptions };
    //this.options = angular.extend({}, this.defaultOptions, this.cropitOptions);

    if ( this.employeeImage &&
      this.employeeImage.profileImageInfo &&
      this.employeeImage.profileImageInfo.length
    ) {
      this.employeeId = this.employeeImage.employeeId;
      this.clientId = this.employeeImage.clientId;

      if (this.oldImage == null) this.oldImage = { ...this.employeeImage };
      else {
        this.oldImage.extraLarge = { ...this.employeeImage.extraLarge };
      }

      this.isExistingImage = true;
      this.useEmployeeAvatar = false;
      this.token = this.employeeImage.sasToken;
      this.source = this.oldImage['extraLarge'].url;
      this.imagePickerButtonText = CHANGE_TEXT;
    } else if (this.image) {
      this.token = this.image.token;
      this.source = this.image.source;
      this.isExistingImage = true;
      this.useEmployeeAvatar = false;
      this.employeeId = this.image.employeeId;
      this.clientId = this.image.clientId;
      let imageKey: string = this.getImageKeyDescription(this.image.imageSize);

      if (imageKey == null) return;

      this.oldImageKey = imageKey;
      this.oldImage = <IEmployeeImage>{};
      this.oldImage[imageKey] = {
        hasImage: true,
        url: this.image.source,
      };
      this.imagePickerButtonText = CHANGE_TEXT;
    } else {
      // get resources needs to upload a brand new image

      // setting the image description name... defaults to profile
      let name =
        this.imageType == ImageType.companyLogo
          ? 'logo'
          : this.imageType == ImageType.companyHero
          ? 'hero'
          : 'profile';

      this.accountService.getUserInfo().subscribe((u) => {
        this.user = u;
        this.image = {
          resourceId: null,
          clientId: this.user.selectedClientId(),
          employeeId: null,
          hasImage: false,
          imageSize: this.imageSize,
          imageType: this.imageType,
          name: name,
          source: null,
          token: null,
        };
        this.clientId = this.user.selectedClientId();
        this.employeeId = this.user.lastEmployeeId;
      });
    }
  }

  /**
   * Handles instantiating JQuery Cropit plugin on the DOM so that the user may interact with it.
   */
  private initializeCropit(): void {
    setTimeout(() => {
      this.cropper = $('div#image-editor');
      if (this.source) {
        let image = new Image();
        let canvas = document.createElement('canvas');
        let canvasContext = canvas.getContext('2d');
        image.crossOrigin = 'anonymous';
        image.onload = () => {
          canvas.width = image.width;
          canvas.height = image.height;
          canvasContext.drawImage(image, 0, 0, image.width, image.height);
          let dataUrl = canvas.toDataURL();
          this.cropper.cropit('imageSrc', dataUrl);
          this.showEditControls = false;
        };
        image.src = this.source;

        this.imagePickerButtonText = CHANGE_TEXT;
      }

      this.cropper.cropit(this.options);

      if (!this.isExistingImage) {
        this.preview = $('.cropit-preview-image-container');
        this.preview.off('click').on('click', function (e: Event) {
          e.preventDefault();
          e.stopPropagation();
          document.getElementById('cropit-image-input').click();
        });
      }
    }, 100);
  }

  private delete(employeeAvatar: IEmployeeAvatars = null): void {
    this.removeCropitImage();

    // call the correct delete method based on the image type
    if (
      this.imageType == ImageType.companyLogo ||
      this.imageType == ImageType.companyHero
    ) {
      this.deleteClientResourceImage(employeeAvatar);
    } else {
      this.deleteProfileImage(employeeAvatar);
    }
  }

  private deleteClientResourceImage(
    employeeAvatar: IEmployeeAvatars = null
  ): void {
    this.image.name =
      this.image.imageType == ImageType.companyLogo ? 'logo' : 'hero';

    this.resourceApiService
      .deleteClientImageResource(
        this.clientId,
        this.image.resourceId,
        this.image.name
      )
      .subscribe(() => {
        this.isExistingImage = false;
        this.isWrongFileType = false;
        this.hasImageChanged = false;
        this.hideZoom = false;

        if (this.oldImageKey) {
          this.oldImage[this.oldImageKey].hasImage = false;
          this.oldImage[this.oldImageKey].url = null;
        }

        this.source = null;
        this.token = null;

        this.outlinePrimaryClass = false;
        this.restartCropit();
        this.closeModal(employeeAvatar);
      });
  }

  private deleteProfileImage(employeeAvatar): void {
    this.resourceApiService
      .deleteProfileImage(this.clientId, this.employeeId)
      .subscribe(() => {
        this.msgSvc.setTemporarySuccessMessage(
          'Your image has been deleted successfully.',
          6000
        );
        this.isExistingImage = false;
        this.isWrongFileType = false;
        this.hideZoom = false;

        if (this.oldImage && this.oldImage[this.oldImageKey]) {
          this.oldImage[this.oldImageKey].hasImage = false;
          this.oldImage[this.oldImageKey].url = null;
        }

        this.source = null;
        this.token = null;

        this.outlinePrimaryClass = false;

        this.restartCropit();
        this.dialogRef.close({ ...this.employeeImage, imageHasChanges: true });
      });
  }

  /**
   * Remove click event listener that handles changing image on the page registered by cropit.
   *
   */
  private destroyClickEventListener(): void {
    setTimeout(() => {
      $('.cropit-preview-image-container').off('click');
    }, 100);
  }

  /**
   * Reset zoom slider state based on the browser.
   *
   * TODO: Does this handle IE? Is it necessary to do something different here for IE?
   */
  private resetZoomState(): void {
    setTimeout(() => {
      let $input = $('#cropit-image-zoom-input');

      if (navigator.userAgent.toLowerCase().match('edge')) {
        $input.css({
          background:
            'linear-gradient(to right, #FAE47D 0%, #FAE47D 0%, #dbdbdb 0%, #dbdbdb 100%)',
          height: '18px',
        });
      } else if (navigator.userAgent.toLowerCase().match('firefox')) {
        $input.css({
          background:
            'linear-gradient(to right, #FAE47D 0%, #FAE47D 0%, #dbdbdb 0%, #dbdbdb 100%)',
          height: '6px',
        });
      } else if (navigator.userAgent.toLowerCase().match('chrome')) {
        $input.css(
          'background',
          'linear-gradient(to right, #FAE47D 0%, #FAE47D 0%, #dbdbdb 0%, #dbdbdb 100%)'
        );
      }
    }, 100);
  }

  /**
   * Resets input range style, reinitializes the cropit plugin on our element
   * and then removes any leftover cropit click handlers and reattaches them to bring the
   * entire plugin to a new state.
   */
  private restartCropit(): void {
    this.resetZoomState();
    setTimeout(() => {
      $('.image-editor').cropit('reenable');
      $('.cropit-preview-image-container')
        .off('click')
        .on('click', function (e) {
          e.preventDefault();
          e.stopPropagation();
          document.getElementById('cropit-image-input').click();
        });
    }, 100);
  }

  /**
   * If the user tries to upload a bad file type, we remove the current image in the cropit
   * and display the error message to them.
   */
  private removeCropitImage() {
    setTimeout(() => {
      $('.cropit-preview-background').removeAttr('src');
      $('.cropit-preview-image').removeAttr('src');
    }, 100);
  }

  /**
   * Show error message in the cropit plugin
   */
  private showWrongFileMessage(): void {
    this.showEditControls = false;
    this.outlinePrimaryClass = false;
    this.isWrongFileType = true;
  }

  /**
   * Set the image details for employee that gets saved back to the ImageModalService to be
   * passed back and forth from the modal to the main template.
   *
   * @param image
   * @param token
   * @param profileImage
   */
  private setImageDetails(
    image: IProfileImageDto,
    token: string,
    profileImage: IEmployeeImage
  ): IEmployeeImage {
    let result = { ...profileImage } as IEmployeeImage;
    let img: IImage = {
      hasImage: true,
      url: image.source,
    };

    result.sasToken = token;

    switch (image.imageSizeType) {
      case ImageSizeType.SM:
        result.small = img;
        this.oldImageKey = 'small';
        break;
      case ImageSizeType.MD:
        result.medium = img;
        this.oldImageKey = 'medium';
        break;
      case ImageSizeType.LG:
        result.large = img;
        this.oldImageKey = 'large';
        break;
      case ImageSizeType.XL:
        result.extraLarge = img;
        this.oldImageKey = 'extraLarge';
        break;
      default:
        break;
    }

    if (result.extraLarge) result.extraLarge.url = img.url + token;

    if (result.profileImageInfo && result.profileImageInfo.length) {
      result.profileImageInfo[0] = image;
    } else {
      result.profileImageInfo = [image];
    }

    this.oldImage = result;
    return result;
  }

  /**
   * Takes the database image size type and returns the model property description.
   *
   * @param imageSize
   */
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

  clearOutImage() {
    if (
      this.employeeImage.extraLarge &&
      this.employeeImage.extraLarge.hasImage
    ) {
      this.employeeImage.extraLarge.hasImage = false;
      this.employeeImage.extraLarge.url = null;
    }

    if (
      this.employeeImage.profileImageInfo &&
      this.employeeImage.profileImageInfo.length
    ) {
      this.employeeImage.profileImageInfo = [];
    }
  }

  updateAvatartOnOldImage() {}

  ngAfterViewInit() {}
}

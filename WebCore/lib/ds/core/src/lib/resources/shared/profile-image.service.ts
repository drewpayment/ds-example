import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AccountService } from "@ds/core/account.service";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
import { UserInfo } from "@ds/core/shared";
import { IEmployee } from '@ds/core/resources/shared/employee.model';
import { IProfileImageDto } from '@ds/core/resources/shared/profile-image.model';
import { ImageDto } from '@ds/core/resources/shared/employee-image.model';
import { ImageType } from '@ds/core/resources/shared/image-type.model';
import { ImageSizeType } from '@ds/core/resources/shared/image-size-type.model';
import { ICropitOptions } from '@ds/core/resources/shared/cropit-options.model';
import { ResourceType } from '@ds/core/resources/shared/resource-type.model';
import { IEmployeeSearchResult } from 'lib/ds/employees/src/lib/search/shared/models/employee-search-result';

@Injectable({
    providedIn: 'root'
})
export class ProfileImageService {
    private readonly api = 'api';
    user: UserInfo;

    private currentEmployee: IEmployeeSearchResult;
    private employeeImageSource: string;
    private employeeSasToken: string;
    private employeeList: IEmployeeSearchResult[];

    private image: ImageDto;
    private imageKey: string;
    private imageType: ImageType;
    private imageSize: ImageSizeType;

    private userCropitOptions: ICropitOptions;

    constructor(
        private http: HttpClient,
        private accountService: AccountService,
        private msg: DsMsgService

    ) {
        this.currentEmployee = <IEmployeeSearchResult>{};
        this.employeeImageSource = null;
        this.employeeSasToken = null;
    }

    getCurrentEmployee(): IEmployeeSearchResult {
        return this.currentEmployee;
    }

    setCurrentEmployee(employee: IEmployeeSearchResult | IEmployee, imageType: ImageType = null, imageSize: ImageSizeType = null): void {
        if (imageType) this.setImageType(imageType);
        if (imageSize) this.setImageSize(imageSize);

        for (var e in employee) {
            // skip loop if the prop is from prototype
            if (!employee.hasOwnProperty(e)) continue;

            // check if value is different and set if it doesn't match new value
            if (this.currentEmployee[e] != employee[e]) this.currentEmployee[e] = employee[e];
        }

        var thisImage: IProfileImageDto;
        for (let i = 0; i < employee.profileImage.profileImageInfo.length; i++) {
            let info = employee.profileImage.profileImageInfo[i];
            if (info.source == null) continue;
            thisImage = info;
        }

        if (thisImage == null) {
            thisImage = {
                imageSizeType: this.imageSize || ImageSizeType.XL,
                imageType: this.imageType || ImageType.profile,
                sourceTypeId: ResourceType.AzureProfileImage,
                resourceId: null,
                source: null
            }

            if (employee.profileImage.extraLarge && employee.profileImage.extraLarge.hasImage) {
                thisImage.source = employee.profileImage.extraLarge.url;
            } else if (employee.profileImage.large && employee.profileImage.large.hasImage) {
                thisImage.source = employee.profileImage.large.url;
            } else if (employee.profileImage.medium && employee.profileImage.medium.hasImage) {
                thisImage.source = employee.profileImage.medium.url;
            } else if (employee.profileImage.small && employee.profileImage.small.hasImage) {
                thisImage.source = employee.profileImage.small.url;
            }
        }

        if (thisImage) this.imageKey = this.getImageSizeTypeKey(thisImage.imageSizeType);

        var imageCompare: ImageDto = {
            employeeId: employee.employeeId,
            clientId: employee.clientId,
            token: employee.profileImage.sasToken,
            imageType: thisImage.imageType,
            imageSize: thisImage.imageSizeType,
            hasImage: thisImage.source != null,
            source: thisImage.source
        };

        if (this.image == null || JSON.stringify(imageCompare) !== JSON.stringify(this.image)) this.image = imageCompare;
    }

    setCropitOptions(options: ICropitOptions): void {
        this.userCropitOptions = options;
    }

    getCropitOptions(): ICropitOptions {
        return this.userCropitOptions;
    }

    setImage(image: ImageDto): void {
        if (JSON.stringify(image) === JSON.stringify(this.image)) return;
        this.setEmployeeImageSource(image.source);
        let key = this.getImageSizeTypeKey(image.imageSize);
        this.image = image;
        this.imageKey = key;
    }

    getImage(): ImageDto {
        return this.image;
    }

    setImageType(type: ImageType): void {
        this.imageType = type;
    }

    getImageType(): ImageType {
        return this.imageType;
    }

    setImageSize(size: ImageSizeType): void {
        this.imageSize = size;
    }

    getImageSize(): ImageSizeType {
        return this.imageSize;
    }

    setImageKey(key: string): void {
        this.imageKey = key;
    }

    getImageKey(): string {
        return this.imageKey;
    }

    getEmployeeImageSource(): string {
        return this.employeeImageSource;
    }

    setEmployeeImageSource(url: string): void {
        this.employeeImageSource = url;
    }

    getEmployeeSasToken(): string {
        return this.employeeSasToken;
    }

    setEmployeeSasToken(token: string): void {
        this.employeeSasToken = token;
    }

    setEmployeeList(employeeList: IEmployeeSearchResult[]): void {
        this.employeeList = employeeList;
    }

    getEmployeeList(): IEmployeeSearchResult[] {
        return this.employeeList;
    }

    getImageSizeTypeKey(type: ImageSizeType): string {
        let result: string;
        switch (type) {
            case ImageSizeType.XL:
                result = 'extraLarge';
                break;
            case ImageSizeType.LG:
                result = 'large';
                break;
            case ImageSizeType.MD:
                result = 'medium';
                break;
            case ImageSizeType.SM:
                result = 'small';
                break;
        }
        return result;
    }

    //getPermissions(isAllowedActions: boolean, permissions: string[]): ng.IPromise<{}> {
    //    let result = {};
    //    return this.accountService.getAllowedActions(isAllowedActions)
    //        .then((actions: string[]) => {
    //            for (let i = 0; i < permissions.length; i++) {
    //                let index = actions.indexOf(permissions[i]);
    //                // result[permissions[i]] = index > -1;
    //                // FIXME: DEVELOPMENT ONLY - need to remove before release
    //                result[permissions[i]] = true;
    //                // FIXME: DEVELOPMENT ONLY - REMOVE ^^^^ BEFORE RELEASE
    //            }
    //            return result;
    //        });
    //}
}

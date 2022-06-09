import { IProfileImage } from './profile-image.model';
import { IContactSearchResult } from './contact-search-result.model';
import { IContact } from './contact.model';
import { IEmployeeImage, IImage, ImageSizeType } from '@ajs/core/ds-resource/models';
import { IEEOCEmployeeInfo } from '@ds/core/employees/shared/employee-eeoc.model';
import { IEmployeeSearchResult } from '@ds/employees/search/shared/models/employee-search-result';

export function LoadProfileImages(profile: IProfileImage): IProfileImage {

    if (!profile)
        return profile;

    if (!profile.profileImageInfo)
        profile.profileImageInfo = [];

    profile.profileImageInfo.forEach(info => {
        const image: IImage = {
            hasImage: true,
            url: `${info.source}${profile.sasToken}`
        };
        switch (info.imageSizeType) {
            case ImageSizeType.XL:
                profile.extraLarge = image;
                break;
            case ImageSizeType.LG:
                profile.large = image;
                break;
            case ImageSizeType.MD:
                profile.medium = image;
                break;
            case ImageSizeType.SM:
                profile.small = image;
                break;
        }
    });

    const nullImage: IImage = {
        hasImage: false,
        url: null
    };
    if (!profile.extraLarge)
        profile.extraLarge = nullImage;
    if (!profile.large)
        profile.large = nullImage;
    if (!profile.medium)
        profile.medium = nullImage;
    if (!profile.small)
        profile.small = nullImage;

    return profile;
}

export function ContactProfileImageLoader(contact: IContact): IContact {
    if (!contact)
        return contact;

    LoadProfileImages(contact.profileImage);

    return contact;
}

export function ContactsProfileImageLoader(contacts: IContact[]): IContact[] {
    if (!contacts)
        return contacts;

    contacts.forEach(ContactProfileImageLoader);

    return contacts;
}

export function ContactSearchResultProfileImageLoader(result: IContactSearchResult): IContactSearchResult {
    if (!result)
        return result;

    ContactsProfileImageLoader(result.results);

    return result;
}

export class ProfileImageLoaderPipe {

    constructor() { }

    static EmployeeProfileImageLoader(employee: IEmployeeSearchResult): IEmployeeSearchResult {
        if (!employee) return employee;

        this.LoadEmployeeProfileImages(employee.profileImage);
    }

    static EEOCEmployeeProfileImageLoader(employee: IEEOCEmployeeInfo): IEEOCEmployeeInfo {
        if (!employee) return employee;

        this.LoadEmployeeProfileImages(employee.profileImage);
    }

    static LoadEmployeeProfileImages(profile: IEmployeeImage): IEmployeeImage {

        if (!profile)
            return profile;

        if (!profile.profileImageInfo)
            profile.profileImageInfo = [];

        profile.profileImageInfo.forEach(info => {
            const image: IImage = {
                hasImage: true,
                url: `${info.source}${profile.sasToken}`
            };
            switch (info.imageSizeType) {
                case <number>ImageSizeType.XL:
                    profile.extraLarge = image;
                    break;
                case <number>ImageSizeType.LG:
                    profile.large = image;
                    break;
                case <number>ImageSizeType.MD:
                    profile.medium = image;
                    break;
                case <number>ImageSizeType.SM:
                    profile.small = image;
                    break;
            }
        });

        const nullImage: IImage = {
            hasImage: false,
            url: null
        };
        if (!profile.extraLarge)
            profile.extraLarge = nullImage;
        if (!profile.large)
            profile.large = nullImage;
        if (!profile.medium)
            profile.medium = nullImage;
        if (!profile.small)
            profile.small = nullImage;

        return profile;
    }
}

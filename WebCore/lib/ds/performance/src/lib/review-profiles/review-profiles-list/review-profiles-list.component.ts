import { Component, OnInit } from '@angular/core';
import { IReviewProfileBasic } from '../shared/review-profile-basic.model';
import { ReviewProfilesApiService } from '../review-profiles-api.service';
import { ReviewProfileContextService } from '../review-profile-context.service';
import { IReviewProfileSetup } from '../shared/review-profile-setup.model';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { IDsConfirmOptions } from '@ajs/ui/confirm/ds-confirm.interface';

interface IReviewProfileView extends IReviewProfileBasic {
    hovered: boolean;
}

@Component({
    selector: 'ds-review-profiles-list',
    templateUrl: './review-profiles-list.component.html',
    styleUrls: ['./review-profiles-list.component.scss']
})
export class ReviewProfilesListComponent implements OnInit {

    @Output()
    editProfileChange = new EventEmitter<IReviewProfileSetup>();

    reviewProfiles: IReviewProfileView[];
    isLoading = true;
    isArchivedView = false;

    get hasReviewProfiles() {
        return this.reviewProfiles && this.reviewProfiles.length;
    }

    constructor(
        private profileSvc: ReviewProfilesApiService,
        private profileCtx: ReviewProfileContextService,
        private msgSvc: DsMsgService,
        private confirmSvc: DsConfirmService) {
    }

    ngOnInit() {
        this.reloadProfiles(false, false);
    }

    addProfile() {
        this.profileCtx.addReviewProfile().subscribe(profile => this.onSetupProfileChanged(profile));
    }

    editProfile(profile: IReviewProfileBasic) {
        this.profileCtx.loadActiveReviewProfileSetup(profile.reviewProfileId).subscribe(profile => this.onSetupProfileChanged(profile));
    }

    onSetupProfileChanged(profile:IReviewProfileSetup) {
        if(profile)
            this.editProfileChange.emit(profile);
    }

    viewArchived() {
        this.reloadProfiles(true);
    }

    viewCurrent() {
        this.reloadProfiles(false);
    }

    archive(profile: IReviewProfileBasic) {

        let opts: IDsConfirmOptions = {
            actionButtonText: "Archive",
            bodyText: "Archive review profile?  Profile will no longer be available when creating or scheduling reviews.",
            closeButtonText: "Cancel"
        };

        this.confirmSvc.showModal(null, opts).then(() => {
            this.msgSvc.loading(true);
            this.profileSvc.archiveReviewProfile(profile.reviewProfileId)
                .subscribe(
                    () => {
                        this.reviewProfiles = this.reviewProfiles.filter(x => x.reviewProfileId !== profile.reviewProfileId);
                        this.msgSvc.loading(false);
                    },
                    (error:HttpErrorResponse) => this.msgSvc.showWebApiException(error.error));
        });
    }

    restore(profile: IReviewProfileBasic) {
        this.msgSvc.loading(true);
        this.profileSvc.restoreReviewProfile(profile.reviewProfileId)
            .subscribe(
                () => {
                    this.reviewProfiles = this.reviewProfiles.filter(x => x.reviewProfileId !== profile.reviewProfileId);
                    this.msgSvc.loading(false);
                },
                (error:HttpErrorResponse) => this.msgSvc.showWebApiException(error.error));
    }

    private reloadProfiles(isArchived = false, displayLoadingMessage = true) {
        if (displayLoadingMessage)
            this.msgSvc.loading(true);

        this.profileSvc.getReviewProfiles(isArchived)
            .subscribe(
                profiles => {
                    this.reviewProfiles = <IReviewProfileView[]>profiles;
                    this.isLoading = false;
                    this.isArchivedView = isArchived;

                    if (displayLoadingMessage)
                        this.msgSvc.loading(false);
                },
                (error:HttpErrorResponse) => this.msgSvc.showWebApiException(error.error));
    }
}

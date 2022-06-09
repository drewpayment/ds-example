import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileImageComponent } from './profile-image/profile-image.component';
import { ImageUploaderComponent } from './image-uploader/image-uploader.component';
import { ConfirmModalComponent } from './confirm-modal/confirm-modal.component';
import { HttpClientModule } from '@angular/common/http';
import { DsDialogModule } from '@ds/core/ui/ds-dialog';
import { AvatarModule } from '../ui/avatar/avatar.module';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../ui/material';


@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        DsDialogModule,
        FormsModule,
        AvatarModule,
        MaterialModule
    ],
    declarations: [
        ProfileImageComponent,
        ImageUploaderComponent,
        ConfirmModalComponent
    ],
    exports: [
        ProfileImageComponent,
        ImageUploaderComponent,
        ConfirmModalComponent
    ],
    entryComponents:[
        ImageUploaderComponent,
        ConfirmModalComponent
    ]
})
export class DsCoreResourcesModule { }

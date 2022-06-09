import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { IReviewRating } from "../";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

export interface RatingsEditDialogData {
    rating:IReviewRating,
    allRatings:IReviewRating[]
}

@Component({
  selector: 'ds-ratings-edit-dialog',
  templateUrl: './ratings-edit-dialog.component.html',
  styleUrls: ['./ratings-edit-dialog.component.scss']
})
export class RatingsEditDialogComponent {
    form:FormGroup;
    rating:IReviewRating;
    allRatings:IReviewRating[];
    modelChanged:boolean;
    
    constructor(
        public dialogRef:MatDialogRef<RatingsEditDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:RatingsEditDialogData,
        private fb:FormBuilder
    ) {
        this.rating = data.rating;
        this.allRatings = data.allRatings;
        this.createForm();
    }

    saveRating():void {
        if(this.form.invalid) return;
        if(!this.modelChanged) {
            this.onNoClick();
            return;
        }
        this.rating = this.prepareSaveRating();
        this.resetForm();
        this.dialogRef.close(this.rating);
    }

    onNoClick():void {
        this.dialogRef.close();
    }

    changedValidity():void {
        for(const p in this.form.value) {
            this.modelChanged = this.form.value[p] != this.rating[p];
            if(this.modelChanged) break;
        }
    }

    private createForm():void {
        this.form = this.fb.group({
            label: [this.rating.label || '', Validators.required],
            description: [this.rating.description || '', [Validators.required, Validators.maxLength(150)]]
        });
    }

    private resetForm():void {
        this.form.reset({
            label: this.rating.label || '',
            description: this.rating.description || '',
            rating: this.rating.rating || ''
        });
    }

    private prepareSaveRating():IReviewRating {
        const model = this.form.value;

        const saveRating:IReviewRating = {
            reviewRatingId: this.rating.reviewRatingId,
            clientId: this.rating.clientId,
            label: model.label as string,
            description: model.description as string,
            rating: this.rating.rating
        };

        return saveRating;
    }
}

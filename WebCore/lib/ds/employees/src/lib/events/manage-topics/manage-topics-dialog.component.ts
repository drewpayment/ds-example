import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ICustomizeSenderData, IEmailData } from "apps/ds-company/src/app/models/correspodence-template-data";
import { CorrespondenceTemplateApiService } from "apps/ds-company/src/app/services/correspondence-template-api.service";
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";
import { catchError, switchMap, tap } from "rxjs/operators";
import { of } from "rxjs/internal/observable/of";
import { HttpErrorResponse } from "@angular/common/http";
import { ConfirmDialogService } from "@ds/core/ui/ds-confirm-dialog/ds-confirm-dialog.service";
import { EMPTY } from "rxjs";
import { IClientTopic, IClientSubTopic } from "@ds/core/shared/event.model";
import { EventService } from "lib/services/event.service";

@Component({
  selector: 'ds-manage-topics-dialog',
  templateUrl: './manage-topics-dialog.component.html',
  styleUrls: ['./manage-topics-dialog.component.scss']
})
export class ManageTopicsDialogComponent implements OnInit {

    clientId: number;
    dataSaving: boolean = false;
    errMessage: string;
    formSubmitted: boolean = false;
    selectedClientTopic:IClientTopic;
    selectedClientSubTopic:IClientSubTopic;
    clientTopics:IClientTopic[] = [];
    clientSubTopics:IClientSubTopic[] = [];


    form: FormGroup;
    //form2: FormGroup;
    constructor(
        public dialogRef:MatDialogRef<ManageTopicsDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data:any,
        private eventApi: EventService,
        private fb: FormBuilder,
        private msg: NgxMessageService,
        private confirmDialog: ConfirmDialogService,
    ) {}

    ngOnInit() {
        this.clientId = this.data.clientId;
        this.createTopicForm();
        //this.createSubTopicForm();
        this.loadClientTopics();
    }

    loadClientTopics() {
        this.clientTopics = [];
        this.eventApi.getClientTopics(this.clientId).subscribe(clientTopics => {
            this.clientTopics = clientTopics || [];
        });
    }

    topicChange() {
        this.clientSubTopics = [];
        let clientTopicId = Number(this.form.value.clientTopic);
        if (clientTopicId && clientTopicId > 0) {
            this.form.controls['clientTopicName'].patchValue(this.getTopicName(clientTopicId));
            //this.form.controls['clientTopicName'].patchValue(e.target.options[e.target.options.selectedIndex].text);
            this.loadSubTopics(clientTopicId);
        }
        else {
            this.form.controls['clientTopicName'].patchValue('');
            this.form.controls['clientSubTopicName'].patchValue('');
        }
    }

    subTopicChange() {
        let clientSubTopicId = Number(this.form.value.clientSubTopic);
        if (clientSubTopicId && clientSubTopicId > 0) {
            this.form.controls['clientSubTopicName'].patchValue(this.getSubTopicName(clientSubTopicId));
            //this.form.controls['clientSubTopicName'].patchValue(e.target.options[e.target.options.selectedIndex].text);
        }
        else
            this.form.controls['clientSubTopicName'].patchValue('');
    }


    loadSubTopics(topicId:number) {
        this.clientSubTopics = [];
        if (topicId) {
            this.eventApi.getClientSubTopics(this.clientId, topicId).subscribe(subs => {
                subs = subs || [];
                this.clientSubTopics = subs.filter(x => x.description);
                this.form.controls['clientSubTopic'].patchValue(0);
                this.form.controls['clientSubTopicName'].patchValue('');
            });
        }
    }

    private createTopicForm() {
        this.form = this.fb.group({
             clientTopic: this.fb.control('0', []),
             clientTopicName: this.fb.control('', [Validators.required]),

             clientSubTopic: this.fb.control('0', []),
             clientSubTopicName: this.fb.control('', [Validators.required]),
          });

          this.selectedClientTopic = {
            clientId: this.clientId,
            clientTopicId: 0,
            description: ''
          };

          this.selectedClientSubTopic = {
            clientTopicId: 0,
            topicDescription: '',
            clientSubTopicId: 0,
            description: ''
          };
    }

    saveTopic() {
        this.formSubmitted = true;

        (this.form.controls['clientSubTopicName'] as FormControl).clearValidators();
        (this.form.controls['clientSubTopicName'] as FormControl).updateValueAndValidity();

        this.form.updateValueAndValidity();
        if (this.form.invalid) return;

        this.dataSaving = true;
        this.prepareClientTopicModel();
        this.msg.loading(true,'Sending...' );

        this.eventApi.updateClientTopic(this.clientId, this.selectedClientTopic).subscribe(result => {
            if (!!result) {
                this.msg.setSuccessMessage("Topic saved successfully.");

                if (this.selectedClientTopic.clientTopicId == 0) {
                    this.clientTopics.push(result);
                    this.form.controls['clientTopic'].patchValue(result.clientTopicId);
                }
                else {
                    let item = this.clientTopics.find(x => x.clientTopicId == this.selectedClientTopic.clientTopicId);
                    if (item) 
                        item.description = result.description;
                }

                (this.form.controls['clientSubTopicName'] as FormControl).clearValidators();
                (this.form.controls['clientSubTopicName'] as FormControl).updateValueAndValidity();
        
                this.eventApi.refreshTopics(true);
            }
          },
          (error: HttpErrorResponse) => {
              if (error.error.errors[0].msg.indexOf("Description already exists") !== -1)
                this.msg.setErrorMessage('Name already exists.');
              else
              this.msg.setErrorMessage(error.error);
          });

        this.dataSaving = false;
        this.setAllValidators();
    }

    private setAllValidators() {
        (this.form.controls['clientTopicName'] as FormControl).setValidators([Validators.required]);
        (this.form.controls['clientTopicName'] as FormControl).updateValueAndValidity();
        
        (this.form.controls['clientSubTopicName'] as FormControl).setValidators([Validators.required]);
        (this.form.controls['clientSubTopicName'] as FormControl).updateValueAndValidity();
    }

    private clearAll() {
        this.form.controls['clientTopic'].patchValue(0);
        this.form.controls['clientTopicName'].patchValue('');

        this.form.controls['clientSubTopic'].patchValue(0);
        this.form.controls['clientSubTopicName'].patchValue('');
    }

    private prepareClientTopicModel(): void {
        Object.assign(this.selectedClientTopic, {
            clientTopicId: this.form.value.clientTopic,
            description: this.form.value.clientTopicName
        });
    }

    private prepareClientSubTopicModel(): void {
        Object.assign(this.selectedClientSubTopic, {
            clientTopicId: this.form.value.clientTopic,
            description: this.form.value.clientSubTopicName,
            clientSubTopicId: this.form.value.clientSubTopic,
            topicDescription: this.form.value.clientTopicName
        });
    }

    deleteTopic() {
        let topicToDelete = Number(this.form.value.clientTopic);
        let msg:string = this.getTopicName(topicToDelete) ;
        const options = {
            title: `Are you sure you want to delete ${msg}?`,
            message: "",
            confirm: "Delete",
        };
    
        this.confirmDialog.open(options);
        this.confirmDialog.confirmed()
        .pipe(
            switchMap((confirmed) => {
                if (confirmed) {
                    if(topicToDelete == 0 ) 
                        return of(true);
                    else {
                        this.msg.loading( true,'Sending...');
                        return this.eventApi.deleteClientTopic(this.clientId, topicToDelete);
                    }
                } 
                else {
                    return EMPTY;
                }
            })
        )
        .subscribe(done => {
            if(done) {
                // remove from the list
                var inx = this.clientTopics.findIndex((x) => x.clientTopicId == topicToDelete);
                if(inx>-1) this.clientTopics.splice(inx, 1);

                this.msg.setSuccessMessage('Client Topic deleted successfully.');
                this.clearAll();

                (this.form.controls['clientTopicName'] as FormControl).clearValidators();
                (this.form.controls['clientTopicName'] as FormControl).updateValueAndValidity();

                this.eventApi.refreshTopics(true);
            }}, 
            (error: HttpErrorResponse) => {
                if (error.error.errors[0].msg.indexOf("currently in use") !== -1)
                    this.msg.setErrorMessage('Topic is currently in use and cannot be deleted.');
                else
                    this.msg.setErrorMessage(error.error);
            }
        );
    }
    

    deleteSubTopic() {
        let subTopicToDelete = Number(this.form.value.clientSubTopic);
        let msg:string = this.getSubTopicName(subTopicToDelete) ;
        const options = {
            title: `Are you sure you want to delete ${msg}?`,
            message: "",
            confirm: "Delete",
        };
    
        this.confirmDialog.open(options);
        this.confirmDialog.confirmed()
        .pipe(
            switchMap((confirmed) => {
                if (confirmed) {
                    if(subTopicToDelete == 0 ) 
                        return of(true);
                    else {
                        this.msg.loading( true,'Sending...');
                        return this.eventApi.deleteClientSubTopic(this.clientId, subTopicToDelete);
                    }
                } 
                else {
                    return EMPTY;
                }
            })
        )
        .subscribe(done => {
            if(done) {
                // remove from the list
                var inx = this.clientSubTopics.findIndex((x) => x.clientSubTopicId == subTopicToDelete);
                if(inx>-1) this.clientSubTopics.splice(inx, 1);

                this.msg.setSuccessMessage('Client Sub-Topic deleted successfully.');
                this.clearAll();

                (this.form.controls['clientTopicName'] as FormControl).clearValidators();
                (this.form.controls['clientTopicName'] as FormControl).updateValueAndValidity();
        
                this.eventApi.refreshTopics(true);
            }}, 
            (error: HttpErrorResponse) => {
                if (error.error.errors[0].msg.indexOf("currently in use") !== -1)
                    this.msg.setErrorMessage('Sub-Topic is currently in use and cannot be deleted.');
                else
                    this.msg.setErrorMessage(error.error);
            }
        );
    }

    saveSubTopic() {
        this.formSubmitted = true;

        (this.form.controls['clientTopicName'] as FormControl).clearValidators();
        (this.form.controls['clientTopicName'] as FormControl).updateValueAndValidity();

        this.form.updateValueAndValidity();
        if (this.form.invalid) return;

        this.dataSaving = true;
        this.prepareClientSubTopicModel();
        this.msg.loading(true,'Sending...' );

        this.eventApi.updateClientSubTopic(this.clientId, this.selectedClientSubTopic).subscribe(result => {
            if (!!result) {
                this.msg.setSuccessMessage("Sub-Topic saved successfully.");

                if (this.selectedClientSubTopic.clientSubTopicId == 0) {
                    this.clientSubTopics.push(result);
                    this.form.controls['clientSubTopic'].patchValue(result.clientSubTopicId);
                }
                else {
                    let item = this.clientSubTopics.find(x => x.clientSubTopicId == this.selectedClientSubTopic.clientSubTopicId);
                    if (item) 
                        item.description = result.description;
                }

                this.eventApi.refreshTopics(true);
            }
          },
          (error: HttpErrorResponse) => {
              if (error.error.errors[0].msg.indexOf("Description already exists") !== -1)
                this.msg.setErrorMessage('Name already exists.');
              else
              this.msg.setErrorMessage(error.error);
          });

        this.dataSaving = false;
        this.setAllValidators();
    }

    private getTopicName(topicId){
        let k = this.clientTopics.find(x => x.clientTopicId==topicId);
        return k ? k.description : "";
    }
    
    private getSubTopicName(subTopicId){
        let k = this.clientSubTopics.find(x=>x.clientSubTopicId==subTopicId);
        return k ? k.description : "";
    }
    
    onNoClick(){
        this.dialogRef.close();
    }
}
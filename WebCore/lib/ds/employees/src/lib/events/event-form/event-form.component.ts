import { Component, Inject, OnInit,  ElementRef, Input, Output, EventEmitter, ViewChild, HostListener} from "@angular/core";
import { FormControl, FormGroup, Validators, FormBuilder, AbstractControl } from '@angular/forms';

import { UserInfo } from '@ds/core/shared/user-info.model';
import { tap, filter, switchMap } from 'rxjs/operators';

import { ICountry } from '@ds/core/location/shared/country.model';

import { ChangeTrackerService } from '@ds/core/ui/forms/change-track/change-tracker.service';
import * as moment from 'moment';
import { IEvent, IClientTopic, IClientSubTopic } from "@ds/core/shared/event.model";
import { HttpErrorResponse } from '@angular/common/http';
import { of } from 'rxjs/internal/observable/of';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { EventService } from "lib/services/event.service";
import { NgxMessageService } from "@ds/core/ngx-message/ngx-message.service";

@Component({
    selector: 'ds-event-form',
    templateUrl: './event-form.component.html',
  })
  export class EventFormComponent implements OnInit {
      
    @Input() user: UserInfo;
    @Input() event: IEvent;
    @Input() pageSubmitted: boolean;
    @Input() isAdding: boolean;

    @Output() statusChange = new EventEmitter();
    private _isEssMode = false;
    @Input()
    get isEssMode(): boolean {
        return this._isEssMode;
    }
    set isEssMode(value: boolean) {
        this._isEssMode = coerceBooleanProperty(value);
    }

    submitted: boolean;
    disableView: boolean = false;
    clientId:number;
    hasEditPermissions: boolean;
    countries: ICountry[];
    f: FormGroup;
    formSubmitted: boolean;
    pageTitle: string;
    clientTopicId: number;
    private _editMode: boolean;
    clientTopics:IClientTopic[]=[];
    subTopics:IClientSubTopic[]=[];

    constructor(
        private fb: FormBuilder,
        private eventApi: EventService,
        private msg: NgxMessageService,
        private changeTrackerService: ChangeTrackerService,
    ) { }

    ngOnInit(): void {
        this.clientId = this.user.selectedClientId();
        this.pageTitle = "";
        if(!this.f) this.createForm();
        
        const loadDropdowns$ = this.eventApi.getClientTopics(this.clientId).pipe(
            switchMap(topics => {
                this.clientTopics = topics||[];
                if(this.event.clientSubTopicId)
                    return this.eventApi.getClientSubTopic(this.event.clientSubTopicId) ;
                else
                    return of(null) ;
            })
            ,switchMap(subTopic => {
                if(this.event.clientTopicId){
                    this.pageTitle = subTopic.topicDescription ;
                    return this.eventApi.getClientSubTopics(this.clientId, this.event.clientTopicId) ;
                } else {
                    return of([]) ;
                }
            })
            ,tap(subTopics => this.subTopics = subTopics||[])
            );

        loadDropdowns$.subscribe(x => {
            this.updateForm();
            if (this.f.value.editable && this.f.value.viewable)
                this.f.controls['viewable'].disable();
            else 
                this.f.controls['viewable'].enable();
        })
    }

    clearDrawer(){
        this.statusChange.emit(-1);
    }
    formatNumber(event:Event){
        let ctrl = <HTMLInputElement>event.target;
        let valu = ctrl.value.replace(/-/g,'');
        if(valu.length > 6) valu = valu.substring(0,3)+'-'+valu.substring(3,6)+'-'+valu.substring(6);
        else if(valu.length > 3) valu = valu.substring(0,3)+'-'+valu.substring(3);
        ctrl.value = valu;
    }
    topicChange(e) {
        this.subTopics = [];
        if (e.target.value) this.loadSubTopics(Number(e.target.value), null);
    }

    loadSubTopics(topicId:number, stateId: number) {
        this.subTopics = [];
        if (topicId) {
            this.eventApi.getClientSubTopics(this.clientId, topicId).subscribe(subs => {
                this.subTopics = subs || [];
                if(this.subTopics.length > 0){
                    this.f.patchValue({clientSubTopic : this.subTopics[0].clientSubTopicId });
                }
            });
        }
    }

    private createForm(): void {
        this.f = this.fb.group({
            clientTopic: this.fb.control(this.event.clientTopicId || '', [Validators.required]),
            clientSubTopic: this.fb.control(this.event.clientSubTopicId || '', [Validators.required]),
            eventDesc: this.fb.control(this.event.event || '', []),
            eventDate: this.fb.control(this.event.eventDate || '', [Validators.required]),
            expirationDate: this.fb.control(this.event.expirationDate || '', []),
            eventDuration: this.fb.control(this.event.duration || '', []),
            viewable: this.fb.control(this.event.isEmployeeViewable || '', []),
            editable: this.fb.control(this.event.isEmployeeEditable || '', []),
        });
    }

    private updateForm(): void{
        this.f.setValue({
            clientTopic: this.event.clientTopicId || '',
            clientSubTopic: this.event.clientSubTopicId || '',
            eventDesc: this.event.event || '',
            eventDate: this.event.eventDate || '',
            expirationDate: this.event.expirationDate || '',
            eventDuration: this.event.duration || '',
            viewable: this.event.isEmployeeViewable || false,
            editable: this.event.isEmployeeEditable || false,
        });
    }

    private updateModel(): void {
        let subTopicId = this.f.value.clientSubTopic ? this.f.value.clientSubTopic : this.subTopics[0].clientSubTopicId;
        Object.assign( this.event, {
            clientTopicId: this.f.value.clientTopic,
            clientSubTopicId: this.f.value.clientSubTopic,
            event: this.f.value.eventDesc,
            eventDate: this.f.value.eventDate ? moment(this.f.value.eventDate).toDate() : null,
            expirationDate: this.f.value.expirationDate ? moment(this.f.value.expirationDate).toDate() : null,
            duration: this.f.value.eventDuration,
            employeeId: this.event.employeeId,
            isEmployeeViewable: this.f.value.editable ? true : this.f.value.viewable,            
            isEmployeeEditable: this.f.value.editable,
        });
    }

    ngAfterViewInit() {

    }

    close() {
        this.f.reset();
        this.clearDrawer();
    }

    saveEvent(): void {
        this.formSubmitted = true;
        this.f.updateValueAndValidity();
        if (this.f.invalid) return;

        this.updateModel();
        this.msg.loading(true,'Sending...' );
        this.eventApi.updateEmployeeEvent(this.event.employeeId, this.event).pipe(
            tap( (result:IEvent) => {
                this.msg.setSuccessMessage("Event saved successfully." );
                this.changeTrackerService.clearIsDirty();
                this.event.isDirty = false;
                this.event.employeeEventId = result.employeeEventId;
                this.eventApi.setActiveEvent(this.event);
                this.statusChange.emit(result.employeeEventId);
            })
        ).subscribe( x=> {}, (error: HttpErrorResponse) => {
            this.msg.setErrorResponse(error);
        });
    }
    
    @HostListener("keydown.esc", ['$event'])
    onEsc(event) {
        event.preventDefault();
        event.stopPropagation();
        this.clearDrawer();
    }

    private getTopicName(topicId){
        let k = this.clientTopics.find(x => x.clientTopicId==topicId);
        return k ? k.description : "";
    }
    
    private getSubTopicName(subTopicId){
        let k = this.subTopics.find(x=>x.clientSubTopicId==subTopicId);
        return k ? k.description : "";
    }

    employeeViewableChanged() {
        if (!this.f.value.viewable && this.f.value.editable)
            this.f.patchValue({viewable : true });
    }

    employeeEditableChanged() {
        if (this.f.value.editable && !this.f.value.viewable)
            this.f.patchValue({viewable : true });

        if (this.f.value.editable && this.f.value.viewable)
            this.f.controls['viewable'].disable();
        else 
            this.f.controls['viewable'].enable();
    }
}
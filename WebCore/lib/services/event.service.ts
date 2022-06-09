import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { throwError, Observable, BehaviorSubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import * as saveAs from 'file-saver';
import { UserType } from '@ds/core/shared';
import { IFormStatusData } from '@ajs/onboarding/shared/models';
import { IClientSubTopic, IClientTopic, IEvent } from "@ds/core/shared/event.model";

@Injectable({
    providedIn: 'root'
})
export class EventService {
    static readonly CLIENTS_API_BASE = "api/clients";
    static readonly EMPLOYEE_API_BASE = "api/employee";

    private activeEvent$ = new BehaviorSubject<IEvent>(null);
    activeEvent: Observable<IEvent> = this.activeEvent$.asObservable();

    topicsChanged$ = new BehaviorSubject<boolean>(null);
    topicsChanged: Observable<boolean> = this.topicsChanged$.asObservable();


    constructor(private httpClient: HttpClient) {
    }
    getEmployeeEvents(employeeId:number):Observable<IEvent[]> {
        return this.httpClient.get<IEvent[]>(
            `${EventService.EMPLOYEE_API_BASE}/${employeeId}/events`);
    }

    getEmployeeEvent(eventId:number):Observable<IEvent> {
        return this.httpClient.get<IEvent>(
            `${EventService.EMPLOYEE_API_BASE}/events/${eventId}`);
    }

    updateEmployeeEvent(employeeId:number, dto:IEvent):Observable<IEvent> {
        return this.httpClient.post<IEvent>(
            `${EventService.EMPLOYEE_API_BASE}/${employeeId}/events/update`, dto);
    }

    getClientTopics(clientId:number):Observable<IClientTopic[]> {
        return this.httpClient.get<IClientTopic[]>(
            `${EventService.CLIENTS_API_BASE}/${clientId}/topics`);
    }

    updateClientTopic(clientId:number, dto:IClientTopic):Observable<IClientTopic> {
        return this.httpClient.post<IClientTopic>(
            `${EventService.CLIENTS_API_BASE}/${clientId}/topics/update`, dto);
    }

    updateClientSubTopic(clientId:number, dto:IClientSubTopic):Observable<IClientSubTopic> {
        return this.httpClient.post<IClientSubTopic>(
            `${EventService.CLIENTS_API_BASE}/${clientId}/sub-topics/update`, dto);
    }

    getClientSubTopics(clientId:number,topicId:number):Observable<IClientSubTopic[]> {
        return this.httpClient.get<IClientSubTopic[]>(
            `${EventService.CLIENTS_API_BASE}/${clientId}/topics/${topicId}/sub-topics`);
    }

    getClientSubTopicsByIds(clientId:number,subTopicIds:number[]):Observable<IClientSubTopic[]> {
        return this.httpClient.post<IClientSubTopic[]>(
            `${EventService.CLIENTS_API_BASE}/${clientId}/sub-topics`, subTopicIds);
    }

    getClientSubTopic(subTopicId:number):Observable<IClientSubTopic> {
        return this.httpClient.get<IClientSubTopic>(
            `${EventService.CLIENTS_API_BASE}/sub-topic/${subTopicId}`);
    }

    setActiveEvent(val: IEvent) {
        this.activeEvent$.next(val);
    }
    
    deleteActiveEvent(data: IEvent) {
        return this.httpClient.delete<IEvent>(
            `${EventService.EMPLOYEE_API_BASE}/${data.employeeId}/events/${data.employeeEventId}`);
    }

    deleteClientTopic(clientId: number, clientTopicId: number) {
        return this.httpClient.delete<IClientTopic>(
            `${EventService.CLIENTS_API_BASE}/${clientId}/topics/${clientTopicId}`);
    }

    deleteClientSubTopic(clientId: number, clientSubTopicId: number) {
        return this.httpClient.delete<IClientSubTopic>(
            `${EventService.CLIENTS_API_BASE}/${clientId}/sub-topics/${clientSubTopicId}`);
    }

    refreshTopics(refreshNow: boolean) {
        this.topicsChanged$.next(refreshNow);
    }
}
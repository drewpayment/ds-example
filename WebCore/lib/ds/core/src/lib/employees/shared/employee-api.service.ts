import { Injectable } from "@angular/core";
import { HttpParams, HttpClient } from "@angular/common/http";
import { IContact, ContactsProfileImageLoader } from "../../contacts";
import { map, shareReplay } from "rxjs/operators";
import { Observable } from "rxjs";
import { IEmployeeStatus } from "@ajs/employee/models";
import { IEmployeeBasicStatusData } from "@ajs/employee/hiring/shared/models";
import { IEmployeeTerminationReason } from "@ajs/employee/models/employee-termination-reason.model";
import {
  IEmployeeNotes,
  INewEmployeeNote,
  INoteSource,
  IEmpName,
  IEmpAttachmentChanges,
  INoteTag,
  IShareSettings,
} from "./employee-notes-api.model";
import { IElectronicConsents } from "./electronic-consents.model";
import { IEmployeeAvatars } from "./employee-avatars.model";
import { IEmployeeSearchResult, IEmployeeSearchResultResponseData } from '@ds/employees/search/shared/models/employee-search-result';
import { IEmployeeSearchFilter } from '@ds/employees/search/shared/models/employee-search-filter';
import { EmployeeSearchOptions } from '@ds/employees/search/shared/models/employee-search-options';
import { IEmployeeSearchSetting } from '@ajs/employee/search/shared/models';

@Injectable({
  providedIn: "root",
})
export class EmployeeApiService {
  static SERVICE_NAME = "EmployeeApiService";

  private api = "api/employees";

  defaultSearchFilters = this.getEmployeeSearchFilters().pipe(shareReplay());

  constructor(private http: HttpClient) {}

  /**
   * Gets contacts available to use when setting up a review.
   */
  getSupervisorsForEmployee(
    employeeId: number,
    haveActiveEmployee?: boolean,
    excludeTimeClockOnly?: boolean,
    ifSupervisorGetSubords?: boolean
  ) {
    let options = {
      params: new HttpParams().set(
        "haveActiveEmployee",
        (!!haveActiveEmployee).toString()
      ),
    };

    if (excludeTimeClockOnly) {
      options.params = options.params.set(
        "excludeTimeClockOnly",
        excludeTimeClockOnly.toString()
      );
    }

    if (ifSupervisorGetSubords) {
      options.params = options.params.set(
        "ifSupervisorGetSubords",
        true.toString()
      );
    }

    return this.http
      .get<IContact[]>(`${this.api}/${employeeId}/direct-supervisors`, options)
      .pipe(map(ContactsProfileImageLoader));
  }

  getDirectSupervisorForEmployee(employeeId: number) {
    return this.http.get<number>(`${this.api}/${employeeId}/direct-supervisor`);
  }

  getSupervisorsForClient() {
    return this.http
      .get<IContact[]>(`${this.api}/all-possible-supers`)
      .pipe(map(ContactsProfileImageLoader));
  }
  getEmployeeProfileCard(employeeId: number): Observable<IContact> {
    return this.http
      .get<IContact>(`${this.api}/${employeeId}/profile-card`)
      .pipe(
        map((card) => (card ? [card] : [])),
        map(ContactsProfileImageLoader),
        map((cards) => (cards.length > 0 ? cards[0] : null))
      );
  }

  getEmployeeSearchFilters(
    filterSupsBySupsWithActiveEmp?: boolean,
    excludeTimeClockOnly?: boolean,
    ifSupervisorGetSubords?: boolean
  ): Observable<IEmployeeSearchFilter[]> {
    var params = new HttpParams();

    if (filterSupsBySupsWithActiveEmp) {
      params = params.set(
        "filterSupsBySupsWithActiveEmp",
        filterSupsBySupsWithActiveEmp.toString()
      );
    }

    if (excludeTimeClockOnly) {
      params = params.set(
        "excludeTimeClockOnly",
        excludeTimeClockOnly.toString()
      );
    }

    if (ifSupervisorGetSubords) {
      params = params.set("ifSupervisorGetSubords", true.toString());
    }

    return this.http.get<IEmployeeSearchFilter[]>(
      `${this.api}/search/options`,
      { params: params }
    );
  }

  search(options?: EmployeeSearchOptions): Observable<IEmployeeSearchResultResponseData> {
    let params = <any>{};
    if (options) {
      for (let key in options) {
        const value = options[key];
        if (key !== 'filters') {
          params[key] = value ? value.toString().replace(/_group/gi, '') : value;
        }
        else {
          options.filters.forEach(filter => {
            if (filter.$selected) {
              params[filter.$selected.filterType] = filter.$selected.id;
            }
          });
        }
      }
    }

    return this.http.get<IEmployeeSearchResultResponseData>(`${this.api}/search`, { params });
  }

  setSearchSettings(opts: IEmployeeSearchSetting):Observable<IEmployeeSearchSetting> {
    return this.http.put<IEmployeeSearchSetting>(`${this.api}/search/defaults`, opts);
  }

  getSearchSettings(): Observable<IEmployeeSearchSetting> {
    return this.http.get<IEmployeeSearchSetting>(`${this.api}/search/defaults`);
  }

  updateLastEmployeeFromEmployeeSearchResult(dto: IEmployeeSearchResult): Observable<boolean> {
    return this.updateLastEmployee(dto.clientId, dto.employeeId);
  }

  updateLastEmployee(clientId: number, employeeId: number): Observable<any> {
    return this.http.post<any>(`api/employee/last-employee`, {clientId, employeeId});
  }

  // Gets the termination reason (or null if DNE) for employeeId.
  getEmployeeTerminationReason(
    employeeId: number
  ): Observable<IEmployeeTerminationReason> {
    return this.http.get<IEmployeeTerminationReason>(
      `api/employee/termination-reason/${employeeId}`
    );
  }

  // Gets list of all possible termination reasons.
  getEmployeeTerminationReasonsList(): Observable<
    IEmployeeTerminationReason[]
  > {
    return this.http.get<IEmployeeTerminationReason[]>(
      `${this.api}/termination-reasons`
    );
  }

  getEmployeeStatusList(): Observable<IEmployeeStatus[]> {
    return this.http.get<IEmployeeStatus[]>(`${this.api}/status`);
  }

  getEmployeeBasicPayInfo(): Observable<IEmployeeBasicStatusData> {
    return this.http.get<IEmployeeBasicStatusData>(
      `${this.api}/employee-basic-pay-info`
    );
  }

  saveEmployeeStatus(dtos: IEmployeeBasicStatusData) {
    const url = `${this.api}/saveEmployeeStatus`;
    return this.http.post<IEmployeeBasicStatusData>(url, dtos);
  }

  changeEmployeeStatusFromSeparationDate(dto: IEmployeeBasicStatusData) {
    const url = `api/employee/change-employee-status-from-separation-date`;
    return this.http.post<IEmployeeBasicStatusData>(url, dto);
  }

  // employeenotes
  getEmployeeNotes(employeeId: number) {
    return this.http.get<IEmployeeNotes[]>(`api/employees/${employeeId}/notes`);
  }

  getArchivedEmployeeNotes(employeeId: number) {
    return this.http.get<IEmployeeNotes[]>(
      `api/employees/${employeeId}/archived-notes`
    );
  }

  getNoteSources() {
    return this.http.get<INoteSource[]>(`api/note-sources`);
  }

  saveEmployeeNote(employeeNotes: INewEmployeeNote) {
    return this.http.post<INewEmployeeNote>(
      `api/employees/${employeeNotes.employeeId}/new-note`,
      employeeNotes
    );
  }

  saveShareSettings(shareSettings: IShareSettings) {
    return this.http.post<IShareSettings>(
      `api/employees/${shareSettings.remarkId}/save-share-settings`,
      shareSettings
    );
  }

  getShareSettings(remarkId: number) {
    return this.http.get<INoteSource[]>(
      `api/employees/${remarkId}/share-settings`
    );
  }

  getEmployeeName(employeeId: number) {
    return this.http.get<IEmpName>(`api/employees/${employeeId}/name`);
  }

  archiveEmployeeNote(remarkId: number) {
    return this.http.post(
      `api/employees/archive-employee-note/${remarkId}`,
      remarkId
    );
  }

  restoreEmployeeNote(remarkId: number) {
    return this.http.post(
      `api/employees/restore-employee-note/${remarkId}`,
      remarkId
    );
  }

  removeEmployeeNoteTag(remarkId: number, noteTagId) {
    return this.http.get<IEmpName>(
      `api/employees/note/${remarkId}/remove-tag/${noteTagId}`
    );
  }

  getClientNoteTags() {
    return this.http.get<INoteTag[]>(`api/employees/clients-note-tags`);
  }

  registerEmployeeNoteTagsChanges(tagChagnes: INoteTag[], remarkId: number) {
    return this.http.post(
      `api/employees/notes/change-tags/${remarkId}`,
      tagChagnes
    );
  }

  createClientNoteTag(tagName: string[]) {
    return this.http.post<INoteTag[]>(
      `api/employees/clients-note-tags/create`,
      tagName
    );
  }

  getCurrentUserHRBlockedAndViewOnly() {
    return this.http.get<any>(`api/account/HRBlockedAndViewOnly`);
  }

  registerEmployeeAttachmentChanges(changeDto: IEmpAttachmentChanges) {
    return this.http.post<any>(
      `api/employees/remark/${changeDto.remarkId}/attachment/${changeDto.attachmentId}/${changeDto.change}`,
      changeDto
    );
  }
  // end employeenotes

  getEmployeeConsentData(employeeId: number) {
    return this.http.get<IElectronicConsents>(
      `api/employeew2consent/getemployeew2consentdata/${employeeId}`
    );
  }
  updateEmployeeConsent(dto: IElectronicConsents) {
    return this.http.post<IElectronicConsents>(
      `api/employeew2consent/postemployeew2consentdata`,
      dto
    );
  }
  getEmployeeAvatar(employeeId: number) {
    return this.http.get<IEmployeeAvatars>(
      `api/employees/${employeeId}/avatar`
    );
  }
  saveEmployeeAvatar(dto: IEmployeeAvatars) {
    return this.http.post<IEmployeeAvatars>(`api/employees/save-avatar`, dto);
  }
}

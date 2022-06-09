import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject, forkJoin } from 'rxjs';
import { EmployeeAttachmentFolderDetail, EmployeeAttachmentFolder } from './models';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';

@Injectable({
  providedIn: 'root'
})
export class AttachmentsService {

  employeeFolders$ = new BehaviorSubject<EmployeeAttachmentFolderDetail[]>(null);
  user$ = new BehaviorSubject<UserInfo>(null);
  folderId = 0;
  private employeeAttachmentApi = `api/employeeAttachment`;
  private resourceApi = `api/resources`;

  constructor(private http: HttpClient, private accountService: AccountService) { }

  getEmployeeFolderList(employeeId: number, clientId: number, employeeVisible: Boolean) : Observable<EmployeeAttachmentFolderDetail[]> {
    let url = `${this.employeeAttachmentApi}/folderList/employeeId/${employeeId}/clientId/${clientId}/employeeVisible/${employeeVisible}`;

    let params = new HttpParams();
    params = params.append('employeeId', employeeId.toString());
    params = params.append('clientId', clientId.toString());

    return this.http.get<EmployeeAttachmentFolderDetail[]>(url, {params: params});
  }

  fetchFakeResolver() {
    this.accountService.getUserInfo()
      .subscribe((u : UserInfo) => {
        this.user$.next(u);
        this.fetchEmployeeFolders(u.selectedEmployeeId(), u.selectedClientId(), true);
    });
  }

  fetchEmployeeFolders(employeeId: number, clientId: number, employeeVisible: Boolean) {
    this.getEmployeeFolderList(employeeId, clientId, employeeVisible).subscribe(res => {
      if (res != null && res.length > 0 && this.folderId == 0) {
        const defPerfFolder = res.find((folder) => {
          return folder.isDefaultPerformanceFolder;
        });
        if (defPerfFolder != null)
          this.folderId = defPerfFolder.employeeFolderId;
      }

      this.employeeFolders$.next(res);
    });
  }
  setEmployeeFolderList(folders: EmployeeAttachmentFolderDetail[]) {
    this.employeeFolders$.next(folders);
  }

  getFolderId() {
    return this.folderId;
  }

}

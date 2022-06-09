import { IJobProfileBasicInfoData } from '@ajs/job-profiles/shared/models';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root',
})
export class JobProfilesService {

  private api = 'api/job-profiles';

  constructor(private http: HttpClient) {}

  getJobProfilesBasicInfo(): Observable<IJobProfileBasicInfoData[]> {
    return this.http.get<IJobProfileBasicInfoData[]>(`${this.api}/basic`);
  }

  getJobProfileBasicInfo(jobProfileId: number): Observable<IJobProfileBasicInfoData> {
    return this.http.get<IJobProfileBasicInfoData>(`${this.api}/${jobProfileId}/basic-info`);
  }

}

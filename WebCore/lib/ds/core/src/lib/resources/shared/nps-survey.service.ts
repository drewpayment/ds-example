import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { INpsQuestionDto } from "./nps-question-dto.model";
import { INpsResponseDto } from "./nps-response-dto.model";

@Injectable({
  providedIn: "root",
})
export class NPSSurveyService {
  constructor(private http: HttpClient) {}

  static readonly NPSSURVEY_API_BASE = "api/Nps";

  getNPSActiveQuestionByUserType(userTypeId: number) {
    const url = `${NPSSurveyService.NPSSURVEY_API_BASE}/activeQuestion/${userTypeId}`;
    return this.http.get<INpsQuestionDto>(url);
  }
  saveNPSResponse(npsResponse: INpsResponseDto) {
    const url = `${NPSSurveyService.NPSSURVEY_API_BASE}/response`;
    return this.http.post(url, npsResponse);
  }
}

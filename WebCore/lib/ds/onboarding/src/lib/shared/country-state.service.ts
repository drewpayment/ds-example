import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { throwError, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { isNotUndefinedOrNull } from "@util/ds-common";
import { ICountry, IState, ICounty } from './country.model'

@Injectable({
    providedIn: 'root'
})
export class CountryStateService {

    private locationApi = "api/location";

    constructor(private http: HttpClient){}

    getCountryList() {
        const url = `${this.locationApi}/countries`;
        return this.http.get<ICountry[]>(url);
    }

    getStatesByCountry(countryId) {
        const url = `${this.locationApi}/countries/${countryId}/states`;
        return this.http.get<IState[]>(url);
    }

    getCountiesByState(stateId) {
        const url = `${this.locationApi}/states/${stateId}`;
        return this.http.get<ICounty[]>(url);
    }
    
    getStatesForUSA()  {
        return this.getStatesByCountry(1);
    };
    
}
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ICountry } from './country.model';
import { IState } from './state.model';
import { ICounty } from './county.model';

@Injectable({
    providedIn: 'root'
})
export class LocationApiService {

  API_BASE_URL = 'api/location';
  defaults = {
      countries: {
          usa: 1
      },
      states: {
          michigan: 1
      }
  };

  countriesCache: any;
  stateCache: any = {};
  countyCache: any = {};

  constructor(
    private http: HttpClient
  ) {}

  /**
   * Returns the current country list.
   * @param {Boolean} reload - If truthy, will force countries to be reloaded from the server.
   * @returns {} 
   */
  getCountryList(reload?) {
    return this.http.get<ICountry[]>(`${this.API_BASE_URL}/countries`);
  }

  /**
  * @ngdoc method
  * @name getStatesByCountry
  *
  * @description
  * Retrieves a list of states for the specified country.
  */
  getStatesByCountry(countryId, reload = false) {
    return this.http.get<IState[]>(`${this.API_BASE_URL}/countries/${countryId}/states`);      
  }

  /**
  * @ngdoc method
  * @name getStatesForUSA
  *
  * @description
  * Retrieves a list of states for the united states.
  */
  getStatesForUSA = () => {
      return this.getStatesByCountry(this.defaults.countries.usa);
  }

  /**
   * Gets the counties for the specified state.
   * @param {Number} stateId - ID of the state to get counties for.
   * @returns {} 
   */
  getCountiesByState(stateId, reload = false) {
    return this.http.get<ICounty[]>(`${this.API_BASE_URL}/states/${stateId}/counties`);     
  }

}

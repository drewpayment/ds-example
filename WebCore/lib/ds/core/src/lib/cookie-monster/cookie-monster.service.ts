import { Injectable, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root'
})
export class CookieMonsterService {

  constructor(
    @Inject(DOCUMENT) private document: any
  ) { }

  /*******************************************************************************
   * @description Save a cookie in the browser.
   * 
   * @param name Should be pagename_elementidref (e.g. MobilePunch_btnPunch)
   * @param value 
   * @param includePath Include the path variable for the cookie
   * @param days  Days from now until expiration.
   * @param hours Hours from now until expiration.
   * @param minutes Minutes from now until expiration.
   * @param seconds Seconds from now until expiration.
   *******************************************************************************/
  setCookie(name: string, value: any, includePath: boolean, days: number, hours: number, minutes: number, seconds: number) {
    // let date = moment();

    // if (days) date.add(days, 'd');
    // if (hours) date.add(hours, 'h');
    // if (minutes) date.add(minutes, 'm');
    // if (seconds) date.add(seconds, 's');
    let date = new Date();
    if (days) date.setDate(date.getDate() + days);
    if (hours) date.setHours(date.getHours() + hours);
    if (minutes) date.setMinutes(date.getMinutes() + minutes);
    if (seconds) date.setSeconds(date.getSeconds() + seconds);

    let expirationDate = date.toUTCString();

    let path = window.location.pathname.split('/')[1];

    document.cookie = encodeURIComponent(name) + "=" + encodeURIComponent(value || "") +
    // IE is rejecting cookies when you specify the domain property... IDK why..
    // ";domain=" + window.location.hostname + 
    // ";path=/" + (includePath ? path + '/' : '') +
    ";expires=" + expirationDate + ';';

  }

  /**********************************************************
   * @description Gets a cookie from the broswer
   * 
   * @param name Name of cookie (e.g. MobilePunch_btnPunch)
   * 
   * @returns value of cookie or null.
   **********************************************************/
  getCookie(name : string) {
    const nameEq = name + "=";
    const ca = document.cookie.split(';');

    for (let i = 0; i < ca.length; i++) {
      let c = ca[i];
      while(c.charAt(0) == ' ')
        c = c.substring(1, c.length)
      if (c.indexOf(nameEq) == 0)
        return c.substring(nameEq.length, c.length);
    }

    return null;
  }

  /**************************************************
   * @description Removes a cookie from the browsers
   * 
   * @param name Name of Cookie (e.g. MobilePunch_btnPunch)
   *************************************************/
  removeCookie(name: string) {
    this.document.cookie = name + "=; Max-age=-99999999;";
  }
}

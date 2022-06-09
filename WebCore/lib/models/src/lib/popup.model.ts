import {IPopupSettings} from './popup-settings.model';

export interface IPopup {
  url?:string,
  windowName?:string,
  settings?:IPopupSettings,
  window?:any
}

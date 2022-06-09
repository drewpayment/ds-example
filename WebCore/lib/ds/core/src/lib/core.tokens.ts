import { InjectionToken } from '@angular/core';


export const WINDOW = new InjectionToken<string>('Window');

export const windowProvider = {
    provide: WINDOW,
    useValue: Window,
};

export const CORE_ENVIRONMENT = new InjectionToken<object>('core.environment');

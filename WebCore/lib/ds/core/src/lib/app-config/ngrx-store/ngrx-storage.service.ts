import { Injectable, InjectionToken, Inject } from '@angular/core';
import { StorageService, StorageDecoder } from 'ngx-webstorage-service';

export const NGRX_STORAGE_TOKEN = new InjectionToken<StorageService>('ds.ngrx.storage');

@Injectable()
export class NgrxStorageService {
    
    constructor(@Inject(NGRX_STORAGE_TOKEN) private store: StorageService) {}
    
    setSavedState(state: any, localStorageKey: string) {
        this.store.set(localStorageKey, JSON.stringify(state));
    }
    
    getSavedState<T>(localStorageKey: string, decoder?: StorageDecoder<any>): T {
        if (decoder) {
            return JSON.parse(this.store.get(localStorageKey, decoder)) as T;
        }
        
        const result = this.store.get(localStorageKey);
        
        return result != null ? JSON.parse(result) as T : null;
    }
    
}

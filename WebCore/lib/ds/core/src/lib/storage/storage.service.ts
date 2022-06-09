import { Injectable, Inject, InjectionToken } from '@angular/core';
import { StorageService, StorageDecoder, StorageEncoder, StorageTranscoders, StorageTranscoder } from 'ngx-webstorage-service';
import { DsStorageResult } from './models/store-result';

export const DS_STORAGE_SERVICE = new InjectionToken<StorageService>('ds.storage.service');
export const DS_STORAGE_DEFAULT_TRANSCODER = new InjectionToken<StorageTranscoder<any>>('useDefaultTranscoder', {
    providedIn: 'root',
    factory: () => StorageTranscoders.JSON
});

@Injectable({
    providedIn: 'root'
})
export class DsStorageService {
    static SERVICE_NAME = 'DsStorageService';
    
    constructor(@Inject(DS_STORAGE_SERVICE) private store: StorageService, 
        @Inject(DS_STORAGE_DEFAULT_TRANSCODER) private defaultTranscoder: StorageTranscoder<any>) {
        this.store.withDefaultTranscoder(this.defaultTranscoder);
    }
    
    has(key: string): DsStorageResult {
        const result = new DsStorageResult();
        if (!this._isKeyValid(key)) return result.setToFail();
        if (this.store.has(key)) 
            return result.setToSuccess();
        else 
            return result.setToFail();
    }
    
    get(key: string, decoder?: StorageDecoder<any>): DsStorageResult {
        if (!this._isKeyValid(key)) return null;
        let data;
        if (decoder) {
            data = this.store.get(key, decoder);
        } else {
            data = this.store.get(key);
        }
        return new DsStorageResult(data);
    }
    
    set(key: string, value: any, encoder?: StorageEncoder<any>): DsStorageResult {
        if (!this._isKeyValid(key)) 
            return new DsStorageResult().setToFail();
        if (encoder) {
            this.store.set(key, value, encoder);
        } else {
            this.store.set(key, value);
        }
        return new DsStorageResult(value);
    }
    
    private _isKeyValid(key: string): boolean {
        return key != null && key.length > 0;
    }
    
}

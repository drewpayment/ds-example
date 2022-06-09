import { coerceBooleanProperty } from '@angular/cdk/coercion';

export class DsStorageResult {
    private _success = false;
    set success(value: boolean) {
        this._success = coerceBooleanProperty(value);
    }
    get success(): boolean {
        return this._success;
    }
    
    get data() {
        return this.payload;
    }
    
    get hasError(): boolean {
        return !this.success;
    }
    
    constructor(private payload?: any, private overrideSuccess: boolean = false) {
        if (!this.overrideSuccess && this.payload !== undefined) this.evaluateSuccess();
    }
    
    setData(data: any): DsStorageResult {
        if (data == null) return;
        this.payload = data;
        this.evaluateSuccess();
        return this;
    }
    
    setToSuccess(): DsStorageResult {
        this.success = true;
        return this;
    }
    
    setToFail(): DsStorageResult {
        this.success = false;
        return this;
    }
    
    private evaluateSuccess(): void {
        this.success = this.payload != null;
    }
}

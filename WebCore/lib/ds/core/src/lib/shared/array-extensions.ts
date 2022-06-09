import { coerceArray } from '@angular/cdk/coercion';

declare global {
    interface Array<T> {
        extend(other_array: T[]): void;
    }
}

Array.prototype.extend = function(other_array) {
    /* You should include a test to check whether other_array really is an array */
    other_array = coerceArray(other_array);
    other_array.forEach((v) => { this.push(v); }, this);
};

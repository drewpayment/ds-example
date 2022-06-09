import { Pipe, PipeTransform } from '@angular/core';
import { coerceNumberProperty } from '@angular/cdk/coercion';
import { II9DocumentData } from "@ajs/onboarding/shared/models";


@Pipe({
    name: 'i9DocumentType'
})
export class I9DocumentTypePipe implements PipeTransform {
    
    transform(data: II9DocumentData[], requestType: string) {
        data.filter(item =>  (item.category == requestType) );
        return data ? data.filter(item =>  (item.category == requestType) ) : [];
    }
}

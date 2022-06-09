import { PipeTransform, Pipe } from '@angular/core';

@Pipe({
    name: 'dsCustomFilterCallback',
    pure: false
})
export class DsCustomFilterCallbackPipe implements PipeTransform {
    transform(items: any[], dsCustomFilterCallback: (item: any) => boolean): any {
        if (!items || !dsCustomFilterCallback) {
            return items;
        }
        return items.filter(item => dsCustomFilterCallback(item));
    }
}

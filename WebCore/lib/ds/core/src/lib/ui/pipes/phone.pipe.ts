import { PipeTransform, Pipe } from '@angular/core';
import { parsePhoneNumber, CountryCode } from 'libphonenumber-js/min';

@Pipe({
    name: 'phone'
})
export class PhonePipe implements PipeTransform {
    
    transform(value: number | string, country: string): string {
        try {
            const phoneNumber = parsePhoneNumber(`${value}`, country as CountryCode);
            return phoneNumber.formatNational();
        } catch (err) {
            return `${value}`;
        }
    }

    transformInternational(value: number | string, country: string): string {
        try {
            const phoneNumber = parsePhoneNumber(`${value}`, country as CountryCode);
            /// COLLINB CREATED THIS SO WE COULD USE IT FOR MOBILE ON THE EMPLOYEE EMERGENCY CONTACT PAGE
            return phoneNumber.formatInternational().replace(new RegExp(' ', 'gi'), '-');
        } catch (err) {
            return `${value}`;
        }
    }
    
}

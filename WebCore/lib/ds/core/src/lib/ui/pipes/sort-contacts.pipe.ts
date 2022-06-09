import { Pipe, PipeTransform } from '@angular/core';
import { IContact } from '@ds/core/contacts/shared/contact.model';

export function NormalizeString(stringToNormalize: string){
  return (stringToNormalize || "").toLowerCase().trim();
}

export function SortContacts(contacts: IContact[]): IContact[] {
 return (contacts || []).sort((firstContact, secondContact) => {
const typeDiff = firstContact.userType - secondContact.userType;
const firstNameDiff = NormalizeString(firstContact.firstName).localeCompare(NormalizeString(secondContact.firstName));
const lastNameDiff = NormalizeString(firstContact.lastName).localeCompare(NormalizeString(secondContact.lastName));

return typeDiff || lastNameDiff || firstNameDiff;
});
}



@Pipe({
  name: 'sortContacts'
})
export class SortContactsPipe implements PipeTransform {

  transform = SortContacts

}

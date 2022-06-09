import { Pipe, PipeTransform } from '@angular/core';
import { IContact } from '@ds/core/contacts';
import { Maybe } from '@ds/core/shared/Maybe';
import { UserType } from '@ds/core/shared';

function getUserTypeString(contact: Maybe<IContact>, usertype: UserType, userTypeString: string): string {
  return contact.map(x => x.userType === usertype ? true : null).map(x => userTypeString).valueOr("")
}

export function ConvertContactToName(contact: IContact): string {
  var result = "";
  const safeContact = new Maybe(contact);

  result += getUserTypeString(safeContact, UserType.companyAdmin, "C - ");
  result += getUserTypeString(safeContact, UserType.supervisor, "S - ");
  result += getUserTypeString(safeContact, UserType.employee, "E - ");
  result += safeContact.map(x => x.lastName + ", ").valueOr("");
  result += safeContact.map(x => x.firstName).valueOr("");

  return result;
}

@Pipe({
  name: 'contactToName'
})
export class ContactToNamePipe implements PipeTransform {

  transform = ConvertContactToName;

}

@Pipe({
  name: 'mapToContact'
})
export class MapToContactPipe implements PipeTransform {

  transform(value: any, mapper: (args: any) => IContact[]) {
    return mapper(value);
  }

}

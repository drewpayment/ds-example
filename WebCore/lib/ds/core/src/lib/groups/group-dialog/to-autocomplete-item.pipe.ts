import { Pipe, PipeTransform } from '@angular/core';
import { AutocompleteItem } from '../shared/autocomplete-item.model';
import { Maybe } from '@ds/core/shared/Maybe';

export type MapToAutocomplete<T> = <U extends AutocompleteItem>(val: T) => U | AutocompleteItem

@Pipe({
  name: 'toAutocompleteItem'
})
export class ToAutocompleteItemPipe<T> implements PipeTransform {

  transform(value: T[], mapper: MapToAutocomplete<T>): any {
    return new Maybe(value).map(x => x.map(mapper)).valueOr(<AutocompleteItem[]>[]);
  }

}

import { Pipe, PipeTransform } from '@angular/core';


@Pipe({name: 'formatMissingField'})
export class FormatMissingFieldPipe implements PipeTransform {
  transform(value: string[]): string {
    let result = "";
    value.forEach((v, i, a) => {
      if (i == (value.length - 1))
        result += v;
      else
        result += `${v}, `;
    });
    return result;
  }
}

import { Pipe, PipeTransform } from '@angular/core';
import { Maybe } from './Maybe';

type func = (val: any) => any;

@Pipe({
    name: 'toMaybe'
})
export class ToMaybe implements PipeTransform {
    /**
     * This should allow for generic types when they start working correctly in pipes
     * @see https://github.com/angular/angular/issues/21224
     */
  transform(maybe: any, maps?: func[] | func): Maybe<any> {

     // convert maps input to array of functions
      const funcsAsArray = new Maybe(maps)
      .map(x => !Array.isArray(x) ? [x] : x)
      .valueOr([] as func[]);

      // apply all functions to result (if any)
      var result = new Maybe(maybe);
      for(var i = 0; i < funcsAsArray.length; i++){
          const current = funcsAsArray[i];
          result = result.map(current);
      }
    return result;
  }
}

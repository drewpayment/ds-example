import { Pipe, PipeTransform } from '@angular/core';
import { formatNumber } from '@angular/common';

@Pipe({
  name: 'evalScoreToString'
})
export class EvalScoreToStringPipe implements PipeTransform {
  transform(methodTypeName: string, score: number): string {
    const formatter = (convertedNumber: number) => formatNumber(convertedNumber, navigator.language || (<any>navigator).browserLanguage, '1.2-2');
    if('Average'.localeCompare(methodTypeName) == 0){
      return formatter(score) + ' / 5';
    } else {
      const convertedScore = (score / 5) * 100;
      return formatter(convertedScore) + '%';
    }
  }

}

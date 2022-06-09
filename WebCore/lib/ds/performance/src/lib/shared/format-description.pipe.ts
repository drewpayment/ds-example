import { Pipe, PipeTransform } from '@angular/core';
import { listDesc } from './list-desc.func';

@Pipe({
  name: 'formatDescription',
  pure: true
})
export class FormatDescriptionPipe implements PipeTransform {
  transform = listDesc;
}


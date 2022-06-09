import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

/**
 * Workaround from https://github.com/angular/angular/issues/12009
 * 
 * Not putting in core because we probably don't want this to be widely used.  It bypasses angular's sanitization/security checks
 */
@Pipe({
  name: 'safeStyle'
})
export class SafeStylePipe implements PipeTransform {

  constructor(private sanitizer: DomSanitizer) { }

  transform(value) { return this.sanitizer.bypassSecurityTrustStyle(value); }

}

import { Directive, TemplateRef } from '@angular/core';

@Directive({
  selector: 'ng-template[dsDsExpansionContent]'
})
export class DsExpansionContentDirective {

  constructor(public _template:TemplateRef<any>) { }

}

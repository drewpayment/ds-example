import { Directive, ViewContainerRef, Input, ComponentFactoryResolver } from '@angular/core';
import { FieldTypeToComponent } from './dynamic-feedback-components';
import { IFeedbackResponse } from '..';


@Directive({
  selector: '[ds-feedback]'
})
export class FeedbackDirective {

  @Input()
  set feedback(val: IFeedbackResponse) {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(FieldTypeToComponent(val.fieldType));
    this.viewContainerRef.clear();
    let componentRef = this.viewContainerRef.createComponent(componentFactory);
    (<any>componentRef.instance).feedback = val;
  }

  constructor(public viewContainerRef: ViewContainerRef, public componentFactoryResolver: ComponentFactoryResolver) { }

}

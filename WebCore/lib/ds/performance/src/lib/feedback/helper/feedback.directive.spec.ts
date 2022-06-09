import { FeedbackDirective } from './feedback.directive';
import { ViewContainerRef, ComponentFactoryResolver } from '@angular/core';

describe('FeedbackDirective', () => {
  it('should create an instance', () => {
    const directive = new FeedbackDirective(<ViewContainerRef>{}, <ComponentFactoryResolver>{});
    expect(directive).toBeTruthy();
  });
});

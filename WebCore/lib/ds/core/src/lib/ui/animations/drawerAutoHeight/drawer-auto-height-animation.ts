import { animate, state, style, transition, trigger } from "@angular/animations";


export const changeDrawerHeightOnOpen = trigger('changeDrawerHeightOnOpen', [
  state('false', style({
    minHeight: '0px',
  })),
  state('true',   style({
    minHeight: '500px',
  })),
  transition('false => true', animate('400ms ease-in')),
  transition('true => false', animate('300ms ease-in'))
]);
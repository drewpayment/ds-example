import { animate, state, style, transition, trigger } from "@angular/animations";


export const changeDrawerHeightOnOpen = trigger('changeDrawerHeightOnOpen', [
  state('false', style({
    minHeight: '0px',
  })),
  state('true',   style({
    minHeight: '500px',
  })),
  transition('false => true', animate('100ms ease-in')),
  transition('true => false', animate('300ms ease-out'))
]);

export const changeDrawerHeightOnOpenSm = trigger('changeDrawerHeightOnOpenSm', [
  state('false', style({
    minHeight: '0px',
  })),
  state('true',   style({
    minHeight: '300px',
  })),
  transition('false => true', animate('100ms ease-in')),
  transition('true => false', animate('300ms ease-out'))
]);

export const matDrawerAfterHeightChange = trigger('matDrawerAfterHeightChange', [
  state('false', style({
    transform: 'translate3d(100%, 0, 0)',
    visibility: 'hidden'
  })),
  state('true',   style({
    transform: '0',
    visibility: 'visible'
  })),
  transition('false => true', animate('300ms ease-in')),
  transition('true => false', animate('300ms ease-out'))
]);